using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Event;
using Apache.Ignite.Core.Cluster;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Messaging;
using Apache.Ignite.Core.Resource;
using Apache.Ignite.Core.Transactions;

namespace Node
{
    public class Listener : IMessageListener<string>
    {
        public bool Invoke(Guid nodeId, string message)
        {
            Console.WriteLine("rrrecieved: " + message);
            return true;
        }
    }

    internal class Program
    {
        private static void StartIgniteNode()
        {
            if (!string.IsNullOrEmpty(IgniteWorkingDirectory))
            {
                Environment.SetEnvironmentVariable("IGNITE_WORK_DIR", IgniteWorkingDirectory);
            }

            if (!string.IsNullOrEmpty(IgniteBinDirectory))
            {
                Environment.SetEnvironmentVariable("IGNITE_HOME", IgniteBinDirectory);
            }
            Environment.SetEnvironmentVariable("IGNITE_NO_ASCII", "true");
            Environment.SetEnvironmentVariable("IGNITE_QUIET", "true");

            var ignite = Ignition.TryGetIgnite() ?? Ignition.StartFromApplicationConfiguration("igniteConfiguration", IgniteConfigFileName);
        }

        public static string IgniteBinDirectory => @"D:\Work\master\Web\bin";

        public static string IgniteWorkingDirectory => @"D:\Work\master\Web\data\Database";

        public static string IgniteConfigFileName => @"D:\Work\master\Web\Ignite.config";

        public static async Task Main(string[] args)
        {
            StartIgniteNode();
        }
    }
}