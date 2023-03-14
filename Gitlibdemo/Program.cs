using System;
using System.IO;
using LibGit2Sharp;

namespace Gitlibdemo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");

                if (!Directory.Exists(testDir))
                {
                    Directory.CreateDirectory(testDir);
                }

                var cloneOptions = new CloneOptions()
                {
                    BranchName = "main",
                    CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                    {
                        Username = "kulivers2",
                        Password = "xotourlife9991"
                    }
                };
                Repository.Clone("https://gitlab.com/test8643011/testpublic.git", testDir, cloneOptions);
            }
            catch (Exception e)
            {
                Console.WriteLine("========================++++++++++++++++++++++++++++++++++++++++++++++++++++++++========================");
                Console.WriteLine($"e.Message: {e.Message}");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"e.InnerException?.Message: {e.InnerException?.Message}");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"e.StackTrace: {e.StackTrace}");
                Console.WriteLine("========================++++++++++++++++++++++++++++++++++++++++++++++++++++++++========================");
            }
        }
    }
}