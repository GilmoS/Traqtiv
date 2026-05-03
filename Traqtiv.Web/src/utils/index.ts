import { WorkoutType, WorkoutStatus, RecommendationType, AlertSeverity } from '../types'

// ─── תאריכים ──────────────────────────────────────────────
export const formatDate = (dateStr: string) =>
  new Date(dateStr).toLocaleDateString('he-IL',{day: '2-digit', month: '2-digit', year: 'numeric'})

export const formatDateShort = (dateStr: string) =>new Date(dateStr).toLocaleDateString('he-IL', {day: '2-digit', month: '2-digit'})

export const formatDateTime = (dateStr: string) =>new Date(dateStr).toLocaleString('he-IL', {day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit'})

export const todayISO = () => new Date().toISOString().split('T')[0]

// ─── Workout ──────────────────────────────────────────────
export const workoutTypeLabel: Record<WorkoutType, string> = {
  [WorkoutType.Strength]: 'Strength',
  [WorkoutType.Cardio]: 'Cardio',
  [WorkoutType.Flexibility]: 'Flexibility',
}

export const workoutTypeColor: Record<WorkoutType, string> = {
  [WorkoutType.Strength]: 'var(--orange)',
  [WorkoutType.Cardio]: 'var(--blue)',
  [WorkoutType.Flexibility]: 'var(--purple)',
}

export const workoutStatusLabel: Record<WorkoutStatus, string> = {
  [WorkoutStatus.Planned]: 'Planned',
  [WorkoutStatus.Completed]: 'Completed',
}

// ─── Alerts ───────────────────────────────────────────────
export const alertSeverityLabel: Record<AlertSeverity, string> = {
  [AlertSeverity.Low]: 'Low',
  [AlertSeverity.Medium]: 'Medium',
  [AlertSeverity.High]: 'High',
}

export const alertSeverityColor: Record<AlertSeverity, string> = {
  [AlertSeverity.Low]: 'var(--accent)',
  [AlertSeverity.Medium]: 'var(--yellow)',
  [AlertSeverity.High]: 'var(--red)',
}

// ─── Recommendations ──────────────────────────────────────
export const recommendationTypeLabel: Record<RecommendationType, string> = {
  [RecommendationType.Weather]: 'Weather',
  [RecommendationType.Overload]: 'Overload',
  [RecommendationType.Inactivity]: 'Inactivity',}

// generic function to prepare chart data by sorting items by date and adding a label
export const prepareChartData = <T extends { date?: string; measuredAt?: string }>(items: T[],dateKey: 'date' | 'measuredAt' = 'date') =>
  [...items].sort((a, b) =>new Date(a[dateKey] as string).getTime() - new Date(b[dateKey] as string).getTime()).map(item => ({ ...item, label: formatDateShort(item[dateKey] as string)}))