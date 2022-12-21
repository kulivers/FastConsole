class Program
{
    public static async Task Main(string[] args)
    {
        var esUrl = "http://localhost:9200";
        var esManager = new EsManager(esUrl);
        esManager.DeleteIndexes(esManager.GetIndexNames());
    }
}