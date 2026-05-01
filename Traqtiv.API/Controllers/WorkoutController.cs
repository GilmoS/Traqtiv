using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Helpers;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{
    // WorkoutController handles workout-related endpoints such as creating, updating, retrieving, and deleting workouts
    // It inherits from BaseController to utilize common response methods and is protected by the Authorize attribute to ensure that only authenticated users can access its endpoints
    // The controller uses the IWorkoutService to perform operations related to workouts
    [Authorize]
    public class WorkoutController : BaseController
    {
        private readonly IWorkoutService _workoutService;


        // Constructor for WorkoutController that injects the IWorkoutService dependency
        // This allows the controller to use workout services for handling workout-related logic
        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }


        // GET api/workout endpoint for retrieving all workouts for the authenticated user
        // It extracts the user ID from the JWT token using the JwtHelper and calls the GetWorkoutsAsync method of the IWorkoutService
        // The method returns an appropriate response based on the result of the service call
        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _workoutService.GetWorkoutsAsync(userId);
            return OkResponse(result);
        }

        // GET api/workout/{id} endpoint for retrieving a specific workout by its ID for the authenticated user
        // It extracts the user ID from the JWT token and calls the GetWorkoutByIdAsync method of the IWorkoutService with the workout ID from the route
        // The method returns an appropriate response based on the result of the service call, including a not found response if the workout does not exist or does not belong to the user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(Guid id)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _workoutService.GetWorkoutByIdAsync(userId, id);
            if (!result.Success)
                return NotFoundResponse(result.Message);

            return OkResponse(result);
        }

        // POST api/workout endpoint for creating a new workout for the authenticated user
        // It accepts an AddWorkoutDto object in the request body, extracts the user ID from the JWT token, and calls the AddWorkoutAsync method of the IWorkoutService
        // The method returns an appropriate response based on the result of the service call
        [HttpPost]
        public async Task<IActionResult> AddWorkout([FromBody] AddWorkoutDto request)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _workoutService.AddWorkoutAsync(userId, request);
            return OkResponse(result);
        }

        // PUT api/workout/{id} endpoint for updating an existing workout for the authenticated user
        // It accepts an UpdateWorkoutDto object in the request body, extracts the user ID from the JWT token, and calls the UpdateWorkoutAsync method of the IWorkoutService with the workout ID from the route
        // The method returns an appropriate response based on the result of the service call, including a not found response if the workout does not exist or does not belong to the user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(Guid id, [FromBody] UpdateWorkoutDto request)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _workoutService.UpdateWorkoutAsync(userId, id, request);
            if (!result.Success)
                return NotFoundResponse(result.Message);

            return OkResponse(result);
        }

        // DELETE api/workout/{id} endpoint for deleting a workout for the authenticated user
        // It extracts the user ID from the JWT token and calls the DeleteWorkoutAsync method of the IWorkoutService with the workout ID from the route
        // The method returns an appropriate response based on the result of the service call, including a not found response if the workout does not exist or does not belong to the user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(Guid id)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            await _workoutService.DeleteWorkoutAsync(userId, id);
            return OkResponse(new { Success = true, Message = "Workout deleted successfully" });
        }
    }
}