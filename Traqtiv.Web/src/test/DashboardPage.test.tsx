import { describe, test, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

const mockUseWorkouts = vi.fn()
const mockUseAuth = vi.fn()

vi.mock('../hooks', () => ({
  useWorkouts: () => mockUseWorkouts(),
  useDailyActivity: () => ({ data: [], isLoading: false, isError: false }),
  useAlerts: () => ({ data: [], isLoading: false, isError: false }),
  useWeather: () => ({ data: null }),
  useBodyMetrics: () => ({ data: [], isLoading: false }),
}))

vi.mock('../context/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}))

import { DashboardPage } from '../pages/DashboardPage'

describe('DashboardPage', () => {
  test('shows greeting with user name', () => {
    mockUseAuth.mockReturnValue({ user: { firstName: 'Gil', lastName: 'Moshe' } })
    mockUseWorkouts.mockReturnValue({ data: [], isLoading: false, isError: false })
    render(<MemoryRouter><DashboardPage /></MemoryRouter>)
    expect(screen.getByText('Hello, Gil 👋')).toBeInTheDocument()
  })

  test('shows error when workouts fail to load', () => {
    mockUseAuth.mockReturnValue({ user: { firstName: 'Gil', lastName: 'Moshe' } })
    mockUseWorkouts.mockReturnValue({ data: [], isLoading: false, isError: true })
    render(<MemoryRouter><DashboardPage /></MemoryRouter>)
    expect(screen.getByText('Unable to load workout data')).toBeInTheDocument()
  })
})