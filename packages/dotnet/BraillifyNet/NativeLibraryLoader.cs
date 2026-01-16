#if NETCOREAPP3_0_OR_GREATER
// .NET Core 3.0+: NativeLibrary 클래스 사용 가능

namespace Braillify;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
#if NET9_0_OR_GREATER
using System.Threading;
#endif

internal static class NativeLibraryLoader
{
    private static bool _isLoaded;
#if NET9_0_OR_GREATER
    // .NET 9+: Lock 클래스 사용 - 더 효율적인 동기화
    private static readonly Lock _lock = new();
#else
    private static readonly object _lock = new object();
#endif

    internal static void EnsureLoaded()
    {
        if (_isLoaded)
        {
            return;
        }

        lock (_lock)
        {
            if (_isLoaded)
            {
                return;
            }

            NativeLibrary.SetDllImportResolver(typeof(NativeLibraryLoader).Assembly, DllImportResolver);
            _isLoaded = true;
        }
    }

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != "braillify_native")
        {
            return IntPtr.Zero;
        }

        var rid = GetRuntimeIdentifier();
        var libraryFileName = GetLibraryFileName();

        // AppContext.BaseDirectory 사용 (NativeAOT 호환)
        var baseDir = AppContext.BaseDirectory;
        var paths = new[]
        {
            // NuGet 패키지 구조: runtimes/{rid}/native/{lib}
            Path.Combine(baseDir, "runtimes", rid, "native", libraryFileName),

            // 개발 환경: 직접 경로
            Path.Combine(baseDir, libraryFileName),

            // 상위 디렉토리 탐색
            Path.Combine(baseDir, "..", "runtimes", rid, "native", libraryFileName),
        };

        foreach (var path in paths)
        {
            if (File.Exists(path) && NativeLibrary.TryLoad(path, out var handle))
            {
                return handle;
            }
        }

        // 기본 로딩 시도 (시스템 PATH 등)
        if (NativeLibrary.TryLoad(libraryName, assembly, searchPath, out var defaultHandle))
        {
            return defaultHandle;
        }

        return IntPtr.Zero;
    }

    private static string GetRuntimeIdentifier()
    {
        var arch = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm64 => "arm64",
            Architecture.Arm => "arm",
            _ => "x64"
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"win-{arch}";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return $"linux-{arch}";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return $"osx-{arch}";
        }

        // 알 수 없는 플랫폼은 Linux로 가정
        return $"linux-{arch}";
    }

    private static string GetLibraryFileName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "braillify_native.dll";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "libbraillify_native.dylib";
        }

        // Linux 및 기타
        return "libbraillify_native.so";
    }
}

#endif
