import React, { createContext, useContext, useState } from 'react'
import { login as loginRequest } from '../services/authService'

const AuthContext = createContext(null)

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem('ems_user')
    return stored ? JSON.parse(stored) : null
  })

  const login = async (username, password) => {
    const data = await loginRequest(username, password)
    localStorage.setItem('ems_token', data.token)
    localStorage.setItem('ems_user', JSON.stringify({ username: data.username, role: data.role }))
    setUser({ username: data.username, role: data.role })
    return data
  }

  const logout = () => {
    localStorage.removeItem('ems_token')
    localStorage.removeItem('ems_user')
    setUser(null)
  }

  const isAuthenticated = !!localStorage.getItem('ems_token')

  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)
