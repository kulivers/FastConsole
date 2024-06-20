using System;
using System.IO;

namespace ConfigValidation
{
    public class WritableDirectoryAttribute : ExistingDirectoryAttribute
    {
        public override bool IsValid(object value)
        {
            if (!base.IsValid(value))
            {
                return false;
            }

            try
            {
                using (File.Create(Path.Combine((string)value, Path.GetRandomFileName()), 1, FileOptions.None))
                {
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        
    }
}