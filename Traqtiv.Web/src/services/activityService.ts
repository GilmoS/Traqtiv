import apiClient from './apiClient'
import type { DailyActivityDto, AddDailyActivityDto, ActivitySummaryDto } from '../types'
// This file contains functions for interacting with the daily activity API endpoints.
// It provides methods to get all daily activities, add a new daily activity, and get a summary of activities within a date range.
export const activityService = {
    // Fetches all daily activity records for the user.
  getAll: () =>
    apiClient.get<DailyActivityDto[]>('/api/dailyactivity').then(r => r.data),
// Adds a new daily activity record for the user with the provided data.
  add: (data: AddDailyActivityDto) =>
    apiClient.post<DailyActivityDto>('/api/dailyactivity', data).then(r => r.data),
// Fetches a summary of daily activities for the user within the specified date range.
  getSummary: (from: string, to: string) =>
    apiClient.get<ActivitySummaryDto>(`/api/dailyactivity/summary?from=${from}&to=${to}`).then(r => r.data),
}