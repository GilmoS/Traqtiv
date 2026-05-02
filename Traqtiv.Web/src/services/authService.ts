import apiClient from './apiClient'
import type { LoginRequestDto, RegisterRequestDto, AuthResponseDto } from '../types'
// This file defines the authService object, which provides methods for user authentication.
// The authService object has two methods: login and register. Both methods send a POST request to the appropriate API endpoint with the provided data and return the response data.
export const authService = {

    // The login method takes a LoginRequestDto object as an argument, sends a POST request to the '/api/auth/login' endpoint, and returns the response data as an AuthResponseDto object.
  login: (data: LoginRequestDto) =>
    apiClient.post<AuthResponseDto>('/api/auth/login', data).then(r => r.data),
  
    // The register method takes a RegisterRequestDto object as an argument, sends a POST request to the '/api/auth/register' endpoint, and returns the response data as an AuthResponseDto object.
  register: (data: RegisterRequestDto) =>
    apiClient.post<AuthResponseDto>('/api/auth/register', data).then(r => r.data),
}