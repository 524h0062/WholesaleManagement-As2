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
  Post-order stock was not captured.

- Failed order:
  The failed attempt left order count, detail count and
  checked stock quantities unchanged.
  The temporary test trigger was subsequently removed.

- Authorization and lockout:
  Evidence covers role restrictions, rejection of access
  to another agent's order, account lockout after five
  failed login attempts, and resetting the test account.

## Limitations

- The duplicate-item stock update has not been verified.
- The exact-stock order test has not been executed.
- Concurrent ordering has not been verified.
- Unit-test execution results must be reported separately
  from SQL Server integration tests.
