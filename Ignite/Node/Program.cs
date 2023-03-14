using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Event;
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
        public static async Task Main(string[] args)
        {
            var cacheName = "___extensions_TeamNetwork_extension_n3.Forward.cmw_xmlns_httpwwww3org19990222-rdf-syntax-ns_type";
            var ignite = Ignition.TryGetIgnite() ?? Ignition.Start();
            var names = ignite.GetCacheNames();
            foreach (var name in names)
            {
                var cache = ignite.GetCache<dynamic, dynamic>(name);
            }
            // var cache = ignite.GetCache<int, int>(cacheName);//No
        }
    }
}