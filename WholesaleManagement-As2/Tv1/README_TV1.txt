ASSIGNMENT 2 — TV1 HANDOVER
B2B WHOLESALE MANAGEMENT SYSTEM
Database (10 marks) + WinForms Admin (25 marks)
Prepared for TV1 handover — 09/11/2026
Student: Lê Trần Thanh Trúc (524H0073)

1. WHAT IS INSIDE

01_Database
  01_Standalone_Create_Schema.sql   Create WholesaleManagementDB and 5 tables
  02_Seed_Data.sql                  Insert test records for all 5 tables
  03_Verify_Data.sql                Check seeded data and order totals
  04_Reset_Demo_Admin.sql           Unlock local admin01 after password tests
  05_Integrity_Audit.sql            Check invalid values, orphans and duplicates

02_WinForms
  AdminBackOffice.sln               Open with Visual Studio 2022
  AdminBackOffice/*.cs              C# .NET Framework 4.8 WinForms source
  AdminBackOffice/App.config        SQL Server connection settings

03_Diagrams
  Database_ERD.png/.svg/.dot        Database diagram and editable DOT source
  WinForms_Flow.png/.svg/.dot       TV1 functional-flow diagram
  UI_Concept_Not_Screenshot.png    UI illustration only; not execution evidence

04_Documentation
  tv1.docx                         Historical TV1 draft/template material

05_Checklists
  SE_Lab_QT2_Assignment2_Team_Workbook_Template.xlsx
                                   Historical team workbook template

06_Evidence_To_Capture
  REQUIRED_REAL_SCREENSHOTS.txt   Actual screenshots to collect in Windows/SSMS

2. INSTALL / PREPARE

Windows 10/11, Visual Studio 2022 with .NET desktop development workload,
.NET Framework 4.8 targeting pack, SQL Server 2019 Express or higher, SSMS.
The prepared WinForms source does not require external NuGet packages.

3. RUN DATABASE (STANDALONE TV1 TEST)

Open SSMS, connect to your local SQL Server instance.
Open 01_Database/01_Standalone_Create_Schema.sql and Execute.
Open 01_Database/02_Seed_Data.sql and Execute.
Open 01_Database/03_Verify_Data.sql and Execute.
Open 01_Database/05_Integrity_Audit.sql and Execute.

Expected counts from the SQL seed file:
Item = 20, Agent = 15, Order = 18, OrderDetail = 27, Users = 18.
The audit should show zero violations. These counts are PREPARED values,
not the result of a SQL Server run performed in this environment.

4. RUN WINFORMS

Open 02_WinForms/AdminBackOffice.sln in Visual Studio 2022.
Open AdminBackOffice/App.config and edit the WholesaleDB connection string.
Example for local SQL Express with Windows authentication:
Data Source=.\SQLEXPRESS;Initial Catalog=WholesaleManagementDB;Integrated Security=True;Encrypt=False

For default SQL Server local instance, change Data Source to a dot (.); your
instance may have another name, e.g. (localdb)\MSSQLLocalDB or a LAN server.
Do not insert real database passwords in source code or GitHub.
Build > Build Solution, then press F5.

Seed TEST ACCOUNTS ONLY:
Admin:  admin01 / Admin@2026!
Admin:  admin02 / Admin@2026!
Agent:  agent01 / Agent@2026!

Agent credentials must be refused by the WinForms login form.
These are shared local demo credentials and must never be used in production.
Admin login locks after 5 incorrect attempts; to reset admin01 locally run
01_Database/04_Reset_Demo_Admin.sql in SSMS.

5. WHAT TO TEST AS TV1

Login: accepted Admin, rejected Agent, bad password, 5-failure lock.
Items: search, add, edit price/stock, delete unused item, reject deleting
       an item referenced by an OrderDetail, reject old RowVersion update.
Agents: search, add, edit, delete unused agent, reject deleting a referenced one.
        Create an Agent-role Web login for a selected registered agent.
Orders: read Order/OrderDetail, filter statuses, change Pending to Shipped.
Manifest: preview a selected order; Export HTML; open browser and Print / Save as PDF.
Dashboard: best-selling shipped items, low-stock threshold and shipped
           revenue by individual agent, date filter.
Integration: TV2 creates a Web order; refresh desktop to see it; mark it
             Shipped; TV2 checks My Orders and stock remains unchanged on shipping.

6. IMPORTANT: WEB CODE FIRST INTEGRATION

TV2 proposal: ASP.NET Core MVC Code First (Option C).
The TV1 standalone SQL schema creates database tables explicitly. For the
final TV2 Code First migration, CREATE A CLEAN EMPTY DATABASE and apply TV2's
matching EF Core migration. Do not create the schema with TV1 SQL and then
apply an initial EF Core migration that tries to create the same tables.
Run 02_Seed_Data.sql after a fresh EF schema only if its table names,
types and constraints match 01_Standalone_Create_Schema.sql. The final
group instructions use the SQL-script route; see the root README.md.

TV1 owns the database contract and WinForms, not the Web order transaction.
TV2 must implement the atomic order + details + stock decrement transaction.
TV3 verifies unit tests, integration and actual evidence.

7. SUBMISSION / EVIDENCE STATUS

DONE IN FILES: proposed schema, seed, queries, WinForms source, drawings,
report draft, checklists, technical handover.
NOT YET VERIFIED HERE: SQL Server execution, Visual Studio build/F5,
WinForms screens, Print to PDF, Code First integration, Git commits and PRs.
No fake screenshot or pass result has been inserted.

The lecturer asks for Report.docx, SourceCode.zip and Readme.txt submitted
by the group leader. TV1 must provide only the TV1 portion and real evidence
for TV2/TV3 integration; the final group report needs all three contributions.

8. GROUP SIGN-OFF

TV1: Lê Trần Thanh Trúc (524H0073) ______________________________
TV2: [Name / Student ID] __________________________________
TV3: [Name / Student ID] __________________________________

Confirmed shared contract date: ____/____/2026
Git repository and TV1 branch: ________________________________________

Final Assignment 2 package is assembled by the group leader: Report.docx, SourceCode.zip and Readme.txt. Do not submit the TV1-only ZIP as the complete group submission.

FINAL PACKAGE NOTE (19 Nov 2026)
This file preserves the earlier TV1 handover context. The consolidated
Report.docx, Team_Workbook.xlsx and root Readme.txt supersede its historical
pending statuses. Missing handover filenames above have been corrected to
the files actually present. Application source and SQL scripts are unchanged.
