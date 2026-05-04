import { useState } from 'react'
import { Scale, Heart, BarChart2, Plus } from 'lucide-react'
import { useBodyMetrics, useAddMetrics } from '../hooks'
import { Card, StatCard, Button, Spinner, Modal, Input, EmptyState } from '../components/ui'
import { MetricsChart } from '../components/charts'
import { formatDate, prepareChartData } from '../utils'
import type { AddMetricsDto } from '../types'

export function MetricsPage() {
  const { data: metrics = [], isLoading } = useBodyMetrics()
  const addMutation = useAddMetrics()
  const [showModal, setShowModal] = useState(false)
  // Form state for adding new metrics
  // Default values can be adjusted as needed
  const [form, setForm] = useState<AddMetricsDto>({
    weight: 70,
    restingHeartRate: 60,
    bmi: 22,
  })

  // Handlers for form inputs
  // This function returns a change handler for each input field, updating the corresponding value in the form state
  const set = (k: keyof AddMetricsDto) =>(e: React.ChangeEvent<HTMLInputElement>) =>setForm(prev => ({ ...prev, [k]: Number(e.target.value) }))
 // Sort metrics by measuredAt date in descending order to get the latest entry
  const sorted = [...metrics].sort((a, b) => new Date(b.measuredAt).getTime() - new Date(a.measuredAt).getTime())

  const latest = sorted[0]

  // Prepare data for the chart, taking the latest 20 entries and reversing them for chronological order
  const chartData = prepareChartData(sorted.slice(0, 20).reverse(), 'measuredAt').map(m => ({
      label: m.label,
      weight: m.weight,
      bmi: m.bmi,
    }))
// Handler for adding new metrics, which calls the mutation and then closes the modal
  const handleAdd = async () => {await addMutation.mutateAsync(form),setShowModal(false)}



  return (
    <div style={{ padding: '28px 32px', maxWidth: 1100 }}>

      {/* Header */}
      <div 
        className="fade-in" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 28 }}>
        <div>
          <h1 style={{ fontSize: 26, fontWeight: 700 }}>Body Metrics</h1>
          <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
            {metrics.length} measurements recorded
          </p>
        </div>
        <Button onClick={() => setShowModal(true)}>
          <Plus size={16} /> New Measurement
        </Button>
      </div>

      {/* Latest Stats */}
      {latest && (<div className="fade-in-1" style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: 16, marginBottom: 24 }}>
          <StatCard label="Weight" value={latest.weight} unit="kg" icon={<Scale size={18} />} color="var(--orange)" />
          <StatCard label="BMI" value={latest.bmi} icon={<BarChart2 size={18} />} color="var(--purple)" />
          <StatCard label="Resting Heart Rate" value={latest.restingHeartRate} unit="bpm" icon={<Heart size={18} />} color="var(--red)" />
        </div>
      )}

      {/* Chart */}
      {chartData.length > 1 && (
        <Card className="fade-in-2" style={{ marginBottom: 20 }}>
          <h2 style={{ fontSize: 15, fontWeight: 700, marginBottom: 16 }}>Weight & BMI Trend</h2>
          <MetricsChart data={chartData} />
        </Card>
      )}

      {/* History */}
      <Card className="fade-in-3">
        <h2 style={{ fontSize: 15, fontWeight: 700, marginBottom: 16 }}>Measurement History</h2>
        {isLoading? <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}><Spinner size={28} /></div>
          : sorted.length === 0
            ? <EmptyState icon={<Scale />} message="No measurements yet" />
            : <div 
                style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                {sorted.map((m, i) => (
                  <div 
                    key={m.id} style={{
                    display: 'grid',
                    gridTemplateColumns: '160px 1fr 1fr 1fr',
                    gap: 12,
                    alignItems: 'center',
                    padding: '12px 16px',
                    background: 'var(--bg-secondary)',
                    borderRadius: 'var(--radius-sm)',
                    borderLeft: i === 0 ? '3px solid var(--accent)' : '3px solid transparent',
                  }}>
                    <span style={{ fontSize: 14, fontWeight: i === 0 ? 600 : 400 }}>
                      {formatDate(m.measuredAt)}
                      {i === 0 && (<span style={{ fontSize: 10, color: 'var(--accent)', marginLeft: 6 }}>
                          latest
                        </span>
                      )}
                    </span>
                    {[
                      { val: m.weight, unit: 'kg', color: 'var(--orange)', label: 'Weight' },
                      { val: m.bmi, unit: '', color: 'var(--purple)', label: 'BMI' },
                      { val: m.restingHeartRate, unit: 'bpm', color: 'var(--red)', label: 'Resting HR' },
                    ].map(item => (
                      <div 
                        key={item.label} style={{ textAlign: 'center' }}>
                        <div 
                            style={{ fontSize: 16, fontWeight: 700, color: item.color }}>
                          {item.val}
                          <span style={{ fontSize: 11, color: 'var(--text-muted)', marginLeft: 2 }}>{item.unit}</span>
                        </div>
                        <div style={{ fontSize: 10, color: 'var(--text-muted)' }}>{item.label}</div>
                      </div>
                    ))}
                  </div>
                ))}
              </div>
        }
      </Card>

      {/* Modal */}
      {showModal && (<Modal title="New Measurement" onClose={() => setShowModal(false)}>
          <div 
            style={{ display: 'flex', flexDirection: 'column', gap: 14 }}>
            <Input label="Weight (kg)" type="number" value={form.weight} onChange={set('weight')} step={0.1} min={1} />
            <Input label="BMI" type="number" value={form.bmi} onChange={set('bmi')} step={0.1} min={1} />
            <Input label="Resting Heart Rate (bpm)" type="number" value={form.restingHeartRate} onChange={set('restingHeartRate')} min={30} max={200} />
            <div 
              style={{ display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 8 }}>
              <Button variant="secondary" onClick={() => setShowModal(false)}>Cancel</Button>
              <Button onClick={handleAdd} loading={addMutation.isPending}>Save</Button>
            </div>
          </div>
        </Modal>
      )}

    </div>
  )
}