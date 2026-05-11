import { test, expect } from '@playwright/test'

// פרטי משתמש קיים לבדיקות
const TEST_EMAIL = 'gil.moshe23@gmail.com'
const TEST_PASSWORD = 'Gil123'

async function login(page: any) {
  await page.goto('http://localhost:5173/login')
  await page.fill('input[type="email"]', TEST_EMAIL)
  await page.fill('input[type="password"]', TEST_PASSWORD)
  await page.click('button[type="submit"]')
  await page.waitForSelector('text=Hello', { timeout: 10000 })
}

test.describe('Workouts', () => {
  test('can navigate to workouts page', async ({ page }) => {
    await login(page)
    await page.click('text=Workouts')
    await expect(page).toHaveURL('http://localhost:5173/workouts')
    await expect(page.getByRole('heading', { name: 'Workouts' })).toBeVisible()
  })

  test('can open new workout modal', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/workouts')
    await page.click('text=New Workout')
    await expect(page.getByRole('heading', { name: 'New Workout' })).toBeVisible()
  })

  test('shows validation error when duration is 0', async ({ page }) => {
    await login(page)
    await page.goto('http://localhost:5173/workouts')
    await page.click('text=New Workout')
    await page.waitForSelector('text=New Workout', { timeout: 5000 })

    let alertMessage = ''
  page.on('dialog', async dialog => {
    alertMessage = dialog.message()
    await dialog.accept()
  })

  const inputs = page.locator('input[type="number"]')
  await inputs.first().fill('0')
  await page.click('text=Add')

  await page.waitForTimeout(1000)
  expect(alertMessage).toBe('Duration must be greater than 0')

    
  })
})