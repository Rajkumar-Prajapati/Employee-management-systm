import api from './api'

export const getEmployees = async (params) => {
  const { data } = await api.get('/employees', { params })
  return data
}

export const getEmployee = async (id) => {
  const { data } = await api.get(`/employees/${id}`)
  return data
}

export const createEmployee = async (payload) => {
  const { data } = await api.post('/employees', payload)
  return data
}

export const updateEmployee = async (id, payload) => {
  const { data } = await api.put(`/employees/${id}`, payload)
  return data
}

export const deleteEmployee = async (id) => {
  const { data } = await api.delete(`/employees/${id}`)
  return data
}

export const bulkDeleteEmployees = async (ids) => {
  const { data } = await api.post('/employees/bulk-delete', ids)
  return data
}

export const getEmployeeStats = async () => {
  const { data } = await api.get('/employees/stats')
  return data
}
