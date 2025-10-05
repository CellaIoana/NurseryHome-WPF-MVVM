NurseryHome — WPF MVVM + SQL Server
A complete WPF MVVM application for managing a nursery with role-based dashboards: Admin, Parent, Educator.
Implements full CRUD, authentication, and SQL Server persistence in a clean MVVM architecture.
✨ Features
•	🔐 Login & Authentication with role management (Admin / Parent / Educator)
•	👩‍💼 Admin Dashboard — CRUD for children, parents, educators, groups
•	📚 Educator Dashboard — manage groups, programs, materials, announcements, absences
•	👨‍👩‍👧 Parent Dashboard — view children’s info, updates, announcements
•	🗄️ Database integration via DatabaseManager.cs with SQL Server
•	🧩 Organized in MVVM layers: Models / Views / ViewModels
⚡ Quick Start
1) Clone the repo
```bash
git clone git@github.com:CellaIoana/NurseryHome-WPF-MVVM.git
cd NurseryHome-WPF-MVVM
```
2) Configure SQL Server
- Create a database named NurseryHome in SQL Server
- Run the provided Database/NurseryHome.sql script (if available)
- Update the connection string in App.config:
```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=.;Database=NurseryHome;Trusted_Connection=True;"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```
3) Build & Run
- Open solution NurseryHome.sln in Visual Studio
- Build Solution → Run (F5)
🧱 Project Structure
```
NurseryHome-WPF-MVVM/
 ├─ NurseryHome.sln
 ├─ NurseryHome/
 │   ├─ Models/         # Data models (Child, Parent, Educator, Group...)
 │   ├─ ViewModels/     # MVVM logic (RelayCommand, AdminVM, ParentVM...)
 │   ├─ Views/          # XAML windows & dashboards
 │   ├─ Database/       # DatabaseManager.cs & helpers
 │   └─ App.config      # DB connection string
 └─ docs/               # screenshots, diagrams
```
📸 Screenshots
![Admin Dashboard](docs/admin_dashboard.png)
![Educator Dashboard](docs/educator_dashboard.png)
🛠️ Tech Stack
•	Language: C#
•	Framework: WPF MVVM
•	Database: SQL Server
•	Tools: Visual Studio, GitHub Actions
🗺️ Roadmap
•	Unit tests for DatabaseManager
•	Export reports (PDF/CSV)
•	Notifications via email
📝 License
MIT License — see LICENSE for details.

