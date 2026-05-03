import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { workoutService } from '../services/workoutService'
import { activityService } from '../services/activityService'
import { metricsService } from '../services/metricsService'
import { recommendationService } from '../services/recommendationService'
import { weatherService } from '../services/weatherService'
import type { AddWorkoutDto, UpdateWorkoutDto, AddDailyActivityDto, AddMetricsDto, UpdateProfileDto } from '../types'

// ─── Workouts ─────────────────────────────────────────────
export const useWorkouts = () =>
  useQuery({ queryKey: ['workouts'], queryFn: workoutService.getAll })


export const useAddWorkout = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (data: AddWorkoutDto) => workoutService.add(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['workouts'] }),
  })
}

export const useUpdateWorkout = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateWorkoutDto }) =>
      workoutService.update(id, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['workouts'] }),
  })
}

export const useDeleteWorkout = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => workoutService.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['workouts'] }),
  })
}

// ─── Activity ─────────────────────────────────────────────
export const useDailyActivity = () =>
  useQuery({ queryKey: ['activity'], queryFn: activityService.getAll })

export const useAddActivity = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (data: AddDailyActivityDto) => activityService.add(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['activity'] }),
  })
}

export const useActivitySummary = (from: string, to: string) =>
  useQuery({
    queryKey: ['activity-summary', from, to],
    queryFn: () => activityService.getSummary(from, to),
  })

// ─── Metrics ──────────────────────────────────────────────
export const useBodyMetrics = () =>
  useQuery({ queryKey: ['metrics'], queryFn: metricsService.getMetrics })

export const useAddMetrics = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (data: AddMetricsDto) => metricsService.addMetrics(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['metrics'] }),
  })
}

export const useProfile = () =>
  useQuery({ queryKey: ['profile'], queryFn: metricsService.getProfile })

export const useUpdateProfile = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (data: UpdateProfileDto) => metricsService.updateProfile(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['profile'] }),
  })
}

// ─── Recommendations ──────────────────────────────────────
export const useRecommendations = () =>
  useQuery({ queryKey: ['recommendations'], queryFn: recommendationService.getRecommendations })

export const useAlerts = () =>
  useQuery({ queryKey: ['alerts'], queryFn: recommendationService.getAlerts })

export const useMarkAlertRead = () => {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => recommendationService.markAlertRead(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['alerts'] }),
  })
}

// ─── Weather ──────────────────────────────────────────────
export const useWeather = () =>
  useQuery({
    queryKey: ['weather'],
    queryFn: weatherService.getCurrent,
    staleTime: 5 * 60 * 1000, // cache for 5 minutes
  })