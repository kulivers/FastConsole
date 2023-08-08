internal class Program
{
    public static void Main(string[] args)
    {
        var instanceYmlModel = new InstanceYmlModel()
        {
            IsFederationAuthEnabled = 1,
            BackupPath = "/var/lib/comindware/Backup",
            DatabasePath = "/var/lib/comindware/Database",
            TempPath = "/var/lib/comindware/Temp",
            StreamsPath = "/var/lib/comindware/Streams",
            ConfigPath = "/var/www/comindware",
            ElasticsearchUri = "http://localhost:9200"
        };
        var serialize = PlatformYmlSerializer.Serialize(instanceYmlModel);
    }
}