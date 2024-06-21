namespace Comindware.Bootloading.Core.Configuration.Entities;

public class PlatformInstanceModel
{
    public PlatformInstanceModel()
    {
    }
    
    
    public string ConfigName;
    public string ElasticsearchUri;
    public MessageQueueModel Mq { get; set; }
}
public class PlatformInstanceModel2
{
    public string DatabaseName;
    public string ConfigName;
    public bool? IsWindowsAuthorization;
    public bool? IsLinuxSSOAuthorization;
    public bool? ManageAdapterHost = true;
    public int? LdapAuthenticationType;
    public int? LinuxAuthenticationType;
    public Version Version;
    public int? IsFederationAuthEnabled;
    public string ConfigPath;
    public string KafkaBootstrapServer;
    public string KafkaGroupId;
    public string KafkaExclusiveGroupId;
    public string KafkaQueuePrefix;
    public string DatabasePath;
    public string LogsPath;
    public string InstanceName;
    public bool? IsTestEnvironment;
    public string ElasticsearchUri;
    public PlatformMessageQueueModel Mq { get; set; }

    //unused, but deserializer need this
    public string Path;
    public int? Port;
    public string Fqdn;
    public bool? IsEnabledIisLogs;
    public string WebSiteName;
    public string Description;
    public string Creator;
    public bool? IsEnabled;
    public string InstancePath;
    public string InstanceWebSitePath;
    public bool? IsRegistered;
    public bool? StatusWebSite;
    public string WalPath;
    public bool? IsWalDisabled;
    public string TempPath;
    public string StreamsPath;
    public string BackupPath;
    public string TempWorkingDir;
}
public class MessageQueueModel
{
    public bool? Enabled { get; set; }

    public string Server { get; set; }

    public string Name { get; set;  }

    public string Group { get; set;  }

    public string Node { get; set; }
}
public class PlatformMessageQueueModel : MessageQueueModel
{
    public Dictionary<int, DataChannelModel> Adapter { get; set; }
}
public class DataChannelModel
{
    public bool? Enabled { get; set; }
    public ProducerModel Producer { get; set; }
    public ConsumerModel Consumer { get; set; }

    public DataChannelModel() { }
    public DataChannelModel(DataChannelModel other)
    {
        if (other == null)
        {
            return;
        }

        Enabled = other.Enabled;
        Producer = new ProducerModel(other.Producer);
        Consumer = new ConsumerModel(other.Consumer);
    }
}
public class ProducerModel : QueueClientModel
{
    public ProducerModel() { }
    public ProducerModel(ProducerModel other) : base(other) { }
}
public class QueueClientModel
{
    public bool? Enabled { get; set; }

    public QueueClientModel() { }
    public QueueClientModel(QueueClientModel other)
    {
        Enabled = other.Enabled;
    }
}
public class ConsumerModel : QueueClientModel
{
    public ConsumerModel() { }
    public ConsumerModel(ConsumerModel other) : base(other) { }
}