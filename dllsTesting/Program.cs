using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace dllsTesting
{
    internal class Program
    {
        private const string Istestmode = "IsTestMode";

        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable(Istestmode, "0");
            var exeFile = @"D:\dev\master2\Adapters\Adapter.AdapterHost\bin\Release\net6.0\Comindware.Adapter.AdapterHost.exe";
            var cfg = @"D:\dev\master2\Web\adapterhost.config";
            
            var startInfo = new ProcessStartInfo
            {
                FileName = @"D:\dev\master2\Adapters\Adapter.AdapterHost\bin\Release\net6.0\Comindware.Adapter.AdapterHost.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                Arguments = @"D:\dev\master2\Web\adapterhost.config",
                // EnvironmentVariables = { { Istestmode, "0" } },
            };
            var p = new System.Diagnostics.Process
            {
                StartInfo = startInfo
            };
            p.Start();
            
            StringBuilder q = new StringBuilder();
            while ( ! p.HasExited ) {
                q.Append(p.StandardOutput.ReadToEnd());
            }
            q.Append(p.StandardOutput.ReadToEnd());
            string r = q.ToString();
            

            Console.WriteLine(r);
        }
    }
}