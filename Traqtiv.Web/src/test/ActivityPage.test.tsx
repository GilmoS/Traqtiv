import { describe, test, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'

// מדמה את ה-hooks
const mockUseDailyActivity = vi.fn()

vi.mock('../hooks', () => ({
  useDailyActivity: () => mockUseDailyActivity(),
  useAddActivity: () => ({ mutateAsync: vi.fn(), isPending: false }),
}))

import { ActivityPage } from '../pages/ActivityPage'

describe('ActivityPage', () => {
  test('shows empty state when no activities', () => {
    mockUseDailyActivity.mockReturnValue({ data: [], isLoading: false, isError: false })
    render(<ActivityPage />)
    expect(screen.getByText('No activity data yet')).toBeInTheDocument()
  })

  test('shows error message when server is unavailable', () => {
    mockUseDailyActivity.mockReturnValue({ data: [], isLoading: false, isError: true })
    render(<ActivityPage />)
    expect(screen.getByText('Unable to connect to server. Please try again later.')).toBeInTheDocument()
  })
})