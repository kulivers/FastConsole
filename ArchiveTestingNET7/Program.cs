// See https://aka.ms/new-console-template for more information

using System.IO.Compression;

internal class Program
{
    public static void Main(string[] args)
    {
        var path = "D:\\Playground\\Garbage\\FastConsole\\DiskSpaceTesting\\test.cdbbz";
        var path2 = "D:\\Playground\\Garbage\\FastConsole\\DiskSpaceTesting\\test3.cdbbz";
        using (var fileStream = new FileStream(path2, FileMode.Open))
        {
            var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read);
            var readOnlyCollection = zipArchive.Entries;
        }
    }
}