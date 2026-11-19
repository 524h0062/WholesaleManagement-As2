# Wholesale Management — Assignment 2

A wholesale management system with a Web Agent Portal
and a WinForms Back Office sharing a SQL Server database.

## Repository

https://github.com/524h0062/WholesaleManagement-As2

## Project folders

- Tv1/01_Database: database creation, seed and verification scripts.
- Tv1/02_WinForms: WinForms Back Office.
- TV2: Web Agent Portal and unit tests.
- Final Report.docx and Team_Workbook.xlsx are beside SourceCode.zip in the submission.
- TV3_TestEvidence: integration testing screenshots and test summary.

## Requirements

- Windows.
- Visual Studio 2022 version 17.8 or later with ASP.NET and web development,
  and .NET desktop development workloads.
- .NET 8 SDK for the Web application.
- SQL Server Express and SQL Server Management Studio.
- Internet access for restoring NuGet packages.

Install the .NET Framework 4.8 targeting pack for the WinForms project.

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

Order #28 combines A4 quantities 3 + 4 into one saved detail.
A4 stock changes from 165 to 158; order/detail counts become 21/31.
The eight submitted unit tests passed in the supplied Test Explorer evidence.
Exact-stock and concurrent SQL Server integration tests remain Not run.
Full desktop CRUD, manifest, dashboard and desktop lockout runtime coverage
is incomplete.

The failed-save test demonstrates unchanged persistent data. Later
troubleshooting identified an SQL OUTPUT/trigger compatibility error;
execution of the intended trigger THROW is not established. The earlier
cleanup image checks TR_TV3_TestRollback, not TR_TV#_TestRollBack.
Successful Order #27 demonstrates recovery after troubleshooting.

## Troubleshooting

- Connection failure: check the SQL Server service, instance name,
  database name and connection strings.
- Order saving failure after rollback testing: inspect remaining
  temporary test triggers and remove only the test trigger.
- Remove temporary diagnostic messages before submission.

## Recorded contributions

The existing report and workbook record the following allocation.
The group should confirm these percentages against the actual work.

| Member | Student ID | Responsibility | Contribution |
|---|---|---|---|
| Lê Trần Thanh Trúc | 524H0073 | Database and WinForms | 33% |
| Quan Bội San | 524H0185 | Web Agent Portal | 34% |
| Diệp Phương Nghi | 524H0062 | Unit tests, integration, configuration, Git and report | 33% |
| Total | | | 100% |

## Git evidence

Main branch: master. TV3 branch: tv3/testing-report.
PR #1: https://github.com/524h0062/WholesaleManagement-As2/pull/1
The supplied evidence shows this PR merged, with no peer review recorded.
Git was introduced after the initial implementation; the initial import
is not a complete development history. No new Git push is performed by
extracting this final submission ZIP.

## Submission and historical references

Submit Report.docx, SourceCode.zip and Readme.txt together.
Team_Workbook.xlsx is the supporting working record.
Extract SourceCode.zip before opening either Visual Studio solution.
Use Report.docx and Team_Workbook.xlsx as the current consolidated record.
The TV1 handover report/checklist and original task-allocation document
are retained as historical references. Their earlier pending statuses
and template instructions are not the final group results.
Video demo: not provided (optional).
