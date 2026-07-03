import React, { useState } from 'react'
import { toast } from 'react-toastify'
import { MdPictureAsPdf, MdGridOn, MdDownload } from 'react-icons/md'
import Layout from '../components/Layout'
import {
  downloadEmployeePdf,
  downloadEmployeeExcel,
  downloadSalaryPdf,
  downloadAttendanceExcel,
} from '../services/reportService'

const reportCards = [
  {
    key: 'employee-pdf',
    title: 'Employee Directory (PDF)',
    description: 'Full employee list with department and designation details.',
    icon: <MdPictureAsPdf />,
    action: downloadEmployeePdf,
  },
  {
    key: 'employee-excel',
    title: 'Employee Directory (Excel)',
    description: 'Editable spreadsheet of every employee record.',
    icon: <MdGridOn />,
    action: downloadEmployeeExcel,
  },
  {
    key: 'salary-pdf',
    title: 'Salary Report (PDF)',
    description: 'Salary breakdown by employee and department.',
    icon: <MdPictureAsPdf />,
    action: downloadSalaryPdf,
  },
  {
    key: 'attendance-excel',
    title: 'Attendance Report (Excel)',
    description: 'Attendance history across all employees.',
    icon: <MdGridOn />,
    action: downloadAttendanceExcel,
  },
]

const Reports = () => {
  const [downloading, setDownloading] = useState(null)

  const handleDownload = async (card) => {
    setDownloading(card.key)
    try {
      await card.action()
      toast.success(`${card.title} downloaded`)
    } catch {
      toast.error('Failed to generate report')
    } finally {
      setDownloading(null)
    }
  }

  return (
    <Layout title="Reports">
      <div className="grid-cards">
        {reportCards.map((card) => (
          <div className="report-card" key={card.key}>
            <div className="report-card__icon">{card.icon}</div>
            <h3>{card.title}</h3>
            <p>{card.description}</p>
            <button
              className="btn btn--primary btn--block"
              onClick={() => handleDownload(card)}
              disabled={downloading === card.key}
            >
              <MdDownload /> {downloading === card.key ? 'Generating...' : 'Download'}
            </button>
          </div>
        ))}
      </div>
    </Layout>
  )
}

export default Reports
