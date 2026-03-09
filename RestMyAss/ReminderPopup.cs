using System;
using System.Drawing;
using System.Windows.Forms;

namespace RestMyAss
{
    public class ReminderPopup : Form
    {
        private readonly Label _lblMessage;
        private readonly Button _btnOk;

        public ReminderPopup(string title, string message)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            TopMost = true;
            System.Drawing.Icon appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon != null)
            {
                Icon = appIcon;
            }

            Width = 620;
            Height = 260;

            _lblMessage = new Label();
            _lblMessage.Left = 18;
            _lblMessage.Top = 20;
            _lblMessage.Width = 566;
            _lblMessage.Height = 150;
            _lblMessage.Text = message;
            _lblMessage.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _lblMessage.AutoEllipsis = false;

            _btnOk = new Button();
            _btnOk.Text = "OK";
            _btnOk.Width = 100;
            _btnOk.Height = 34;
            _btnOk.Left = (Width - _btnOk.Width) / 2 - 10;
            _btnOk.Top = 175;
            _btnOk.DialogResult = DialogResult.OK;

            Controls.Add(_lblMessage);
            Controls.Add(_btnOk);

            AcceptButton = _btnOk;
        }

        public static void ShowReminder(string title, string message)
        {
            using (ReminderPopup dialog = new ReminderPopup(title, message))
            {
                dialog.Activate();
                dialog.BringToFront();
                dialog.ShowDialog();
            }
        }
    }
}

