import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { MdAdd, MdEdit, MdDelete, MdClose } from 'react-icons/md'
import Layout from '../components/Layout'
import Loader from '../components/Loader'
import {
  getDepartments,
  createDepartment,
  updateDepartment,
  deleteDepartment,
} from '../services/departmentService'

const Departments = () => {
  const [departments, setDepartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [formOpen, setFormOpen] = useState(false)
  const [editing, setEditing] = useState(null)
  const [form, setForm] = useState({ name: '', description: '' })

  const load = async () => {
    setLoading(true)
    try {
      setDepartments(await getDepartments())
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const openAdd = () => {
    setEditing(null)
    setForm({ name: '', description: '' })
    setFormOpen(true)
  }

  const openEdit = (dept) => {
    setEditing(dept)
    setForm({ name: dept.name, description: dept.description || '' })
    setFormOpen(true)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      if (editing) {
        await updateDepartment(editing.id, form)
        toast.success('Department updated')
      } else {
        await createDepartment(form)
        toast.success('Department created')
      }
      setFormOpen(false)
      load()
    } catch {
      toast.error('Something went wrong')
    }
  }

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this department?')) return
    try {
      await deleteDepartment(id)
      toast.success('Department deleted')
      load()
    } catch (err) {
      toast.error(err?.response?.data?.message || 'Failed to delete')
    }
  }

  return (
    <Layout title="Departments">
      <div className="toolbar">
        <div />
        <button className="btn btn--primary" onClick={openAdd}><MdAdd /> Add Department</button>
      </div>

      {loading ? <Loader /> : (
        <div className="grid-cards">
          {departments.map((d) => (
            <div className="dept-card" key={d.id}>
              <div className="dept-card__header">
                <h3>{d.name}</h3>
                <span className="badge badge--indigo">{d.employeeCount} employees</span>
              </div>
              <p>{d.description || 'No description provided'}</p>
              <div className="dept-card__actions">
                <button className="icon-btn" onClick={() => openEdit(d)}><MdEdit /></button>
                <button className="icon-btn icon-btn--danger" onClick={() => handleDelete(d.id)}><MdDelete /></button>
              </div>
            </div>
          ))}
        </div>
      )}

      {formOpen && (
        <div className="modal-overlay">
          <div className="modal modal--sm">
            <div className="modal__header">
              <h2>{editing ? 'Edit Department' : 'Add Department'}</h2>
              <button className="icon-btn" onClick={() => setFormOpen(false)}><MdClose /></button>
            </div>
            <form className="modal__body" onSubmit={handleSubmit}>
              <div className="form-field">
                <label>Name</label>
                <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
              </div>
              <div className="form-field">
                <label>Description</label>
                <textarea rows={3} value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
              </div>
              <div className="modal__footer">
                <button type="button" className="btn btn--ghost" onClick={() => setFormOpen(false)}>Cancel</button>
                <button type="submit" className="btn btn--primary">{editing ? 'Save Changes' : 'Create'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </Layout>
  )
}

export default Departments
