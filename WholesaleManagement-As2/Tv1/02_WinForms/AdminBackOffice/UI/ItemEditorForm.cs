using System;
using System.Drawing;
using System.Windows.Forms;
using AdminBackOffice.Models;

namespace AdminBackOffice.UI
{
    public sealed class ItemEditorForm : Form
    {
        private readonly TextBox itemName = Ui.TextInput(290);
        private readonly TextBox size = Ui.TextInput(290);
        private readonly NumericUpDown price = new NumericUpDown();
        private readonly NumericUpDown stock = new NumericUpDown();
        private readonly ItemRecord record;

        public ItemRecord Result { get; private set; }

        public ItemEditorForm(ItemRecord source)
        {
            record = source ?? new ItemRecord();
            Text = record.ItemID == 0 ? "Add item" : "Edit item";
            ClientSize = new Size(425, 364);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Ui.Canvas;
            Font = Ui.Normal;

            AddLabel("Item name", 22);
            itemName.Location = new Point(28, 49);
            itemName.Text = record.ItemName;
            Controls.Add(itemName);
            AddLabel("Size / package", 92);
            size.Location = new Point(28, 119);
            size.Text = record.Size;
            Controls.Add(size);
            AddLabel("Price (VND)", 162);
            price.Location = new Point(28, 189);
            price.Width = 290;
            price.Maximum = 999999999999;
            price.DecimalPlaces = 2;
            price.ThousandsSeparator = true;
            price.Value = record.Price;
            Controls.Add(price);
            AddLabel("Stock quantity", 232);
            stock.Location = new Point(28, 259);
            stock.Width = 290;
            stock.Maximum = 100000000;
            stock.Value = record.StockQuantity;
            Controls.Add(stock);
            var save = Ui.Button("SAVE ITEM", (s, e) => Save(), true, 145);
            save.Location = new Point(28, 308);
            Controls.Add(save);
            var cancel = Ui.Button("CANCEL", (s, e) => { DialogResult = DialogResult.Cancel; Close(); }, false, 120);
            cancel.Location = new Point(186, 308);
            Controls.Add(cancel);
            AcceptButton = save;
        }

        private void AddLabel(string text, int y)
        {
            Controls.Add(new Label { Text = text, AutoSize = true, Location = new Point(28, y), ForeColor = Ui.Ink });
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(itemName.Text) || string.IsNullOrWhiteSpace(size.Text))
            {
                MessageBox.Show("Item name and size are required.");
                return;
            }
            Result = new ItemRecord
            {
                ItemID = record.ItemID,
                ItemName = itemName.Text.Trim(),
                Size = size.Text.Trim(),
                Price = price.Value,
                StockQuantity = Convert.ToInt32(stock.Value),
                RowVersion = record.RowVersion
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
