using Microsoft.EntityFrameworkCore;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Data;
using Traqtiv.API.Models.Entities;

namespace Traqtiv.API.DAL
{
    public class SmartFitnessDal : ISmartFitnessDal
    {
        private readonly SmartFitnessDb _db;

        public SmartFitnessDal(SmartFitnessDb db)
        {
            _db = db;
        }

        // Users
        public async Task<User?> GetUserByIdAsync(Guid id)
            => await _db.Users.FindAsync(id);

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        // Workouts
        public async Task<Workout?> GetWorkoutByIdAsync(Guid id)
            => await _db.Workouts.FindAsync(id);

        public async Task<List<Workout>> GetWorkoutsByUserIdAsync(Guid userId)
            => await _db.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ToListAsync();

        public async Task AddWorkoutAsync(Workout workout)
        {
            await _db.Workouts.AddAsync(workout);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            _db.Workouts.Update(workout);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteWorkoutAsync(Guid id)
        {
            var workout = await _db.Workouts.FindAsync(id);
            if (workout != null)
            {
                _db.Workouts.Remove(workout);
                await _db.SaveChangesAsync();
            }
        }

        // BodyMetrics
        public async Task<List<BodyMetrics>> GetMetricsByUserIdAsync(Guid userId)
            => await _db.BodyMetrics
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.MeasuredAt)
                .ToListAsync();

        public async Task AddMetricsAsync(BodyMetrics metrics)
        {
            await _db.BodyMetrics.AddAsync(metrics);
            await _db.SaveChangesAsync();
        }

        // DailyActivity
        public async Task<DailyActivity?> GetDailyActivityAsync(Guid userId, DateTime date)
            => await _db.DailyActivities
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == date.Date);

        public async Task<List<DailyActivity>> GetActivitiesByRangeAsync(Guid userId, DateTime from, DateTime to)
            => await _db.DailyActivities
                .Where(d => d.UserId == userId && d.Date >= from && d.Date <= to)
                .OrderBy(d => d.Date)
                .ToListAsync();

        public async Task AddDailyActivityAsync(DailyActivity activity)
        {
            await _db.DailyActivities.AddAsync(activity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateDailyActivityAsync(DailyActivity activity)
        {
            _db.DailyActivities.Update(activity);
            await _db.SaveChangesAsync();
        }

        // Recommendations
        public async Task<List<Recommendation>> GetRecommendationsByUserIdAsync(Guid userId)
            => await _db.Recommendations
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task AddRecommendationAsync(Recommendation recommendation)
        {
            await _db.Recommendations.AddAsync(recommendation);
            await _db.SaveChangesAsync();
        }

        public async Task MarkRecommendationAsReadAsync(Guid id)
        {
            var recommendation = await _db.Recommendations.FindAsync(id);
            if (recommendation != null)
            {
                recommendation.IsRead = true;
                await _db.SaveChangesAsync();
            }
        }

        // Alerts
        public async Task<List<Alert>> GetAlertsByUserIdAsync(Guid userId)
            => await _db.Alerts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task AddAlertAsync(Alert alert)
        {
            await _db.Alerts.AddAsync(alert);
            await _db.SaveChangesAsync();
        }

        public async Task MarkAlertAsReadAsync(Guid id)
        {
            var alert = await _db.Alerts.FindAsync(id);
            if (alert != null)
            {
                alert.IsRead = true;
                await _db.SaveChangesAsync();
            }
        }
    }
}
