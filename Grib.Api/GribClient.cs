using Grib.Api.Interop.Util;
using System;
using System.IO;
using System.Threading;
using Grib.Api.Interop;

namespace Grib.Api;

public class GribClient
{
    protected readonly Lazy<AutoRef> LoadLibraryLazy;

    public GribClient(DirectoryInfo directoryInfo)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        LoadLibraryLazy = new(() =>
        {
            var definitionsPath = Path.Combine(directoryInfo.FullName, @"Grib.Api\definitions");

            if (!File.Exists(Path.Combine(definitionsPath, "boot.def")))
            {
                throw new GribApiFatalException("Failed to locate 'boot.def' file.");
            }

            PutEnvVar("GRIB_DEFINITION_PATH", definitionsPath);

            var platform = (IntPtr.Size == 8) ? "x64" : "x86";
            var gribNativeLibPath = Path.Combine(directoryInfo.FullName, @"Grib.Api\lib\win", platform, "Grib.Api.Native.dll");

            if (!File.Exists(gribNativeLibPath))
            {
                throw new GribApiFatalException("Failed to locate 'Grib.Api.Native.dll' file.");
            }

            var href = Win32.LoadWin32Library(gribNativeLibPath);
            GribApiNative.HookGribExceptions();
            return href;
        }, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public GribFile OpenGribFile(FileInfo fileInfo)
    {
        var _ = LoadLibraryLazy.Value;
        return new GribFile(fileInfo);
    }

    private static void PutEnvVar(string name, string val)
    {
        Environment.SetEnvironmentVariable(name, val, EnvironmentVariableTarget.Process);
        Win32._putenv_s(name, val);
    }
}