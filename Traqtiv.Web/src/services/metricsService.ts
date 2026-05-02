import apiClient from './apiClient'
import type { BodyMetricsDto, AddMetricsDto, UserProfileDto, UpdateProfileDto, BaseResponseDto } from '../types'
// This file contains functions to interact with the metrics-related API endpoints
// It uses the apiClient to make HTTP requests and returns the relevant data
export const metricsService = {
// Function to get the user's profile information
  getProfile: () =>
    apiClient.get<UserProfileDto>('/api/user/profile').then(r => r.data),
// Function to update the user's profile information
  updateProfile: (data: UpdateProfileDto) =>
    apiClient.put<BaseResponseDto>('/api/user/profile', data).then(r => r.data),
// Function to get the user's body metrics history
  getMetrics: () =>
    apiClient.get<BodyMetricsDto[]>('/api/user/metrics').then(r => r.data),
// Function to add new body metrics for the user
  addMetrics: (data: AddMetricsDto) =>
    apiClient.post<BodyMetricsDto>('/api/user/metrics', data).then(r => r.data),
}