namespace FastConsole.models;

public class TeamcityTestsResult
{
    public int Order { get; set; }
    public string TestName { get; set; }
    public string Status { get; set; }

    public string Duration { get; set; }
    // Order,TestName,Status,Duration
}