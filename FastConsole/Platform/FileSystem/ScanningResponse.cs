namespace Comindware.Gateway.Api;

public class ScanningResponse : WorkflowResponse
{
    public ScanningStatus Status { get; set; }

    public string Message { get; set; }

    public ScanningResponse(ScanningStatus status, string message)
    {
        Status = status;
        Message = message;
    }
}