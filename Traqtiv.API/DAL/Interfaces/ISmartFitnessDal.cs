using Traqtiv.API.Models.Entities;

namespace Traqtiv.API.DAL.Interfaces
{
    public interface ISmartFitnessDal
    {
        // Users
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);

        // Workouts
        Task<Workout?> GetWorkoutByIdAsync(Guid id);
        Task<List<Workout>> GetWorkoutsByUserIdAsync(Guid userId);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(Guid id);

        // BodyMetrics
        Task<List<BodyMetrics>> GetMetricsByUserIdAsync(Guid userId);
        Task AddMetricsAsync(BodyMetrics metrics);

        // DailyActivity
        Task<DailyActivity?> GetDailyActivityAsync(Guid userId, DateTime date);
        Task<List<DailyActivity>> GetActivitiesByRangeAsync(Guid userId, DateTime from, DateTime to);
        Task AddDailyActivityAsync(DailyActivity activity);
        Task UpdateDailyActivityAsync(DailyActivity activity);

        // Recommendations
        Task<List<Recommendation>> GetRecommendationsByUserIdAsync(Guid userId);
        Task AddRecommendationAsync(Recommendation recommendation);
        Task MarkRecommendationAsReadAsync(Guid id);

        // Alerts
        Task<List<Alert>> GetAlertsByUserIdAsync(Guid userId);
        Task AddAlertAsync(Alert alert);
        Task MarkAlertAsReadAsync(Guid id);
    }
}
