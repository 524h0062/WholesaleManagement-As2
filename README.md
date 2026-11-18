# Wholesale Management — Assignment 2

A wholesale management system with a Web Agent Portal
and a WinForms Back Office sharing a SQL Server database.

## Repository

https://github.com/524h0062/WholesaleManagement-As2

## Project folders

- Tv1/01_Database: database creation, seed and verification scripts.
- Tv1/02_WinForms: WinForms Back Office.
- TV2: Web Agent Portal, unit tests and report files.
- TV3_TestEvidence: integration testing screenshots and test summary.

## Requirements

- Windows.
- Visual Studio with ASP.NET and web development,
  and .NET desktop development workloads.
- .NET 8 SDK for the Web application.
- SQL Server Express and SQL Server Management Studio.
- Internet access for restoring NuGet packages.

Install any additional targeting pack requested by Visual Studio
when opening the WinForms project.

## Database setup

For a new database, open the scripts in Tv1/01_Database
using SQL Server Management Studio and execute in this order:

1. 01_Standalone_Create_Schema.sql
2. 02_Seed_Data.sql
3. 03_Verify_Data.sql
4. 05_Integrity_Audit.sql

Database name: WholesaleManagementDB.

Do not rerun the seed script merely to start the application.
Do not apply an initial EF migration that recreates tables
already created by the SQL scripts.

## Connection configuration

Update the WholesaleDB connection string in:

- TV2/WholesaleAgentPortal/appsettings.Development.json
- Tv1/02_WinForms/AdminBackOffice/App.config

Both applications must connect to the same SQL Server instance
and the same WholesaleManagementDB database.

Use the SQL Server instance name on the target computer.
The instance used during testing was:
DESKTOP-RF4B5N1\SQLEXPRESS

## Run the Web Agent Portal

1. Open TV2/TV2_AgentPortal.sln in Visual Studio.
2. Restore NuGet packages and build the solution.
3. Set WholesaleAgentPortal as the startup project.
4. Run using the Development profile.
5. Sign in using an Agent account.

## Run the WinForms Back Office

1. Open Tv1/02_WinForms/AdminBackOffice.sln.
2. Restore NuGet packages and build the solution.
3. Run the WinForms application.
4. Sign in using an Admin account.

## Demo accounts

These accounts are for the local seeded demonstration database.

| Application | Username | Password |
|---|---|---|
| Web | agent01 | Agent@2026! |
| WinForms | admin01 | Admin@2026! |

## Run unit tests

1. Open TV2/TV2_AgentPortal.sln.
2. Select Test > Test Explorer.
3. Build the solution and select Run All Tests.
4. Read the actual results in Test Explorer.

Unit-test results are separate from SQL Server integration tests.

## Testing evidence

See TV3_TestEvidence and the testing section of the report.

Recorded evidence includes:
- Web-to-WinForms order processing.
- Single-line order creation and stock deduction.
- Duplicate-item consolidation.
- Failed-order persistence checks.
- Authorization and account lockout checks.

Duplicate-item stock verification, exact-stock ordering and
concurrent ordering are not fully evidenced and must not
be reported as passed.

## Troubleshooting

- Connection failure: check the SQL Server service, instance name,
  database name and connection strings.
- Order saving failure after rollback testing: inspect remaining
  temporary test triggers and remove only the test trigger.
- Remove temporary diagnostic messages before submission.
