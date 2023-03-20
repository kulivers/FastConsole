using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    internal class Program
    {
        public static IEnumerable<string> SAA()
        {
            yield break;
        }
        public static void Main(string[] args)
        {
            var obsolete = new ObsoleteInstance()
            {
                BackupsDir = @"/var/www/comindware/Backup",
                ConfigPath = @"/var/www/comindware/",
                DatabaseDir = @"/var/www/comindware/data",
                TempDir = @"/var/www/comindware/Temp",
                StreamsDir = @"/var/www/comindware/Streams"
                
            };
            var serialize = YAMLHelper.Serialize(obsolete);
            File.WriteAllText(@"C:\Users\ekul\Desktop\cmwdata.yml", serialize);
        }
    }
}