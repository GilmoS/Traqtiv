using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Services
{
    /// Provides operations to create, retrieve and update daily activity records for users.
    /// This service maps entity models to DTOs and delegates persistence to ISmartFitnessDal.
    /// It also notifies IRecommendationService when daily activities are updated.
    public class DailyActivityService : IDailyActivityService
    {
        private readonly ISmartFitnessDal _dal;
        private readonly IRecommendationService _recommendationService;

        /// Initializes a new instance of the DailyActivityService class.
        public DailyActivityService(ISmartFitnessDal dal, IRecommendationService recommendationService)
        {
            _dal = dal;
            _recommendationService = recommendationService;
        }
        /// Retrieves daily activity records for the specified user for the past month.
        /// Returns a list of DailyActivityDto objects.
        /// Each DTO has Success set to true. If the user has no activities an empty list is returned.
        public async Task<List<DailyActivityDto>> GetActivitiesAsync(Guid userId)
        {
            var activities = await _dal.GetActivitiesByRangeAsync(
                userId,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow);

            return activities.Select(a => new DailyActivityDto
            {
                Success = true,
                Steps = a.Steps,
                CaloriesBurned = a.CaloriesBurned,
                ActiveMinutes = a.ActiveMinutes,
                DistanceKm = a.DistanceKm,
                Date = a.Date.ToUniversalTime()
            }).ToList();
        }

        /// Adds or updates a daily activity record for the specified user and date.
        /// If a record for the specified date already exists, it is updated. Otherwise, a new record is created.
        /// Notifies the recommendation service about the update.
        public async Task AddDailyActivityAsync(Guid userId, AddDailyActivityDto request)
        {
            var existing = await _dal.GetDailyActivityAsync(userId, request.Date.ToUniversalTime());

            if (existing != null)
            {
                existing.Steps = request.Steps;
                existing.CaloriesBurned = request.CaloriesBurned;
                existing.ActiveMinutes = request.ActiveMinutes;
                existing.DistanceKm = request.DistanceKm;
                await _dal.UpdateDailyActivityAsync(existing);
            }
            else
            {
                var activity = new DailyActivity
                {
                    UserId = userId,
                    Steps = request.Steps,
                    CaloriesBurned = request.CaloriesBurned,
                    ActiveMinutes = request.ActiveMinutes,
                    DistanceKm = request.DistanceKm,
                    Date = request.Date.ToUniversalTime()   
                };
                await _dal.AddDailyActivityAsync(activity);
            }

            /// (Observer) Notify RecommendationService about the updated daily activity
            await _recommendationService.OnDailyActivityUpdatedAsync(userId);
        }
        /// Retrieves a summary of daily activities for the specified user over a given date range.
        /// Returns an ActivitySummaryDto containing the aggregated data. If no activities are found, the totals will be zero.
        public async Task<ActivitySummaryDto> GetActivitySummaryAsync(Guid userId, DateTime from, DateTime to)
        {
            var activities = await _dal.GetActivitiesByRangeAsync(userId, from, to);

            return new ActivitySummaryDto
            {
                Success = true,
                TotalSteps = activities.Sum(a => a.Steps),
                TotalCaloriesBurned = activities.Sum(a => a.CaloriesBurned),
                TotalActiveMinutes = activities.Sum(a => a.ActiveMinutes),
                TotalDistanceKm = activities.Sum(a => a.DistanceKm),
                DateFrom = from.ToUniversalTime(),
                DateTo = to.ToUniversalTime()
            };
        }
    }
}