
# 🎓 Final Year Project - LMS Backend

Welcome to the **Learning Management System (LMS) Backend** — a robust and scalable REST API built with **.NET 8**, designed to power a full-featured educational platform with role-based access control, dynamic resource management, and real-time learning engagement.

> 🛠️ This backend supports all academic, administrative, and interactive functionalities of the LMS through secure and well-documented APIs.

---
## 📽️ Demo

🎬 [Watch the Demo Video](https://www.youtube.com/watch?v=iNSvIdEwFKw) 

---

## 🚀 Core Features

### 🔐 Authentication & Authorization
- Built with **ASP.NET Core Identity** for secure user management
- JWT-based authentication and role-based authorization
- Predefined roles: `Admin`, `Teacher`, `Student`, and support for dynamic role management

### 🧩 Modular Academic Management
- APIs for managing Campuses, Schools, Departments, Programs, Semesters, and Batches
- User module supporting Students, Faculty, and Staff
- Dynamic role & permission system with admin controls

### 📚 Course & Activity Management
- CRUD operations for Courses and CourseSections
- Activity management (Assignments, Quizzes, Projects) with file upload support
- Integrated Azure Blob Storage for resource management

### 📈 Academic Tracking
- Attendance management with session details
- Automatic grading based on defined weightages
- Grade visibility controls for instructors
- Reports generation per student or course section

### 💬 Engagement
- Discussion boards per CourseSection
- Activity submissions and comments tracking

---

## ⚙️ Tech Stack

- 🧠 **.NET 8** (ASP.NET Core Web API)
- 🗃 **Entity Framework Core** – ORM for SQL Server
- 🔐 **ASP.NET Identity** – Auth & Role Management
- 📦 **AutoMapper** – Clean mapping between Entities and DTOs
- 📊 **Swagger** – Live API documentation (via Swashbuckle)
- 📫 **Postman** – API testing and collaboration
- ☁️ **Azure Blob Storage** – File storage integration
- 🧾 **Serilog** – Structured logging

---

## 📦 Dependencies

The following NuGet packages are used in this project:

- `Swashbuckle.AspNetCore` (Swagger documentation)
- `AutoMapper` (object mapping)
- `Azure.Storage.Blobs` (file uploads)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (user auth)
- `Microsoft.EntityFrameworkCore.SqlServer`, `Design`, `Tools`
- `SendGrid` (email notifications)
- `Serilog` (logging and diagnostics)
- `CsvHelper` (CSV data parsing)

You **do not need to install these manually** — just run:

```bash
dotnet restore
````

This will restore all required packages from the `.csproj` file automatically.

---

## 📁 Getting Started

### ✅ Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* SQL Server (or compatible RDBMS)
* Postman (for API testing)
* Azure Blob Storage credentials (optional for resource uploads)

### 🔧 Installation

```bash
git clone https://github.com/ali-shoaib-goraya/LMS_Backend.git
cd LMS_Backend
dotnet restore
```

### 🔐 Configuration

Create or update `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=FYP_LMS;Trusted_Connection=True;"
  },
  "JWT": {
    "Key": "your_secret_key_here",
    "Issuer": "LMS_API",
    "Audience": "LMS_Client"
  },
  "AzureBlobStorage": {
    "ConnectionString": "your_blob_connection_string",
    "ContainerName": "your_container_name"
  }
}
```

### 🗃️ Run Migrations

```bash
dotnet ef database update
```

### ▶️ Launch the API

```bash
dotnet run
```

API will be available at: `https://localhost:7244/`

---

## 📘 API Documentation

* Swagger UI: [`https://localhost:7244/swagger/index.html`](https://localhost:7244/swagger/index.html)

---

## 🧪 Key Project Concepts

* **Clean Architecture** – Layered structure (Controllers, Services, Repositories)
* **DTO Pattern** – Request/Response DTOs for separation of concerns
* **Unit of Work + Repository Pattern** – Ensures transactional consistency
* **Global Exception Handling** – Middleware for unified error responses
* **Validation** – Via `DataAnnotations` and/or `FluentValidation`

---

## 👨‍💻 Contributor

* **Ali Shoaib** – Backend Architecture, API Design, Authentication & Full Feature Implementation

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

> 🧠 Designed for academic excellence. Built with precision.

```
