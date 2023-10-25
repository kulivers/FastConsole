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