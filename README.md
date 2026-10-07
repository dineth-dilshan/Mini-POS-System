[README.md](https://github.com/user-attachments/files/33164598/README.md)
<div align="center">

# Mini POS System

### A C# Windows Forms project for managing library records

![C#](https://img.shields.io/badge/Language-C%23-239120?logo=csharp&logoColor=white)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server%20LocalDB-CC2927)

Manage books, members, and borrowing records through a desktop interface.

</div>

## About the Project

This project demonstrates how C# Windows Forms and a local SQL Server database can be used to build a desktop management application. It includes account registration, login, book management, member management, and book borrowing and returns.

**Repository naming:** The repository is named `Mini-POS-System`, but its current source code implements a library management system. Retail billing, payments, and receipt printing are not implemented in this version.

## Features

- **Account registration and login:** Create an account with password confirmation and duplicate username checks, then log in to access the dashboard.
- **Book management:** Add, update, and delete books, with title, author, shelf location, and available copies.
- **Member management:** Add, update, and delete members, with name, member type, contact number, email, and status.
- **Borrowing:** Select a member and a book, check available copies, record the loan, and reduce available stock.
- **Due dates:** Automatically set the due date to 14 days after borrowing.
- **Returns:** Record the return date, mark the loan as returned, restore available stock, and prevent repeated returns of an already returned record.
- **Borrowing history:** View records with member names, book titles, dates, and return status.
- **Status filtering:** Filter borrowing records by `Returned` or `Not Returned`.

## Technology Stack

| Component | Technology |
| --- | --- |
| Programming language | C# |
| Desktop interface | Windows Forms |
| Target framework | .NET Framework 4.8 |
| Database | SQL Server Express LocalDB |
| Data access | ADO.NET / System.Data.SqlClient |
| Development environment | Visual Studio |

## Getting Started

### Requirements

- Windows
- Visual Studio with the **.NET desktop development** workload
- .NET Framework **4.8 targeting pack**
- SQL Server Express LocalDB with an `MSSQLLocalDB` instance
- Git or GitHub Desktop to clone the repository

### 1. Clone the Repository

```bash
git clone https://github.com/dineth-dilshan/Mini-POS-System.git
cd Mini-POS-System
```

Alternatively, use **File → Clone repository → URL** in GitHub Desktop.

### 2. Open the Project

Open `WindowsFormsApplication3.csproj` in Visual Studio. A solution file is not currently included in the repository.

### 3. Configure the Database Connection

The repository includes `LibraryDB.mdf` and `LibraryDB_log.ldf`. The source files currently use a database path from the original development computer.

Replace the `AttachDbFilename` value with the full path to your local copy of `LibraryDB.mdf` in each of these files:

- `Login.cs`
- `Register.cs`
- `Book.cs`
- `Member.cs`
- `Borrow and Return.cs`

Example connection string—replace the example path with your own:

```csharp
SqlConnection conn = new SqlConnection(
    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\Mini-POS-System\LibraryDB.mdf;Integrated Security=True");
```

Use Visual Studio's SQL Server Object Explorer to check that LocalDB can open the supplied database. The application queries the tables `user`, `Books`, `Members`, and `BorrowRecords`.

### 4. Build and Run

1. Select **Build → Build Solution**.
2. Press **F5** to run the application.
3. Use the registration screen to create an account, then log in.
4. Open the book and member screens to add records.
5. Open the borrow and return screen to issue books and record returns.

## Main Project Files

| File | Purpose |
| --- | --- |
| `WindowsFormsApplication3.csproj` | Project configuration and framework target |
| `Program.cs` | Application entry point |
| `Login.cs` | Account login |
| `Register.cs` | Account registration |
| `librarydashboard.cs` | Navigation to the management screens |
| `Book.cs` | Book records and available copies |
| `Member.cs` | Member records |
| `Borrow and Return.cs` | Loans, returns, and status filtering |
| `LibraryDB.mdf` | Local SQL Server database |
| `LibraryDB_log.ldf` | Database transaction log |
| `Properties/` | Application resources and settings |

Each form also has a `.Designer.cs` file for its interface layout and a `.resx` file for resources.

## Current Limitations

- Database paths must be configured manually in multiple source files.
- Book and member search handlers need correction before their search features can be relied on.
- Passwords are currently stored and compared directly; password hashing should be implemented before using real accounts.
- Borrowing and stock updates use separate database commands rather than a transaction.
- This README is based on source inspection; a Windows build and runtime check have not been performed as part of its preparation.

## Possible Improvements

- Move the database connection string into application configuration.
- Improve validation, error handling, and database connection cleanup.
- Fix book and member searching.
- Add password hashing and role-based access.
- Use database transactions for borrowing and returns.
- Add overdue reminders, reports, and data export.
- Add screenshots and automated tests.

## Contributing

Suggestions and improvements are welcome. Open an issue to describe a bug or proposed feature, or submit a pull request with a clear explanation of your changes.

## Author

[dineth-dilshan](https://github.com/dineth-dilshan)

## License

No license file is currently included. Contact the repository owner regarding permission to reuse or distribute the project.
