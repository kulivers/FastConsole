namespace Comindware.Gateway.Api;

public class ScanningResponse : WorkflowResponse
{
    public ScanningResponse(ScanningStatus status, string message)
    {
        Status = status;
        Message = message;
    }

    public ScanningStatus Status { get; set; }

    public string Message { get; set; }
}