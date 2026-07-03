import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { MdAdd, MdEdit, MdDelete, MdSearch } from 'react-icons/md'
import Layout from '../components/Layout'
import Loader from '../components/Loader'
import EmployeeForm from '../components/EmployeeForm'
import {
  getEmployees,
  createEmployee,
  updateEmployee,
  deleteEmployee,
} from '../services/employeeService'
import { getDepartments } from '../services/departmentService'

const Employees = () => {
  const [employees, setEmployees] = useState([])
  const [departments, setDepartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [formOpen, setFormOpen] = useState(false)
  const [editingEmployee, setEditingEmployee] = useState(null)
  const [total, setTotal] = useState(0)
  const [page, setPage] = useState(1)
  const pageSize = 8

  const loadEmployees = async () => {
    setLoading(true)
    try {
      const data = await getEmployees({ search, page, pageSize })
      setEmployees(data.data)
      setTotal(data.total)
    } catch {
      toast.error('Failed to load employees')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    getDepartments().then(setDepartments).catch(() => {})
  }, [])

  useEffect(() => {
    loadEmployees()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page])

  const handleSearchSubmit = (e) => {
    e.preventDefault()
    setPage(1)
    loadEmployees()
  }

  const openAddForm = () => {
    setEditingEmployee(null)
    setFormOpen(true)
  }

  const openEditForm = (emp) => {
    setEditingEmployee(emp)
    setFormOpen(true)
  }

  const handleSubmit = async (payload) => {
    try {
      if (editingEmployee) {
        await updateEmployee(editingEmployee.id, payload)
        toast.success('Employee updated')
      } else {
        await createEmployee(payload)
        toast.success('Employee added')
      }
      setFormOpen(false)
      loadEmployees()
    } catch (err) {
      toast.error(err?.response?.data?.message || 'Something went wrong')
    }
  }

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this employee?')) return
    try {
      await deleteEmployee(id)
      toast.success('Employee deleted')
      loadEmployees()
    } catch {
      toast.error('Failed to delete employee')
    }
  }

  const totalPages = Math.max(1, Math.ceil(total / pageSize))

  return (
    <Layout title="Employees">
      <div className="toolbar">
        <form className="search-box" onSubmit={handleSearchSubmit}>
          <MdSearch />
          <input
            placeholder="Search by name, email, designation..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </form>
        <button className="btn btn--primary" onClick={openAddForm}>
          <MdAdd /> Add Employee
        </button>
      </div>

      <div className="card">
        {loading ? (
          <Loader />
        ) : (
          <>
            <table className="table">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Department</th>
                  <th>Designation</th>
                  <th>Salary</th>
                  <th>Status</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {employees.length === 0 && (
                  <tr><td colSpan={7} className="empty-cell">No employees found</td></tr>
                )}
                {employees.map((emp) => (
                  <tr key={emp.id}>
                    <td>
                      <div className="employee-name">
                        <span className="avatar">{emp.firstName[0]}{emp.lastName[0]}</span>
                        {emp.firstName} {emp.lastName}
                      </div>
                    </td>
                    <td>{emp.email}</td>
                    <td>{emp.departmentName || '-'}</td>
                    <td>{emp.designation}</td>
                    <td>${Number(emp.salary).toLocaleString()}</td>
                    <td>
                      <span className={`badge badge--${emp.status === 'Active' ? 'green' : 'red'}`}>
                        {emp.status}
                      </span>
                    </td>
                    <td className="table-actions">
                      <button className="icon-btn" onClick={() => openEditForm(emp)}><MdEdit /></button>
                      <button className="icon-btn icon-btn--danger" onClick={() => handleDelete(emp.id)}><MdDelete /></button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div className="pagination">
              <button disabled={page <= 1} onClick={() => setPage((p) => p - 1)}>Previous</button>
              <span>Page {page} of {totalPages}</span>
              <button disabled={page >= totalPages} onClick={() => setPage((p) => p + 1)}>Next</button>
            </div>
          </>
        )}
      </div>

      <EmployeeForm
        open={formOpen}
        onClose={() => setFormOpen(false)}
        onSubmit={handleSubmit}
        departments={departments}
        initialData={editingEmployee}
      />
    </Layout>
  )
}

export default Employees
