using Confluent.Kafka;
using Confluent.Kafka.Admin;

var bootstrapServers = "192.168.0.127:9092";


var topicNames = new[] { "requestTopic", "responseTopic", };
var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers, }).Build();

var meta = adminClient.GetMetadata("requestTopic", new TimeSpan(1231232131));

// await adminClient.DeleteTopicsAsync(new[] { "requestTopic" });


var topicSpecifications = new TopicSpecification[] { new() { Name = "requestTopic", ReplicationFactor = 1, NumPartitions = 3 } };
await adminClient.CreateTopicsAsync(topicSpecifications, new CreateTopicsOptions(){RequestTimeout = TimeSpan.FromSeconds(10)});


await adminClient.DeleteTopicsAsync(new[] { "requestTopic" });

try
{
    await adminClient.CreateTopicsAsync(new[]
        { new TopicSpecification() { Name = "requestTopic", NumPartitions = 7 } });
}
catch
{
}

return;
await RecreateTopics(bootstrapServers, topicNames);

async Task RecreateTopics(string bootstrapServers, string[] strings)
{
    var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers, }).Build();
    
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

async Task DeleteTopics(IAdminClient adminClient1, string[] strings1)
{
    try
    {
        foreach (var s in strings1)
        {
            await adminClient1.DeleteTopicsAsync(new[] { s },
                new DeleteTopicsOptions() { RequestTimeout = TimeSpan.FromSeconds(5) });
        }
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
        {
            await adminClient1.CreateTopicsAsync((new TopicSpecification[]
                { new() { Name = topicName, ReplicationFactor = brokers, NumPartitions = 5 } }));
        }
    }
    catch (CreateTopicsException e)
    {
        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    }
}