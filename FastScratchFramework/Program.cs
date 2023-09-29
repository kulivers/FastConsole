using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Aspose.Cells;
using Comindware.Bootloading.Core.Configuration.Models;

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
            if (!System.IO.File.Exists(path))
                throw new ArgumentException(String.Format("path \"{0}\" does not exist", path));
        }   
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);
            var fullPath = Path.GetFullPath(dir);
            Console.WriteLine(fullPath);
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
}