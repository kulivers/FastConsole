using Gateway.Api.Platform;
using System;
using System.Collections.Generic;

namespace Comindware.Gateway.Api;

public class Result
{
    public Exception Exception { get; set; }
    
    public ResultType Type { get; set; }
    
    public Result(ResultType resultType)
    {
        this.Type = resultType;
    }

    public Result(ResultType resultType, Exception exception) : this(resultType)
    {
        Exception = exception;
    }
}

public enum ResultType
{
    Success,
    Exception,
    NotFound
}

public class Result<T> : Result
{
    public T? Data { get; }
    
    public Result(ResultType resultType, T? data) : base(resultType)
    {
        Data = data;
    }

    public Result(ResultType resultType, T? data, Exception e) : base(resultType, e)
    {
        Data = data;
    }
    
    public Result(ResultType resultType, Exception e) : base(resultType, e)
    {
    }
}

public class ProcessedFilesResult : Result
{
    public List<ProcessedStream> UploadedFiles { get; set; } = new();
    
    public List<ProcessedStream> MarkedAsRejectedFiles { get; set;} = new();
    
    public List<ProcessedStream> UploadedWithErrorFiles { get; set;} = new();
    
    public List<ProcessedStream> MarkedAsRejectedWithErrorFiles { get; set;} = new();

    public ProcessedFilesResult(ResultType resultType) : base(resultType)
    {
    }

    public ProcessedFilesResult(ResultType resultType, Exception exception) : base(resultType, exception)
    {
    }

    public ProcessedFilesResult(Result other) : base(other.Type, other.Exception)
    {
    }
}

public class ProcessedStream
{
    public string? StreamId { get; set; }
        
    public string? RevisionId { get; set; }
        
    public Exception? Exception { get; set; }
    
    public string? ErrorMessage { get; set; }

    public ProcessedStream()
    {
    }
    
    public ProcessedStream(StreamInfo streamInfo, Exception exception)
    {
        StreamId = streamInfo.StreamId;
        RevisionId = streamInfo.RevisionId;
        Exception = exception;
    }
}