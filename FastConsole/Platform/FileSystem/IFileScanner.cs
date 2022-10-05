namespace Comindware.Gateway.Api;

public interface IFileScanner
{
    Task<WorkflowResponse> StartScanningAsync(FileInfo file, CancellationToken token);

    Task<WorkflowResponse> GetScanningResultAsync(FileInfo file, CancellationToken token);

    Task<WorkflowResponse> DeleteScanningResultsAsync(string? scanId, CancellationToken token);
}