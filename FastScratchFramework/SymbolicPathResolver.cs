using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Comindware.Platform.Core.Util;

namespace Comindware.Platform.Core
{
    public class SymbolicPathResolver
    {
        private readonly ISymbolicPathResolver _symbolicPathResolver;

        public SymbolicPathResolver()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _symbolicPathResolver = new WindowsSymbolicPathResolver();
            }
            else
            {
                _symbolicPathResolver = new LinuxSymbolicPathResolver();
            }
        }

        public bool TryGetFinalPathName(string path, out string finalPath)
        {
            return _symbolicPathResolver.TryGetFinalPathName(path, out finalPath);
        }

        public bool CreateSymbolicDirectoryLink(string lpSymlinkFileName, string lpTargetFileName)
        {
            return _symbolicPathResolver.CreateSymbolicDirectoryLink(lpSymlinkFileName, lpTargetFileName);
        }

        public bool CreateSymbolicFileLink(string lpSymlinkFileName, string lpTargetFileName)
        {
            return _symbolicPathResolver.CreateSymbolicFileLink(lpSymlinkFileName, lpTargetFileName);
        }

        private interface ISymbolicPathResolver
        {
            bool TryGetFinalPathName(string path, out string finalPath);
            bool CreateSymbolicDirectoryLink(string lpSymlinkFileName, string lpTargetFileName);
            bool CreateSymbolicFileLink(string lpSymlinkFileName, string lpTargetFileName);
        }

        private class WindowsSymbolicPathResolver : ISymbolicPathResolver
        {
            private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

            private const uint FILE_READ_EA = 0x0008;
            private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;

            public bool CreateSymbolicDirectoryLink(string lpSymlinkFileName, string lpTargetFileName)
            {
                return CreateSymbolicLink(lpSymlinkFileName, lpTargetFileName, SymbolicLink.Directory);
            }

            public bool CreateSymbolicFileLink(string lpSymlinkFileName, string lpTargetFileName)
            {
                return CreateSymbolicLink(lpSymlinkFileName, lpTargetFileName, SymbolicLink.File);
            }

            public bool TryGetFinalPathName(string path, out string finalPath)
            {
                var h = CreateFile(path,
                    FILE_READ_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    IntPtr.Zero,
                    FileMode.Open,
                    FILE_FLAG_BACKUP_SEMANTICS,
                    IntPtr.Zero);
                if (h == INVALID_HANDLE_VALUE)
                {
                    finalPath = default;
                    return false;
                }

                try
                {
                    var sb = new StringBuilder(1024);
                    var res = GetFinalPathNameByHandle(h, sb, 1024, 0);
                    if (res == 0)
                    {
                        finalPath = default;
                        return false;
                    }

                    finalPath = sb.ToString();
                    return true;
                }
                catch
                {
                    finalPath = default;
                    return false;
                }
                finally
                {
                    CloseHandle(h);
                }
            }

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable,
                out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CreateFile(
                [MarshalAs(UnmanagedType.LPTStr)] string filename,
                [MarshalAs(UnmanagedType.U4)] uint access,
                [MarshalAs(UnmanagedType.U4)] FileShare share,
                IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] uint flagsAndAttributes,
                IntPtr templateFile);

            [DllImport("kernel32.dll")]
            static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

            private enum SymbolicLink
            {
                File = 0,
                Directory = 1
            }
        }

        private class LinuxSymbolicPathResolver : ISymbolicPathResolver
        {
            private readonly LinuxBashInvoker _linuxBashInvoker = new LinuxBashInvoker();

            public bool TryGetFinalPathName(string path, out string finalPath)
            {
                try
                {
                    finalPath = _linuxBashInvoker.Bash($"realpath {path}");
                    Console.WriteLine($"input: {path}. realpath: {finalPath}");
                    return true;
                }
                catch
                {
                    finalPath = default;
                    return false;
                }
            }

            public bool CreateSymbolicDirectoryLink(string lpSymlinkFileName, string lpTargetFileName)
            {
                try
                {
                    _linuxBashInvoker.Bash($"ln -s {lpTargetFileName} {lpSymlinkFileName}", false);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool CreateSymbolicFileLink(string lpSymlinkFileName, string lpTargetFileName)
            {
                try
                {
                    _linuxBashInvoker.Bash($"ln -s {lpTargetFileName} {lpSymlinkFileName}", false);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}