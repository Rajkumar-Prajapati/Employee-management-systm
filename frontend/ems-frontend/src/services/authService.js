import api from './api'

export const login = async (username, password) => {
  const { data } = await api.post('/auth/login', { username, password })
  return data
}

export const register = async (payload) => {
  const { data } = await api.post('/auth/register', payload)
  return data
}
