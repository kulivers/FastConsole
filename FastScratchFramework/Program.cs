using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Comindware.Platform.Core;
using Comindware.Platform.Core.Util;
using Monitor.Core.Utilities;

namespace FastScratchMVC
{
    
    internal class Program
    {
        private int FILE_NAME_NORMALIZED = 0x0;

        public static void Main(string[] args)
        {
            Main2();
        }

        private static void Main2()
        {
            var validator = new BackupPathValidator();
            var pathResolver = new SymbolicPathResolver();
            var path = @"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotEx";
            var tempDir = "/home/ekul/tempDir";
            
            var symbNotEx = @"D:\Work\master\Tests\Platform\bin\data\Temp\aaa";
            var juncNotExist = @"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotEx321";
            var s3 = JunctionPoint.GetTarget(juncNotExist);
            var s2 = JunctionPoint.GetTarget(symbNotEx);
            var s2313 = JunctionPoint.GetTarget2(juncNotExist);
            var s221321 = JunctionPoint.GetTarget2(symbNotEx);
            var s21 = NativeMethods.GetFinalPathName(juncNotExist);
            var s231 = NativeMethods.GetFinalPathName(symbNotEx);
            var defaultBackupPath = "/home/ekul/backups";
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            if (Directory.Exists(defaultBackupPath))
            {
                Directory.Delete(defaultBackupPath, true);
            }

            Directory.CreateDirectory(defaultBackupPath);
            Directory.CreateDirectory(tempDir);

            //link -> existingDir
            var linkToEx = Path.Combine(tempDir, "existingDirLink");
            pathResolver.CreateSymbolicDirectoryLink(linkToEx, defaultBackupPath);
            Console.WriteLine("1" + validator.ValidateDirectory(linkToEx).Status);

            //link -> NotExistingDir
            var target = Path.Combine(defaultBackupPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(target);
            var linkToNotEx = Path.Combine(tempDir, "linkToNotEx");
            pathResolver.CreateSymbolicDirectoryLink(linkToNotEx, target);
            Directory.Delete(target, true);
            Console.WriteLine("2" + validator.ValidateDirectory(linkToNotEx).Status);

            //link -> link -> existingDir
            target = linkToEx;
            var linkLinkToEx = Path.Combine(tempDir, "linkLinkToEx");
            pathResolver.CreateSymbolicDirectoryLink(linkLinkToEx, target);
            Console.WriteLine("3" + validator.ValidateDirectory(linkLinkToEx).Status);

            //link -> link -> NotExistingDir
            target = linkToNotEx;
            var linkLinkToNotEx = Path.Combine(tempDir, "linkLinkToNotEx");
            pathResolver.CreateSymbolicDirectoryLink(linkLinkToNotEx, target);
            Console.WriteLine("4" + validator.ValidateDirectory(linkLinkToNotEx).Status);

            //link -> notExistingLink
            var linkToNotExLink = Path.Combine(tempDir, "linkToNotExLink");
            target = linkLinkToNotEx;
            pathResolver.CreateSymbolicDirectoryLink(linkToNotExLink, target);
            Directory.Delete(target, true);
            Console.WriteLine("5" + validator.ValidateDirectory(linkToNotExLink).Status);
        }
    }
    public static class NativeMethods
    {
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        private const uint FILE_READ_EA = 0x0008;
        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            [MarshalAs(UnmanagedType.LPTStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] uint access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] uint flagsAndAttributes,
            IntPtr templateFile);

        public static string GetFinalPathName(string path)
        {
            var h = CreateFile(path, 
                FILE_READ_EA, 
                FileShare.ReadWrite | FileShare.Delete, 
                IntPtr.Zero, 
                FileMode.Open, 
                FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero);
            if (h == INVALID_HANDLE_VALUE)
                return null;

            try
            {
                var sb = new StringBuilder(1024);
                var res = GetFinalPathNameByHandle(h, sb, 1024, 0);
                if (res == 0)
                    return null;
                return sb.ToString();
            }
            finally
            {
                CloseHandle(h);
            }
        }
    }

}