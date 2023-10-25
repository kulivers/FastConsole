using System;
using System.IO;
using System.Linq;
using Comindware.Platform.Api.Data;


namespace Comindware.Platform.Core
{
    internal class BackupPathValidator
    {
        public PathValidationResult ValidateFilePath(string filePath, bool checkFileNotExists = true)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectFileName, string.Empty);
            }

            var invalidPathChars = Path.GetInvalidPathChars();
            if (filePath.Any(c => invalidPathChars.Contains(c)))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectFileName, string.Empty);
            }

            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var fileName = Path.GetFileName(filePath);
            if (fileName.Any(c => invalidFileNameChars.Contains(c)))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectFileName, string.Empty);
            }

            if (!Path.IsPathRooted(filePath))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectPath, string.Format(string.Empty, filePath));
            }

            if (checkFileNotExists && File.Exists(filePath))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedFileAlreadyExsists, string.Empty);
            }

            return ValidateDirectory(Path.GetDirectoryName(filePath));
        }

        public PathValidationResult ValidateDirectory(string dirPath)
        {
            if (!new SymbolicPathResolver().TryGetFinalPathName(dirPath, out var finalPath))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectPath, string.Format(string.Empty, dirPath));
            }

            if (!System.IO.Directory.Exists(finalPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(finalPath);
                }
                catch (System.UnauthorizedAccessException e)
                {
                    return PathValidationResult.ErrorResult(BackupSessionStatus.FailedInaccessiblePath, e.Message);
                }
                catch (Exception e)
                {
                    return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectPath, e.Message);
                }
            }

            if (new DirectoryInfo(finalPath).Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedInaccessiblePath, string.Empty);
            }

            try
            {
                var mockFile = Path.Combine(finalPath, Guid.NewGuid().ToString("N"));
                using (File.Create(mockFile, 1, FileOptions.DeleteOnClose))
                {
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedInaccessiblePath, e.Message);
            }
            catch (Exception e)
            {
                return PathValidationResult.ErrorResult(BackupSessionStatus.FailedIncorrectPath, e.Message);
            }

            return PathValidationResult.SuccessResult;
        }
    }

    internal struct PathValidationResult
    {
        public bool Success;
        public BackupSessionStatus Status;
        public string Error;

        public static PathValidationResult SuccessResult => new PathValidationResult() { Success = true, Status = default, Error = null };

        public static PathValidationResult ErrorResult(BackupSessionStatus status, string error) => new PathValidationResult()
        {
            Status = status,
            Error = error,
            Success = false,
        };
    }
}