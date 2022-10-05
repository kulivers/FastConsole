public class Person 
{
    public int Age { get; set; }
    public string Name { get; set; }
    public List<int> ints { get; set; }

    public Person(int age, string name)
    {
        Age = age;
        Name = name;
        Longs = new List<long>() { 123131132 };
    }

    public List<long> Longs { get; set; }

    public Person()
    {
    }

    public override bool Equals(object? obj)
    {
        var person = obj as Person;
        if (person == null)
        {
            return false;
        }

        return person.Age == Age && person.Name == Name;
    }
}