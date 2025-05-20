# BANKING_APP

<em>Manage your finances securely and efficiently with our web-based banking platform.</em>

<!-- BADGES -->
<img src="https://img.shields.io/github/languages/top/mihaimiron1/Proiect_TW?style=flat&color=0080ff" alt="repo-top-language">
<img src="https://img.shields.io/github/languages/count/mihaimiron1/Proiect_TW?style=flat&color=0080ff" alt="repo-language-count">

<em>Built with the tools and technologies:</em>

<img src="https://img.shields.io/badge/C%23-239120.svg?style=flat&logo=c-sharp&logoColor=white" alt="C#">
<img src="https://img.shields.io/badge/ASP.NET-512BD4.svg?style=flat&logo=dotnet&logoColor=white" alt=".NET">
<img src="https://img.shields.io/badge/SQL%20Server-CC2927.svg?style=flat&logo=microsoft-sql-server&logoColor=white" alt="SQL Server">
<img src="https://img.shields.io/badge/Entity%20Framework-68217A.svg?style=flat&logo=entity-framework&logoColor=white" alt="Entity Framework">
<img src="https://img.shields.io/badge/Bootstrap-563D7C.svg?style=flat&logo=bootstrap&logoColor=white" alt="Bootstrap">

</div>
<br>

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Usage](#usage)
  - [Troubleshooting](#Troubleshooting)
- [Project Structure](#project-structure)
- [License](#license)

---

## Overview

**BANKING_APP** is a web-based banking system that allows users to create and manage accounts, perform money transfers, and review transaction history. Designed using the ASP.NET MVC framework, it showcases real-world financial interactions in a simplified academic context.

---

## Features

- 🔐 User registration and login system  
- 🧾 View balance and account summary  
- 💰 Deposit, withdraw, and transfer funds  
- 🗂️ Transaction history and records  
- 🛠️ Admin panel (basic controls for demonstration)

---

## Getting Started

### Prerequisites

Ensure you have the following installed:

- Visual Studio 2019 or later  
- .NET Framework 4.7.2 or newer  
- SQL Server (LocalDB or Express)

### Installation

1. **Clone the repository:**

```sh
git clone https://github.com/mihaimiron1/Proiect_TW

```
2. **Open the solution in Visual Studio:**
```sh
cd Proiect_TW
```
3. **Restore NuGet packages and build the project.**
4. **Update the database:**
Open the Package Manager Console and run:
```sh
Update-Database
```
### Usage

After building, run the app by pressing F5 or selecting Start Debugging in Visual Studio.

### Troubleshooting
If you encounter errors related to Razor or the compiler platform, run the following NuGet command:
```sh
Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```
This will reinstall necessary compilation dependencies for ASP.NET.

## Project Structure
/Controllers       --> Business logic and routing  
/Models            --> Data models and DB entities  
/Views             --> Razor views for UI  
/App_Data          --> Local database storage  
## License
This project is developed for academic purposes only and should not be used in production environments.
No warranties or guarantees are provided.



