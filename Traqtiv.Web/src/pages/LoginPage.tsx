import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { authService } from '../services/authService'
import { metricsService } from '../services/metricsService'
import { Button, Input } from '../components/ui'
import { Zap } from 'lucide-react'
// Login page component with form handling and authentication logic
// This component provides a login form for users to enter their email and password.
//  It handles form submission, calls the authentication service, and manages loading and error states.
//  Upon successful login, it stores the token and user profile, then navigates to the home page.
export function LoginPage() {
  const [email, setEmail] = useState('') 
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.SyntheticEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const auth = await authService.login({ email, password })
      localStorage.setItem('traqtiv_token', auth.token)
      const profile = await metricsService.getProfile()
      login(auth.token, profile)
      navigate('/')
    } catch {
      setError('Invalid email or password')
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
      <div style={{ width: '100%', maxWidth: 400 }} className="fade-in">

        {/* Logo */}
        <div style={{ textAlign: 'center', marginBottom: 40 }}>
          <div style={{
            width: 56, height: 56,
            background: 'var(--accent)',
            borderRadius: 14,
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            margin: '0 auto 16px',
          }}>
            <Zap size={28} color="#0a0c0f" fill="#0a0c0f" />
          </div>
          <h1 style={{ fontSize: 32, fontWeight: 800, color: 'var(--text-primary)' }}>
            traqtiv
          </h1>
          <p style={{ fontSize: 13, color: 'var(--text-muted)', marginTop: 4, letterSpacing: '0.08em' }}>
            TRACK. ACTIV. FITNESS.
          </p>
        </div>

        {/* Form Card */}
        <div style={{
          background: 'var(--bg-card)',
          border: '1px solid var(--border)',
          borderRadius: 'var(--radius)',
          padding: 32,
        }}>
          <h2 style={{ fontSize: 20, fontWeight: 700, marginBottom: 24 }}>Sign In</h2>

          <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
            <Input
              label="Email"
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              placeholder="you@example.com"
              required
            />
            <Input
              label="Password"
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              placeholder="••••••••"
              required
            />

            {error && (
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
              style={{ width: '100%', marginTop: 4 }}
            >
              Sign In
            </Button>
          </form>

          <p style={{ fontSize: 13, color: 'var(--text-muted)', textAlign: 'center', marginTop: 20 }}>
            Don't have an account?{' '}
            <Link to="/register" style={{ color: 'var(--accent)', fontWeight: 600 }}>
              Sign Up
            </Link>
          </p>
        </div>

      </div>
    </div>
  )
}