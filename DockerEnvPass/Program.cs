// See https://aka.ms/new-console-template for more information

using YamlDotNet.Serialization;

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
        try
        {
            var user1 = new User() { Name = "1231" };
            var s = yml2(user1);
            var path = args[0];
            Console.WriteLine(File.ReadAllText(path));
            Console.WriteLine("args[0] " + path);
            var user = yml(path);
            Console.WriteLine("user.Name " + user.Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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