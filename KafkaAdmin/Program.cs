using Confluent.Kafka;
using Confluent.Kafka.Admin;

class Program
{
    public static async Task Main(string[] args)
    {
        var bootstrapServers = "10.9.7.13:9092";
        var topicName = "adapterhost_deploy_request_queue";
        var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromDays(1));
        var configResource = new ConfigResource() { Name = topicName, Type = ResourceType.Topic };
        var configEntries = new List<ConfigEntry>()
        {
            new ConfigEntry() { Name = "retention.ms", Value = (-1).ToString() },
            new ConfigEntry() { Name = "max.message.bytes", Value = (104857600).ToString() },//время на удаление сообщений
            new ConfigEntry() { Name = "max.message.bytes", Value = (20971520).ToString() },
        };
        var dictionary = new Dictionary<ConfigResource, List<ConfigEntry>>() { { configResource, configEntries } };
        // await adminClient.AlterConfigsAsync(dictionary);
        var listGroups = adminClient.ListGroups(TimeSpan.FromSeconds(3));
        
        await adminClient.DeleteTopicsAsync(new[] { topicName });
        var topicSpecifications = new TopicSpecification[] { new() { Name = topicName, NumPartitions = 20 } };

        await adminClient.CreateTopicsAsync(topicSpecifications);
        ;
        // await DeleteTopics(new[] { TopicName });
        // var topicSpecifications = new List<TopicSpecification>() { new() { Name = "coolTopic", NumPartitions = 100 } };
        // await adminClient.CreateTopicsAsync(topicSpecifications);

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