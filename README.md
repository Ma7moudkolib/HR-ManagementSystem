# HR Management System (Full-Stack)

A modern, scalable Human Resources (HR) management system built with ASP.NET Core (.NET 9) on the backend and Angular on the frontend. This solution provides a robust RESTful API and a responsive UI for managing companies, departments, employees, attendance, leave, and user authentication. Designed for extensibility and security, it is suitable for organizations of any size.

---

## 🚀 Tech Stack

- **Backend:** ASP.NET Core API, Entity Framework Core, SQL Server (Clean Architecture with Domain, Application, Infrastructure, and Web API layers).
- **Frontend:** Angular (Latest), Tailwind CSS, Reactive Forms.

---

## 🌟 Key Features

- **JWT Authentication (Login/Register)**
  - Secure registration and login with JWT-based authentication.
- **Employee & Company CRUD Management**
  - Create, update, retrieve, and delete company and employee records.
  - Manage departments within companies.
- **Attendance Tracking (Check-in/out)**
  - Employee check-in/check-out and attendance history.
- **Leave Management & Approval Workflow**
  - Submit, approve, reject leave requests and track leave balances.
- **Payroll Generation & Summaries**
  - Generate and view payroll summaries based on DTOs.
- **API Documentation**
  - Interactive Swagger UI for exploring and testing endpoints.

---

## 🏗️ Architecture

The application follows a full-stack architecture:
- **Backend:** 4-layer clean architecture (Domain, Application, Infrastructure, and Web API) ensuring separation of concerns.
- **Frontend:** Modular Angular application utilizing standalone components, lazy-loaded routes, Tailwind CSS for styling, and reactive forms for state management.

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Node.js & npm (for the Angular Frontend)
- Visual Studio 2022 or later

### Setup

1. **Clone the repository:**
2. **Configure the database:**
   - Update your connection string in `appsettings.json`.

3. **Build and run the application:**
   - Backend: `dotnet run`
   - Frontend: Navigate to `Frontend` directory, run `npm install`, and then `npm start`.

4. **Access the Application & API documentation:**
   - API Docs: Navigate to `https://localhost:{port}/swagger` in your browser.
   - Frontend: Open `http://localhost:4200` in your browser.

---

## 🧩 API Overview

### Authentication

- `POST /api/authentication/register` – Register a new user
- `POST /api/authentication/login` – Login and receive JWT token

### Companies

- `GET /api/companies` – List all companies
- `GET /api/companies/{id}` – Get company by ID
- `POST /api/companies` – Create a new company
- `PUT /api/companies/{id}` – Update company details
- `DELETE /api/companies/{id}` – Delete a company

### Departments

- `GET /api/companies/{companyId}/department` – List departments for a company
- `GET /api/companies/{companyId}/department/{departmentId}` – Get department details
- `PUT /api/companies/{companyId}/department/{departmentId}` – Update department
- `DELETE /api/companies/{companyId}/department/{departmentId}` – Delete department

### Employees

- `GET /api/companies/{companyId}/employees` – List employees for a company
- `GET /api/companies/{companyId}/employees/{id}` – Get employee details
- `POST /api/companies/{companyId}/employees` – Add new employee
- `PUT /api/companies/{companyId}/employees/{id}` – Update employee
- `DELETE /api/companies/{companyId}/employees/{id}` – Delete employee

### Attendance

- `POST /api/attendance/checkin/{employeeId}` – Employee check-in
- `POST /api/attendance/checkout/{employeeId}` – Employee check-out
- `GET /api/attendance/employee/{employeeId}` – Get attendance records

### Leave

- `GET /api/leave/status/{employeeId}` – Get leave requests for employee
- `GET /api/leave/balances/{employeeId}` – Get leave balances
- `POST /api/leave` – Create leave request
- `POST /api/leave/approve/{leaveRequestId}` – Approve leave request
- `POST /api/leave/reject/{leaveRequestId}` – Reject leave request

---

## 🛡️ Security

- JWT-based authentication for all protected endpoints.
- Identity management for user registration and login.

---

## 🧪 Testing

The project includes a comprehensive unit test suite to ensure code quality and reliability:

### Test Projects

- **CompanyEmployees.Tests** – Unit tests covering:
  - `AttendanceServiceTests.cs` – Attendance tracking functionality
  - `AuthenticationServiceTests.cs` – User authentication and authorization
  - `CompanyServiceTests.cs` – Company management operations
  - `DepartmentServiceTests.cs` – Department management operations
  - `EmployeeServiceTests.cs` – Employee management operations
  - `LeaveServiceTests.cs` – Leave request and balance management
  - `PayrollServiceTests.cs` – Payroll processing functionality

### Running Tests

1. **Run all tests:**
   ```bash
   dotnet test CompanyEmployees.Tests/CompanyEmployees.Tests.csproj
   ```

2. **Run specific test class:**
   ```bash
   dotnet test CompanyEmployees.Tests/CompanyEmployees.Tests.csproj --filter ClassName
   ```

3. **Run with verbose output:**
   ```bash
   dotnet test CompanyEmployees.Tests/CompanyEmployees.Tests.csproj --verbosity normal
   ```

---

## 📚 Documentation

- **Swagger UI** is enabled in development mode for interactive API exploration and testing.

---

## 🤝 Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

---

## 📄 License

This project is licensed under the MIT License.

---

## 👨‍💻 Authors

- [Mahmoud Kolib](https://github.com/Ma7moudkolib)

---

> Built with .NET 9 and Angular.
