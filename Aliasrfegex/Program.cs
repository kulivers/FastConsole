using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Aliasrfegex
{
    public class ForcedPlatformConfigModel
    {
        public string DatabasePath { get; set; }
        public string TempPath { get; set; }
        public string StreamsPath { get; set; }
        public string LogsPath { get; set; }
        public string ElasticsearchUri { get; set; }
        public string KafkaBootstrapServer { get; set; }
        public string KafkaGroupId { get; set; }
    }
    internal class Program
    {
        private static readonly Regex AliasRegex = new Regex(@"^[_A-ZА-Я][_0-9A-ZА-Я]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string BuildAliasFromName(string name)
        {
            var translitedName = TextUtils.Translit(name);
            var onlySymbolsNDigits = new Regex(@"[^\p{L}\p{N}]+").Replace(translitedName, "");
            var noDigitsOnStart = Regex.Replace(onlySymbolsNDigits, @"^\d+", "");
            return noDigitsOnStart;
        }

        public static void Main(string[] args)
        {
        }
    }
}