import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { authService } from '../services/authService'
import { metricsService } from '../services/metricsService'
import { Button, Input } from '../components/ui'
import { Zap } from 'lucide-react'


/// Registration page component
// Allows users to create a new account by providing their details
// Handles form submission, displays errors, and redirects on success
export function RegisterPage() {
  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    dateOfBirth: '',
  })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const set = (k: keyof typeof form) =>(e: React.ChangeEvent<HTMLInputElement>) => setForm(prev => ({ ...prev, [k]: e.target.value }))

  const handleSubmit = async (e: React.SyntheticEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const auth = await authService.register(form)
      localStorage.setItem('traqtiv_token', auth.token)
      const profile = await metricsService.getProfile()
      login(auth.token, profile)
      navigate('/')
    } catch (error: any) {
      if (error.response?.data?.message) {
        setError(error.response.data.message)
      } else {
        setError('An error occurred while creating your account. Please try again.')
      }
    } finally {
      setLoading(false)
    }
  }
  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      background: 'var(--bg-primary)',
      padding: 20,
    }}>
      <div style={{ width: '100%', maxWidth: 420 }} className="fade-in">

        
       {/* Logo */}
              <div style={{ padding: '20px 20px 16px', borderBottom: '1px solid var(--border)' }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                      <img
                          src="/appicon.png"
                          style={{ width: 32, height: 32, borderRadius: 8, objectFit: 'contain' }} />
                      <div>
                          <div style={{ fontWeight: 800, fontSize: 20, color: 'var(--text-primary)' }}>
                              traqtiv
                          </div>
                          <div style={{ fontSize: 10, color: 'var(--text-muted)', letterSpacing: '0.08em' }}>
                              TRACK. ACTIV. FITNESS.
                          </div>
                      </div>
                  </div>
              </div>

        {/* Form Card */}
        <div style={{
          background: 'var(--bg-card)',
          border: '1px solid var(--border)',
          borderRadius: 'var(--radius)',
          padding: 32,
        }}>
          <h2 style={{ fontSize: 20, fontWeight: 700, marginBottom: 24 }}>Create Account</h2>

          <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 14 }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <Input label="First Name" value={form.firstName} onChange={set('firstName')} required />
              <Input label="Last Name" value={form.lastName} onChange={set('lastName')} required />
            </div>
            <Input label="Email" type="email" value={form.email} onChange={set('email')} required />
            <Input label="Password" type="password" value={form.password} onChange={set('password')} required />
            <Input label="Date of Birth" type="date" value={form.dateOfBirth} onChange={set('dateOfBirth')} required />

            {error && 
            (
              <div style={{
                background: 'var(--red-dim)',
                border: '1px solid var(--red)',
                borderRadius: 'var(--radius-sm)',
                padding: '10px 14px',
                fontSize: 13,
                color: 'var(--red)',
              }}>
                {error}
              </div>
            )}

            <Button
              type="submit"
              loading={loading}
              style={{ width: '100%', justifyContent: 'center', marginTop: 4 }}
            >
              Create Account
            </Button>
          </form>

          <p style={{ fontSize: 13, color: 'var(--text-muted)', textAlign: 'center', marginTop: 20 }}>
            Already have an account?{' '}
            <Link to="/login" style={{ color: 'var(--accent)', fontWeight: 600 }}>
              Sign In
            </Link>
          </p>
        </div>

      </div>
    </div>
  )
}