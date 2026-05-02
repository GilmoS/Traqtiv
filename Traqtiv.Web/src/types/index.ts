


// ─── Auth ────────────────────────────────────────────────
export interface LoginRequestDto {
  email: string
  password: string
}
// ─── Register ────────────────────────────────────────────────
export interface RegisterRequestDto {
  firstName: string
  lastName: string
  email: string
  password: string
  dateOfBirth: string
}
// ─── Auth Response ────────────────────────────────────────────────
export interface AuthResponseDto {
  success: boolean
  message: string
  token: string
  userId: string
}

// ─── User ────────────────────────────────────────────────
export interface UserProfileDto {
  id: string
  firstName: string
  lastName: string
  email: string
  dateOfBirth: string
  createdAt: string
}
// ─── Update Profile ────────────────────────────────────────────────
export interface UpdateProfileDto {
  firstName: string
  lastName: string
  dateOfBirth: string
}

// ─── Body Metrics ────────────────────────────────────────
export interface BodyMetricsDto {
  id: string
  userId: string
  weight: number
  restingHeartRate: number
  bmi: number
  measuredAt: string
}
// ─── Add Body Metrics ────────────────────────────────────────
export interface AddMetricsDto {
  weight: number
  restingHeartRate: number
  bmi: number
}

// ─── Workout ─────────────────────────────────────────────
export const WorkoutType = {
  Strength: 0,
  Cardio: 1,
  Flexibility: 2
} as const
export type WorkoutType = typeof WorkoutType[keyof typeof WorkoutType]
// ─── Workout Status ─────────────────────────────────────────────
export const WorkoutStatus = {
  Planned: 0,
  Completed: 1
} as const
export type WorkoutStatus = typeof WorkoutStatus[keyof typeof WorkoutStatus]
// ─── Workout DTOs ─────────────────────────────────────────────
export interface WorkoutDto {
  id: string
  userId: string
  type: WorkoutType
  durationMinutes: number
  status: WorkoutStatus
  caloriesBurned: number
  date: string
  notes: string
}
// ─── Add Workout DTO ─────────────────────────────────────────────
export interface AddWorkoutDto {
  type: WorkoutType
  durationMinutes: number
  status: WorkoutStatus
  caloriesBurned: number
  date: string
  notes: string
}
// ─── Update Workout DTO ─────────────────────────────────────────────
export interface UpdateWorkoutDto {
  type: WorkoutType
  durationMinutes: number
  status: WorkoutStatus
  caloriesBurned: number
  date: string
  notes: string
}

// ─── Daily Activity ───────────────────────────────────────
export interface DailyActivityDto {
  id: string
  userId: string
  date: string
  steps: number
  caloriesBurned: number
  activeMinutes: number
  distanceKm: number
}
// ─── Add Daily Activity DTO ───────────────────────────────────────
export interface AddDailyActivityDto {
  steps: number
  caloriesBurned: number
  activeMinutes: number
  distanceKm: number
  date: string
}
// ─── Activity Summary ───────────────────────────────────────
export interface ActivitySummaryDto {
  totalSteps: number
  totalCaloriesBurned: number
  totalActiveMinutes: number
  totalDistanceKm: number
  dateFrom: string
  dateTo: string
}

// ─── Recommendations & Alerts ────────────────────────────
export const RecommendationType = {
  Weather: 0,
  Overload: 1,
  Inactivity: 2
} as const
export type RecommendationType = typeof RecommendationType[keyof typeof RecommendationType]
// ─── Alert Severity ─────────────────────────────────────────────
export const AlertSeverity = {
  Low: 0,
  Medium: 1,
  High: 2
} as const
export type AlertSeverity = typeof AlertSeverity[keyof typeof AlertSeverity]
// ─── Recommendation & Alert DTOs ─────────────────────────────────────────────
export interface RecommendationDto {
  id: string
  userId: string
  type: RecommendationType
  message: string
  createdAt: string
  isRead: boolean
}
export interface AlertDto {
  id: string
  userId: string
  type: number
  message: string
  createdAt: string
  isRead: boolean
  severity: AlertSeverity
}

// ─── Weather ──────────────────────────────────────────────
export interface WeatherResponseDto {
  success: boolean
  message: string
  temperature: number
  description: string
  airQualityIndex: number
  airQualityDescription: string
}

// ─── Base Response ────────────────────────────────────────
export interface BaseResponseDto {
  success: boolean
  message: string
}