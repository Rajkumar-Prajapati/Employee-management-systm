import React, { useEffect, useState } from 'react'
import { MdPeople, MdCheckCircle, MdCancel, MdApartment, MdAttachMoney } from 'react-icons/md'
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid } from 'recharts'
import Layout from '../components/Layout'
import StatCard from '../components/StatCard'
import Loader from '../components/Loader'
import { getEmployeeStats } from '../services/employeeService'
import { getDepartments } from '../services/departmentService'

const Dashboard = () => {
  const [stats, setStats] = useState(null)
  const [departments, setDepartments] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    (async () => {
      try {
        const [statsData, deptData] = await Promise.all([getEmployeeStats(), getDepartments()])
        setStats(statsData)
        setDepartments(deptData)
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  const chartData = departments.map((d) => ({ name: d.name, employees: d.employeeCount }))

  return (
    <Layout title="Dashboard">
      {loading ? (
        <Loader />
      ) : (
        <>
          <div className="stats-grid">
            <StatCard icon={<MdPeople />} label="Total Employees" value={stats?.total ?? 0} accent="indigo" />
            <StatCard icon={<MdCheckCircle />} label="Active Employees" value={stats?.active ?? 0} accent="green" />
            <StatCard icon={<MdCancel />} label="Inactive Employees" value={stats?.inactive ?? 0} accent="red" />
            <StatCard icon={<MdApartment />} label="Departments" value={stats?.departments ?? 0} accent="amber" />
            <StatCard
              icon={<MdAttachMoney />}
              label="Avg. Salary"
              value={`$${Number(stats?.avgSalary ?? 0).toLocaleString(undefined, { maximumFractionDigits: 0 })}`}
              accent="purple"
            />
          </div>

          <div className="card chart-card">
            <h3>Employees by Department</h3>
            <ResponsiveContainer width="100%" height={320}>
              <BarChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" stroke="#2a2f45" />
                <XAxis dataKey="name" stroke="#9ca3af" />
                <YAxis stroke="#9ca3af" allowDecimals={false} />
                <Tooltip contentStyle={{ background: '#1e2235', border: 'none', borderRadius: 8, color: '#fff' }} />
                <Bar dataKey="employees" fill="#6366f1" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </>
      )}
    </Layout>
  )
}

export default Dashboard
