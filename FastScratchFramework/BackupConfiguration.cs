public class BackupConfiguration
{
    /// <summary>
    /// Backup configuration identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Backup folder location
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Backup file name
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Backup contains streams
    /// </summary>
    public bool WithStreams { get; set; }

    /// <summary>
    /// Backup contains scripts
    /// </summary>
    public bool WithScripts { get; set; }

    /// <summary>
    /// Backup description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Launch schedule
    /// </summary>
    /// <summary>
    /// Number of backup archives which wouldn't be deleted automatically
    /// </summary>
    public uint KeepRecent { get; set; }

    /// <summary>
    /// If config is disabled then backup will not be run neither manually nor by schedule
    /// </summary>
    public bool IsDisabled { get; set; }
}