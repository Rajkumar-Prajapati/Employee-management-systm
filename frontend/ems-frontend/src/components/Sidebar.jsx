import React from 'react'
import { NavLink } from 'react-router-dom'
import {
  MdDashboard,
  MdPeople,
  MdApartment,
  MdEventAvailable,
  MdAssessment,
} from 'react-icons/md'

const links = [
  { to: '/', label: 'Dashboard', icon: <MdDashboard />, end: true },
  { to: '/employees', label: 'Employees', icon: <MdPeople /> },
  { to: '/departments', label: 'Departments', icon: <MdApartment /> },
  { to: '/attendance', label: 'Attendance', icon: <MdEventAvailable /> },
  { to: '/reports', label: 'Reports', icon: <MdAssessment /> },
]

const Sidebar = () => (
  <aside className="sidebar">
    <div className="sidebar__brand">
      <span className="sidebar__logo">E</span>
      <span className="sidebar__title">EMS</span>
    </div>
    <nav className="sidebar__nav">
      {links.map((l) => (
        <NavLink
          key={l.to}
          to={l.to}
          end={l.end}
          className={({ isActive }) => `sidebar__link${isActive ? ' sidebar__link--active' : ''}`}
        >
          <span className="sidebar__icon">{l.icon}</span>
          {l.label}
        </NavLink>
      ))}
    </nav>
    <div className="sidebar__footer">
      <p>Employee Management System</p>
      <p className="sidebar__version">v1.0.0</p>
    </div>
  </aside>
)

export default Sidebar
