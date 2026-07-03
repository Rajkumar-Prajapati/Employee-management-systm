import api from './api'

export const getAttendance = async (params) => {
  const { data } = await api.get('/attendance', { params })
  return data
}

export const markAttendance = async (payload) => {
  const { data } = await api.post('/attendance/mark', payload)
  return data
}

export const deleteAttendance = async (id) => {
  const { data } = await api.delete(`/attendance/${id}`)
  return data
}
