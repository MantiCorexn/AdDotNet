# BloodBank

A web app for managing blood donors and their donation history, built with **ASP.NET Core MVC** and **Entity Framework Core** on **SQL Server**.

## Features

- **Donor management:** add, edit and delete donors (name, blood group, contact, city, last donation date)
- **Reports:**
  - Filter donors by blood group
  - Sort donors by most recent donation
  - Donation count per donor
  - Total blood volume collected
- **Validation** on both server and client (data annotations + jQuery unobtrusive validation)
- **Safe by default:** anti-forgery tokens on every form, overposting protection on create/edit, donors with donation history can't be deleted

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Data access | Entity Framework Core 10 |
| Database | SQL Server / SQL Server Express |
| UI | Razor views, Bootstrap 5 |

## Data model

```
Donor (DonorId, FullName, BloodGroup, ContactNo, City, LastDonationDate)
  1 ──── * Donation (DonationId, DonorId, DonationDate, VolumeMl, CampName)
```

## Getting started

**Requirements:** .NET 10 SDK and SQL Server Express (or any SQL Server instance).

```bash
git clone https://github.com/MantiCorexn/BloodBank-MVC.git
cd BloodBank-MVC/BloodBank
dotnet run
```

In development the database (`BloodBankDB`) is created automatically on first run.

The default connection string targets a local SQL Server Express instance (`.\SQLEXPRESS`) with Windows authentication. To use a different server without editing the committed config, set it with user secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:BloodBankDB" "Server=YOUR_SERVER;Database=BloodBankDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

## Project structure

```
BloodBank/
├── Controllers/   DonorController (CRUD), ReportsController (reports)
├── Models/        Donor, Donation, BloodBankDbContext, view models
├── Views/         Razor views (Donor, Reports, shared layout)
└── Program.cs     Service registration and request pipeline
```
