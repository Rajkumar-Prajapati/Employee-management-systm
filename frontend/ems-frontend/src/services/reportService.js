import api from './api'

const downloadFile = async (url, filename) => {
  const response = await api.get(url, { responseType: 'blob' })
  const blobUrl = window.URL.createObjectURL(new Blob([response.data]))
  const link = document.createElement('a')
  link.href = blobUrl
  link.setAttribute('download', filename)
  document.body.appendChild(link)
  link.click()
  link.remove()
}

export const downloadEmployeePdf = () => downloadFile('/reports/employees/pdf', 'employee-directory.pdf')
export const downloadEmployeeExcel = () => downloadFile('/reports/employees/excel', 'employee-directory.xlsx')
export const downloadSalaryPdf = () => downloadFile('/reports/salary/pdf', 'salary-report.pdf')
export const downloadAttendanceExcel = () => downloadFile('/reports/attendance/excel', 'attendance-report.xlsx')
