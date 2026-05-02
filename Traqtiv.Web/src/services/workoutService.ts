import apiClient from './apiClient'
import type { WorkoutDto, AddWorkoutDto, UpdateWorkoutDto, BaseResponseDto } from '../types'
// This file defines the workoutService object that provides methods to interact with the workout API endpoints.
// Each method corresponds to a specific API endpoint and HTTP method, allowing you to perform CRUD operations on workout data.
export const workoutService = {
// The getAll method retrieves a list of all workouts by sending a GET request to the /api/workout endpoint. It returns a promise that resolves to an array of WorkoutDto objects.
  getAll: () =>
    apiClient.get<WorkoutDto[]>('/api/workout').then(r => r.data),
// The getById method retrieves a specific workout by its ID by sending a GET request to the /api/workout/{id} endpoint. It returns a promise that resolves to a WorkoutDto object.
  getById: (id: string) =>
    apiClient.get<WorkoutDto>(`/api/workout/${id}`).then(r => r.data),
// The add method creates a new workout by sending a POST request to the /api/workout endpoint with the workout data in the request body. It returns a promise that resolves to the created WorkoutDto object.
  add: (data: AddWorkoutDto) =>
    apiClient.post<WorkoutDto>('/api/workout', data).then(r => r.data),
// The update method updates an existing workout by sending a PUT request to the /api/workout/{id} endpoint with the updated workout data in the request body. It returns a promise that resolves to a BaseResponseDto object indicating the success of the operation.
  update: (id: string, data: UpdateWorkoutDto) =>
    apiClient.put<BaseResponseDto>(`/api/workout/${id}`, data).then(r => r.data),
// The delete method removes a workout by sending a DELETE request to the /api/workout/{id} endpoint. It returns a promise that resolves to a BaseResponseDto object indicating the success of the operation.
  delete: (id: string) =>
    apiClient.delete<BaseResponseDto>(`/api/workout/${id}`).then(r => r.data),
}