namespace FastConsole.models;

public class CloudConfig
{
    public InstanceModel Instance { get; set; }
    public TemplateModel Template { get; set; }
    public ProductModel Product { get; set; }

    public CloudConfig()
    {
    }

    public CloudConfig(InstanceModel instance, TemplateModel template, ProductModel product)
    {
        Instance = instance;
        Template = template;
        Product = product;
    }
}

public class InstanceModel : IEquatable<InstanceModel>
{
    public InstanceModel()
    {
    }

    public InstanceModel(InstanceModel other)
    {
        Name = other.Name;
        Port = other.Port;
        Path = other.Path;
        FQDN = other.FQDN;
        IsEnabledIisLogs = other.IsEnabledIisLogs;
        IsWindowsAuthorization = other.IsWindowsAuthorization;
        Version = other.Version;
        WebSiteName = other.WebSiteName;
        Description = other.Description;
        Creator = other.Creator;
        IsEnabled = other.IsEnabled;
        InstancePath = other.InstancePath;
        InstanceWebSitePath = other.InstanceWebSitePath;
        IsRegistered = other.IsRegistered;
        IsFederationAuthEnabled = other.IsFederationAuthEnabled;
        StatusWebSite = other.StatusWebSite;
        ConfigPath = other.ConfigPath;
        BackupPath = other.BackupPath;
        DatabasePath = other.DatabasePath;
        TempPath = other.TempPath;
        StreamsPath = other.StreamsPath;
        WalPath = other.WalPath;
        IsWalDisabled = other.IsWalDisabled;
        LogsPath = other.LogsPath;
    }

    public string Name { get; set; }
    public int? Port { get; set; }
    public string FQDN { get; set; }
    public bool? IsEnabledIisLogs { get; set; }
    public bool? IsWindowsAuthorization { get; set; }
    public Version Version { get; set; }
    public string WebSiteName { get; set; }
    public string Description { get; set; }
    public string Creator { get; set; }
    public bool? IsEnabled { get; set; }
    public string InstancePath { get; set; }
    public string Path { get; set; }
    public string InstanceWebSitePath { get; set; }
    public bool? IsRegistered { get; set; } = true;
    public int? IsFederationAuthEnabled { get; set; } = 0;
    public bool? StatusWebSite { get; set; }
    public string ConfigPath { get; set; }
    public string BackupPath { get; set; }
    public string DatabasePath { get; set; }
    public string TempPath { get; set; }
    public string StreamsPath { get; set; }
    public string WalPath { get; set; }
    public bool? IsWalDisabled { get; set; }
    public string LogsPath { get; set; }
    public string ScriptsPath { get; set; }

    public bool Equals(InstanceModel other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name &&
               Port == other.Port &&
               FQDN == other.FQDN &&
               IsEnabledIisLogs == other.IsEnabledIisLogs &&
               IsWindowsAuthorization == other.IsWindowsAuthorization &&
               Equals(Version, other.Version) &&
               WebSiteName == other.WebSiteName &&
               Description == other.Description &&
               Creator == other.Creator &&
               IsEnabled == other.IsEnabled &&
               InstancePath == other.InstancePath &&
               Path == other.Path &&
               InstanceWebSitePath == other.InstanceWebSitePath &&
               IsRegistered == other.IsRegistered &&
               IsFederationAuthEnabled == other.IsFederationAuthEnabled &&
               StatusWebSite == other.StatusWebSite &&
               ConfigPath == other.ConfigPath &&
               BackupPath == other.BackupPath &&
               DatabasePath == other.DatabasePath &&
               TempPath == other.TempPath &&
               StreamsPath == other.StreamsPath &&
               WalPath == other.WalPath &&
               IsWalDisabled == other.IsWalDisabled &&
               LogsPath == other.LogsPath && ScriptsPath == other.ScriptsPath;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((InstanceModel)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Port.GetHashCode();
            hashCode = (hashCode * 397) ^ (FQDN != null ? FQDN.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsEnabledIisLogs.GetHashCode();
            hashCode = (hashCode * 397) ^ IsWindowsAuthorization.GetHashCode();
            hashCode = (hashCode * 397) ^ (Version != null ? Version.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (WebSiteName != null ? WebSiteName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Creator != null ? Creator.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsEnabled.GetHashCode();
            hashCode = (hashCode * 397) ^ (InstancePath != null ? InstancePath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (InstanceWebSitePath != null ? InstanceWebSitePath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsRegistered.GetHashCode();
            hashCode = (hashCode * 397) ^ IsFederationAuthEnabled.GetHashCode();
            hashCode = (hashCode * 397) ^ StatusWebSite.GetHashCode();
            hashCode = (hashCode * 397) ^ (ConfigPath != null ? ConfigPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (BackupPath != null ? BackupPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (DatabasePath != null ? DatabasePath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (TempPath != null ? TempPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (StreamsPath != null ? StreamsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (WalPath != null ? WalPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsWalDisabled.GetHashCode();
            hashCode = (hashCode * 397) ^ (LogsPath != null ? LogsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ScriptsPath != null ? ScriptsPath.GetHashCode() : 0);
            return hashCode;
        }
    }
}

public class TemplateModel : IEquatable<TemplateModel>
{
    public string ProductDir { get; set; }
    public string MsiLogsPath { get; set; }
    public string LogsPath { get; set; }
    public string DiagnosticsPath { get; set; }
    public string BackupDescription { get; set; }
    public string BackupPath { get; set; }
    public string BackupFileName { get; set; }
    public int? CountDaysBackupSessionFilter { get; set; }
    public string ElasticsearchPath { get; set; }
    public string ElasticsearchUri { get; set; }
    public string JavaDir { get; set; }
    public string KibanaDir { get; set; }
    public string InstanceDir { get; set; }
    public string InstanceLogsPath { get; set; }
    public string InstanceStreamsPath { get; set; }
    public Version IisVersion { get; set; }
    public bool? IisChecked { get; set; }
    public bool? IisLogsEnabled { get; set; }
    public string Language { get; set; }
    public int? Port { get; set; }
    public string Fqdn { get; set; }
    public string TempPath { get; set; }
    public int? FrameworkDotNetVersion { get; set; }
    public string Dotnet60Version { get; set; }
    public string ProductType { get; set; }
    public bool? IsWalDisabled { get; set; }

    public bool Equals(TemplateModel other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ProductDir == other.ProductDir &&
               MsiLogsPath == other.MsiLogsPath &&
               LogsPath == other.LogsPath &&
               DiagnosticsPath == other.DiagnosticsPath &&
               BackupDescription == other.BackupDescription &&
               BackupPath == other.BackupPath &&
               BackupFileName == other.BackupFileName &&
               CountDaysBackupSessionFilter == other.CountDaysBackupSessionFilter &&
               ElasticsearchPath == other.ElasticsearchPath &&
               ElasticsearchUri == other.ElasticsearchUri &&
               JavaDir == other.JavaDir &&
               KibanaDir == other.KibanaDir &&
               InstanceDir == other.InstanceDir &&
               InstanceLogsPath == other.InstanceLogsPath &&
               InstanceStreamsPath == other.InstanceStreamsPath &&
               Equals(IisVersion, other.IisVersion) &&
               IisChecked == other.IisChecked &&
               IisLogsEnabled == other.IisLogsEnabled &&
               Language == other.Language &&
               Port == other.Port &&
               Fqdn == other.Fqdn &&
               TempPath == other.TempPath &&
               FrameworkDotNetVersion == other.FrameworkDotNetVersion &&
               Dotnet60Version == other.Dotnet60Version &&
               ProductType == other.ProductType &&
               IsWalDisabled == other.IsWalDisabled;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TemplateModel)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (ProductDir != null ? ProductDir.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (MsiLogsPath != null ? MsiLogsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (LogsPath != null ? LogsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (DiagnosticsPath != null ? DiagnosticsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (BackupDescription != null ? BackupDescription.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (BackupPath != null ? BackupPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (BackupFileName != null ? BackupFileName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ CountDaysBackupSessionFilter.GetHashCode();
            hashCode = (hashCode * 397) ^ (ElasticsearchPath != null ? ElasticsearchPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ElasticsearchUri != null ? ElasticsearchUri.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (JavaDir != null ? JavaDir.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (KibanaDir != null ? KibanaDir.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (InstanceDir != null ? InstanceDir.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (InstanceLogsPath != null ? InstanceLogsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (InstanceStreamsPath != null ? InstanceStreamsPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (IisVersion != null ? IisVersion.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IisChecked.GetHashCode();
            hashCode = (hashCode * 397) ^ IisLogsEnabled.GetHashCode();
            hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Port.GetHashCode();
            hashCode = (hashCode * 397) ^ (Fqdn != null ? Fqdn.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (TempPath != null ? TempPath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ FrameworkDotNetVersion.GetHashCode();
            hashCode = (hashCode * 397) ^ (Dotnet60Version != null ? Dotnet60Version.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ProductType != null ? ProductType.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsWalDisabled.GetHashCode();
            return hashCode;
        }
    }
}

public class ProductModel : IEquatable<ProductModel>
{
    public Version Version { get; set; }
    public string Path { get; set; }
    public int? InstanceCount { get; set; }
    public bool? IsEnabled { get; set; }


    public bool Equals(ProductModel other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Version, other.Version) && Path == other.Path && InstanceCount == other.InstanceCount && IsEnabled == other.IsEnabled;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ProductModel)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Version != null ? Version.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ InstanceCount.GetHashCode();
            hashCode = (hashCode * 397) ^ IsEnabled.GetHashCode();
            return hashCode;
        }
    }
}