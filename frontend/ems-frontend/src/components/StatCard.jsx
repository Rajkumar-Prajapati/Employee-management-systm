import React from 'react'

const StatCard = ({ icon, label, value, accent }) => (
  <div className="stat-card">
    <div className={`stat-icon stat-icon--${accent}`}>{icon}</div>
    <div>
      <p className="stat-value">{value}</p>
      <p className="stat-label">{label}</p>
    </div>
  </div>
)

export default StatCard
