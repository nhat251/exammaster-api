# 📋 README

## Mô Tả Dự Án

Đây là một **dự án cơ sở (Base Project)** được thiết kế để clone về và sử dụng làm nền tảng cho các dự án API khác. Project này cung cấp một cấu trúc đầy đủ với các tính năng xác thực và phân quyền có sẵn.

## Công Nghệ Sử Dụng

- **Framework**: .NET (ASP.NET Core)
- **Authentication**: JWT (JSON Web Tokens)
- **Authorization**: Identity Framework
- **Architecture**: Clean Architecture Pattern / Domain Driven Design
- **Database**: Entity Framework Core

## Cấu Trúc Dự Án

```
├── Application/          # Lớp ứng dụng (Services, DTOs, Validators)
├── Domain/               # Lớp miền (Entities, Interfaces)
├── Infrastructure/       # Lớp cơ sở hạ tầng (Repositories, DbContext)
├── Common/               # Lớp chung (Exceptions, Pagination)
├── Config/               # Cấu hình JWT
└── codebase-api/         # API Controllers
```

## Hướng Dẫn Sử Dụng

### 1. Clone Dự Án

```bash
git clone https://github.com/nhat251/codebase.git
cd codebase
```

### 2. Lấy về & Tùy chỉnh dự án (Đổi tên + Database)

- Chọn một tên dự án có ý nghĩa và thay thế toàn bộ chuỗi `codebase` trong namespace và tên project bằng tên mới.
- Các bước chính:
	1. Đổi tên thư mục và file (thư mục solution / project) sang `YourProjectName` hoặc tên bạn chọn.
	2. Open file `.sln` bằng notepad để thay đổi `codebase` thành `YourProjectName`.
	3. Double click file `.sln` để vào IDE. Dùng chức năng thay thế trong IDE để đổi `codebase` thành `YourProjectName`.
	4. Cập nhật cấu hình database: chỉnh sửa connection string trong `appsettings.json` (hoặc nơi bạn lưu cấu hình). Nếu bạn đổi tên database, cập nhật tên database trong connection string.

Sau khi đổi tên project và cập nhật connection string, tạo migration khởi tạo (từ thư mục gốc của repo):

```bash
cd Infrastructure
dotnet ef migrations add INIT_DATABASE --startup-project ../codebase-api --output-dir Migrations

# Sau khi tạo migrations, áp dụng lên database
dotnet ef database update --startup-project ../codebase-api
```

### 3. Xóa Remote Cũ và Thêm Remote Mới

```bash
# Xóa remote cũ
git remote remove origin

# Thêm remote mới
git remote add origin <your-new-repository-url>

# Tạo branch mới nếu cần
git branch -M main

# Push lên repository mới
git push -u origin main
```

### 4. Cài Đặt Dependencies

```bash
dotnet restore
```

<!-- Merged into section 2 -->

### 5. Chạy Ứng Dụng

Bạn có thể chạy ứng dụng bằng một trong hai cách:

**Cách 1: Sử dụng autorun.bat (Windows)**
```bash
autorun.bat
```
Đơn giản nhất - chỉ cần double-click file `autorun.bat` trong thư mục gốc để khởi động server API.

**Cách 2: Sử dụng dotnet CLI**
```bash
cd codebase-api
dotnet run
```

Server sẽ khởi động trên các port được cấu hình trong `autorun.bat` (mặc định: `https://localhost:7068` và `http://localhost:5261`).

## Tính Năng Chính

- ✅ **JWT Authentication**: Hệ thống xác thực với JWT Token
- ✅ **User Management**: Quản lý người dùng
- ✅ **Refresh Token**: Hỗ trợ refresh token
- ✅ **Token Blacklist**: Danh sách đen token
- ✅ **Global Exception Handling**: Xử lý ngoại lệ toàn cầu
- ✅ **Pagination**: Hỗ trợ phân trang
- ✅ **Validation**: Validator cho các request
- ✅ **Mapper**: AutoMapper cho DTO mapping

## Thêm Tính Năng Mới

Khi clone project này, bạn có thể:

1. Thêm các Entity mới trong `Domain/Entities/`
2. Tạo Repository mới kế thừa từ `IGenericRepository`
3. Viết Business Logic trong `Application/Services/`
4. Tạo DTOs trong `Application/DTOs/`
5. Thêm Controllers mới trong `codebase-api/Controllers/`
6. Tạo Migrations cho Database nếu có thay đổi schema

---

## 🇬🇧 English Version

### Project Description

This is a **Base Project** designed to be cloned and used as a foundation for other API projects. This project provides a complete structure with built-in authentication and authorization features.

### Technology Stack

- **Framework**: .NET (ASP.NET Core)
- **Authentication**: JWT (JSON Web Tokens)
- **Authorization**: Identity Framework
- **Architecture**: Clean Architecture Pattern
- **Database**: Entity Framework Core / Domain Driven Design

### Project Structure

```
├── Application/          # Application layer (Services, DTOs, Validators)
├── Domain/               # Domain layer (Entities, Interfaces)
├── Infrastructure/       # Infrastructure layer (Repositories, DbContext)
├── Common/               # Common layer (Exceptions, Pagination)
├── Config/               # JWT Configuration
└── codebase-api/         # API Controllers
```

### Usage Guide

#### 1. Clone the Project

```bash
git clone https://github.com/nhat251/codebase.git
cd codebase
```

#### 2. Get & Customize the Project (Rename + Database)

- Choose a meaningful project name and replace every instance of `codebase` in namespaces and project names with your new name.
- Main steps:
	1. Rename solution/project folders and files to your new project name.
	2. Open the `.sln` file in Notepad and replace all `codebase` occurrences.
	3. Open the solution in your IDE and use the Replace feature to rename all remaining `codebase` references.
	4. Update your database configuration: edit the connection string in `appsettings.json` (or wherever you store configuration). If you rename your database, update its name in the connection string.

After renaming and updating the connection string, create the initial migration and apply it:

```bash
cd Infrastructure
dotnet ef migrations add INIT_DATABASE --startup-project ../codebase-api --output-dir Migrations

# Apply migrations to the database
dotnet ef database update --startup-project ../codebase-api
```

#### 3. Remove Old Remote and Add New Remote

```bash
# Remove old remote
git remote remove origin

# Add new remote
git remote add origin <your-new-repository-url>

# Create new branch if needed
git branch -M main

# Push to new repository
git push -u origin main
```

#### 4. Install Dependencies

```bash
dotnet restore
```

<!-- Merged into section 2 -->

#### 5. Run the Application

You can run the application using either method:

**Option 1: Using autorun.bat (Windows)**
```bash
autorun.bat
```
Simply double-click the `autorun.bat` file in the root directory to start the API server.

**Option 2: Using dotnet CLI**
```bash
cd codebase-api
dotnet run
```

The application will start on the configured ports (default: `https://localhost:7068` and `http://localhost:5261`).

### Key Features

- ✅ **JWT Authentication**: Authentication system with JWT tokens
- ✅ **User Management**: User management functionality
- ✅ **Refresh Token**: Refresh token support
- ✅ **Token Blacklist**: Token blacklist functionality
- ✅ **Global Exception Handling**: Global exception handling
- ✅ **Pagination**: Pagination support
- ✅ **Validation**: Request validators
- ✅ **Mapper**: AutoMapper for DTO mapping

### Adding New Features

When cloning this project, you can:

1. Add new Entities in `Domain/Entities/`
2. Create new Repository inheriting from `IGenericRepository`
3. Write Business Logic in `Application/Services/`
4. Create DTOs in `Application/DTOs/`
5. Add new Controllers in `codebase-api/Controllers/`
6. Create Migrations for Database schema changes

### Contact

If you have any questions, please contact the development team.

---

## 👨‍💻 Người Phát Triển

- **Birthday Nguyen**
  - GitHub: [@nhat251](https://github.com/nhat251)
  - Facebook: [Birthday Nguyen](https://www.facebook.com/nhat251)
