import { test, expect } from '@playwright/test'

const TEST_EMAIL = 'gil.moshe23@gmail.com'
const TEST_PASSWORD = 'Gil123'

async function login(page: any) {
  await page.goto('http://localhost:5173/login')
  await page.fill('input[type="email"]', TEST_EMAIL)
  await page.fill('input[type="password"]', TEST_PASSWORD)
  await page.click('button[type="submit"]')
  await page.waitForSelector('text=Hello', { timeout: 10000 })
}

// ─── Navigation ───────────────────────────────────────────
test.describe('Navigation', () => {
  test('can navigate to all pages', async ({ page }) => {
    await login(page)
    
    await page.click('text=Workouts')
    await expect(page).toHaveURL('http://localhost:5173/workouts')
    
    await page.click('text=Daily Activity')
    await expect(page).toHaveURL('http://localhost:5173/activity')
    
    await page.click('text=Body Metrics')
    await expect(page).toHaveURL('http://localhost:5173/metrics')
    
    await page.click('text=Recommendations')
    await expect(page).toHaveURL('http://localhost:5173/recommendations')
    
    await page.click('text=Profile')
    await expect(page).toHaveURL('http://localhost:5173/profile')
  })
})

// ─── Daily Activity ───────────────────────────────────────
test.describe('Daily Activity', () => {
  test('can open add activity modal', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/activity')
    await page.click('text=Add Activity')
    await expect(page.getByRole('heading', { name: 'Add Activity' })).toBeVisible()
  })

  test('shows validation error when steps exceed limit', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/activity')
    await page.click('text=Add Activity')
    await page.waitForSelector('text=Add Activity', { timeout: 5000 })

    let alertMessage = ''
    page.on('dialog', async dialog => {
      alertMessage = dialog.message()
      await dialog.accept()
    })

    const inputs = page.locator('input[type="number"]')
    await inputs.nth(0).fill('999999')
    await page.getByRole('button', { name: 'Add' }).last().click()
    await page.waitForTimeout(1000)
    expect(alertMessage).toBe('Steps cannot exceed 100,000 per day')
  })

  test('closes modal on cancel', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/activity')
    await page.click('text=Add Activity')
    await page.waitForSelector('text=Add Activity', { timeout: 5000 })
    await page.click('text=Cancel')
    await expect(page.getByRole('heading', { name: 'Add Activity' })).not.toBeVisible()
  })
})

// ─── Body Metrics ─────────────────────────────────────────
test.describe('Body Metrics', () => {
  test('can open new measurement modal', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/metrics')
    await page.click('text=New Measurement')
    await expect(page.getByRole('heading', { name: 'New Measurement' })).toBeVisible()
  })

  test('closes modal on cancel', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/metrics')
    await page.click('text=New Measurement')
    await page.waitForSelector('text=New Measurement', { timeout: 5000 })
    await page.click('text=Cancel')
    await expect(page.getByRole('heading', { name: 'New Measurement' })).not.toBeVisible()
  })
})

// ─── Profile ──────────────────────────────────────────────
test.describe('Profile', () => {
  test('shows user details', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/profile')
    await expect(page.getByRole('heading', { name: 'Profile' })).toBeVisible()
    await expect(page.getByText('Update Details')).toBeVisible()
  })

  test('shows success message after saving', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/profile')
    await page.click('text=Save Changes')
    await expect(page.getByText('✓ Profile updated successfully')).toBeVisible({ timeout: 5000 })
  })
})