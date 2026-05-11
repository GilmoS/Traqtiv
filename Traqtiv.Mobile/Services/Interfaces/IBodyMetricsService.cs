namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for body metrics services, providing methods to retrieve and add body metrics data.
public interface IBodyMetricsService : IService
{
    // Retrieves a list of body metrics data asynchronously.
    Task<List<BodyMetricsDto>> GetMetricsAsync();

    // Adds new body metrics data asynchronously, returning true if the operation was successful.
    Task<bool> AddMetricsAsync(AddMetricsDto request);
}