import { createContext, useContext, useState, useCallback, type ReactNode } from 'react'
import type { UserProfileDto } from '../types'

// Create the AuthContext with default values
// The default values are placeholders 
interface AuthState {
  token: string | null
  user: UserProfileDto | null
  isAuthenticated: boolean
}

// Define the shape of the AuthContext, including state and actions
// The AuthContextType extends AuthState to include the login, logout, and updateUser functions
interface AuthContextType extends AuthState {
  login: (token: string, user: UserProfileDto) => void
  logout: () => void
  updateUser: (user: UserProfileDto) => void
}

// Create the AuthContext with a default value of null
const AuthContext = createContext<AuthContextType | null>(null)

// AuthProvider component that wraps the application and provides the AuthContext
// The AuthProvider manages the authentication state and provides functions to log in, log out, and update the user profile
export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>(() => {
    const token = localStorage.getItem('traqtiv_token')
    const userStr = localStorage.getItem('traqtiv_user')
    const user = userStr ? JSON.parse(userStr) as UserProfileDto : null
    return { token, user, isAuthenticated: !!token }
  })

// The login function saves the token and user profile to localStorage and updates the state
  const login = useCallback((token: string, user: UserProfileDto) => {
    localStorage.setItem('traqtiv_token', token)
    localStorage.setItem('traqtiv_user', JSON.stringify(user))
    setState({ token, user, isAuthenticated: true })
  }, [])
// The logout function removes the token and user profile from localStorage and resets the state
  const logout = useCallback(() => {
    localStorage.removeItem('traqtiv_token')
    localStorage.removeItem('traqtiv_user')
    setState({ token: null, user: null, isAuthenticated: false })
  }, [])
// The updateUser function updates the user profile in localStorage and state without affecting the token or authentication status
  const updateUser = useCallback((user: UserProfileDto) => {
    localStorage.setItem('traqtiv_user', JSON.stringify(user))
    setState(prev => ({ ...prev, user }))
  }, [])

  return (
    <AuthContext.Provider value={{ ...state, login, logout, updateUser }}>
      {children}
    </AuthContext.Provider>
  )
}

// Custom hook to use the AuthContext
// This hook provides an easy way to access the authentication state and actions from any component within the AuthProvider
export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be inside AuthProvider')
  return ctx
}