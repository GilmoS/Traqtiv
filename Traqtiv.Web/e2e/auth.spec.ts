import { test, expect } from '@playwright/test'

test.describe('Authentication', () => {
  test('login page loads correctly', async ({ page }) => {
    await page.goto('http://localhost:5173/login')
    await expect(page.getByRole('heading', { name: 'Sign In' })).toBeVisible()
    await expect(page.getByPlaceholder('you@example.com')).toBeVisible()
    await expect(page.getByPlaceholder('••••••••')).toBeVisible()
  })

  test('shows error on invalid login', async ({ page }) => {
    await page.goto('http://localhost:5173/login')
    await page.fill('input[type="email"]', 'wrong@email.com')
    await page.fill('input[type="password"]', 'wrongpassword')
    await page.click('button[type="submit"]')
    await expect(page.getByText('Invalid email or password')).toBeVisible()
  })

  test('redirects to login when not authenticated', async ({ page }) => {
    await page.goto('http://localhost:5173/workouts')
    await expect(page).toHaveURL('http://localhost:5173/login')
  })
})