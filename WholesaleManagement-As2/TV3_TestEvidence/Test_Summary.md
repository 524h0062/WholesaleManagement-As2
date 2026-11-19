# TV3 Testing Summary

## Verified results

- End-to-end order processing:
  Order #19 was created through the Web portal and displayed
  in WinForms. Its status was changed to Shipped and the
  updated status was displayed on the Web portal.

- Single-line order:
  Order #27 contained 2 units of A4 Copy Paper.
  Total: 137,000 VND.
  A4 stock decreased from 167 to 165.
  Order count increased from 19 to 20.
  Order detail count increased from 29 to 30.

- Duplicate-item order:
  Order #28 combined two A4 Copy Paper inputs, 3 and 4,
  into one saved detail with quantity 7.
  Total: 479,500 VND.
  A4 stock decreased from 165 to 158. A3 stock remained 73.
  Order count increased from 20 to 21; detail count from 30 to 31.
  Evidence: TV3_Duplicate_After.png; TV3_Duplicate_Detail.png.

- Failed order:
  The failed attempt left order count, detail count and
  checked stock quantities unchanged.
  Later troubleshooting identified dbo.[TR_TV#_TestRollBack] and an
  OUTPUT/trigger compatibility error. The evidence proves unchanged
  persistent data, not execution of the intended trigger THROW.
  The cleanup image checks TR_TV3_TestRollback, a different name.
  Order #27 confirms recovery after removing the remaining test trigger.

- Authorization and lockout:
  Evidence covers role restrictions, rejection of access
  to another agent's order, account lockout after five
  failed login attempts, and resetting the test account.

- Unit tests:
  The submitted suite contains eight in-memory OrderRules tests.
  TV3_UnitTests_8Passed.png shows all eight passed.

- Shipping stock:
  E2E_07_Stock_After_Shipped.png shows A4 167 and A3 73, unchanged.

## Limitations

- The exact-stock order test has not been executed.
- Concurrent ordering has not been verified.
- Unit-test execution results must be reported separately
  from SQL Server integration tests.

- Full desktop CRUD, manifest, dashboard and desktop lockout runtime coverage
  is incomplete.
- PR #1 was merged without a recorded peer review; Git began after the
  initial implementation.
