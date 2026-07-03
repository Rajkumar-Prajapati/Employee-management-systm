using ClosedXML.Excel;
using EMS.Backend.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EMS.Backend.Services
{
    public class ReportService : IReportService
    {
        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateEmployeePdf(List<Employee> employees)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Text("Employee Directory")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            foreach (var title in new[] { "#", "Name", "Email", "Department", "Designation", "Status" })
                            {
                                header.Cell().Element(CellStyleHeader).Text(title).SemiBold().FontColor(Colors.White);
                            }
                        });

                        int i = 1;
                        foreach (var e in employees)
                        {
                            table.Cell().Element(CellStyle).Text(i.ToString());
                            table.Cell().Element(CellStyle).Text($"{e.FirstName} {e.LastName}");
                            table.Cell().Element(CellStyle).Text(e.Email);
                            table.Cell().Element(CellStyle).Text(e.Department?.Name ?? "-");
                            table.Cell().Element(CellStyle).Text(e.Designation);
                            table.Cell().Element(CellStyle).Text(e.Status);
                            i++;
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ").FontSize(8);
                        x.Span(DateTime.Now.ToString("dd MMM yyyy HH:mm")).FontSize(8).SemiBold();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateSalaryPdf(List<Employee> employees)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Text("Salary Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Green.Darken2);

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            foreach (var title in new[] { "#", "Name", "Department", "Salary" })
                            {
                                header.Cell().Element(CellStyleHeader).Text(title).SemiBold().FontColor(Colors.White);
                            }
                        });

                        int i = 1;
                        decimal total = 0;
                        foreach (var e in employees)
                        {
                            table.Cell().Element(CellStyle).Text(i.ToString());
                            table.Cell().Element(CellStyle).Text($"{e.FirstName} {e.LastName}");
                            table.Cell().Element(CellStyle).Text(e.Department?.Name ?? "-");
                            table.Cell().Element(CellStyle).Text(e.Salary.ToString("C2"));
                            total += e.Salary;
                            i++;
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateEmployeeExcel(List<Employee> employees)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Employees");

            string[] headers = { "Id", "First Name", "Last Name", "Email", "Phone", "Department", "Designation", "Salary", "Date Of Joining", "Status" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(1, c + 1).Value = headers[c];
                ws.Cell(1, c + 1).Style.Font.Bold = true;
                ws.Cell(1, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#4F46E5");
                ws.Cell(1, c + 1).Style.Font.FontColor = XLColor.White;
            }

            int row = 2;
            foreach (var e in employees)
            {
                ws.Cell(row, 1).Value = e.Id;
                ws.Cell(row, 2).Value = e.FirstName;
                ws.Cell(row, 3).Value = e.LastName;
                ws.Cell(row, 4).Value = e.Email;
                ws.Cell(row, 5).Value = e.Phone;
                ws.Cell(row, 6).Value = e.Department?.Name ?? "-";
                ws.Cell(row, 7).Value = e.Designation;
                ws.Cell(row, 8).Value = e.Salary;
                ws.Cell(row, 9).Value = e.DateOfJoining.ToString("yyyy-MM-dd");
                ws.Cell(row, 10).Value = e.Status;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] GenerateAttendanceExcel(List<Attendance> records)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Attendance");

            string[] headers = { "Id", "Employee", "Date", "Check In", "Check Out", "Status" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(1, c + 1).Value = headers[c];
                ws.Cell(1, c + 1).Style.Font.Bold = true;
                ws.Cell(1, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#4F46E5");
                ws.Cell(1, c + 1).Style.Font.FontColor = XLColor.White;
            }

            int row = 2;
            foreach (var a in records)
            {
                ws.Cell(row, 1).Value = a.Id;
                ws.Cell(row, 2).Value = a.Employee != null ? $"{a.Employee.FirstName} {a.Employee.LastName}" : "-";
                ws.Cell(row, 3).Value = a.Date.ToString("yyyy-MM-dd");
                ws.Cell(row, 4).Value = a.CheckIn?.ToString(@"hh\:mm") ?? "-";
                ws.Cell(row, 5).Value = a.CheckOut?.ToString(@"hh\:mm") ?? "-";
                ws.Cell(row, 6).Value = a.Status;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static IContainer CellStyleHeader(IContainer container) =>
            container.Background(Colors.Blue.Darken2).Padding(5).AlignMiddle();

        private static IContainer CellStyle(IContainer container) =>
            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignMiddle();
    }
}
