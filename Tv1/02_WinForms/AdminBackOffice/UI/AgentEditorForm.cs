using System;
using System.Drawing;
using System.Windows.Forms;
using AdminBackOffice.Models;

namespace AdminBackOffice.UI
{
    public sealed class AgentEditorForm : Form
    {
        private readonly TextBox agentName = Ui.TextInput(330);
        private readonly TextBox address = Ui.TextInput(330);
        private readonly TextBox phone = Ui.TextInput(330);
        private readonly AgentRecord original;

        public AgentRecord Result { get; private set; }

        public AgentEditorForm(AgentRecord source)
        {
            original = source ?? new AgentRecord();
            Text = original.AgentID == 0 ? "Add agent" : "Edit agent";
            ClientSize = new Size(440, 350);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Ui.Canvas;
            Font = Ui.Normal;
            AddField("Agent name", agentName, 22, original.AgentName);
            AddField("Address", address, 95, original.Address);
            AddField("Phone", phone, 168, original.Phone);
            var save = Ui.Button("SAVE AGENT", (s, e) => Save(), true, 147);
            save.Location = new Point(27, 265);
            Controls.Add(save);
            var cancel = Ui.Button("CANCEL", (s, e) => { DialogResult = DialogResult.Cancel; Close(); }, false, 123);
            cancel.Location = new Point(187, 265);
            Controls.Add(cancel);
            AcceptButton = save;
        }

        private void AddField(string caption, TextBox field, int y, string value)
        {
            Controls.Add(new Label { Text = caption, AutoSize = true, Location = new Point(28, y) });
            field.Location = new Point(28, y + 25);
            field.Text = value;
            Controls.Add(field);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(agentName.Text) || string.IsNullOrWhiteSpace(address.Text) || string.IsNullOrWhiteSpace(phone.Text))
            {
                MessageBox.Show("Please complete all agent fields.");
                return;
            }
            Result = new AgentRecord
            {
                AgentID = original.AgentID,
                AgentName = agentName.Text.Trim(),
                Address = address.Text.Trim(),
                Phone = phone.Text.Trim()
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
