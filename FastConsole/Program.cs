using System.IO.MemoryMappedFiles;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

public struct Person
{
    public int Age { get; set; }

    public Person(int age)
    {
        Age = age;
    }
}
class Program
{
    public static async Task Main(string[] args)
    {
        var parentArray = new[] {"cmw","xmlns:","http://www.w3.org/1999/02/22-rdf-syntax-ns#","type"};
        var foo = new string[2] { parentArray[0], string.Concat(parentArray.Skip(1)) };
        var person = new Person(321);
        var size = System.Runtime.InteropServices.Marshal.SizeOf(person);
        long offset = 0x10000000; // 256 megabytes
        long length = 0x20000000; // 512 megabytes
        using var mmf = MemoryMappedFile.CreateOrOpen(@"ra", size);
        // Create a random access view, from the 256th megabyte (the offset)
        // to the 768th megabyte (the offset plus length).
        using var accessor = mmf.CreateViewAccessor(offset, length);
        // Make changes to the view.
        for (long i = 0; i < length; i += size)
        {
            accessor.Write(i, ref person);
        }
    }
}
public static class Utils
{
    public static int SizeOf<T>(T obj)
    {
        return SizeOfCache<T>.SizeOf;
    }

    private static class SizeOfCache<T>
    {
        public static readonly int SizeOf;

        static SizeOfCache()
        {
            var dm = new DynamicMethod("func", typeof(int),
                Type.EmptyTypes, typeof(Utils));

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, typeof(T));
            il.Emit(OpCodes.Ret);

            var func = (Func<int>)dm.CreateDelegate(typeof(Func<int>));
            SizeOf = func();
        }
    }
}


public struct MyColor
{
    public short Red;
    public short Green;
    public short Blue;
    public short Alpha;

    // Make the view brighter.
    public void Brighten(short value)
    {
        Red = (short)Math.Min(short.MaxValue, (int)Red + value);
        Green = (short)Math.Min(short.MaxValue, (int)Green + value);
        Blue = (short)Math.Min(short.MaxValue, (int)Blue + value);
        Alpha = (short)Math.Min(short.MaxValue, (int)Alpha + value);
    }
}