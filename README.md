# 🎓 Final Year Project - LMS Backend

Welcome to the **Learning Management System (LMS) Backend** — a robust and scalable REST API built with **.NET 8**, designed to power a full-featured educational platform with role-based access control, dynamic resource management, and real-time learning engagement.

> 🛠️ This backend supports all academic, administrative, and interactive functionalities of the LMS through secure and well-documented APIs.

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
- 🧪 **Swagger** – Live API documentation
- 📫 **Postman** – API testing and collaboration
- ☁️ **Azure Blob Storage** – File storage integration

---

## 📁 Getting Started

### 📦 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server or any configured RDBMS
- Postman (for testing)
- Azure Blob Storage account (for file uploads)

### 🔧 Installation

```bash
git clone https://github.com/your-org/fyp-backend.git
cd fyp-backend
````

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

> 🔁 Don’t forget to apply migrations if needed.

```bash
dotnet ef database update
```

### ▶️ Run the Application

```bash
dotnet run
```

API will be available at: `https://localhost:7244/`

---

## 📘 API Documentation

* ✅ Swagger UI: https://localhost:7244/swagger/index.html)

---

## 🧪 Key Project Concepts

* **Clean Architecture**: Layered structure (Controllers, Services, Repositories)
* **DTO Pattern**: Use of Request/Response DTOs for all endpoints
* **Unit of Work + Repository Pattern**: To ensure transactional consistency
* **Global Exception Handling**: Custom middleware for consistent error responses
* **Validation**: Using FluentValidation or DataAnnotations

---

## 👨‍💻 Contributor

* **Ali Shoaib** – Backend Architecture, API Design, Authentication & Full Feature Implementation

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

> 🧠 Designed for academic excellence. Built with precision.

```

