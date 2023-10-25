using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Comindware.Platform.Core;

namespace FastScratchMVC
{
    public class Person
    {
        private string _name;

        public string Name
        {
            get
            {
                Console.WriteLine("Im in getter");
                return _name;
            }
            set
            {
                Console.WriteLine("Im in setter");
                _name = value;
            }
        }
    }

    public class SimpleAssemblyLoader : MarshalByRefObject
    {
        public void Load(string path)
        {
            ValidatePath(path);

            Assembly.Load(path);
        }

        public void LoadFrom(string path)
        {
            ValidatePath(path);

            Assembly.LoadFrom(path);
        }

        private void ValidatePath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!File.Exists(path))
                throw new ArgumentException(String.Format("path \"{0}\" does not exist", path));
        }
    }

    [Flags]
    public enum ResourcePermissions
    {
        Undefined,

        ContextUserExpression = 0x01, //Разрешено применение expression
        ContextObjectCondition = 0x02, //Разрешено применение condition
        Read = 0x04, // Просматривать контейнер и его объекты (записи, процессы, задачи)
        Create = 0x08, // Создавать объекты в контейнере (записи, процессы, задачи)
        Update = 0x40, // Редактировать объекты в контейнере (записи, процессы, задачи)
        Delete = 0x80, // Удалять объекты в контейнере (записи, процессы, задачи)
        Execute = 0x100, // Выполнение конкретной операции
        FullAccess = 0x200, // Все права на объекты + изменение шаблона (удалять шаблон - не дает)
    }

    internal class Program
    {
        private static string bashProgramName = "/bin/bash";

        public static void Main(string[] args)
        {
            var validator = new BackupPathValidator();
            var pathResolver = new SymbolicPathResolver();
            var tempDir = "/home/ekul/tempDir";
            var defaultBackupPath = "/home/ekul/backups";
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            if (!Directory.Exists(defaultBackupPath))
            {
                Directory.CreateDirectory(defaultBackupPath);
            }
            
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

public class BackupConfigPathComparer : IEqualityComparer<BackupConfiguration>
{
    public bool Equals(BackupConfiguration x, BackupConfiguration y)
    {
        if (x != null && y != null)
        {
            return Path.GetFullPath(x.Path).Equals(Path.GetFullPath(y.Path));
        }

        return x == null && y == null;
    }

    public int GetHashCode(BackupConfiguration obj)
    {
        return obj.Path.GetHashCode();
    }
}

public class BackupConfiguration
{
    /// <summary>
    /// Backup configuration identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Backup folder location
    /// </summary>
    public string Path { get; set; }

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
    /// Backup description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Launch schedule
    /// </summary>
    /// <summary>
    /// Number of backup archives which wouldn't be deleted automatically
    /// </summary>
    public uint KeepRecent { get; set; }

    /// <summary>
    /// If config is disabled then backup will not be run neither manually nor by schedule
    /// </summary>
    public bool IsDisabled { get; set; }
}