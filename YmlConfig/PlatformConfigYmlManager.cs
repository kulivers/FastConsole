using YamlDotNet.Core;

public class PlatformConfigYmlManager
{
    private readonly char[] _trimCharacters = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
    
    public InstanceYmlModel ReadInstance(string path)
    {
        try
        {
            try
            {
                var instance = PlatformYmlSerializer.ReadModel<InstanceYmlModel>(path);
                if (instance == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(instance.ConfigName))
                {
                    instance.ConfigName = Path.GetFileNameWithoutExtension(path);
                }

                if (string.IsNullOrEmpty(instance.InstanceName))
                {
                    instance.InstanceName = instance.ConfigName;
                }

                return instance;
            }
            catch (YamlException)
            {
                return null;
            }
        }
        catch (Exception e)
        {
 
            return null;
        }
    }

    public void Write(InstanceYmlModel instance1, string path)
    {
        if (instance1 == null || string.IsNullOrEmpty(instance1.ConfigName))
        {
            return;
        }
        
        var instance = new InstanceYmlModel(instance1);
        PlatformYmlSerializer.WriteToFile(path, instance);
    }
}