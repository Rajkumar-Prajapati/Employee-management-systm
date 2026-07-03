# Employee Management System

A full-stack Employee Management System built as two independent projects:

- **`backend/`** — ASP.NET Core 8 Web API + MySQL (EF Core / Pomelo) + JWT authentication
- **`frontend/`** — React.js (JavaScript, no TypeScript) + Vite, with a modern dark UI

## Features

- Secure login with JWT authentication (BCrypt password hashing)
-  Employee CRUD — create, edit, delete, search, filter, paginate
-  Department management with employee counts
-  Daily attendance marking (Present / Absent / Leave / Half Day)
-  Dashboard with live stats and a department bar chart
-  Reports — Employee Directory (PDF & Excel), Salary Report (PDF), Attendance Report (Excel)
-  Fully responsive, attractive dark-themed UI


## 1. Backend Setup (ASP.NET Core + MySQL)

**Requirements:** .NET 8 SDK, MySQL Server (local or remote)


cd backend/EMS.Backend

# Restore packages
dotnet restore

# Update the connection string in appsettings.json
# "DefaultConnection": "server=localhost;port=3306;database=ems_db;user=root;password=YOUR_MYSQL_PASSWORD"

# Run the API (auto-creates the database & seeds an admin user on first run)
dotnet run


The API starts at `https://localhost:7001` (see `Properties/launchSettings.json`) with Swagger UI at `/swagger`.

**Default login:** `admin` / `Admin@123`

### Project structure

EMS.Backend/
├── Controllers/     # Auth, Employees, Departments, Attendance, Reports
├── Models/          # Employee, Department, Attendance, User (EF entities)
├── DTOs/            # Request/response contracts
├── Data/            # ApplicationDbContext + DbInitializer (seeding)
├── Services/        # TokenService (JWT), ReportService (PDF/Excel)
├── Helpers/         # JwtSettings
└── Program.cs        # App bootstrap, DI, CORS, JWT, Swagger




## 2. Frontend Setup (React.js)

cd frontend/ems-frontend
npm install
npm run dev


The app runs at `http://localhost:5173`.

Update the backend URL in `src/services/api.js` if your API runs on a different port:

baseURL: 'https://localhost:7001/api'


### Project structure

ems-frontend/
├── src/
│   ├── components/   # Sidebar, Navbar, Layout, EmployeeForm, StatCard, Loader...
│   ├── pages/         # Login, Dashboard, Employees, Departments, Attendance, Reports
│   ├── context/        # AuthContext (JWT storage + login/logout)
│   ├── services/       # axios API clients (auth, employee, department, attendance, reports)
│   └── styles/          # index.css — global dark theme
└── vite.config.js




## 3. Typical Workflow

1. Start MySQL, update the backend connection string, run `dotnet run`.
2. Run `npm run dev` in the frontend.
3. Log in with `admin` / `Admin@123`.
4. Add departments first, then employees (each employee needs a department).
5. Mark daily attendance from the **Attendance** page.
6. Download PDF/Excel reports from the **Reports** page.

## Tech Stack

| Layer     | Technology |
|-----------|------------|
| Frontend  | React.js, Vite, React Router, Axios, Recharts, React Icons, React Toastify |
| Backend   | ASP.NET Core 8 Web API, Entity Framework Core, JWT Bearer Auth |
| Database  | MySQL (via Pomelo.EntityFrameworkCore.MySql) |
| Reports   | QuestPDF (PDF), ClosedXML (Excel) |
