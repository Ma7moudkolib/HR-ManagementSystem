# HR Management System

A modern, scalable Human Resources (HR) management system built with ASP.NET Core (.NET 9). This solution provides a robust RESTful API for managing companies, departments, employees, attendance, leave, and user authentication. Designed for extensibility and security, it is suitable for organizations of any size.

---

## 🚀 Features

- **User Authentication & Authorization**
  - Secure registration and login with JWT-based authentication.
- **Company Management**
  - Create, update, retrieve, and delete company records.
- **Department Management**
  - Manage departments within companies, including CRUD operations.
- **Employee Management**
  - Add, update, retrieve, and remove employees for each company.
- **Attendance Tracking**
  - Employee check-in/check-out and attendance history.
- **Leave Management**
  - Submit, approve, reject leave requests and track leave balances.
- **API Documentation**
  - Interactive Swagger UI for exploring and testing endpoints.

---

## 📦 Project Structure

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or later

### Setup

1. **Clone the repository:**
2. **Configure the database:**
   - Update your connection string in `appsettings.json`.

3. **Build and run the application:**

4. **Access the API documentation:**
   - Navigate to `https://localhost:{port}/swagger` in your browser.

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

> Built with .NET 9 and Visual Studio 2022.

