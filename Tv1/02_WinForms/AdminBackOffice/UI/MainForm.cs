using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AdminBackOffice.Data;
using AdminBackOffice.Models;
using AdminBackOffice.Services;

namespace AdminBackOffice.UI
{
    public sealed class MainForm : Form
    {
        private readonly TabControl tabs = new TabControl();
        private readonly DataGridView itemsGrid = Ui.Grid();
        private readonly DataGridView agentsGrid = Ui.Grid();
        private readonly DataGridView ordersGrid = Ui.Grid();
        private readonly DataGridView orderDetailsGrid = Ui.Grid();
        private readonly DataGridView bestGrid = Ui.Grid();
        private readonly DataGridView lowGrid = Ui.Grid();
        private readonly DataGridView revenueGrid = Ui.Grid();
        private readonly TextBox itemFilter = Ui.TextInput(220);
        private readonly TextBox agentFilter = Ui.TextInput(220);
        private readonly TextBox orderFilter = Ui.TextInput(120);
        private readonly ComboBox statusFilter = new ComboBox();
        private readonly ComboBox revenueAgent = new ComboBox();
        private readonly NumericUpDown stockThreshold = new NumericUpDown();
        private readonly DateTimePicker dateFrom = new DateTimePicker();
        private readonly DateTimePicker dateTo = new DateTimePicker();
        private readonly Label orderSummary = new Label();
        private readonly Label dashboardSummary = new Label();
        private readonly string signedInUser;

        public MainForm(string userName)
        {
            signedInUser = userName;
            Text = "Wholesale Management | Back Office";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1050, 690);
            BackColor = Ui.Canvas;
            Font = Ui.Normal;

            var top = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Ui.Charcoal };
            top.Controls.Add(new Label { Text = "WHOLESALE  /  BACK OFFICE", AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 18f, FontStyle.Bold), Location = new Point(21, 16) });
            top.Controls.Add(new Label { Text = "Inventory · Agents · Orders · Sales", AutoSize = true, ForeColor = Ui.HeaderMuted, Location = new Point(23, 51) });
            var signOut = Ui.Button("LOG OUT", (s, e) => Close(), false, 102);
            signOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            signOut.Location = new Point(Width - 145, 19);
            top.Resize += (s, e) => signOut.Location = new Point(top.Width - 138, 20);
            top.Controls.Add(signOut);
            var welcome = new Label { Text = "Admin: " + signedInUser, AutoSize = true, ForeColor = Color.White, Font = Ui.Normal, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            welcome.Location = new Point(500, 30);
            top.Resize += (s, e) => welcome.Location = new Point(Math.Max(450, top.Width - 310), 30);
            top.Controls.Add(welcome);
   

            tabs.Dock = DockStyle.Fill;
            tabs.Font = new Font("Segoe UI", 10.2f, FontStyle.Bold);
            tabs.ItemSize = new Size(150, 42);
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.DrawItem += (sender, args) =>
            {
                var active = args.Index == tabs.SelectedIndex;
                var bounds = tabs.GetTabRect(args.Index);
                using (var background = new SolidBrush(active ? Color.White : Color.FromArgb(235, 229, 222)))
                    args.Graphics.FillRectangle(background, bounds);
                TextRenderer.DrawText(args.Graphics, tabs.TabPages[args.Index].Text, tabs.Font, bounds,
                    active ? Ui.Charcoal : Ui.Ink, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                if (active)
                {
                    using (var accent = new SolidBrush(Ui.Brick))
                        args.Graphics.FillRectangle(accent, new Rectangle(bounds.X + 13, bounds.Bottom - 4, bounds.Width - 26, 3));
                }
            };
            tabs.Controls.Add(new TabPage("Items") { BackColor = Ui.Canvas, UseVisualStyleBackColor = false, Controls = { CreateItemsPage() } });
            tabs.Controls.Add(new TabPage("Agents") { BackColor = Ui.Canvas, UseVisualStyleBackColor = false, Controls = { CreateAgentsPage() } });
            tabs.Controls.Add(new TabPage("Orders") { BackColor = Ui.Canvas, UseVisualStyleBackColor = false, Controls = { CreateOrdersPage() } });
            tabs.Controls.Add(new TabPage("Dashboard") { BackColor = Ui.Canvas, UseVisualStyleBackColor = false, Controls = { CreateDashboardPage() } });
            tabs.SelectedIndexChanged += (s, e) => RefreshCurrentTab();

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 96F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            top.Dock = DockStyle.Fill;
            tabs.Dock = DockStyle.Fill;

            mainLayout.Controls.Add(top, 0, 0);
            mainLayout.Controls.Add(tabs, 0, 1);

            Controls.Add(mainLayout);

            top.BringToFront();
            Shown += (s, e) => RefreshCurrentTab();
        }

        private Control CreateItemsPage()
        {
            var toolbar = Ui.Toolbar();
            toolbar.Controls.Add(Ui.FieldLabel("Item / size:"));
            toolbar.Controls.Add(itemFilter);
            toolbar.Controls.Add(Ui.Button("SEARCH", (s, e) => Safe(RefreshItems), false, 100));
            toolbar.Controls.Add(Ui.Button("ADD ITEM", (s, e) => EditItem(false), true, 116));
            toolbar.Controls.Add(Ui.Button("EDIT", (s, e) => EditItem(true), false, 90));
            toolbar.Controls.Add(Ui.Button("DELETE", (s, e) => DeleteItem(), false, 90));
            toolbar.Controls.Add(Ui.Button("REFRESH", (s, e) => Safe(RefreshItems), false, 95));
            itemsGrid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditItem(true); };
            return Ui.Page("Item & Inventory Management", itemsGrid, toolbar);
        }

        private Control CreateAgentsPage()
        {
            var toolbar = Ui.Toolbar();
            toolbar.Controls.Add(Ui.FieldLabel("Agent / phone:"));
            toolbar.Controls.Add(agentFilter);
            toolbar.Controls.Add(Ui.Button("SEARCH", (s, e) => Safe(RefreshAgents), false, 100));
            toolbar.Controls.Add(Ui.Button("ADD AGENT", (s, e) => EditAgent(false), true, 124));
            toolbar.Controls.Add(Ui.Button("EDIT", (s, e) => EditAgent(true), false, 90));
            toolbar.Controls.Add(Ui.Button("DELETE", (s, e) => DeleteAgent(), false, 90));
            toolbar.Controls.Add(Ui.Button("CREATE LOGIN", (s, e) => CreateAgentLogin(), true, 136));
            toolbar.Controls.Add(Ui.Button("REFRESH", (s, e) => Safe(RefreshAgents), false, 95));
            agentsGrid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditAgent(true); };
            return Ui.Page("Agent Management", agentsGrid, toolbar);
        }

        private Control CreateOrdersPage()
        {
            var panel = new Panel { Dock = DockStyle.Fill };
            var gridContainer = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 290, Panel1MinSize = 150, Panel2MinSize = 150 };
            gridContainer.Panel1.Controls.Add(ordersGrid);
            var detailsArea = new Panel { Dock = DockStyle.Fill };
            orderSummary.Dock = DockStyle.Top;
            orderSummary.Height = 42;
            orderSummary.ForeColor = Ui.Charcoal;
            orderSummary.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            orderSummary.Text = "Select an order to see its details.";
            detailsArea.Controls.Add(orderDetailsGrid);
            detailsArea.Controls.Add(orderSummary);
            gridContainer.Panel2.Controls.Add(detailsArea);
            ordersGrid.SelectionChanged += (s, e) => Safe(RefreshOrderDetails);

            var toolbar = Ui.Toolbar();
            statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            statusFilter.Items.AddRange(new object[] { "All", "Pending", "Shipped" });
            statusFilter.SelectedIndex = 0;
            statusFilter.Width = 110;
            statusFilter.Margin = new Padding(5, 8, 12, 4);
            toolbar.Controls.Add(Ui.FieldLabel("Status:"));
            toolbar.Controls.Add(statusFilter);
            toolbar.Controls.Add(Ui.FieldLabel("Order ID:"));
            toolbar.Controls.Add(orderFilter);
            toolbar.Controls.Add(Ui.Button("SEARCH", (s, e) => Safe(RefreshOrders), false, 96));
            toolbar.Controls.Add(Ui.Button("MARK SHIPPED", (s, e) => MarkShipped(), true, 140));
            toolbar.Controls.Add(Ui.Button("PREVIEW", (s, e) => PreviewManifest(), false, 102));
            toolbar.Controls.Add(Ui.Button("EXPORT REPORT", (s, e) => ExportManifest(), false, 145));
            toolbar.Controls.Add(Ui.Button("REFRESH", (s, e) => Safe(RefreshOrders), false, 100));
            panel.Controls.Add(Ui.Page("Incoming Order Management", gridContainer, toolbar));
            return panel;
        }

        private Control CreateDashboardPage()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, BackColor = Ui.Canvas, Padding = new Padding(5) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 54));
            layout.Controls.Add(PanelWithGrid("Best-selling items (shipped orders)", bestGrid), 0, 0);
            layout.Controls.Add(PanelWithGrid("Low-stock warning", lowGrid), 1, 0);
            layout.Controls.Add(PanelWithGrid("Revenue by agent (shipped orders)", revenueGrid), 0, 1);
            layout.SetColumnSpan(revenueGrid.Parent, 2);

            var toolbar = Ui.Toolbar();
            dateFrom.Format = DateTimePickerFormat.Short;
            dateFrom.Width = 120;
            dateFrom.Value = new DateTime(2026, 1, 1);
            dateTo.Format = DateTimePickerFormat.Short;
            dateTo.Width = 120;
            dateTo.Value = DateTime.Today;
            stockThreshold.Minimum = 0;
            stockThreshold.Maximum = 1000000;
            stockThreshold.Value = 30;
            stockThreshold.Width = 65;
            revenueAgent.Width = 170;
            revenueAgent.DropDownStyle = ComboBoxStyle.DropDownList;
            toolbar.Controls.Add(Ui.FieldLabel("From:"));
            toolbar.Controls.Add(dateFrom);
            toolbar.Controls.Add(Ui.FieldLabel("To:"));
            toolbar.Controls.Add(dateTo);
            toolbar.Controls.Add(Ui.FieldLabel("Low stock ≤"));
            toolbar.Controls.Add(stockThreshold);
            toolbar.Controls.Add(Ui.FieldLabel("Agent:"));
            toolbar.Controls.Add(revenueAgent);
            toolbar.Controls.Add(Ui.Button("APPLY FILTERS", (s, e) => Safe(RefreshDashboard), true, 145));
            dashboardSummary.Dock = DockStyle.Bottom;
            dashboardSummary.Height = 33;
            dashboardSummary.ForeColor = Ui.Charcoal;
            dashboardSummary.TextAlign = ContentAlignment.MiddleLeft;
            var wrapper = new Panel { Dock = DockStyle.Fill };
            wrapper.Controls.Add(layout);
            wrapper.Controls.Add(dashboardSummary);
            return Ui.Page("Warehouse Sales Dashboard", wrapper, toolbar);
        }

        private static Control PanelWithGrid(string labelText, DataGridView grid)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(5), Margin = new Padding(7) };
            p.Controls.Add(grid);
            var title = new Label { Text = labelText, ForeColor = Ui.Charcoal, Font = new Font("Segoe UI", 10.5f, FontStyle.Bold), Dock = DockStyle.Top, Height = 33, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(8, 0, 0, 0) };
            p.Controls.Add(title);
            return p;
        }

        private void RefreshCurrentTab()
        {
            Safe(() =>
            {
                if (tabs.SelectedIndex == 0) RefreshItems();
                if (tabs.SelectedIndex == 1) RefreshAgents();
                if (tabs.SelectedIndex == 2) RefreshOrders();
                if (tabs.SelectedIndex == 3)
                {
                    RefreshRevenueAgents();
                    RefreshDashboard();
                }
            });
        }

        private static void Bind(DataGridView grid, DataTable table)
        {
            grid.DataSource = table;
            if (grid.Columns.Contains("RowVersion")) grid.Columns["RowVersion"].Visible = false;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Name == "Price" || column.Name == "TotalVnd" || column.Name == "UnitAmount" || column.Name == "LineTotal" || column.Name == "RevenueVnd")
                    column.DefaultCellStyle.Format = "N0";
                if (column.Name == "OrderDate") column.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }
        }

        private void RefreshItems() { Bind(itemsGrid, ItemRepository.Search(itemFilter.Text)); }
        private void RefreshAgents() { Bind(agentsGrid, AgentRepository.Search(agentFilter.Text)); }
        private void RefreshOrders()
        {
            Bind(ordersGrid, OrderRepository.Search(Convert.ToString(statusFilter.SelectedItem), orderFilter.Text));
            RefreshOrderDetails();
        }

        private static DataRow Selected(DataGridView grid)
        {
            var bound = grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
            return bound == null ? null : bound.Row;
        }

        private void EditItem(bool existing)
        {
            Safe(() =>
            {
                ItemRecord record = null;
                if (existing)
                {
                    var row = Selected(itemsGrid);
                    if (row == null) return;
                    record = new ItemRecord
                    {
                        ItemID = Convert.ToInt32(row["ItemID"]),
                        ItemName = row["ItemName"].ToString(),
                        Size = row["Size"].ToString(),
                        Price = Convert.ToDecimal(row["Price"]),
                        StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                        RowVersion = (byte[])row["RowVersion"]
                    };
                }
                using (var dialog = new ItemEditorForm(record))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    ItemRepository.Save(dialog.Result);
                    RefreshItems();
                }
            });
        }

        private void DeleteItem()
        {
            Safe(() =>
            {
                var row = Selected(itemsGrid);
                if (row == null || !Ui.Confirm("Delete selected item? Items linked to orders cannot be deleted.")) return;
                ItemRepository.Delete(Convert.ToInt32(row["ItemID"]), (byte[])row["RowVersion"]);
                RefreshItems();
            });
        }

        private void EditAgent(bool existing)
        {
            Safe(() =>
            {
                AgentRecord record = null;
                if (existing)
                {
                    var row = Selected(agentsGrid);
                    if (row == null) return;
                    record = new AgentRecord
                    {
                        AgentID = Convert.ToInt32(row["AgentID"]),
                        AgentName = row["AgentName"].ToString(),
                        Address = row["Address"].ToString(),
                        Phone = row["Phone"].ToString()
                    };
                }
                using (var dialog = new AgentEditorForm(record))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    AgentRepository.Save(dialog.Result);
                    RefreshAgents();
                }
            });
        }

        private void DeleteAgent()
        {
            Safe(() =>
            {
                var row = Selected(agentsGrid);
                if (row == null || !Ui.Confirm("Delete selected agent? Agents linked to orders or users cannot be deleted.")) return;
                AgentRepository.Delete(Convert.ToInt32(row["AgentID"]));
                RefreshAgents();
            });
        }

        private void CreateAgentLogin()
        {
            Safe(() =>
            {
                var row = Selected(agentsGrid);
                if (row == null) return;
                using (var dialog = new AgentLoginEditorForm(Convert.ToInt32(row["AgentID"]), row["AgentName"].ToString()))
                    dialog.ShowDialog(this);
            });
        }

        private int SelectedOrderId()
        {
            var row = Selected(ordersGrid);
            return row == null ? 0 : Convert.ToInt32(row["OrderID"]);
        }

        private void RefreshOrderDetails()
        {
            var id = SelectedOrderId();
            if (id <= 0)
            {
                Bind(orderDetailsGrid, new DataTable());
                orderSummary.Text = "Select an order to see its details.";
                return;
            }
            Bind(orderDetailsGrid, OrderRepository.GetDetails(id));
            orderSummary.Text = "ORDER #" + id + "  |  Order details and purchase-time unit amounts";
        }

        private void MarkShipped()
        {
            Safe(() =>
            {
                var id = SelectedOrderId();
                if (id <= 0 || !Ui.Confirm("Confirm Order #" + id + " has been shipped? This does not deduct stock again.")) return;
                if (OrderRepository.MarkShipped(id)) MessageBox.Show("Order marked as Shipped.");
                else MessageBox.Show("Order is no longer Pending. Refresh to see its latest status.");
                RefreshOrders();
            });
        }

        private void PreviewManifest()
        {
            Safe(() =>
            {
                var id = SelectedOrderId();
                if (id == 0) return;
                ManifestService.Preview(id, this);
            });
        }

        private void ExportManifest()
        {
            Safe(() =>
            {
                var id = SelectedOrderId();
                if (id <= 0) return;
                using (var save = new SaveFileDialog { Title = "Export Shipping Manifest", Filter = "HTML report (*.html)|*.html", FileName = "ShippingManifest_Order_" + id + ".html" })
                {
                    if (save.ShowDialog(this) != DialogResult.OK) return;
                    ManifestService.ExportHtml(id, save.FileName);
                    MessageBox.Show("Shipping Manifest exported. Open the file and choose Print / Save as PDF.");
                    if (Ui.Confirm("Open exported report now?")) Process.Start(save.FileName);
                }
            });
        }

        private void RefreshRevenueAgents()
        {
            var current = revenueAgent.SelectedValue;
            var data = AgentRepository.Lookup();
            var row = data.NewRow();
            row["AgentID"] = 0;
            row["AgentName"] = "All agents";
            data.Rows.InsertAt(row, 0);
            revenueAgent.DataSource = data;
            revenueAgent.DisplayMember = "AgentName";
            revenueAgent.ValueMember = "AgentID";
            if (current != null && !(current is DataRowView))
            {
                try { revenueAgent.SelectedValue = Convert.ToInt32(current); }
                catch (Exception) { revenueAgent.SelectedIndex = 0; }
            }
            else revenueAgent.SelectedIndex = 0;
        }

        private void RefreshDashboard()
        {
            var start = dateFrom.Value.Date;
            var end = dateTo.Value.Date.AddDays(1);
            if (end <= start) throw new ArgumentException("The end date must be on or after the start date.");
            var agent = revenueAgent.SelectedValue;
            var agentId = 0;
            if (agent != null && !(agent is DataRowView)) int.TryParse(agent.ToString(), out agentId);
            var best = DashboardRepository.BestSelling(start, end);
            var low = DashboardRepository.LowStock(Convert.ToInt32(stockThreshold.Value));
            var revenue = DashboardRepository.RevenueByAgent(start, end, agentId);
            Bind(bestGrid, best);
            Bind(lowGrid, low);
            Bind(revenueGrid, revenue);
            decimal total = 0;
            foreach (DataRow row in revenue.Rows) total += Convert.ToDecimal(row["RevenueVnd"]);
            dashboardSummary.Text = "Shipped revenue in selected period: " + total.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VND    |    Low-stock items: " + low.Rows.Count;
        }

        private static void Safe(Action action)
        {
            try { action(); }
            catch (Exception error) { Ui.Error(error); }
        }
    }
}
