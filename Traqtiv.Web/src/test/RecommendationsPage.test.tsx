import { describe, test, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'

const mockUseAlerts = vi.fn()
const mockUseRecommendations = vi.fn()

vi.mock('../hooks', () => ({
  useAlerts: () => mockUseAlerts(),
  useRecommendations: () => mockUseRecommendations(),
  useMarkAlertRead: () => ({ mutate: vi.fn() }),
}))

import { RecommendationsPage } from '../pages/RecommendationsPage'

describe('RecommendationsPage', () => {
  test('shows empty alerts state', () => {
    mockUseAlerts.mockReturnValue({ data: [], isLoading: false, isError: false })
    mockUseRecommendations.mockReturnValue({ data: [], isLoading: false, isError: false })
    render(<RecommendationsPage />)
    expect(screen.getByText('No alerts — the system is monitoring you!')).toBeInTheDocument()
  })

  test('shows error when alerts fail to load', () => {
    mockUseAlerts.mockReturnValue({ data: [], isLoading: false, isError: true })
    mockUseRecommendations.mockReturnValue({ data: [], isLoading: false, isError: false })
    render(<RecommendationsPage />)
    expect(screen.getByText('Unable to connect to server. Please try again later.')).toBeInTheDocument()
  })
})