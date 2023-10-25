namespace Comindware.Platform.Api.Data
{
    public enum BackupSessionStatus
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Session is in queue
        /// </summary>
        InQueue,
        /// <summary>
        /// Session is in progress
        /// </summary>
        InProgress,
        /// <summary>
        /// Session finished
        /// </summary>
        Completed,
        /// <summary>
        /// Session was finished by user before completion
        /// </summary>
        Aborted,
        /// <summary>
        /// Failed  
        /// </summary>
        Failed,
        /// <summary>
        /// Failed due to max sessions count in queue
        /// </summary>
        FailedQueueOverflow,
        /// <summary>
        /// Missing path
        /// </summary>
        FailedPathNotFound,
        /// <summary>
        /// Incorrect Path
        /// </summary>
        FailedIncorrectPath,
        /// <summary>
        /// Access to path is denied 
        /// </summary>
        FailedInaccessiblePath,
        /// <summary>
        /// Incorrect file name
        /// </summary>
        FailedIncorrectFileName,
        /// <summary>
        /// File with a same name already exists
        /// </summary>
        FailedFileAlreadyExsists,
    }
}
