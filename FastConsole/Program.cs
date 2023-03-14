using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Aspose.Cells;
using FastConsole;
using FastConsole.models;
using Microsoft.Win32;
using Nest;

public class ForcedPlatformConfigProperties
{
    public string Fqdn { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        var pa = @"C:\ProgramData\Comindware\Configs\Instance\1.yml";
        File.Delete(pa);
    }
}
public class InstanceModel
{
    public string Name;
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

}