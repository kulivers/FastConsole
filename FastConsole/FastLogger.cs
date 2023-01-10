using System.Text;

public static class FastLogger
{
    private static bool _inited;
    public static readonly string Path;

    static FastLogger()
    {
        Path = CreateFile();
        _inited = true;
    }

    public static void Log(string log)
    {
        log = $"{log}\n";
        using var file = File.Open(Path, FileMode.Open, FileAccess.ReadWrite);
        var buffer = new byte[file.Length];

        while (file.Read(buffer, 0, buffer.Length) != 0)
        {
        }

        if (!file.CanWrite)
            throw new ArgumentException("The specified file cannot be written.", "file");

        file.Position = 0;
        var data = Encoding.Unicode.GetBytes(log);
        file.SetLength(buffer.Length + data.Length);
        file.Write(buffer, 0, buffer.Length);
        file.Write(data, 0, data.Length);
    }

    private static string CreateFile()
    {
        var name = DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".txt";
        var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), name);
        File.Create(path).Dispose();
        return path;
    }
}