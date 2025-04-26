![.NET Core](https://img.shields.io/badge/.NET_Core-8.0-blueviolet)
![License: MIT](https://img.shields.io/badge/License-MIT-green)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20MacOS-lightgrey)


# 📦 Inventory Management System

A simple and efficient **C# Console Application** for managing inventory operations.

---

## ✨ Features
- 📋 Manage products (Add, Update, Delete)
- 🛒 Track suppliers and purchases
- 📦 Monitor inventory levels
- 🖥️ Command-line interface (CLI) based

---

## 🚀 Technologies Used
- C# .NET Core (Console App)
- Entity Framework Core (for database interaction)
- SQL Server

---

## 🛠️ Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/Vaydelux/InventoryManagementSystem.git
```

### 2. Set Up the Database
- Configure `appsettings.json` (if any) with your SQL Server connection string.
- Apply EF Core migrations:
```bash
dotnet ef database update
```

### 3. Build and Run
```bash
dotnet build
dotnet run
```

---

## 🧩 Project Structure

```
├── Models/         # Entity classes
├── Data/           # Database context
├── Program.cs      # Main entry point
├── Services/       # (Optional) Logic handling
```

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 🤝 Contributions

Feel free to fork and contribute! Open a pull request if you have ideas for improvements.

