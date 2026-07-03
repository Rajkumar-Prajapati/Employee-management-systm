import React from 'react'
import Sidebar from './Sidebar'
import Navbar from './Navbar'

const Layout = ({ title, children }) => (
  <div className="app-shell">
    <Sidebar />
    <div className="app-main">
      <Navbar title={title} />
      <main className="app-content">{children}</main>
    </div>
  </div>
)

export default Layout
