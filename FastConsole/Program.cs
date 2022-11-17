using System.Reflection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var data = new Dictionary<string, object>();
        var person = new Person() { Age = 1, Sex = Sex.Women, Item = new Item() { SubItem = new SubItem() { Name = "xyu" } } };
        var propertyInfos = person.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var info in propertyInfos)
        {
            var name = info.Name;
            var value = info.GetValue(person);
            data.Add(name, value);
        }
        var fieldInfos = person.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var info in fieldInfos)
        {
            var name = info.Name;
            var value = info.GetValue(person);
            data.Add(name, value);
        }

        var p2 = new Person();
        var properties = p2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties)
        {
            data.TryGetValue(property.Name, out var value);
            property.SetValue(p2, value);
        }

        var fields = p2.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var field in fields)
        {
            data.TryGetValue(field.Name, out var value);
            field.SetValue(p2, value);
        }
    }
}


public class Person
{
    public int Age { get; set; } 
    public Sex Sex { get; set; }
    public Item Item { get; set; }
}

public class Item
{
    public SubItem SubItem { get; set; }
}

public class SubItem
{
    public string Name;
}

public enum Sex
{
    Men,
    Women
}