import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:7001/api', // update to match your backend's launch URL
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('ems_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error?.response?.status === 401) {
      localStorage.removeItem('ems_token')
      localStorage.removeItem('ems_user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default api
