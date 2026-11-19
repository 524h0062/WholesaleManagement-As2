using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using AdminBackOffice.Data;

namespace AdminBackOffice.Services
{
    public static class ManifestService
    {
        public static void ExportHtml(int orderId, string outputPath)
        {
            var header = OrderRepository.GetHeader(orderId);
            var details = OrderRepository.GetDetails(orderId);
            if (header.Rows.Count == 0 || details.Rows.Count == 0) throw new InvalidOperationException("Order or order details not found.");
            var row = header.Rows[0];
            var content = new StringBuilder();
            content.Append("<!doctype html><html lang=\"en\"><head><meta charset=\"utf-8\"><title>Shipping Manifest</title>");
            content.Append("<style>body{font-family:Arial,sans-serif;color:#1b3145;max-width:900px;margin:35px auto}h1{color:#12324d}table{width:100%;border-collapse:collapse;margin-top:20px}th,td{padding:11px;border-bottom:1px solid #cfd9e3;text-align:left}th{background:#edf3fa}.total{font-weight:bold;text-align:right}small{color:#64748b}@media print{button{display:none}body{margin:15mm}}</style></head><body>");
            content.Append("<h1>SHIPPING MANIFEST</h1><p>Wholesale Management System · Back Office</p>");
            content.Append("<p><b>Order #</b> " + orderId + "<br><b>Ordered:</b> " + ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm") + "<br><b>Status:</b> " + Clean(row["OrderStatus"]) + "</p>");
            content.Append("<p><b>Agent:</b> " + Clean(row["AgentName"]) + "<br><b>Delivery address:</b> " + Clean(row["Address"]) + "<br><b>Phone:</b> " + Clean(row["Phone"]) + "</p>");
            content.Append("<table><thead><tr><th>Item</th><th>Size</th><th>Quantity</th><th>Unit amount (VND)</th><th>Line total (VND)</th></tr></thead><tbody>");
            decimal total = 0;
            foreach (DataRow detail in details.Rows)
            {
                var line = Convert.ToDecimal(detail["LineTotal"]);
                total += line;
                content.Append("<tr><td>" + Clean(detail["ItemName"]) + "</td><td>" + Clean(detail["Size"]) + "</td><td>" + detail["Quantity"] + "</td><td>" + Money(detail["UnitAmount"]) + "</td><td>" + Money(line) + "</td></tr>");
            }
            content.Append("</tbody></table><p class=\"total\">TOTAL: " + Money(total) + " VND</p>");
            content.Append("<p><small>For internal warehouse fulfilment. Not a tax invoice. Stock is deducted when the agent submits the order, not at shipment.</small></p>");
            content.Append("<button onclick=\"window.print()\">PRINT / SAVE AS PDF</button></body></html>");
            File.WriteAllText(outputPath, content.ToString(), Encoding.UTF8);
        }

        public static void Preview(int orderId, IWin32Window parent)
        {
            var document = CreatePrintDocument(orderId);
            using (var preview = new PrintPreviewDialog { Document = document, Width = 1050, Height = 760 })
                preview.ShowDialog(parent);
            document.Dispose();
        }

        private static PrintDocument CreatePrintDocument(int orderId)
        {
            var header = OrderRepository.GetHeader(orderId);
            var details = OrderRepository.GetDetails(orderId);
            if (header.Rows.Count == 0 || details.Rows.Count == 0) throw new InvalidOperationException("Order data is missing.");
            var row = header.Rows[0];
            var lines = new List<string>();
            lines.Add("SHIPPING MANIFEST - ORDER #" + orderId);
            lines.Add("Ordered: " + Convert.ToDateTime(row["OrderDate"]).ToString("yyyy-MM-dd HH:mm") + " | " + row["OrderStatus"]);
            lines.Add("Agent: " + row["AgentName"]);
            lines.Add("Address: " + row["Address"]);
            lines.Add("Phone: " + row["Phone"]);
            lines.Add(new string('-', 92));
            decimal total = 0;
            foreach (DataRow detail in details.Rows)
            {
                total += Convert.ToDecimal(detail["LineTotal"]);
                lines.Add(detail["ItemName"] + " (" + detail["Size"] + ") | Qty: " + detail["Quantity"] + " | Price: " + Money(detail["UnitAmount"]) + " | Total: " + Money(detail["LineTotal"]));
            }
            lines.Add(new string('-', 92));
            lines.Add("TOTAL: " + Money(total) + " VND");
            lines.Add("Internal warehouse record - Not a tax invoice");
            var index = 0;
            var document = new PrintDocument { DocumentName = "ShippingManifest_Order_" + orderId };
            document.BeginPrint += (sender, args) => index = 0;
            document.PrintPage += (sender, args) =>
            {
                var area = args.MarginBounds;
                var y = area.Top;
                using (var font = new Font("Segoe UI", 10f))
                using (var heading = new Font("Segoe UI", 15f, FontStyle.Bold))
                {
                    while (index < lines.Count)
                    {
                        var useFont = index == 0 ? heading : font;
                        var size = args.Graphics.MeasureString(lines[index], useFont, area.Width);
                        if (y + size.Height + 8 > area.Bottom) break;
                        args.Graphics.DrawString(lines[index], useFont, Brushes.Black, new RectangleF(area.Left, y, area.Width, size.Height + 8));
                        y += (int)size.Height + 9;
                        index++;
                    }
                }
                args.HasMorePages = index < lines.Count;
            };
            return document;
        }

        private static string Clean(object text)
        {
            return WebUtility.HtmlEncode(Convert.ToString(text));
        }

        private static string Money(object amount)
        {
            return Convert.ToDecimal(amount).ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
        }
    }
}
