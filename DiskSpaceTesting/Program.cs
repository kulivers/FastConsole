using System.IO.Compression;
using System;
using System.IO;
using System.Linq;

namespace DiskSpaceTesting
{
    internal class Program
    {
        private static string getAvailableSizeDfCommand = "df -ak --output=target,avail";
        private static string getTotalSizeDfCommand = "df -ak --output=target,size";
        private static char[] ColumnSplitter = new char[] { ' ', '\t' };
        private static char[] RowSplitter = new char[] { '\r', '\n' };

        public static bool GetTotalDiskSpace(string location, out ulong freeBytes)
        {
            var linuxBashInvoker = new LinuxBashInvoker();
            freeBytes = 0;
            string[] lines = linuxBashInvoker.Bash(string.Join(" ", getTotalSizeDfCommand))
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

        public static bool GetDiskFreeSpace(string location, out ulong freeBytes)
        {
            var linuxBashInvoker = new LinuxBashInvoker();
            freeBytes = 0;
            string[] lines = linuxBashInvoker.Bash(string.Join(" ", getAvailableSizeDfCommand))
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
        private static long CalculateDirSize(DirectoryInfo d) 
        {
            // Add file sizes.
            var fileInfos = d.GetFiles();
            var size = fileInfos.Sum(fi => fi.Length);
            // Add subdirectory sizes.
            var dis = d.GetDirectories();
            size += dis.Sum(CalculateDirSize);
            return size;  
        }

        private const string _databaseDirectory = "/var/www/"; 

        public static void Main(string[] args)
        {
            var path = "D:\\Playground\\Garbage\\FastConsole\\DiskSpaceTesting\\test.cdbbz";
            
            var dtb = "D:\\Work\\master2\\Web\\data\\Database\\db";
            var db = "D:\\Work\\master2\\Web\\data\\Database\\db";
            var wal = "D:\\Work\\master2\\Web\\data\\Database\\wal";
            var dirSize = CalculateDirSize(new DirectoryInfo(db));//639652243
            var calculateDirSize = CalculateDirSize(new DirectoryInfo(wal));//536870912
            var dtba = CalculateDirSize(new DirectoryInfo(dtb));//639652303
        }
        
        private bool TryCalculateExpectedBackupSize(string path, out long size)
        {
            size = default;
            if (!File.Exists(path))
            {
                return false;
            } 

            if (TryGetUncompressedArchiveSize(path, out size))
            {
                return true;
            }

            if (TryGetDirectorySize(_databaseDirectory, out size))
            {
                return true;
            }

            return false;
        }

        private bool TryGetDirectorySize(string dirPath, out long size)
        {
            size = default;
            try
            {
                size = CalculateDirectorySize(new DirectoryInfo(dirPath));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private long CalculateDirectorySize(DirectoryInfo d) 
        {
            // Add file sizes.
            var fileInfos = d.GetFiles();
            var size = fileInfos.Sum(fi => fi.Length);
            // Add subdirectory sizes.
            var dis = d.GetDirectories();
            size += dis.Sum(CalculateDirectorySize);
            return size;
        }
        
        private bool TryGetUncompressedArchiveSize(string archivePath, out long size)
        {
            size = default;
            if (!File.Exists(archivePath))
            {
                return false;
            }

            try
            {
                using (var fileStream = new FileStream(archivePath, FileMode.Open))
                {
                    using (var zipArchive = new ZipArchive(fileStream))
                    {
                        size = zipArchive.Entries.Sum(entry => entry.Length);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        private struct FileSystemRecord
        {
            public string MountLocation;
            public ulong Available;
            public bool IsValid;

            public FileSystemRecord(string definition)
            {
                IsValid = false;
                var values = definition.Split(ColumnSplitter, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length < 2)
                {
                    MountLocation = string.Empty;
                    Available = 0;
                }

                MountLocation = values[0];
                Available = 0;
                if (ulong.TryParse(values[1], out var available))
                {
                    Available = available;
                    IsValid = true;
                }
            }
        }
    }
}