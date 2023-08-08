using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avro;

namespace FastScratchMVC
{
    public interface Guy{}
    public class Person : Guy
    {
        public string Name { get; set; }
    }
    
    public class BigPerson : Guy
    {
        public string Name { get; set; }
    }
    

    internal class Program
    {
        private static readonly ConcurrentDictionary<string, int> SessionTokensCache = new ConcurrentDictionary<string, int>();
        public static void Main()
        {
            var guys = new List<Guy>(){new Person(), new BigPerson()};
            var bigPersons = guys.Cast<BigPerson>().ToArray();
        }
    }
}