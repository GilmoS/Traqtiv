namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for recommendation services, providing methods to retrieve recommendations and alerts, and to mark alerts as read.
public interface IRecommendationService : IService
{
    // Retrieves a list of recommendations.
    Task<List<RecommendationDto>> GetRecommendationsAsync();

    // Retrieves a list of alerts.
    Task<List<AlertDto>> GetAlertsAsync();

    // Marks an alert as read by its unique identifier, returning true if the operation was successful. 
    Task<bool> MarkAlertAsReadAsync(Guid id);
}