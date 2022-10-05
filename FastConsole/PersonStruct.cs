public struct PersonStruct  
{
    public int Age { get; set; }
    public string Name { get; set; }

    public PersonStruct(int age, string name)
    {
        Age = age;
        Name = name;
    }

}