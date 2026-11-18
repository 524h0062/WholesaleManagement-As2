using System;
using System.Drawing;
using System.Windows.Forms;
using AdminBackOffice.Services;

namespace AdminBackOffice.UI
{
    public sealed class AgentLoginEditorForm : Form
    {
        private readonly TextBox username = Ui.TextInput(320);
        private readonly TextBox email = Ui.TextInput(320);
        private readonly TextBox password = Ui.TextInput(320);
        private readonly int agentId;

        public AgentLoginEditorForm(int selectedAgentId, string agentName)
        {
            agentId = selectedAgentId;
            Text = "Create Agent Web Login";
            ClientSize = new Size(445, 365);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Ui.Canvas;
            Font = Ui.Normal;
            Controls.Add(new Label { Text = "New Web login for: " + agentName, AutoSize = true, Location = new Point(26, 14), Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Ui.Charcoal });
            AddField("Username", username, 58);
            AddField("Email", email, 127);
            AddField("Temporary password", password, 196);
            password.UseSystemPasswordChar = true;
            var save = Ui.Button("CREATE ACCOUNT", (s, e) => Save(), true, 175);
            save.Location = new Point(27, 280);
            Controls.Add(save);
            var cancel = Ui.Button("CANCEL", (s, e) => Close(), false, 110);
            cancel.Location = new Point(220, 280);
            Controls.Add(cancel);
            AcceptButton = save;
        }

        private void AddField(string title, TextBox field, int y)
        {
            Controls.Add(new Label { Text = title, AutoSize = true, Location = new Point(28, y) });
            field.Location = new Point(28, y + 23);
            Controls.Add(field);
        }

        private void Save()
        {
            try
            {
                AgentUserService.CreateAgentLogin(agentId, username.Text, email.Text, password.Text);
                MessageBox.Show("Agent login account created. Ask the agent to change the temporary password when the Web Portal supports it.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception error) { Ui.Error(error); }
        }
    }
}
