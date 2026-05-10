import { Link } from 'react-router-dom'
import { Dumbbell, Footprints, Flame, Clock, Bell, Cloud, Wind, ChevronRight } from 'lucide-react'
import { useWorkouts, useDailyActivity, useAlerts, useWeather, useBodyMetrics } from '../hooks'
import { StatCard, Card, Badge, Spinner } from '../components/ui'
import { WorkoutChart } from '../components/charts'
import { alertSeverityColor, alertSeverityLabel, workoutTypeLabel, workoutTypeColor, prepareChartData, formatDate } from '../utils'
import { WorkoutStatus } from '../types'
import { useAuth } from '../context/AuthContext'

// Dashboard page component
// Displays an overview of the user's fitness data, including recent workouts, daily activity, alerts, weather, and body metrics
// Fetches data using custom hooks and presents it in a visually appealing layout with cards and charts
// Provides quick access to detailed views and encourages user engagement with their fitness journey
export function DashboardPage() {
  const { user } = useAuth()
  const { data: workouts = [], isLoading: wLoading ,isError: wError} = useWorkouts()
  const { data: activities = [] } = useDailyActivity()
  const { data: alerts = [] } = useAlerts()
  const { data: weather } = useWeather()
  const { data: metrics = [] } = useBodyMetrics()

  const today = activities.find(a =>a.date.startsWith(new Date().toISOString().split('T')[0])) // Find today's activity
  const completedWorkouts = workouts.filter(w => w.status === WorkoutStatus.Completed) // Get completed workouts for stats and charts
  const recentWorkouts = [...workouts].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()).slice(0, 5) // Get 5 most recent workouts for display
  const unreadAlerts = alerts.filter(a => !a.isRead)
  const latestMetrics = metrics.length ? metrics.reduce((a, b) => new Date(a.measuredAt) > new Date(b.measuredAt) ? a : b) : null // Get latest body metrics for display
  const chartData = prepareChartData(completedWorkouts.slice(-10)).map(w => ({label: w.label,durationMinutes: w.durationMinutes,caloriesBurned: w.caloriesBurned,})) // Prepare data for workout chart


  return (
    <div style={{ padding: '28px 32px', maxWidth: 1200 }}>

      {/* Header */}
      <div className="fade-in" style={{ marginBottom: 28 }}>
        <h1 style={{ fontSize: 26, fontWeight: 700 }}>
          Hello, {user?.firstName} 👋
        </h1>
        <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
          {new Date().toLocaleDateString('he-IL', {weekday: 'long', day: 'numeric', month: 'long', year: 'numeric'})}
        </p>
      </div>

      {/* Stats Row */}
      <div className="fade-in-1" style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(4, 1fr)',
        gap: 16,
        marginBottom: 24,
      }}>
        <StatCard
          label="Workouts Completed"
          value={completedWorkouts.length}
          icon={<Dumbbell size={18} />}
          color="var(--orange)"
        />
        <StatCard
          label="Steps Today"
          value={today?.steps?.toLocaleString() ?? '—'}
          icon={<Footprints size={18} />}
          color="var(--accent)"
        />
        <StatCard
          label="Calories Today"
          value={today?.caloriesBurned ?? '—'}
          unit="kcal"
          icon={<Flame size={18} />}
          color="var(--red)"
        />
        <StatCard
          label="Active Minutes Today"
          value={today?.activeMinutes ?? '—'}
          unit="min"
          icon={<Clock size={18} />}
          color="var(--blue)"
        />
      </div>


      {/* Main Grid */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 340px', gap: 20 }}>

        {/* Left Column */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>

          {/* Workout Chart */}
          <Card className="fade-in-2">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
              <h2 style={{ fontSize: 16, fontWeight: 700 }}>Recent Workouts</h2>
              <Link to="/workouts" style={{ fontSize: 13, color: 'var(--accent)', display: 'flex', alignItems: 'center', gap: 4 }}>
                View All <ChevronRight size={14} />
              </Link>
            </div>
            {wLoading ? <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}><Spinner /></div>
              : wError
              ? <p style={{ color: 'var(--red)', fontSize: 14, textAlign: 'center', padding: '30px 0' }}>
              Unable to load workout data
              </p>
              : chartData.length
                ? <WorkoutChart data={chartData} />
                : <p style={{ color: 'var(--text-muted)', fontSize: 14, textAlign: 'center', padding: '30px 0' }}>No workout data yet</p>
            }
          </Card>

          {/* Recent Workouts List */}
          <Card className="fade-in-3">
            <h2 style={{ fontSize: 16, fontWeight: 700, marginBottom: 14 }}>Workout Details</h2>
            {recentWorkouts.length === 0
              ? <p style={{ color: 'var(--text-muted)', fontSize: 14 }}>No workouts logged yet</p>
              : <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
                {recentWorkouts.map(w => (
                  <div key={w.id} style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '10px 14px',
                    background: 'var(--bg-secondary)',
                    borderRadius: 'var(--radius-sm)',
                  }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                      <div style={{
                        width: 8, height: 8,
                        borderRadius: '50%',
                        background: workoutTypeColor[w.type],
                      }} />
                      <span style={{ fontSize: 14, fontWeight: 500 }}>
                        {workoutTypeLabel[w.type]}
                      </span>
                    </div>
                    <div style={{ display: 'flex', gap: 16, alignItems: 'center' }}>
                      <span style={{ fontSize: 13, color: 'var(--text-secondary)' }}>{w.durationMinutes} min</span>
                      <span style={{ fontSize: 13, color: 'var(--text-secondary)' }}>{w.caloriesBurned} kcal</span>
                      <span style={{ fontSize: 12, color: 'var(--text-muted)' }}>{formatDate(w.date)}</span>
                      <Badge color={w.status === WorkoutStatus.Completed ? 'var(--accent)' : 'var(--yellow)'}>
                        {w.status === WorkoutStatus.Completed ? 'Completed' : 'Planned'}
                      </Badge>
                    </div>
                  </div>
                ))}
              </div>
            }
          </Card>
        </div>

        {/* Right Column */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>

          {/* Weather */}
          {weather && (<Card className="fade-in-2">
              <div
                 style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 12 }}>
                <Cloud size={16} color="var(--blue)" />
                <span style={{ fontSize: 13, fontWeight: 600, color: 'var(--text-secondary)' }}>
                  Current Weather
                </span>
              </div>
              <div
               style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-end' }}>
                <div>
                  <div style={{ fontSize: 36, fontWeight: 700, color: 'var(--blue)' }}>
                    {weather.temperature}°
                  </div>
                  <div
                   style={{ fontSize: 13, color: 'var(--text-secondary)', marginTop: 2 }}>
                    {weather.description}
                  </div>
                </div>
                <div style={{ textAlign: 'right' }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: 4, fontSize: 12, color: 'var(--text-muted)' }}>
                    <Wind size={12} /> AQI: {weather.airQualityIndex}
                  </div>
                  <div style={{ fontSize: 11, color: 'var(--text-muted)', marginTop: 2 }}>
                    {weather.airQualityDescription}
                  </div>
                </div>
              </div>
            </Card>
          )}

          {/* Alerts */}
          <Card className="fade-in-3">
            <div
             style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 14 }}>
              <div
                style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                <Bell size={16} color="var(--yellow)" />
                <h2 style={{ fontSize: 16, fontWeight: 700 }}>Alerts</h2>
                {unreadAlerts.length > 0 && (
                  <div
                   style={{
                    background: 'var(--red)',
                    color: '#fff',
                    borderRadius: '50%',
                    width: 18, height: 18,
                    fontSize: 10, fontWeight: 700,
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                  }}>
                    {unreadAlerts.length}
                  </div>
                )}
              </div>
              <Link to="/recommendations" style={{ fontSize: 12, color: 'var(--accent)' }}>
                View All
              </Link>
            </div>
            {alerts.length === 0 ? <p style={{ fontSize: 13, color: 'var(--text-muted)', textAlign: 'center', padding: '16px 0' }}>
                  No alerts
                </p>
                : <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
                  {alerts.slice(0, 4).map(a => (
                    <div key={a.id} style={{
                      padding: '10px 12px',
                      borderRadius: 'var(--radius-sm)',
                      background: a.isRead ? 'var(--bg-secondary)' : alertSeverityColor[a.severity] + '10',
                      borderRight: `3px solid ${a.isRead ? 'var(--border)' : alertSeverityColor[a.severity]}`,
                      opacity: a.isRead ? 0.6 : 1,
                    }}>
                      <div style={{ marginBottom: 4 }}>
                        <Badge color={alertSeverityColor[a.severity]}>
                          {alertSeverityLabel[a.severity]}
                        </Badge>
                      </div>
                      <p style={{ fontSize: 12, color: 'var(--text-secondary)', lineHeight: 1.5 }}>
                        {a.message}
                      </p>
                    </div>
                  ))}
                </div>
            }
          </Card>

          {/* Latest Body Metrics */}
          {latestMetrics && (<Card className="fade-in-4">
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 14 }}>
                <h2 style={{ fontSize: 16, fontWeight: 700 }}>Body Metrics</h2>
                <Link to="/metrics" style={{ fontSize: 12, color: 'var(--accent)' }}>History</Link>
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 12 }}>
                {[
                  { label: 'Weight', value: latestMetrics.weight, unit: 'kg', color: 'var(--orange)' },
                  { label: 'BMI', value: latestMetrics.bmi, unit: '', color: 'var(--purple)' },
                  { label: 'Resting HR', value: latestMetrics.restingHeartRate, unit: 'bpm', color: 'var(--red)' },
                ].map(m => (
                  <div key={m.label} style={{ textAlign: 'center' }}>
                    <div style={{ fontSize: 20, fontWeight: 700, color: m.color }}>
                      {m.value}
                      <span style={{ fontSize: 11, color: 'var(--text-muted)', marginLeft: 2 }}>{m.unit}</span>
                    </div>
                    <div style={{ fontSize: 11, color: 'var(--text-muted)', marginTop: 2 }}>{m.label}</div>
                  </div>
                ))}
              </div>
            </Card>
          )}

        </div>
      </div>
    </div>
  )
}