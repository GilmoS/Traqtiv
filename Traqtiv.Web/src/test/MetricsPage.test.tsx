import { describe, test, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'

const mockUseBodyMetrics = vi.fn()

vi.mock('../hooks', () => ({
  useBodyMetrics: () => mockUseBodyMetrics(),
  useAddMetrics: () => ({ mutateAsync: vi.fn(), isPending: false }),
}))

import { MetricsPage } from '../pages/MetricsPage'

describe('MetricsPage', () => {
  test('shows empty state when no measurements', () => {
    mockUseBodyMetrics.mockReturnValue({ data: [], isLoading: false, isError: false })
    render(<MetricsPage />)
    expect(screen.getByText('No measurements yet')).toBeInTheDocument()
  })

  test('shows error message when server is unavailable', () => {
    mockUseBodyMetrics.mockReturnValue({ data: [], isLoading: false, isError: true })
    render(<MetricsPage />)
    expect(screen.getByText('Unable to connect to server. Please try again later.')).toBeInTheDocument()
  })
})