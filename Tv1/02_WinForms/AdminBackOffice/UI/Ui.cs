using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdminBackOffice.UI
{
    public static class Ui
    {
        public static readonly Color Charcoal = Color.FromArgb(54, 51, 50);
        public static readonly Color Brick = Color.FromArgb(153, 74, 58);
        public static readonly Color Canvas = Color.FromArgb(248, 245, 240);
        public static readonly Color Ink = Color.FromArgb(43, 41, 39);
        public static readonly Color Selected = Color.FromArgb(244, 227, 218);
        public static readonly Color Border = Color.FromArgb(214, 207, 199);
        public static readonly Color HeaderMuted = Color.FromArgb(230, 218, 209);
        public static readonly Font Normal = new Font("Segoe UI", 10f);

        public static Button Button(string title, EventHandler click, bool primary = true, int width = 110)
        {
            var control = new Button
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Width = width,
                Height = 37,
                BackColor = primary ? Brick : Color.White,
                ForeColor = primary ? Color.White : Charcoal,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(4)
            };
            control.FlatAppearance.BorderColor = primary ? Brick : Border;
            control.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(132, 60, 47) : Selected;
            control.FlatAppearance.MouseDownBackColor = primary ? Color.FromArgb(109, 49, 39) : Color.FromArgb(234, 218, 208);
            if (click != null) control.Click += click;
            return control;
        }

        public static Label Title(string text)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 17f, FontStyle.Bold), ForeColor = Charcoal, AutoSize = true, Margin = new Padding(6, 5, 12, 8) };
        }

        public static Label FieldLabel(string text)
        {
            return new Label { Text = text, AutoSize = true, ForeColor = Ink, Font = Normal, Margin = new Padding(6, 11, 5, 5) };
        }

        public static TextBox TextInput(int width = 210)
        {
            return new TextBox { Width = width, Font = Normal, Height = 30, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White, ForeColor = Ink, Margin = new Padding(5, 8, 12, 5) };
        }

        public static DataGridView Grid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                EnableHeadersVisualStyles = false,
                GridColor = Border,
                Font = new Font("Segoe UI", 9.3f),
                Margin = new Padding(8)
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(92, 78, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.2f, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 78, 72);
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Ink;
            grid.DefaultCellStyle.SelectionBackColor = Selected;
            grid.DefaultCellStyle.SelectionForeColor = Ink;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 249, 246);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Selected;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Ink;
            return grid;
        }

        public static FlowLayoutPanel Toolbar()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 54,
                Padding = new Padding(6, 5, 5, 3),
                AutoScroll = true,
                WrapContents = false,
                BackColor = Canvas
            };
        }

        public static Panel Page(string title, Control content, Control toolbar)
        {
            var page = new Panel { Dock = DockStyle.Fill, BackColor = Canvas, Padding = new Padding(12) };
            var header = new Panel { Dock = DockStyle.Top, Height = 49 };
            var label = Title(title);
            label.Location = new Point(4, 5);
            header.Controls.Add(label);
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            if (toolbar != null)
            {
                toolbar.Dock = DockStyle.Top;
                page.Controls.Add(toolbar);
            }
            page.Controls.Add(header);
            return page;
        }

        public static void Error(Exception error)
        {
            MessageBox.Show(error.Message, "Operation could not be completed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool Confirm(string message)
        {
            return MessageBox.Show(message, "Please confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
