USE WholesaleManagementDB;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Item)
BEGIN
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'A4 Copy Paper',N'Ream',68500.00,170);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'A3 Copy Paper',N'Ream',119000.00,75);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Ballpoint Pen Blue',N'Box 50',88000.00,250);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Ballpoint Pen Black',N'Box 50',88000.00,190);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Notebook A5',N'Pack 10',98000.00,160);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Notebook A4',N'Pack 10',148000.00,85);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Whiteboard Marker',N'Box 12',156000.00,100);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Permanent Marker',N'Box 12',138000.00,80);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Sticky Notes',N'Pack 12',112000.00,130);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Stapler Metal',N'Unit',69000.00,60);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Staples No.10',N'Box 20',72000.00,195);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Packing Tape',N'Roll 6',88000.00,225);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Bubble Wrap',N'Roll 50m',265000.00,55);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Shipping Box Small',N'Pack 25',165000.00,165);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Shipping Box Large',N'Pack 25',320000.00,85);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'USB Drive 32GB',N'Unit',125000.00,98);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Wireless Mouse',N'Unit',175000.00,70);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'USB Keyboard',N'Unit',215000.00,40);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Desk Organizer',N'Unit',145000.00,25);
    INSERT dbo.Item (ItemName,Size,Price,StockQuantity) VALUES (N'Office Scissors',N'Pack 12',168000.00,33);
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Agent)
BEGIN
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Saigon Retail Supplies',N'12 Nguyen Van Troi, Phu Nhuan, HCMC',N'0903123456');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Minh Anh Stationery',N'78 Le Loi, District 1, HCMC',N'0903234567');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Da Nang Trading Co.',N'201 Nguyen Van Linh, Da Nang',N'0903345678');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Hanoi Office Mart',N'46 Tran Duy Hung, Hanoi',N'0903456789');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Mekong Distributor',N'9 Hung Vuong, Can Tho',N'0903567890');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Binh Duong Wholesale',N'20 Yersin, Thu Dau Mot',N'0903678901');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Vung Tau Stationery',N'12 Ba Cu, Vung Tau',N'0903789012');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Nha Trang Business Supply',N'85 Tran Phu, Nha Trang',N'0903890123');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Dong Nai Office Hub',N'140 Pham Van Thuan, Bien Hoa',N'0903901234');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Hue Commercial Co.',N'5 Hung Vuong, Hue',N'0903012345');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Quang Ninh Retail Point',N'102 Ha Long, Quang Ninh',N'0903123457');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Long An Office Partners',N'7 Nguyen Hue, Tan An',N'0903234568');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Bac Ninh Wholesale',N'18 Ly Thai To, Bac Ninh',N'0903345679');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Gia Lai Distribution',N'33 Tran Phu, Pleiku',N'0903456790');
    INSERT dbo.Agent (AgentName,Address,Phone) VALUES (N'Phu Quoc Trading',N'17 Tran Hung Dao, Phu Quoc',N'0903567901');
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'admin01',N'admin01@wholesale.demo',N'PBKDF2$120000$zvCKtyO5NE+QWSgxva45bA==$OdyK1jUbs3ZcmOWooiaWkHbYWkNt8xgBe2iJZ8PecX4=',0,N'Admin',NULL);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'admin02',N'admin02@wholesale.demo',N'PBKDF2$120000$O1IHmX8dD86+FQfwPpk9Cw==$LDop61v0wdVFQREi6YPXe/XHlQP9XBp9tk2ly4izrd0=',0,N'Admin',NULL);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'admin03',N'admin03@wholesale.demo',N'PBKDF2$120000$H3V+E/v1EOGJmujPLh8/kw==$tPjz7/wsn+qFIUEXkS5R0j9yha4Fybvtjrp3GMZHKxo=',0,N'Admin',NULL);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent01',N'agent01@wholesale.demo',N'PBKDF2$120000$WDrPFn0G3ndxrU/esNAEaQ==$kfjAspyfdk2iWuy5y3SwlYGW0NLwtAMzpLWUWGOmPGs=',0,N'Agent',1);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent02',N'agent02@wholesale.demo',N'PBKDF2$120000$tkQa4eXj+JOe8rOOznBgLg==$T8qOYuk0nmUVDeawBMEGpitsNU6Z/3dWxrMN0uqeyg4=',0,N'Agent',2);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent03',N'agent03@wholesale.demo',N'PBKDF2$120000$vlSq8a8sVMnCJXv+nKzMBg==$Q2SjSL/ou6q5Sud0wRFiEjlVv4nQjQqrsCiS2J48XkQ=',0,N'Agent',3);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent04',N'agent04@wholesale.demo',N'PBKDF2$120000$NNUQB3P9MZLB/PwownTXsw==$+4h7MteJgd89MeD7UCpU+ciOMa6hVNQEL0Kf/G/kHNs=',0,N'Agent',4);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent05',N'agent05@wholesale.demo',N'PBKDF2$120000$rbHPvZ4oTHezhS5bxKErWA==$q5W/kxeOmis5fNrCLcz4F3I6gUbE1p8Yy2UDzJQu6BA=',0,N'Agent',5);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent06',N'agent06@wholesale.demo',N'PBKDF2$120000$yNXEnbwleKURTwB1GCim+g==$uwg9gD8/fEmx+e5gZcbTOhcBqtnXr3KEcpxFkuqlqSo=',0,N'Agent',6);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent07',N'agent07@wholesale.demo',N'PBKDF2$120000$qZDRU/QjTg0nmAHvJ41iIQ==$3u3RLh5nNfG/in4bRz0WyE6ng2OCVCYy1jT8DKSpH2M=',0,N'Agent',7);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent08',N'agent08@wholesale.demo',N'PBKDF2$120000$aLwKvAUb7bCED024+iFGYQ==$RwEOfgRgnS6dZRRv6q60hMQ07VZhSEx1AcWBcoXU5S8=',0,N'Agent',8);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent09',N'agent09@wholesale.demo',N'PBKDF2$120000$3jyy2OPRj0vg9HWg1Sl2NA==$Ggmexra7iBB90vZY+g6Sirr+JiJlOnZKJyiQSoXCGDA=',0,N'Agent',9);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent10',N'agent10@wholesale.demo',N'PBKDF2$120000$cjdwxxtSKOqd5nfpRWV80A==$MMqQ3L0wvDjYf+5YvDbZZOIoQMpzy38EJ6F3UMR/LFE=',0,N'Agent',10);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent11',N'agent11@wholesale.demo',N'PBKDF2$120000$W1Ok+fzJX0hL8Sl9mUKqfA==$DZsi3k3SzjXIsxA6ly68uj7Ku6mz6egurnJqYJUZ3zY=',0,N'Agent',11);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent12',N'agent12@wholesale.demo',N'PBKDF2$120000$RpdXkX7fToxkESiXk1z/jA==$mhi8iXwn5CcoWwtLftoQa5OqQ4DtlqheHl9qD/1QBho=',0,N'Agent',12);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent13',N'agent13@wholesale.demo',N'PBKDF2$120000$cnimu5KpqxT69V3ecR4ddg==$q8F7JpdhLXLXj6WRbfJm80dX1bUgvatGZUHdInh4MMU=',0,N'Agent',13);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent14',N'agent14@wholesale.demo',N'PBKDF2$120000$NVwn3oIRwrasnDW/+7kKMA==$br8z1YT7CopoPAxy0ZIoLL+GHa3cVmwwHCaeTlSGPbQ=',0,N'Agent',14);
    INSERT dbo.Users (UserName,Email,[Password],[Lock],[Role],AgentID) VALUES (N'agent15',N'agent15@wholesale.demo',N'PBKDF2$120000$e8jbrYpJoNMwEmeAG2LK8g==$fd+gA0RFu46nopItNJrXXxJX/kud7MbhhJUD21D+sQ4=',0,N'Agent',15);
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.[Order])
BEGIN
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-12T10:15:00', 1, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-14T11:15:00', 2, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-16T12:15:00', 3, N'Pending');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-18T13:15:00', 4, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-20T14:15:00', 5, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-22T15:15:00', 6, N'Pending');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-24T16:15:00', 7, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-26T09:15:00', 8, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-28T10:15:00', 9, N'Pending');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-09-30T11:15:00', 10, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-02T12:15:00', 11, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-04T13:15:00', 12, N'Pending');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-06T14:15:00', 13, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-08T15:15:00', 14, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-10T16:15:00', 15, N'Pending');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-12T09:15:00', 1, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-14T10:15:00', 2, N'Shipped');
    INSERT dbo.[Order] (OrderDate,AgentID,OrderStatus) VALUES ('2026-10-16T11:15:00', 3, N'Pending');
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.OrderDetail)
BEGIN
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (1,8,3,138000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (1,12,4,88000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (2,11,4,72000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (3,14,5,165000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (3,18,6,215000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (4,17,6,175000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (5,20,1,168000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (5,4,2,88000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (6,3,2,88000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (7,6,3,148000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (7,10,4,69000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (8,9,4,112000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (9,12,5,88000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (9,16,6,125000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (10,15,6,320000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (11,18,1,215000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (11,2,2,119000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (12,1,2,68500.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (13,4,3,88000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (13,8,4,138000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (14,7,4,156000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (15,10,5,69000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (15,14,6,165000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (16,13,6,265000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (17,16,1,125000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (17,20,2,168000.00);
    INSERT dbo.OrderDetail (OrderID,ItemID,Quantity,UnitAmount) VALUES (18,19,2,145000.00);
END
GO
