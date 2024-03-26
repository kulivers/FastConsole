using Confluent.Kafka;
using Confluent.Kafka.Admin;

class Program
{
    public static async Task Main(string[] args)
    {
        
        var topicNames = new List<string>()
        {
            "request_queue_DEV25_cmwdata_deploy_external",
            "reply_queue_DEV25_cmwdata_deploy_external",
            "request_queue_DEV25_cmwdata_outgoing_external",
            "reply_queue_DEV25_cmwdata_outgoing_external",
            "request_queue_DEV25_cmwdata_incoming_external",
            "reply_queue_DEV25_cmwdata_incoming_external",
        };
        var bootstrapServers = "10.9.7.66:9092";
        var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(15));
        foreach (var topic in metadata.Topics.Where(t=>t.Topic.Contains("DEV25")))
        {
            var topicName = topic.Topic;
            await adminClient.DeleteTopicsAsync(new[] { topicName });
        }
        return;
        
        async Task RecreateTopics(string bootstrapServers, string[] strings)
        {
            var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        
            var addBrokers = adminClient.AddBrokers(bootstrapServers);
        
            using (adminClient)
            {
                // var metadata = adminClient.GetMetadata("requestTopic", new TimeSpan(310310));
        
                await adminClient.DeleteTopicsAsync(new[] { "requestTopic" });
                // metadata = adminClient.GetMetadata("requestTopic", new TimeSpan(310310));
        
                // await DeleteTopics(adminClient, strings);
                await CreateTopics(strings, adminClient);
            }
        }
        
        async Task DeleteTopics(string[] strings1)
        {
            var adminClient1 = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        
            try
            {
                foreach (var s in strings1)
                    await adminClient1.DeleteTopicsAsync(new[] { s },
                        new DeleteTopicsOptions { RequestTimeout = TimeSpan.FromSeconds(5) });
            }
            catch
            {
                // ignored
            }
        }
        
        async Task CreateTopics(string[] strings1, IAdminClient adminClient1, short brokers = 3)
        {
            try
            {
                foreach (var topicName in strings1)
                    await adminClient1.CreateTopicsAsync(new TopicSpecification[]
                        { new() { Name = topicName, ReplicationFactor = brokers, NumPartitions = 5 } });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}

public enum Fuck
{
    one,
    two
}