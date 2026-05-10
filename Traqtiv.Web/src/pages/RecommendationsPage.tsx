import { useState } from 'react'
import { Bell, Lightbulb, CheckCheck, Cloud, TrendingUp, AlertTriangle } from 'lucide-react'
import { useRecommendations, useAlerts, useMarkAlertRead } from '../hooks'
import { Badge, Button, Spinner, EmptyState } from '../components/ui'
import { alertSeverityColor, alertSeverityLabel, recommendationTypeLabel, formatDateTime } from '../utils'
import { AlertSeverity, RecommendationType } from '../types'
// this page will show both recommendations and alerts, with tabs to switch between them. It will also allow users to mark alerts as read.
// The recommendations and alerts will be fetched from the backend using the useRecommendations and useAlerts hooks.
//  The markRead function will be used to mark alerts as read when the user clicks the "Mark as Read" button.
//  The page will also show a badge with the number of unread alerts on the "Alerts" tab.

// Map recommendation types to colors
const recColors = {
  [RecommendationType.Weather]: 'var(--blue)',
  [RecommendationType.Overload]: 'var(--orange)',
  [RecommendationType.Inactivity]: 'var(--yellow)',
}

// Map recommendation types to icons
const recIcons = {
  [RecommendationType.Weather]: <Cloud size={16} />,
  [RecommendationType.Overload]: <TrendingUp size={16} />,
  [RecommendationType.Inactivity]: <AlertTriangle size={16} />,
}

//this function will render the recommendations and alerts in a tabbed interface.
//  It will show a loading spinner while the data is being fetched, and an empty state if there are no recommendations or alerts to show.
//  The user can switch between the "Alerts" and "Recommendations" tabs, and mark alerts as read.
export function RecommendationsPage() {
  const { data: recommendations = [], isLoading: rLoading , isError: rError } = useRecommendations() 
  const { data: alerts = [], isLoading: aLoading , isError: aError } = useAlerts()
  const markRead = useMarkAlertRead()
  const [tab, setTab] = useState<'alerts' | 'recommendations'>('alerts')

  const unreadAlerts = alerts.filter(a => !a.isRead)
  const highAlerts = alerts.filter(a => a.severity === AlertSeverity.High && !a.isRead)
  
  return (
    <div style={{ padding: '28px 32px', maxWidth: 900 }}>

      {/* Header */}
      <div 
        className="fade-in" style={{ marginBottom: 28 }}>
        <h1 style={{ fontSize: 26, fontWeight: 700 }}>Recommendations & Alerts</h1>
        <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
          {unreadAlerts.length} unread alerts
          {highAlerts.length > 0 && (
            <span style={{ color: 'var(--red)', marginLeft: 8 }}>
              • {highAlerts.length} critical
            </span>
          )}
        </p>
      </div>

      {/* Tabs */}
      <div
         className="fade-in-1" style={{
        display: 'flex', gap: 4, marginBottom: 20,
        background: 'var(--bg-card)', padding: 4,
        borderRadius: 'var(--radius-sm)', width: 'fit-content',
        border: '1px solid var(--border)',
      }}>
        {(['alerts', 'recommendations'] as const).map(t => (
          <button key={t} onClick={() => setTab(t)} style={{
            padding: '7px 18px', borderRadius: 6,
            fontSize: 13, fontWeight: 600, cursor: 'pointer',
            background: tab === t ? 'var(--bg-secondary)' : 'transparent',
            border: 'none',
            color: tab === t ? 'var(--text-primary)' : 'var(--text-muted)',
            transition: 'all 0.15s',
          }}>
            {t === 'alerts' ? `Alerts (${alerts.length})` : `Recommendations (${recommendations.length})`}
          </button>
        ))}
      </div>

      {/* Alerts Tab */}
      {tab === 'alerts' && (<div className="fade-in" style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {aLoading ? <div style={{ display: 'flex', justifyContent: 'center', padding: 60 }}><Spinner size={28} /></div>
            : aError
            ? <div style={{
            padding: '16px 20px',
            background: 'var(--red-dim)',
            border: '1px solid var(--red)',
            borderRadius: 'var(--radius-sm)',
            color: 'var(--red)',
            fontSize: 14,
            }}>
              Unable to connect to server. Please try again later.
            </div>
            : alerts.length === 0 ? <EmptyState icon={<Bell />} message="No alerts — the system is monitoring you!" />
              : [...alerts].sort((a, b) => { if (a.isRead !== b.isRead) return a.isRead ? 1 : -1
                    return b.severity - a.severity
                  })
                  .map(alert => (
                    <div
                         key={alert.id} style={{
                      padding: '16px 20px',
                      background: 'var(--bg-card)',
                      border: '1px solid var(--border)',
                      borderRadius: 'var(--radius)',
                      borderLeft: `4px solid ${alert.isRead ? 'var(--border)' : alertSeverityColor[alert.severity]}`,
                      opacity: alert.isRead ? 0.6 : 1,
                      transition: 'opacity 0.2s',
                    }}>
                      <div
                         style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                        <div 
                          style={{ display: 'flex', gap: 12, alignItems: 'flex-start', flex: 1 }}>
                          <div style={{
                            width: 36, height: 36, borderRadius: 8, flexShrink: 0,
                            background: alertSeverityColor[alert.severity] + '18',
                            display: 'flex', alignItems: 'center', justifyContent: 'center',
                            color: alertSeverityColor[alert.severity],
                          }}>
                            <Bell size={16} />
                          </div>
                          <div 
                            style={{ flex: 1 }}>
                            <div style={{ display: 'flex', gap: 8, alignItems: 'center', marginBottom: 6 }}>
                              <Badge color={alertSeverityColor[alert.severity]}>
                                {alertSeverityLabel[alert.severity]}
                              </Badge>
                              <span style={{ fontSize: 11, color: 'var(--text-muted)' }}>
                                {formatDateTime(alert.createdAt)}
                              </span>
                            </div>
                            <p style={{ fontSize: 14, color: 'var(--text-secondary)', lineHeight: 1.6 }}>
                              {alert.message}
                            </p>
                          </div>
                        </div>
                        {!alert.isRead && (<Button variant="ghost" size="sm" onClick={() => markRead.mutate(alert.id)}
                            style={{ color: 'var(--accent)', flexShrink: 0 }}>
                            <CheckCheck size={14} /> Mark as Read
                          </Button>
                        )}
                      </div>
                    </div>
                  ))
          }
        </div>
      )}

      {/* Recommendations Tab */}
      {tab === 'recommendations' && (
        <div className="fade-in" style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {rLoading ?
           <div style={{ display: 'flex', justifyContent: 'center', padding: 60 }}><Spinner size={28} /></div>: recommendations.length === 0 ? <EmptyState icon={<Lightbulb />} message="No recommendations yet" />
            : rError
            ? <div style={{
              padding: '16px 20px',
              background: 'var(--red-dim)',
              border: '1px solid var(--red)',
              borderRadius: 'var(--radius-sm)',
              color: 'var(--red)',
              fontSize: 14,
               }}>
              Unable to connect to server. Please try again later.
              </div>  
              : [...recommendations].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()).map(rec => (
                    <div 
                        key={rec.id} style={{
                      padding: '16px 20px',
                      background: 'var(--bg-card)',
                      border: '1px solid var(--border)',
                      borderRadius: 'var(--radius)',
                      borderLeft: `4px solid ${rec.isRead ? 'var(--border)' : recColors[rec.type]}`,
                      opacity: rec.isRead ? 0.65 : 1,
                    }}>
                      <div style={{ display: 'flex', gap: 12, alignItems: 'flex-start' }}>
                        <div style={{
                          width: 36, height: 36, borderRadius: 8, flexShrink: 0,
                          background: recColors[rec.type] + '18',
                          display: 'flex', alignItems: 'center', justifyContent: 'center',
                          color: recColors[rec.type],
                        }}>
                          {recIcons[rec.type]}
                        </div>
                        <div>
                          <div 
                            style={{ display: 'flex', gap: 8, alignItems: 'center', marginBottom: 6 }}>
                            <Badge color={recColors[rec.type]}>
                              {recommendationTypeLabel[rec.type]}
                            </Badge>
                            <span style={{ fontSize: 11, color: 'var(--text-muted)' }}>
                              {formatDateTime(rec.createdAt)}
                            </span>
                          </div>
                          <p style={{ fontSize: 14, color: 'var(--text-secondary)', lineHeight: 1.6 }}>
                            {rec.message}
                          </p>
                        </div>
                      </div>
                    </div>
                  ))
          }
        </div>
      )}

    </div>
  )
}