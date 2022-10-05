namespace Comindware.Gateway.Api;

public class FileInfo
{
    public FileInfo(Directory directory, string path, string name)
    {
        StreamId = System.IO.Path.GetFileName(path);
        if (string.IsNullOrEmpty(StreamId)) throw new ArgumentException(path);

        Name = name;
        Directory = directory;
        Path = path;
        Length = new System.IO.FileInfo(path).Length;
        CreationTime = File.GetCreationTime(path);
    }

    public string StreamId { get; }
    public string Name { get; }
    public Directory Directory { get; set; }
    public string Path { get; set; }
    public long Length { get; set; }
    public DateTime CreationTime { get; set; }
}