using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Comindware.Platform.Core;
using Comindware.Platform.Core.Util;
using Monitor.Core.Utilities;

namespace FastScratchMVC
{
    internal class Program
    {
        public bool GetDiskFreeSpace(string location, out ulong freeBytes)
        {
            freeBytes = 0;
            string[] lines = _linuxBashInvoker.Bash(string.Join(" ", getAvailableSizeDfCommand))
                                              .Split(RowSplitter, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1)
            {
                return false;
            }

            FileSystemRecord condidate = default;
            for (int i = 1; i < lines.Length; i++)
            {
                var current = new FileSystemRecord(lines[i]);
                if (current.IsValid && location.StartsWith(current.MountLocation) &&
                    (string.IsNullOrEmpty(condidate.MountLocation) ||
                     condidate.MountLocation.Length < current.MountLocation.Length))
                {
                    condidate = current;
                }
            }

            freeBytes = condidate.Available * 1024;
            return condidate.IsValid;
        }

        public static void Main(string[] args)
        {
            var count = Count(new List<int>(){3,1,2}, (i, i1) => ++i);
            return;
            var all = Enumerable.Empty<int>().All(x=>false);
            try
            {
                var exists = Directory.Exists("/home/ekul/aaaa");
                Console.WriteLine(exists);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return;
            var pathResolver = new SymbolicPathResolver();
            var tempDir = "/home/ekul/temp";
            var defaultBackupPath = "/home/ekul/defaultBackupPath";
            tempDir = Directory.GetCurrentDirectory() + "temp";
            defaultBackupPath = Directory.GetCurrentDirectory() + "defaultBackupPath";
            if (!System.IO.Directory.Exists(tempDir))
            {
                System.IO.Directory.CreateDirectory(tempDir);
            }
            else
            {
                System.IO.Directory.Delete(tempDir, true);
                System.IO.Directory.CreateDirectory(tempDir);
            }

            if (!System.IO.Directory.Exists(defaultBackupPath))
            {
                System.IO.Directory.CreateDirectory(defaultBackupPath);
            }
            else
            {
                System.IO.Directory.Delete(defaultBackupPath, true);
                System.IO.Directory.CreateDirectory(defaultBackupPath);
            }

            Console.WriteLine("START");

            var targetDirectory = Path.Combine(defaultBackupPath, Guid.NewGuid().ToString());
            var linkLinkToNotEx = Path.Combine(tempDir, "linkLinkToNotEx");
            pathResolver.CreateSymbolicDirectoryLink(linkLinkToNotEx, targetDirectory);
            var linkToNotExLink = Path.Combine(tempDir, "linkToNotExLink");
            pathResolver.CreateSymbolicDirectoryLink(linkToNotExLink, linkLinkToNotEx);
            Console.WriteLine(linkLinkToNotEx);
            File.Delete(linkLinkToNotEx);
        }

        private static void Main2()
        {
            var validator = new BackupPathValidator();
            var pathResolver = new SymbolicPathResolver();
            pathResolver.TryGetFinalPathName(@"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotExLink", out var aaa);

            var juncNotExist = @"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotEx321";
            var s3 = JunctionPoint.GetJunction(juncNotExist);
            var s2313 = JunctionPoint.GetSymbolicLink(juncNotExist);

            var symbNotEx = @"D:\Work\master\Tests\Platform\bin\data\Temp\aaa";
            var s2 = JunctionPoint.GetJunction(symbNotEx);
            var s221321 = JunctionPoint.GetSymbolicLink(symbNotEx);

            var linktonotex = @"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotEx";
            var s1232 = JunctionPoint.GetJunction(linktonotex);
            var s2211321 = JunctionPoint.GetSymbolicLink(linktonotex);

            var linkToExisting = @"D:\Work\master\Tests\Platform\bin\data\Temp\existingDirLink";
            var s1231212 = JunctionPoint.GetJunction(linkToExisting);
            var s2211312321 = JunctionPoint.GetSymbolicLink(linkToExisting);
            return;
            var tempDir = "/home/ekul/tempDir";
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
}