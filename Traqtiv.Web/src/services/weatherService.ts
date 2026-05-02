import apiClient from './apiClient'
import type { WeatherResponseDto } from '../types'
// This file contains the service for fetching weather data from the backend API. It defines a single method, getCurrent, which makes a GET request to the /api/weather endpoint and returns the response data as a WeatherResponseDto object.
// The WeatherResponseDto interface is defined in the types file and includes properties for success, message, temperature, description, airQualityIndex, and airQualityDescription. The getCurrent method uses the apiClient to make the request and returns a promise that resolves to the data from the response.
export const weatherService = {
// The getCurrent method is defined as an arrow function that returns a promise. It uses the apiClient to make a GET request to the /api/weather endpoint and specifies that the response should be of type WeatherResponseDto. The then method is used to extract the data from the response and return it.
  getCurrent: () =>
    apiClient.get<WeatherResponseDto>('/api/weather').then(r => r.data),
}