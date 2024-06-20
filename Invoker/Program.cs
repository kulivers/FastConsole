using System.ComponentModel.DataAnnotations;
using Comindware.Configs.Core;

namespace Invoker
{
    public class Person
    {
        [Required()]
        public string Name { get; set; }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var localizableAttribute = new System.ComponentModel.LocalizableAttribute(true);
            var configManager = new ConfigManager();
            var platformConfig = ConfigManager.Create("D:\\dev\\master2\\Web\\Web.csproj");
        }
    }
}