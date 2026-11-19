using System;
using System.Drawing;
using System.Windows.Forms;
using AdminBackOffice.Services;

namespace AdminBackOffice.UI
{
    public sealed class LoginForm : Form
    {
        private readonly TextBox username = Ui.TextInput(310);
        private readonly TextBox password = Ui.TextInput(310);
        private readonly Label message = new Label();
        private readonly Button login;

        public LoginForm()
        {
            Text = "Wholesale Management | Admin Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(520, 510);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Ui.Canvas;
            Font = Ui.Normal;

            var top = new Panel { Dock = DockStyle.Top, Height = 122, BackColor = Ui.Charcoal };
            top.Controls.Add(new Label { Text = "WHOLESALE MANAGEMENT", Font = new Font("Segoe UI", 18f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(44, 34) });
            top.Controls.Add(new Label { Text = "Back-office administration", Font = new Font("Segoe UI", 10f), ForeColor = Ui.HeaderMuted, AutoSize = true, Location = new Point(47, 78) });
            Controls.Add(top);

            var card = new Panel { BackColor = Color.White, Bounds = new Rectangle(54, 158, 412, 302) };
            card.Controls.Add(new Label { Text = "Sign in as Admin", Location = new Point(30, 22), AutoSize = true, ForeColor = Ui.Charcoal, Font = new Font("Segoe UI", 15f, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = "Username", Location = new Point(32, 73), AutoSize = true, ForeColor = Ui.Ink });
            username.Location = new Point(32, 99);
            card.Controls.Add(username);
            card.Controls.Add(new Label { Text = "Password", Location = new Point(32, 139), AutoSize = true, ForeColor = Ui.Ink });
            password.Location = new Point(32, 165);
            password.UseSystemPasswordChar = true;
            card.Controls.Add(password);
            login = Ui.Button("SIGN IN", (s, e) => SignIn(), true, 310);
            login.Location = new Point(32, 209);
            card.Controls.Add(login);
            message.Location = new Point(32, 260);
            message.Size = new Size(355, 38);
            message.ForeColor = Color.FromArgb(146, 57, 44);
            message.Font = new Font("Segoe UI", 8.7f);
            card.Controls.Add(message);
            Controls.Add(card);
            AcceptButton = login;
        }

        private void SignIn()
        {
            login.Enabled = false;
            message.Text = "";
            try
            {
                var result = AuthService.SignIn(username.Text, password.Text);
                if (!result.Success)
                {
                    message.Text = result.Message;
                    password.Clear();
                    return;
                }
                password.Clear();
                Hide();
                using (var main = new MainForm(result.UserName)) main.ShowDialog(this);
                Show();
                username.Clear();
                password.Clear();
                username.Focus();
            }
            catch (Exception error)
            {
                message.Text = "Unable to sign in: " + error.Message;
            }
            finally
            {
                login.Enabled = true;
            }
        }
    }
}
