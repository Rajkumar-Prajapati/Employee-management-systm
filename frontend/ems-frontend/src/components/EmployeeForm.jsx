import React, { useEffect, useState } from 'react'
import { MdClose } from 'react-icons/md'

const emptyForm = {
  firstName: '',
  lastName: '',
  email: '',
  phone: '',
  gender: 'Male',
  address: '',
  designation: '',
  departmentId: '',
  salary: '',
  status: 'Active',
}

const EmployeeForm = ({ open, onClose, onSubmit, departments, initialData }) => {
  const [form, setForm] = useState(emptyForm)

  useEffect(() => {
    if (initialData) {
      setForm({
        firstName: initialData.firstName || '',
        lastName: initialData.lastName || '',
        email: initialData.email || '',
        phone: initialData.phone || '',
        gender: initialData.gender || 'Male',
        address: initialData.address || '',
        designation: initialData.designation || '',
        departmentId: initialData.departmentId || '',
        salary: initialData.salary || '',
        status: initialData.status || 'Active',
      })
    } else {
      setForm(emptyForm)
    }
  }, [initialData, open])

  if (!open) return null

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    onSubmit({
      ...form,
      departmentId: Number(form.departmentId),
      salary: Number(form.salary),
    })
  }

  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal__header">
          <h2>{initialData ? 'Edit Employee' : 'Add New Employee'}</h2>
          <button className="icon-btn" onClick={onClose}><MdClose /></button>
        </div>
        <form className="modal__body" onSubmit={handleSubmit}>
          <div className="form-grid">
            <div className="form-field">
              <label>First Name</label>
              <input name="firstName" value={form.firstName} onChange={handleChange} required />
            </div>
            <div className="form-field">
              <label>Last Name</label>
              <input name="lastName" value={form.lastName} onChange={handleChange} required />
            </div>
            <div className="form-field">
              <label>Email</label>
              <input type="email" name="email" value={form.email} onChange={handleChange} required />
            </div>
            <div className="form-field">
              <label>Phone</label>
              <input name="phone" value={form.phone} onChange={handleChange} />
            </div>
            <div className="form-field">
              <label>Gender</label>
              <select name="gender" value={form.gender} onChange={handleChange}>
                <option>Male</option>
                <option>Female</option>
                <option>Other</option>
              </select>
            </div>
            <div className="form-field">
              <label>Designation</label>
              <input name="designation" value={form.designation} onChange={handleChange} required />
            </div>
            <div className="form-field">
              <label>Department</label>
              <select name="departmentId" value={form.departmentId} onChange={handleChange} required>
                <option value="">Select department</option>
                {departments.map((d) => (
                  <option key={d.id} value={d.id}>{d.name}</option>
                ))}
              </select>
            </div>
            <div className="form-field">
              <label>Salary</label>
              <input type="number" name="salary" value={form.salary} onChange={handleChange} required min="0" />
            </div>
            <div className="form-field">
              <label>Status</label>
              <select name="status" value={form.status} onChange={handleChange}>
                <option>Active</option>
                <option>Inactive</option>
              </select>
            </div>
            <div className="form-field form-field--full">
              <label>Address</label>
              <textarea name="address" value={form.address} onChange={handleChange} rows={2} />
            </div>
          </div>
          <div className="modal__footer">
            <button type="button" className="btn btn--ghost" onClick={onClose}>Cancel</button>
            <button type="submit" className="btn btn--primary">{initialData ? 'Save Changes' : 'Add Employee'}</button>
          </div>
        </form>
      </div>
    </div>
  )
}

export default EmployeeForm
