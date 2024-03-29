using Grib.Api.Interop.Util;
using System;
using System.IO;
using System.Threading;
using Grib.Api.Interop;
using System.Linq;

namespace Grib.Api;

public class GribClient
{
    private static readonly byte[] GRIB_FILE_END_GTS = [0x0D, 0x0D, 0x0A, 0x03];
    private static readonly byte[] GRIB_FILE_END = [0x37, 0x37, 0x37, 0x37];

    protected readonly Lazy<AutoRef> LoadLibraryLazy;

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public GribClient(DirectoryInfo directoryInfo)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        LoadLibraryLazy = new(() =>
        {
            if (!directoryInfo.Exists) throw new GribApiFatalException("The GRIB library path does not exist.");

            var definitionsPath = Path.Combine(directoryInfo.FullName, @"Grib.Api\definitions");

            if (!File.Exists(Path.Combine(definitionsPath, "boot.def")))
            {
                throw new GribApiFatalException($"Failed to locate 'boot.def' in {definitionsPath}.");
            }

            PutEnvVar("GRIB_DEFINITION_PATH", definitionsPath);

            var platform = Environment.Is64BitProcess ? "x64" : "x86";
            var libraryPath = Path.Combine(directoryInfo.FullName, @"Grib.Api\lib\win", platform);
            var gribNativeLibPath = Path.Combine(libraryPath, "Grib.Api.Native.dll");

            if (!File.Exists(gribNativeLibPath))
            {
                throw new GribApiFatalException($"Failed to locate 'Grib.Api.Native.dll' in {libraryPath}.");
            }

            var href = Win32.LoadWin32Library(gribNativeLibPath);
            GribApiNative.HookGribExceptions();
            return href;
        }, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public GribFile OpenGribFile(FileInfo fileInfo, bool skipValidation = false)
    {
        var _ = LoadLibraryLazy.Value;

        if (!skipValidation)
        {
            if (!FileIsValid(fileInfo.FullName))
            {
                throw new FileLoadException("This file is empty or invalid.");
            }
        }

        return new GribFile(fileInfo);
    }

    private static bool FileIsValid(string fileName)
    {
        try
        {
            using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (fs.Length < 8) { return false; }

            if (!fs.CanRead || !fs.CanSeek) { return false; }

            long offset = -1;
            fs.Seek(offset, SeekOrigin.End);

            // ignore any empty bytes at the end of the file
            while (fs.Position > 0 && fs.ReadByte() == 0x00)
            {
                fs.Seek(--offset, SeekOrigin.End);
            }

            var buffer = new byte[4];

            fs.Seek(offset - 3, SeekOrigin.End);
            fs.Read(buffer, 0, 4);

            return buffer.SequenceEqual(GRIB_FILE_END) || buffer.SequenceEqual(GRIB_FILE_END_GTS);
        } 
        catch (Exception)
        {
            return false;
        }
    }

    private static void PutEnvVar(string name, string val)
    {
        Environment.SetEnvironmentVariable(name, val, EnvironmentVariableTarget.Process);
        Win32._putenv_s(name, val);
    }
}