using System.Runtime.Serialization;

public struct Person
{
    public Person(int age, string name)
    {
        Age = age;
        Name = name;
        Longs = new List<long> { 123131132 };
        Ints = new List<int>() { 131 };
    }
    

    [DataMember(Name = nameof(Age))]
    public int Age { get; set; }
    [DataMember(Name = nameof(Name))]
    public string Name { get; set; }
    
    [DataMember(Name = nameof(Ints))]
    public List<int> Ints { get; set; }

    [DataMember(Name = nameof(Longs))]
    public List<long> Longs { get; set; }
    
}