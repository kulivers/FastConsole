public class InstanceYmlModel
{
    public InstanceYmlModel()
    {
    }

    public InstanceYmlModel(InstanceYmlModel other)
    {
        ConfigName = other.ConfigName;
        IsWindowsAuthorization = other.IsWindowsAuthorization;
        Version = other.Version;
        IsFederationAuthEnabled = other.IsFederationAuthEnabled;
        ConfigPath = other.ConfigPath;
        ElasticsearchUri = other.ElasticsearchUri;
        BackupPath = other.BackupPath;
        DatabasePath = other.DatabasePath;
        TempPath = other.TempPath;
        StreamsPath = other.StreamsPath;
        LogsPath = other.LogsPath;
        Path = other.Path;
        Port = other.Port;
        Fqdn = other.Fqdn;
        IsEnabledIisLogs = other.IsEnabledIisLogs;
        WebSiteName = other.WebSiteName;
        Description = other.Description;
        Creator = other.Creator;
        IsEnabled = other.IsEnabled;
        InstancePath = other.InstancePath;
        InstanceWebSitePath = other.InstanceWebSitePath;
        IsRegistered = other.IsRegistered;
        StatusWebSite = other.StatusWebSite;
        WalPath = other.WalPath;
        IsWalDisabled = other.IsWalDisabled;
        InstanceName = other.InstanceName;
    }

    public string ConfigName;
    public bool? IsWindowsAuthorization;
    public Version Version;
    public int? IsFederationAuthEnabled;
    public string ConfigPath;
    public string ElasticsearchUri;
    public string BackupPath;
    public string DatabasePath;
    public string TempPath;
    public string StreamsPath;
    public string LogsPath;
    public string InstanceName;

    //unused, but deserializer need this
    public string Path;
    public int Port;
    public string Fqdn;
    public bool IsEnabledIisLogs;
    public string WebSiteName;
    public string Description;
    public string Creator;
    public bool IsEnabled;
    public string InstancePath;
    public string InstanceWebSitePath;
    public bool IsRegistered;
    public bool StatusWebSite;
    public string WalPath;
    public bool IsWalDisabled;

    public bool Equals(InstanceYmlModel other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ConfigName == other.ConfigName && InstanceName == other.InstanceName;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((InstanceYmlModel)obj);
    }
}