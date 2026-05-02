import axios from 'axios'

const BASE_URL = 'http://localhost:5203'
// Create an Axios instance with the base URL and default headers
export const apiClient = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

// Add a request interceptor to include the token in the Authorization header
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('traqtiv_token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// Add a response interceptor to handle 401 Unauthorized errors
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('traqtiv_token')
      localStorage.removeItem('traqtiv_user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)
// Export the configured Axios instance for use in other parts of the application
export default apiClient