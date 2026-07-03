import React from 'react'
import { MdLogout, MdPerson } from 'react-icons/md'
import { useAuth } from '../context/AuthContext'
import { useNavigate } from 'react-router-dom'

const Navbar = ({ title }) => {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <header className="navbar">
      <h1 className="navbar__title">{title}</h1>
      <div className="navbar__right">
        <div className="navbar__user">
          <MdPerson />
          <span>{user?.username || 'User'}</span>
          <span className="badge">{user?.role}</span>
        </div>
        <button className="btn btn--ghost" onClick={handleLogout}>
          <MdLogout /> Logout
        </button>
      </div>
    </header>
  )
}

export default Navbar
