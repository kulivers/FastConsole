namespace FastConsole;

public class Response
{
    public TestOccurrence[] testOccurrence { get; set; }
}

public class TestOccurrence
{
    public string id { get; set; }
    public string name { get; set; }
    public string status { get; set; }
    public int duration { get; set; }
    public string runOrder { get; set; }
    public bool newFailure { get; set; }
    public bool muted { get; set; }
    public bool currentlyMuted { get; set; }
    public bool currentlyInvestigated { get; set; }
    public Test test { get; set; }
    public Build build { get; set; }
    public Metadata metadata { get; set; }
    public string logAnchor { get; set; }
}

public class Test
{
    public string id { get; set; }
    public Mutes mutes { get; set; }
    public Investigations investigations { get; set; }
    public ParsedTestName parsedTestName { get; set; }
}

public class Mutes
{
    public object[] mute { get; set; }
}

public class Investigations
{
    public object[] investigation { get; set; }
}

public class ParsedTestName
{
    public string testPackage { get; set; }
    public string testSuite { get; set; }
    public string testClass { get; set; }
    public string testShortName { get; set; }
    public string testNameWithoutPrefix { get; set; }
    public string testMethodName { get; set; }
    public string testNameWithParameters { get; set; }
}

public class Build
{
    public int id { get; set; }
    public string buildTypeId { get; set; }
    public bool personal { get; set; }
    public BuildType buildType { get; set; }
    public string startDate { get; set; }
    public Agent agent { get; set; }
}

public class BuildType
{
    public string internalId { get; set; }
    public Project project { get; set; }
}

public class Project
{
    public string id { get; set; }
    public string parentProjectId { get; set; }
    public bool Virtual { get; set; }
}

public class Agent
{
    public int id { get; set; }
    public string name { get; set; }
}

public class Metadata
{
    public int count { get; set; }
}

