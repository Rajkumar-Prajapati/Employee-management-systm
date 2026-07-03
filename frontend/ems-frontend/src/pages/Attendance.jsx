import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { MdCheckCircle, MdSave } from 'react-icons/md'
import Layout from '../components/Layout'
import Loader from '../components/Loader'
import { getEmployees } from '../services/employeeService'
import { getAttendance, markAttendance } from '../services/attendanceService'

const todayStr = () => new Date().toISOString().slice(0, 10)

const Attendance = () => {
  const [date, setDate] = useState(todayStr())
  const [employees, setEmployees] = useState([])
  const [records, setRecords] = useState({})
  const [loading, setLoading] = useState(true)

  const load = async () => {
    setLoading(true)
    try {
      const empData = await getEmployees({ pageSize: 200 })
      setEmployees(empData.data)

      const attData = await getAttendance({ date })
      const map = {}
      attData.forEach((r) => { map[r.employeeId] = r.status })
      setRecords(map)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [date])

  const setStatus = (empId, status) => {
    setRecords((prev) => ({ ...prev, [empId]: status }))
  }

  const saveAll = async () => {
    try {
      await Promise.all(
        employees.map((e) =>
          markAttendance({
            employeeId: e.id,
            date,
            status: records[e.id] || 'Absent',
          })
        )
      )
      toast.success('Attendance saved for ' + date)
    } catch {
      toast.error('Failed to save attendance')
    }
  }

  return (
    <Layout title="Attendance">
      <div className="toolbar">
        <input type="date" className="date-input" value={date} onChange={(e) => setDate(e.target.value)} />
        <button className="btn btn--primary" onClick={saveAll}><MdSave /> Save Attendance</button>
      </div>

      <div className="card">
        {loading ? <Loader /> : (
          <table className="table">
            <thead>
              <tr>
                <th>Employee</th>
                <th>Department</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {employees.map((e) => (
                <tr key={e.id}>
                  <td>
                    <div className="employee-name">
                      <span className="avatar">{e.firstName[0]}{e.lastName[0]}</span>
                      {e.firstName} {e.lastName}
                    </div>
                  </td>
                  <td>{e.departmentName || '-'}</td>
                  <td>
                    <div className="status-pills">
                      {['Present', 'Absent', 'Leave', 'HalfDay'].map((s) => (
                        <button
                          key={s}
                          className={`pill${records[e.id] === s ? ' pill--active' : ''}`}
                          onClick={() => setStatus(e.id, s)}
                          type="button"
                        >
                          {s === 'Present' && <MdCheckCircle />} {s}
                        </button>
                      ))}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </Layout>
  )
}

export default Attendance
