// Copyright 2015 Eric Millin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Grib.Api.Interop;
using Grib.Api.Interop.SWIG;
using Grib.Api.Interop.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Grib.Api;

/// <summary>
/// GRIB file iterator object that provides methods for reading and writing messages. When iterated, returns
/// instances of the <see cref="GribMessage"/> class.
/// </summary>
public class GribFile : AutoRef, IEnumerable<GribMessage>
{
    private readonly IntPtr fileHandleProxyPtr;

    public GribFile(FileSystemInfo fileSystemInfo)
      : this(fileSystemInfo.FullName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GribFile" /> class. File read rights are shared between processes.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <exception cref="IOException">Could not open file. See inner exception for more detail.</exception>
    /// <exception cref="FileLoadException">The file is empty.</exception>
    public GribFile(string fileName)
    {
        fileHandleProxyPtr = GribApiNative.CreateFileHandleProxy(fileName);

        if (fileHandleProxyPtr == IntPtr.Zero)
        {
            throw new FileLoadException("Could not open file. See inner exception for more detail.", new Win32Exception(Marshal.GetLastWin32Error()));
        }

        var fileHandleProxy = (FileHandleProxy) Marshal.PtrToStructure(fileHandleProxyPtr, typeof(FileHandleProxy))!;

        FileName = fileName;
        Reference = new HandleRef(this, fileHandleProxy.File);
        Context = GribApiProxy.GribContextGetDefault();

        if (Context == null)
        {
            throw new GribApiException("Failed to get context!");
        }

        // set the message count here; the result seems to be connected to the message iterator so
        // that after you begin iterating messages, the count decreases until it reaches 1.
        GribApiProxy.GribCountInFile(Context, this, out var count);
        MessageCount = count;
    }

    /// <summary>
    /// Called when [dispose].
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void OnDispose(bool disposing)
    {
        if (fileHandleProxyPtr != IntPtr.Zero)
        {
            GribApiNative.DestroyFileHandleProxy(fileHandleProxyPtr);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<GribMessage> GetEnumerator()
    {
        var i = 0;

        while (GribMessage.Create(this, i++) is { } msg)
        {
            yield return msg;
        }

        Rewind();
    }

    /// <summary>
    /// Resets the underlying file pointer to the beginning of the file.
    /// </summary>
    public void Rewind()
    {
        GribApiNative.RewindFileHandleProxy(fileHandleProxyPtr);
    }

    /// <summary>
    /// NOT IMPLEMENTED.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes a message to the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="message">The message.</param>
    /// <param name="mode">The mode.</param>
    public static void Write (string path, GribMessage message, FileMode mode = FileMode.Create)
    {
        Write(path, new [] { message }, mode);
    }

    /// <summary>
    /// Writes all messages in the file to the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="file">The file.</param>
    /// <param name="mode">The mode.</param>
    public static void Write(string path, GribFile file, FileMode mode = FileMode.Create)
    {
        Write(path, file as IEnumerable<GribMessage>, mode);
    }

    /// <summary>
    /// Writes messages the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="messages">The messages.</param>
    /// <param name="mode">The mode.</param>
    public static void Write(string path, IEnumerable<GribMessage> messages, FileMode mode = FileMode.Create)
    {
        // TODO: Getting the buffer and writing to file in C++ precludes the need for byte[] copy
        using var fs = new FileStream(path, mode, FileAccess.Write, FileShare.Read, 8192);

        foreach (var message in messages)
        {
            fs.Write(message.Buffer, 0, message.Buffer.Length);
        }
    }

    public string FileName { get; private set; }

    public int MessageCount { get; protected set; }

    public GribContext Context { get; protected set; }
}