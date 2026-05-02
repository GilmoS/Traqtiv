import apiClient from './apiClient'
import type { RecommendationDto, AlertDto, BaseResponseDto } from '../types'
/// Service for fetching recommendations and alerts from the backend API
// This service provides methods to get recommendations, get alerts, and mark alerts as read

export const recommendationService = {
// Fetches a list of recommendations for the current user
  getRecommendations: () =>
    apiClient.get<RecommendationDto[]>('/api/recommendation').then(r => r.data),
// Fetches a specific recommendation by its ID
  getById: (id: string) =>
    apiClient.get<RecommendationDto>(`/api/recommendation/${id}`).then(r => r.data),
// Fetches a list of alerts for the current user
  getAlerts: () =>
    apiClient.get<AlertDto[]>('/api/recommendation/alerts').then(r => r.data),
// Marks a specific alert as read by its ID
  markAlertRead: (id: string) =>
    apiClient.put<BaseResponseDto>(`/api/recommendation/alerts/${id}/read`).then(r => r.data),
}