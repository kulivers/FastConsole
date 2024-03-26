using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Comindware.Platform.Core.Util;
using Monitor.Core.Utilities;
using Nest;

namespace FastScratchMVC
{
    public class User
    {
        public string Id { get; set; }
        public string KafkaBootstrapServer { get; set; }
        public User2 User2 { get; set; }
    }
    public class User2
    {
        public string Id { get; set; }
        public User User { get; set; }
    }
    internal class Program
    {
        private static string RandomLogFile => $"/tmp/{Guid.NewGuid().ToString()}";
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            while (!cts.IsCancellationRequested)
            {
                ;
            }
            
            
            var user2 = (User2)null;
            ClearFolder(@"C:\Users\ekul\Desktop\clear me");
        }

        private static void ClearFolder(string folderName)
        {
            var dir = new DirectoryInfo(folderName);
            foreach(var fi in dir.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                    // ignored
                }
            }

            foreach (var di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }
        
        private static IEnumerable<GroupedBackupConfigPathInfo> GroupConfigurationsByFilePath(IEnumerable<BackupConfiguration> configurations)
        {
            var gropedConfigs = new HashSet<GroupedBackupConfigPathInfo>(new GroupedBackupConfigPathInfoComparer());
            foreach (var cfg in configurations)
            {
                UpdateGroups(gropedConfigs, cfg, cfg.Repository);
                UpdateGroups(gropedConfigs, cfg, cfg.ExtraRepository);
            }

            return gropedConfigs;
        }

        public static void UpdateGroups(HashSet<GroupedBackupConfigPathInfo> groups, BackupConfiguration bc, BackupRepository repo)
        {
            if (repo == null || repo.Type != BackupRepositoryType.FileSystem)
            {
                return;
            }

            var gropedConfigToCompare = new GroupedBackupConfigPathInfo(bc.Id, bc.IsDisabled, bc.Schedule.Type, repo.FileSystemPath);
            if (groups.TryGetValue(gropedConfigToCompare, out var gropedConfig)) //always here cuz in grouped will be instance with new id? 
            {
                gropedConfig.Ids.Add(bc.Id);
            }
            else
            {
                groups.Add(gropedConfigToCompare);
            }
        }
    }

    public class GroupedBackupConfigPathInfo
    {
        public HashSet<string> Ids { get; set; }

        public bool IsDisabled { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public string Path { get; set; }

        public GroupedBackupConfigPathInfo()
        {
        }

        public GroupedBackupConfigPathInfo(string id, bool isDisabled, ScheduleType scheduleType, string path)
        {
            Ids = new HashSet<string>() { id };
            IsDisabled = isDisabled;
            ScheduleType = scheduleType;
            Path = path;
        }
    }

    public enum ScheduleType
    {
        //Automatic,
        TimeInterval,
        Manual,
    }

    public class BackupRepository
    {
        /// <summary>
        /// Backup repository identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A repository type
        /// </summary>
        public BackupRepositoryType Type { get; set; }

        /// <summary>
        /// Path to directory in file system
        /// </summary>
        public string FileSystemPath { get; set; }

        /// <summary>
        /// Bucket name in S3 storage
        /// </summary>
        public string S3Bucket { get; set; }

        public static BackupRepository CreateForFileSystem(string path)
        {
            return new BackupRepository() { Type = BackupRepositoryType.FileSystem, FileSystemPath = path };
        }

        public static BackupRepository CreateForS3(string bucket)
        {
            return new BackupRepository() { Type = BackupRepositoryType.S3, S3Bucket = bucket };
        }
    }

    public enum BackupRepositoryType
    {
        Undefined,
        FileSystem,
        S3
    }

    public class BackupConfiguration
    {
        /// <summary>
        /// Backup configuration identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Backup files repository
        /// </summary>
        public BackupRepository Repository { get; set; }

        /// <summary>
        /// Backup file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Backup contains streams
        /// </summary>
        public bool WithStreams { get; set; }

        /// <summary>
        /// Backup contains scripts
        /// </summary>
        public bool WithScripts { get; set; }

        /// <summary>
        /// Backup contains history data
        /// </summary>
        public bool WithHistory { get; set; }

        /// <summary>
        /// Backup description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Launch schedule
        /// </summary>
        public ScheduleConfiguration Schedule;

        /// <summary>
        /// Number of backup archives which wouldn't be deleted automatically
        /// </summary>
        public uint KeepRecent { get; set; }

        /// <summary>
        /// If config is disabled then backup will not be run neither manually nor by schedule
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Backup file can be downloaded
        /// </summary>
        public bool IsDownloadAllowed { get; set; }

        /// <summary>
        /// Backup configuration is readonly
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Backup files extra repository
        /// </summary>
        public BackupRepository ExtraRepository { get; set; }
    }

    public class ScheduleConfiguration
    {
        public ScheduleConfiguration()
        {
            Type = ScheduleType.Manual;
        }

        public ScheduleConfiguration(TimeSpan? period, IList<Day> daysOfWeek)
        {
            Type = ScheduleType.TimeInterval;
            Period = period;
            DaysOfWeek = daysOfWeek ?? new List<Day> { Day.Monday, Day.Tuesday, Day.Wednesday, Day.Thursday, Day.Friday, Day.Saturday, Day.Sunday };
        }

        public ScheduleConfiguration(TimeSpan? period, IList<Day> daysOfWeek, DateTime? timeFrom, DateTime? timeUpTo)
        {
            Type = ScheduleType.TimeInterval;
            Period = period;
            DaysOfWeek = daysOfWeek ?? new List<Day> { Day.Monday, Day.Tuesday, Day.Wednesday, Day.Thursday, Day.Friday, Day.Saturday, Day.Sunday };
            TimeFrom = timeFrom;
            TimeUpTo = timeUpTo;
        }

        public ScheduleType Type { get; set; }

        public TimeSpan? Period { get; set; }

        public IList<Day> DaysOfWeek { get; set; }

        public DateTime? TimeFrom { get; set; }

        public DateTime? TimeUpTo { get; set; }
    }

    public class GroupedBackupConfigPathInfoComparer : IEqualityComparer<GroupedBackupConfigPathInfo>
    {
        public bool Equals(GroupedBackupConfigPathInfo x, GroupedBackupConfigPathInfo y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            try
            {
                return string.Equals(
                           Path.GetFullPath(x.Path).TrimEnd(Path.DirectorySeparatorChar),
                           Path.GetFullPath(y.Path).TrimEnd(Path.DirectorySeparatorChar),
                           isWindows ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                       && x.IsDisabled == y.IsDisabled && x.ScheduleType == y.ScheduleType;
            }
            catch
            {
                return string.Equals(
                           x.Path?.TrimEnd(Path.DirectorySeparatorChar),
                           y.Path?.TrimEnd(Path.DirectorySeparatorChar),
                           isWindows ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                       && x.IsDisabled == y.IsDisabled && x.ScheduleType == y.ScheduleType;
            }
        }

        public int GetHashCode(GroupedBackupConfigPathInfo obj)
        {
            return obj.Path.GetHashCode();
        }
    }
}