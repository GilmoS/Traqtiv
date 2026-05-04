import { useState } from 'react'
import { Footprints, Plus, Flame, Clock, MapPin } from 'lucide-react'
import { useDailyActivity, useAddActivity } from '../hooks'
import { Card, StatCard, Button, Spinner, Modal, Input, EmptyState } from '../components/ui'
import { ActivityChart } from '../components/charts'
import { formatDate, prepareChartData, todayISO } from '../utils'
import type { AddDailyActivityDto } from '../types'

/// The ActivityPage component displays a list of daily activities, allows users to add new activities, and shows a summary of total steps, calories burned, active minutes, and distance covered.
//  It also includes a chart to visualize the activity data over time.

// The component uses custom hooks to fetch and manage activity data, and it provides a modal form for adding new activities.
//  The activity data is displayed in a sorted list, and the chart is prepared using a utility function to format the data appropriately for visualization.
const emptyForm: AddDailyActivityDto = {
  steps: 0,
  caloriesBurned: 0,
  activeMinutes: 0,
  distanceKm: 0,
  date: todayISO(),
}

// The ActivityPage component is responsible for displaying the user's daily activity data, allowing them to add new activities, and providing a visual representation of their activity trends over time.
//  It uses custom hooks to manage data fetching and mutations, and it includes a modal form for adding new activity entries.
//  The component also calculates total steps, calories burned, active minutes, and distance covered, and it prepares data for the activity chart.
export function ActivityPage() {
  const { data: activities = [], isLoading } = useDailyActivity()
  const addMutation = useAddActivity()
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState<AddDailyActivityDto>(emptyForm)
  // The set function is a higher-order function that returns an event handler for updating the form state based on the input field being changed.
  //  It takes a key of the AddDailyActivityDto type and returns a function that updates the corresponding value in the form state when the input changes.
  const set = (k: keyof AddDailyActivityDto) =>(e: React.ChangeEvent<HTMLInputElement>) =>setForm(prev => ({...prev,[k]: k === 'date' ? e.target.value : Number(e.target.value)}))

  // Calculate total steps, calories burned, active minutes, and distance covered by summing up the respective values from all activities.
  const sorted = [...activities].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())

  const totalSteps = activities.reduce((s, a) => s + a.steps, 0)
  const totalCals = activities.reduce((s, a) => s + a.caloriesBurned, 0)
  const totalMins = activities.reduce((s, a) => s + a.activeMinutes, 0)
  const totalDist = activities.reduce((s, a) => s + a.distanceKm, 0)

// The chartData variable is prepared by sorting the activities by date, taking the most recent 14 entries, reversing the order for chronological display, and mapping the data to a format suitable for the ActivityChart component.
  const chartData = prepareChartData(sorted.slice(0, 14).reverse()).map(a => ({label: a.label,steps: a.steps,activeMinutes: a.activeMinutes,}))

// The handleAdd function is an asynchronous function that handles the submission of the new activity form.
//  It calls the addMutation to add the new activity, then closes the modal and resets the form state to the empty form.
  const handleAdd = async () => {await addMutation.mutateAsync(form),setShowModal(false),setForm(emptyForm)}


  return (
    <div style={{ padding: '28px 32px', maxWidth: 1100 }}>

      {/* Header */}
      <div className="fade-in" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 28 }}>
        <div>
          <h1 style={{ fontSize: 26, fontWeight: 700 }}>Daily Activity</h1>
          <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
            {activities.length} days tracked
          </p>
        </div>
        <Button onClick={() => setShowModal(true)}>
          <Plus size={16} /> Add Activity
        </Button>
      </div>

      {/* Totals */}
      <div className="fade-in-1" style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: 16, marginBottom: 24 }}>
        <StatCard label="Total Steps" value={totalSteps.toLocaleString()} icon={<Footprints size={18} />} color="var(--accent)" />
        <StatCard label="Total Calories" value={totalCals.toLocaleString()} unit="kcal" icon={<Flame size={18} />} color="var(--red)" />
        <StatCard label="Total Active Minutes" value={totalMins.toLocaleString()} unit="min" icon={<Clock size={18} />} color="var(--blue)" />
        <StatCard label="Total Distance" value={totalDist.toFixed(1)} unit="km" icon={<MapPin size={18} />} color="var(--purple)" />
      </div>

      {/* Chart */}
      {chartData.length > 1 && (
        <Card className="fade-in-2" style={{ marginBottom: 20 }}>
          <h2 style={{ fontSize: 15, fontWeight: 700, marginBottom: 16 }}>Last 14 Days</h2>
          <ActivityChart data={chartData} />
        </Card>
      )}

      {/* History List */}
      <Card className="fade-in-3">
        <h2 style={{ fontSize: 15, fontWeight: 700, marginBottom: 16 }}>History</h2>
        {isLoading
          ? <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}><Spinner size={28} /></div>
          : sorted.length === 0
            ? <EmptyState icon={<Footprints />} message="No activity data yet" />
            : <div 
                style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                {sorted.map(a => (<div key={a.id} style={{
                    display: 'grid',
                    gridTemplateColumns: '140px 1fr 1fr 1fr 1fr',
                    gap: 12,
                    alignItems: 'center',
                    padding: '12px 16px',
                    background: 'var(--bg-secondary)',
                    borderRadius: 'var(--radius-sm)',
                  }}>
                    <span style={{ fontSize: 14, fontWeight: 600 }}>{formatDate(a.date)}</span>
                    <div 
                        style={{ textAlign: 'center' }}>
                      <div style={{ fontSize: 15, fontWeight: 700, color: 'var(--accent)' }}>{a.steps.toLocaleString()}</div>
                      <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>steps</div>
                    </div>
                    <div 
                    style={{ textAlign: 'center' }}>
                      <div style={{ fontSize: 15, fontWeight: 700, color: 'var(--red)' }}>{a.caloriesBurned}</div>
                      <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>kcal</div>
                    </div>
                    <div style={{ textAlign: 'center' }}>
                      <div style={{ fontSize: 15, fontWeight: 700, color: 'var(--blue)' }}>{a.activeMinutes}</div>
                      <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>min</div>
                    </div>
                    <div
                     style={{ textAlign: 'center' }}>
                      <div style={{ fontSize: 15, fontWeight: 700, color: 'var(--purple)' }}>{a.distanceKm.toFixed(1)}</div>
                      <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>km</div>
                    </div>
                  </div>
                ))}
              </div>
        }
      </Card>

      {/* Modal */}
      {showModal && (<Modal title="Add Activity" onClose={() => setShowModal(false)}>
          <div 
            style={{ display: 'flex', flexDirection: 'column', gap: 14 }}>
            <Input label="Date" type="date" value={form.date} onChange={set('date')} />
            <div 
               style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <Input label="Steps" type="number" value={form.steps} onChange={set('steps')} min={0} />
              <Input label="Calories Burned" type="number" value={form.caloriesBurned} onChange={set('caloriesBurned')} min={0} />
              <Input label="Active Minutes" type="number" value={form.activeMinutes} onChange={set('activeMinutes')} min={0} />
              <Input label="Distance (km)" type="number" value={form.distanceKm} onChange={set('distanceKm')} min={0} step={0.1} />
            </div>
            <div
               style={{ display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 8 }}>
              <Button variant="secondary" onClick={() => setShowModal(false)}>Cancel</Button>
              <Button onClick={handleAdd} loading={addMutation.isPending}>Add</Button>
            </div>
          </div>
        </Modal>
      )}

    </div>
  )
}