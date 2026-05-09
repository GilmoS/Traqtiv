import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, test, expect, vi } from 'vitest'
import { Button } from '../components/ui'

describe('Button', () => {
  test('renders with correct text', () => {
    render(<Button>Click me</Button>)
    expect(screen.getByText('Click me')).toBeInTheDocument()
  })

  test('shows spinner when loading', () => {
    render(<Button loading={true}>Click me</Button>)
    expect(screen.queryByText('Click me')).toBeInTheDocument()
  })

  test('is disabled when loading', () => {
    render(<Button loading={true}>Click me</Button>)
    expect(screen.getByRole('button')).toBeDisabled()
  })

  test('calls onClick when clicked', async () => {
    const handleClick = vi.fn()
    render(<Button onClick={handleClick}>Click me</Button>)
    await userEvent.click(screen.getByRole('button'))
    expect(handleClick).toHaveBeenCalledTimes(1)
  })
})