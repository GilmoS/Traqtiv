using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Helpers;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Models.Enums;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Services
{
    /// Provides operations to retrieve and manage user recommendations and alerts.
    // This service maps entity models to DTOs and delegates persistence to ISmartFitnessDal.
    // It also generates recommendations and alerts based on user workouts and daily activity patterns.
    public class RecommendationService : IRecommendationService
    {
        private readonly ISmartFitnessDal _dal;

        /// Initializes a new instance of the RecommendationService class.
        /// The data access layer is used to query and persist recommendations and alerts.
        public RecommendationService(ISmartFitnessDal dal)
        {
            _dal = dal;
        }
        // Retrieves recommendations for the specified user.
        // Returns a list of RecommendationDto objects. Each DTO has Success set to true.
        // If the user has no recommendations an empty list is returned.
        public async Task<List<RecommendationDto>> GetRecommendationsAsync(Guid userId)
        {
            var recommendations = await _dal.GetRecommendationsByUserIdAsync(userId);

            return recommendations.Select(r => new RecommendationDto
            {
                Success = true,
                Id = r.Id,
                Type = r.Type,
                Message = r.Message,
                IsRead = r.IsRead,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
        // Retrieves alerts for the specified user.
        // Returns a list of AlertDto objects. Each DTO has Success set to true.
        // If the user has no alerts an empty list is returned.
        public async Task<List<AlertDto>> GetAlertsAsync(Guid userId)
        {
            var alerts = await _dal.GetAlertsByUserIdAsync(userId);

            return alerts.Select(a => new AlertDto
            {
                Success = true,
                Id = a.Id,
                Type = a.Type,
                Message = a.Message,
                IsRead = a.IsRead,
                Severity = a.Severity,
                CreatedAt = a.CreatedAt
            }).ToList();
        }

        // Marks a specific alert as read for the specified user.
        public async Task MarkAlertAsReadAsync(Guid alertId)
        {
            await _dal.MarkAlertAsReadAsync(alertId);
        }

        // overload and recovery recommendations based on recent workouts
        // This method is called by WorkoutService whenever a workout is added.
        // It checks the user's workouts for the past week and generates alerts or recommendations if they have been training too much or consistently.
        public async Task OnWorkoutAddedAsync(Guid userId)
        {
            var workouts = await _dal.GetWorkoutsByUserIdAsync(userId);
            var recentWorkouts = workouts
                .Where(w => DateTimeHelper.IsWithinDays(w.Date, 7))
                .ToList();

            // overload alert - more than 5 workouts in the past week
            if (recentWorkouts.Count > 5)
            {
                await _dal.AddAlertAsync(new Alert
                {
                    UserId = userId,
                    Type = AlertType.Overload,
                    Severity = AlertSeverity.High,
                    Message = "You have trained more than 5 times this week. Consider taking a rest day."
                });
            }

            // recovery recommendation - 3 or more workouts in a row in the past week
            if (recentWorkouts.Count >= 3)
            {
                await _dal.AddRecommendationAsync(new Recommendation
                {
                    UserId = userId,
                    Type = RecommendationType.Overload,
                    Message = "You have been training consistently. Make sure to get enough rest and recovery."
                });
            }
        }


        // inactivity alert and recommendation based on weekly activity
        // This method is called by DailyActivityService whenever a daily activity record is added or updated.
        // It checks the user's activity for the past week and generates alerts or recommendations if they have been inactive.
        public async Task OnDailyActivityUpdatedAsync(Guid userId)
        {
            var today = DateTime.UtcNow;
            var weekStart = DateTimeHelper.StartOfWeek(today);
            var activities = await _dal.GetActivitiesByRangeAsync(userId, weekStart, today);

            var totalStepsThisWeek = activities.Sum(a => a.Steps);
            var activeDaysThisWeek = activities.Count(a => a.Steps > 1000);

            // low activity alert - less than 3 active days in the past week
            if (activeDaysThisWeek < 3 && today.DayOfWeek >= DayOfWeek.Thursday)
            {
                await _dal.AddAlertAsync(new Alert
                {
                    UserId = userId,
                    Type = AlertType.Inactivity,
                    Severity = AlertSeverity.Medium,
                    Message = "You have been inactive for most of this week. Try to add more movement to your day."
                });
            }

            // inactivity recommendation - less than 50,000 steps in the past week
            if (totalStepsThisWeek < 50000 && today.DayOfWeek >= DayOfWeek.Wednesday)
            {
                await _dal.AddRecommendationAsync(new Recommendation
                {
                    UserId = userId,
                    Type = RecommendationType.Inactivity,
                    Message = "You are behind on your weekly step goal. Try to take a walk today!"
                });
            }
        }
    }
}