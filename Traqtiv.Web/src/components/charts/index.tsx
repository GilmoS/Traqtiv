import { LineChart, Line, BarChart, Bar,XAxis, YAxis, CartesianGrid, Tooltip,ResponsiveContainer, Legend} from 'recharts'


const tooltipStyle = {background: 'var(--bg-card)',border: '1px solid var(--border)',borderRadius: 8,fontSize: 12,color: 'var(--text-primary)',}

const axisStyle = { fontSize: 11, fill: 'var(--text-muted)' }

// ─── Workout Chart ────────────────────────────────────────
// This chart shows workout duration and calories burned over time. 
// It uses a bar chart to compare the two metrics side by side for each time period (e.g., days or weeks). 
// The x-axis represents the time periods, while the y-axis shows the values for minutes and calories.
// The tooltip provides detailed information when hovering over each bar, and the legend helps differentiate between the two metrics.
export function WorkoutChart({ data }: {data: Array<{ label: string; durationMinutes: number; caloriesBurned: number }>}) 
{
  return (
    <ResponsiveContainer width="100%" height={200}>
      <BarChart data={data} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
        <CartesianGrid strokeDasharray="3 3" stroke="var(--border)" vertical={false} />
        <XAxis dataKey="label" tick={axisStyle} axisLine={false} tickLine={false} />
        <YAxis tick={axisStyle} axisLine={false} tickLine={false} />
        <Tooltip contentStyle={tooltipStyle} cursor={{ fill: 'var(--accent-dim)' }} />
        <Bar dataKey="durationMinutes" name="Minutes" fill="var(--orange)" radius={[4, 4, 0, 0]} />
        <Bar dataKey="caloriesBurned" name="Calories" fill="var(--blue)" radius={[4, 4, 0, 0]} />
        <Legend wrapperStyle={{ fontSize: 12, paddingTop: 8 }} />
      </BarChart>
    </ResponsiveContainer>
  )
}

// ─── Activity Chart ───────────────────────────────────────
// This chart tracks daily activity levels, showing both steps taken and active minutes. 
// It uses a line chart to display trends over time, with one line representing steps and another for active minutes. 
// The x-axis represents the time periods (e.g., days), while the y-axis shows the values for steps and minutes. 
// The tooltip provides detailed information when hovering over each point, and the legend helps differentiate between the two metrics.
export function ActivityChart({ data }: {data: Array<{ label: string; steps: number; activeMinutes: number }>})
{
  return (
    <ResponsiveContainer width="100%" height={200}>
      <LineChart data={data} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
        <CartesianGrid strokeDasharray="3 3" stroke="var(--border)" vertical={false} />
        <XAxis dataKey="label" tick={axisStyle} axisLine={false} tickLine={false} />
        <YAxis tick={axisStyle} axisLine={false} tickLine={false} />
        <Tooltip contentStyle={tooltipStyle} />
        <Line type="monotone" dataKey="steps" name="Steps" stroke="var(--accent)" strokeWidth={2} dot={{ fill: 'var(--accent)', r: 3 }} />
        <Line type="monotone" dataKey="activeMinutes" name="Active Minutes" stroke="var(--purple)" strokeWidth={2} dot={{ fill: 'var(--purple)', r: 3 }} />
        <Legend wrapperStyle={{ fontSize: 12, paddingTop: 8 }} />
      </LineChart>
    </ResponsiveContainer>
  )
}

// ─── Metrics Chart ────────────────────────────────────────
// This chart compares weight and BMI over time. 
// It uses a line chart to show trends for both metrics, with one line representing weight and another for BMI. 
// The x-axis represents the time periods (e.g., days or weeks), while the y-axis shows the values for weight and BMI. 
// The tooltip provides detailed information when hovering over each point, and the legend helps differentiate between the two metrics.
export function MetricsChart({ data }: {
  data: Array<{ label: string; weight: number; bmi: number }>
}) {
  return (
    <ResponsiveContainer width="100%" height={200}>
      <LineChart data={data} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
        <CartesianGrid strokeDasharray="3 3" stroke="var(--border)" vertical={false} />
        <XAxis dataKey="label" tick={axisStyle} axisLine={false} tickLine={false} />
        <YAxis tick={axisStyle} axisLine={false} tickLine={false} />
        <Tooltip contentStyle={tooltipStyle} />
        <Line type="monotone" dataKey="weight" name="Weight (kg)" stroke="var(--orange)" strokeWidth={2} dot={{ fill: 'var(--orange)', r: 3 }} />
        <Line type="monotone" dataKey="bmi" name="BMI" stroke="var(--purple)" strokeWidth={2} dot={{ fill: 'var(--purple)', r: 3 }} />
        <Legend wrapperStyle={{ fontSize: 12, paddingTop: 8 }} />
      </LineChart>
    </ResponsiveContainer>
  )
}