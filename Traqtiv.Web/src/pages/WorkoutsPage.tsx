import { useState } from 'react'
import { Dumbbell, Plus, Trash2, Pencil } from 'lucide-react'
import { useWorkouts, useAddWorkout, useUpdateWorkout, useDeleteWorkout } from '../hooks'
import { Card, Button, Badge, Spinner, Modal, Input, Select, EmptyState } from '../components/ui'
import { WorkoutChart } from '../components/charts'
import { WorkoutType, WorkoutStatus } from '../types'
import type { AddWorkoutDto } from '../types'
import { workoutTypeLabel, workoutTypeColor, formatDate, prepareChartData } from '../utils'

// Initial form state for adding/editing workouts
// This ensures that when the modal is opened for adding a new workout, the form is reset to these default values.
// The date is set to today's date by default, and other fields have sensible defaults for a new workout entry.
const emptyForm: AddWorkoutDto = {
  type: WorkoutType.Strength,
  durationMinutes: 60,
  status: WorkoutStatus.Completed,
  caloriesBurned: 300,
  date: new Date().toISOString().split('T')[0],
  notes: '',
}

// The WorkoutsPage component is the main page for managing workouts.
//  It allows users to view their workout history, add new workouts, edit existing ones, and delete workouts.
//  It also provides filtering options to view workouts by status (completed, planned, missed) and displays a chart of workout trends over time.
export function WorkoutsPage() {
  const { data: workouts = [], isLoading, isError } = useWorkouts()
  const addMutation = useAddWorkout()
  const updateMutation = useUpdateWorkout()
  const deleteMutation = useDeleteWorkout()

  const [showModal, setShowModal] = useState(false)
  const [editId, setEditId] = useState<string | null>(null)
  const [form, setForm] = useState<AddWorkoutDto>(emptyForm)
  const [filter, setFilter] = useState<'all' | WorkoutStatus>('all')
    
  // The set function is a higher-order function that returns an event handler for updating the form state.
  // It takes a key of the AddWorkoutDto as an argument and returns a function that updates the corresponding field in the form state when an input changes.
  const set = (k: keyof AddWorkoutDto) =>(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => setForm(prev => ({ ...prev,[k]: ['type', 'status'].includes(k)
              ? Number(e.target.value): k === 'durationMinutes' || k === 'caloriesBurned'? Number(e.target.value): e.target.value}))

  // The openAdd function is called when the user clicks the "Add Workout" button.
  //  It resets the form to the emptyForm state, clears any editId (since we're adding a new workout), and opens the modal for adding a workout.
  const openAdd = () => { setForm(emptyForm),setEditId(null) ,setShowModal(true) } 
  
  // The openEdit function is called when the user clicks the edit button on a workout card.
  // It populates the form with the existing workout data, sets the editId to the ID of the workout being edited, and opens the modal for editing.
  const openEdit = (w: typeof workouts[0]) => {
    setForm({type: w.type,durationMinutes: w.durationMinutes,status: w.status,caloriesBurned: w.caloriesBurned,date: w.date.split('T')[0],notes: w.notes,})
    setEditId(w.id)
    setShowModal(true)
  }

  
// The handleSave function is called when the user clicks the "Save" button in the modal after adding or editing a workout.
// It checks if there is an editId to determine whether to call the update mutation (for editing) or the add mutation (for adding a new workout).
// After the mutation is completed, it closes the modal.
  const handleSave = async () => {
    if (form.durationMinutes <= 0) {
      alert('Duration must be greater than 0')
      return
   }
   if (form.status === WorkoutStatus.Completed && new Date(form.date) > new Date()) {
    alert('A completed workout cannot have a future date')
    return
  }
  if (editId) {
    await updateMutation.mutateAsync({ id: editId, data: form })
  } else
     {
      await addMutation.mutateAsync(form)
    }
  setShowModal(false)
}

  // The filtered variable holds the list of workouts to be displayed based on the selected filter.
  //  If the filter is set to 'all', it includes all workouts otherwise, it filters workouts by their status (completed, planned, or missed).
  const filtered = filter === 'all' ? workouts: workouts.filter(w => w.status === filter) 

  const sorted = [...filtered].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()) // The sorted variable holds the list of workouts sorted by date in descending order (most recent first).

  // This ensures that when workouts are displayed, the most recent workouts appear at the top of the list.
  const chartData = prepareChartData(workouts.filter(w => w.status === WorkoutStatus.Completed).slice(-14)).map(w => ({label: w.label,durationMinutes: w.durationMinutes,caloriesBurned: w.caloriesBurned,}))

return (
    <div style={{ padding: '28px 32px', maxWidth: 1200 }}>

      {/* Header */}
      <div 
      className="fade-in" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 28 }}>
        <div>
          <h1 style={{ fontSize: 26, fontWeight: 700 }}>Workouts</h1>
          <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
            {workouts.length} workouts logged
          </p>
        </div>
        <Button onClick={openAdd}>
          <Plus size={16} /> New Workout
        </Button>
      </div>

      {/* Chart */}
      {chartData.length > 1 && (<Card className="fade-in-1" style={{ marginBottom: 20 }}>
          <h2 style={{ fontSize: 15, fontWeight: 700, marginBottom: 16 }}>Performance Over Time</h2>
          <WorkoutChart data={chartData} />
        </Card>
      )}

      {/* Filter Buttons */}
      <div 
      className="fade-in-2" style={{ display: 'flex', gap: 8, marginBottom: 16 }}>
        {(['all', WorkoutStatus.Completed, WorkoutStatus.Planned] as const).map(f => (
          <button
            key={f}
            onClick={() => setFilter(f)}
            style={{
              padding: '6px 16px',
              borderRadius: 20,
              fontSize: 13,
              fontWeight: 600,
              cursor: 'pointer',
              border: '1px solid',
              transition: 'all 0.15s',
              borderColor: filter === f ? 'var(--accent)' : 'var(--border)',
              background: filter === f ? 'var(--accent-dim)' : 'transparent',
              color: filter === f ? 'var(--accent)' : 'var(--text-secondary)',
            }}
          >
            {f === 'all' ? 'All' : f === WorkoutStatus.Completed ? 'Completed' : 'Planned'}
          </button>
        ))}
      </div>

      {/* Workouts List */}
      <div 
        className="fade-in-3" style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
        {isLoading ? <div style={{ display: 'flex', justifyContent: 'center', padding: 60 }}><Spinner size={28} /></div>
        :isError ?  <div style={{
            padding: '16px 20px',
          background: 'var(--red-dim)',
          border: '1px solid var(--red)',
          borderRadius: 'var(--radius-sm)',
          color: 'var(--red)',
          fontSize: 14,
          }}>
        Unable to connect to server. Please try again later.
        </div>
          : sorted.length === 0 ? <EmptyState icon={<Dumbbell />} message="No workouts yet. Click 'New Workout' to get started!" />
          
            : sorted.map(w => (
              <div
                key={w.id} style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                padding: '14px 18px',
                background: 'var(--bg-card)',
                border: '1px solid var(--border)',
                borderRadius: 'var(--radius-sm)',
                borderLeft: `3px solid ${workoutTypeColor[w.type]}`,
              }}>
                <div
                     style={{ display: 'flex', alignItems: 'center', gap: 16 }}>
                  <div>
                    <div
                         style={{ fontSize: 15, fontWeight: 600 }}>{workoutTypeLabel[w.type]}</div>
                    <div
                         style={{ fontSize: 12, color: 'var(--text-muted)', marginTop: 2 }}>{formatDate(w.date)}</div>
                  </div>
                </div>
                <div
                     style={{ display: 'flex', gap: 20, alignItems: 'center' }}>
                  <div
                     style={{ textAlign: 'center' }}>
                    <div style={{ fontSize: 16, fontWeight: 700 }}>{w.durationMinutes}</div>
                    <div
                         style={{ fontSize: 10, color: 'var(--text-muted)' }}>min</div>
                  </div>
                  <div 
                         style={{ textAlign: 'center' }}>
                    <div style={{ fontSize: 16, fontWeight: 700, color: 'var(--red)' }}>{w.caloriesBurned}</div>
                    <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>kcal</div>
                  </div>
                  <Badge color={w.status === WorkoutStatus.Completed ? 'var(--accent)' : 'var(--yellow)'}>
                    {w.status === WorkoutStatus.Completed ? 'Completed' : 'Planned'}
                  </Badge>
                  <div 
                    style={{ display: 'flex', gap: 6 }}>
                    <Button variant="ghost" size="sm" onClick={() => openEdit(w)}>
                      <Pencil size={14} />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => {
                        if (window.confirm('Are you sure you want to delete this workout?')) {
                          deleteMutation.mutate(w.id)
                        }
                      }}
                      style={{ color: 'var(--red)' }}>
                      <Trash2 size={14} />
                    </Button>
                  </div>
                </div>
              </div>
            ))
        }
      </div>

      {/* Modal */}
      {showModal && (<Modal title={editId ? 'Edit Workout' : 'New Workout'} onClose={() => setShowModal(false)}>
          <div 
            style={{ display: 'flex', flexDirection: 'column', gap: 14 }}>
            <Select label="Type" value={form.type} onChange={set('type')}>
              <option value={WorkoutType.Strength}>Strength</option>
              <option value={WorkoutType.Cardio}>Cardio</option>
              <option value={WorkoutType.Flexibility}>Flexibility</option>
            </Select>
            <Select label="Status" value={form.status} onChange={set('status')}>
              <option value={WorkoutStatus.Completed}>Completed</option>
              <option value={WorkoutStatus.Planned}>Planned</option>
            </Select>
            <div 
                style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <Input label="Duration (minutes)" type="number" value={form.durationMinutes} onChange={set('durationMinutes')} min={1} />
              <Input label="Calories Burned" type="number" value={form.caloriesBurned} onChange={set('caloriesBurned')} min={0} />
            </div>
            <Input label="Date" type="date" value={form.date} onChange={set('date')} />
            <div>
              <label style={{ fontSize: 13, fontWeight: 600, color: 'var(--text-secondary)', display: 'block', marginBottom: 6 }}>
                Notes
              </label>
              <textarea
                value={form.notes}
                onChange={set('notes') as any}
                rows={3}
                style={{
                  width: '100%',
                  background: 'var(--bg-input)',
                  border: '1px solid var(--border)',
                  borderRadius: 'var(--radius-sm)',
                  padding: '9px 14px',
                  color: 'var(--text-primary)',
                  fontSize: 14,
                  outline: 'none',
                  resize: 'vertical',
                }}
              />
            </div>
            <div 
                style={{ display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 8 }}>
              <Button variant="secondary" onClick={() => setShowModal(false)}>Cancel</Button>
              <Button onClick={handleSave} loading={addMutation.isPending || updateMutation.isPending}>
                {editId ? 'Save' : 'Add'}
              </Button>
            </div>
          </div>
        </Modal>
      )}

    </div>
  )
}