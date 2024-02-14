using System;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

namespace DiskMemoryEater
{
    public class User
    {
        public string Name { get; set; }
    }

    internal class Program
    {
        // [DllImport("libc")]
        // private static extern void abort();

        static void Main(string[] args)
        {
            var user1 = new User(){Name = "1231"};
            var s = yml2(user1);
            Console.WriteLine("args[0] " + args[0]);
            var user = yml(args[0]);
            Console.WriteLine("user.Name " + user.Name);
        }

        public static User yml(string path)
        {
            return new Deserializer().Deserialize<User>(path);
        }
        public static string yml2(User path)
        {
            return new Serializer().Serialize(path);
        }
    }
}