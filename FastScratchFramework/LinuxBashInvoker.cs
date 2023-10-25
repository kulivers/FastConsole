using System;
using System.Diagnostics;
using System.IO;

namespace Comindware.Platform.Core.Util
{
    public class LinuxBashInvoker
    {
        private const string BashProgramName = "/bin/bash";

        public string Bash(string cmd, bool needOutput = true)
        {
            var escapedArgs = cmd.Replace("\"", "'");
            string logFile = $"/tmp/{Guid.NewGuid().ToString()}";
            using (var process = new Process()
                   {
                       StartInfo = new ProcessStartInfo
                       {
                           FileName = BashProgramName,
                           Arguments = needOutput ? $"-c \"{escapedArgs} > {logFile}\"" : $"-c \"{escapedArgs}\"",
                           RedirectStandardOutput = false,
                           UseShellExecute = false,
                           CreateNoWindow = true,
                       }
                   })
            {
                process.Start();
                process.WaitForExit();
                process.Close();
            }

            if (File.Exists(logFile))
            {
                var result = File.ReadAllText(logFile);
                try
                {
                    File.Delete(logFile);
                } catch { }

                return result.TrimEnd(Environment.NewLine.ToCharArray());
            }
            return string.Empty;
        }
    }
}