using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Gitlibdemo
{
    internal class Program
    {
        private static void ClearDir(string directoryPath)
        {
            try
            {
                // Delete all files within the directory
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                // Delete all subdirectories within the directory
                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    subdirectory.Delete(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error clearing directory: " + e.Message);
            }
        }

        private static void CoutPaths(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Console.WriteLine(path);
            }
        }

        private static void coutPaths(IEnumerable<IEnumerable<string>> paths)
        {
            foreach (var path in paths)
            {
                CoutPaths(path);
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("im upadated");
            try
            {
                var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
                var testDir2 = Path.Combine(testDir, "test");

                if (!Directory.Exists(testDir))
                {
                    Directory.CreateDirectory(testDir);
                }

                if (!Directory.Exists(testDir2))
                {
                    Directory.CreateDirectory(testDir2);
                }
                else
                {
                    ClearDir(testDir);
                }

                var cloneOptions = new CloneOptions()
                {
                    BranchName = "main",
                    CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                    {
                        Username = "kulivers2",
                        Password = "xotourlife9991",
                    },
                };
                cloneOptions.CertificateCheck += delegate { return true; };
                var fo = new FetchOptions();
                fo.CertificateCheck += delegate { return true; };
                Console.WriteLine("clonning to "+ testDir2);
                Repository.Clone("https://gitlab.com/test8643011/testpublic.git", testDir2, cloneOptions);
            }
            catch (Exception e)
            {
                Console.WriteLine("========================++++++++++++++++++++++++++++++++++++++++++++++++++++++++========================");
                Console.WriteLine($"e: {e}");   
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
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