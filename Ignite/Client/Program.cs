using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Transactions;

namespace Client
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var ignite = Ignition.StartClient(new IgniteClientConfiguration("localhost:10800"));
            // var ignite = Ignition.TryGetIgnite() ?? Ignition.Start();
            var cacheName = "___extensions_TeamNetwork_extension_n3.Forward.cmw_xmlns_httpwwww3org19990222-rdf-syntax-ns_type";
            var names = ignite.GetCacheNames();
            foreach (var name in names)
            {
            }
        }
    }
}