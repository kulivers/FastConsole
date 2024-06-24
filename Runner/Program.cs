using System.IO;
using Comindware.Bootloading.Core.Configuration;
using Comindware.Bootloading.Core.Configuration.Entities;
using Comindware.Configs.Core;

namespace Runner
{
    internal class Program
    {
        private const string Path = @"D:\dev\other\FastConsole\FastConsole\mock.yml";
        private const string ToWrite = @"D:\dev\other\FastConsole\Runner\aa.yml";

        public static void Main(string[] args)
        {
            File.Copy(Path, ToWrite, true);
            var content = File.ReadAllText(Path);
            var platformInstanceModel = new PlatformInstanceModel()
            {
                // ConfigName = "322",
                // ElasticsearchUri = "1",
                Mq = new PlatformMessageQueueModel()
                {
                    // Server = "LIL UZI XYEVERT", 
                    Name = "XYEYM",
                    Array = new[] { 2, 3 } 
                    
                },
                // Array = new[] { 1, 2, 3 }
            };


            
            if (File.Exists(ToWrite))
            {
                // File.Delete(ToWrite);
            }

            // var rewriteContent = new ConfigChanger().RewriteContent(File.ReadAllText(ToWrite), new YamlFieldCollection());
            PlatformYmlSerializer.WriteToFile(ToWrite, platformInstanceModel);
            // var reborn = PlatformYmlSerializer.ReadContent<PlatformInstanceModel>(File.ReadAllText(ToWrite));
        }
    }
}