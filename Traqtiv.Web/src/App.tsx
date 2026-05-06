

// App.tsx
// Main application component
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { AuthProvider, useAuth } from './context/AuthContext'
import { Layout } from './components/layout/Layout'
import { LoginPage } from './pages/LoginPage'
import { RegisterPage } from './pages/RegisterPage'
import { DashboardPage } from './pages/DashboardPage'
import { WorkoutsPage } from './pages/WorkoutsPage'
import { ActivityPage } from './pages/ActivityPage'
import { MetricsPage } from './pages/MetricsPage'
import { RecommendationsPage } from './pages/RecommendationsPage'
import { ProfilePage } from './pages/ProfilePage'






// Create a QueryClient instance with default options
const queryClient = new QueryClient({defaultOptions: { queries: { retry: 1, staleTime: 30_000 } },}) 

// A wrapper for protected routes
// Redirects to login if not authenticated
function PrivateRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAuth()
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />
}

function AppRoutes() {
  const { isAuthenticated } = useAuth()
  return (
    <Routes>
      <Route path="/login" element={isAuthenticated ? <Navigate to="/" replace /> : <LoginPage />} />
      <Route path="/register" element={isAuthenticated ? <Navigate to="/" replace /> : <RegisterPage />} />
      <Route path="/" element={<PrivateRoute><Layout><DashboardPage /></Layout></PrivateRoute>} />
      <Route path="/workouts" element={<PrivateRoute><Layout><WorkoutsPage /></Layout></PrivateRoute>} />
      <Route path="/activity" element={<PrivateRoute><Layout><ActivityPage /></Layout></PrivateRoute>} />
      <Route path="/metrics" element={<PrivateRoute><Layout><MetricsPage /></Layout></PrivateRoute>} />
      <Route path="/recommendations" element={<PrivateRoute><Layout><RecommendationsPage /></Layout></PrivateRoute>} />
      <Route path="/profile" element={<PrivateRoute><Layout><ProfilePage /></Layout></PrivateRoute>} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}

// Main App component
// Wraps the app with QueryClientProvider and AuthProvider
// Sets up routing with BrowserRouter
// Handles authentication and protected routes
// Provides a clean structure for the application with separation of concerns
export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </AuthProvider>
    </QueryClientProvider>
  )
}