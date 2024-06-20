using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ConfigValidation
{
    public class ReadableFileAttribute : ExistingFileAttribute
    {
        public override bool IsValid(object value)
        {
            if (!base.IsValid(value))
            {
                return false;
            }

            try
            {
                using (var fs = new FileStream((string)value, FileMode.Open))
                {
                    return fs.CanRead;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class ExistingFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return File.Exists((string)value);
        }
    }

    public class FilePathNullableAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string filePath))
            {
                return false;
            }

            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            return !filePath.Any(c => invalidFileNameChars.Contains(c));
        }
    }
}