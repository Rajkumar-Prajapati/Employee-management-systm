import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { MdLock, MdPerson, MdBusinessCenter } from 'react-icons/md'
import { useAuth } from '../context/AuthContext'

const Login = () => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    try {
      await login(username, password)
      toast.success('Welcome back!')
      navigate('/')
    } catch (err) {
      toast.error(err?.response?.data?.message || 'Invalid credentials')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-card__brand">
          <span className="sidebar__logo">E</span>
          <h1>Employee Management System</h1>
          <p>Sign in to manage your workforce</p>
        </div>
        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-field">
            <label><MdPerson /> Username</label>
            <input value={username} onChange={(e) => setUsername(e.target.value)} placeholder="admin" required />
          </div>
          <div className="form-field">
            <label><MdLock /> Password</label>
            <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="••••••••" required />
          </div>
          <button className="btn btn--primary btn--block" disabled={loading}>
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>
        <p className="auth-hint"><MdBusinessCenter /> Default: admin / Admin@123</p>
      </div>
    </div>
  )
}

export default Login
