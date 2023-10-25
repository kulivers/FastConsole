using System.Collections.Generic;
using System.IO;

public class BackupConfigPathComparer : IEqualityComparer<BackupConfiguration>
{
    public bool Equals(BackupConfiguration x, BackupConfiguration y)
    {
        if (x != null && y != null)
        {
            return Path.GetFullPath(x.Path).Equals(Path.GetFullPath(y.Path));
        }

        return x == null && y == null;
    }

    public int GetHashCode(BackupConfiguration obj)
    {
        return obj.Path.GetHashCode();
    }
}