using System;
using System.Drawing;
using System.Windows.Forms;

namespace RestMyAss
{
    public class MathChallengePopup : Form
    {
        private readonly Label _lblMessage;
        private readonly Label _lblQuestion;
        private readonly TextBox _txtAnswer;
        private readonly Button _btnSubmit;
        private readonly int _expectedAnswer;

        public MathChallengePopup(string title, string message)
        {
            Text = title + " - Math Challenge";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ControlBox = false;
            TopMost = true;
            Width = 560;
            Height = 270;

            Random random = new Random(Guid.NewGuid().GetHashCode());
            int a = random.Next(0, 101);
            int b = random.Next(0, 101);
            bool usePlus = random.Next(0, 2) == 0;
            _expectedAnswer = usePlus ? (a + b) : (a - b);
            string expression = usePlus ? (a + " + " + b) : (a + " - " + b);

            _lblMessage = new Label();
            _lblMessage.Left = 18;
            _lblMessage.Top = 18;
            _lblMessage.Width = 520;
            _lblMessage.Height = 52;
            _lblMessage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _lblMessage.Text = message;

            _lblQuestion = new Label();
            _lblQuestion.Left = 18;
            _lblQuestion.Top = 74;
            _lblQuestion.Width = 520;
            _lblQuestion.Height = 40;
            _lblQuestion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblQuestion.Text = "Solve to dismiss reminder: " + expression + " = ?";

            _txtAnswer = new TextBox();
            _txtAnswer.Left = 18;
            _txtAnswer.Top = 122;
            _txtAnswer.Width = 520;
            _txtAnswer.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _btnSubmit = new Button();
            _btnSubmit.Left = 213;
            _btnSubmit.Top = 168;
            _btnSubmit.Width = 130;
            _btnSubmit.Height = 34;
            _btnSubmit.Text = "Submit";
            _btnSubmit.Click += btnSubmit_Click;

            Controls.Add(_lblMessage);
            Controls.Add(_lblQuestion);
            Controls.Add(_txtAnswer);
            Controls.Add(_btnSubmit);
            AcceptButton = _btnSubmit;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int userAnswer;
            if (!int.TryParse(_txtAnswer.Text.Trim(), out userAnswer))
            {
                MessageBox.Show(this, "Please input a number.", "Math Challenge", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtAnswer.Focus();
                _txtAnswer.SelectAll();
                return;
            }

            if (userAnswer != _expectedAnswer)
            {
                MessageBox.Show(this, "Wrong answer. Please try again.", "Math Challenge", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtAnswer.Focus();
                _txtAnswer.SelectAll();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public static void ShowChallenge(string title, string message)
        {
            using (MathChallengePopup popup = new MathChallengePopup(title, message))
            {
                popup.ShowDialog();
            }
        }
    }
}
