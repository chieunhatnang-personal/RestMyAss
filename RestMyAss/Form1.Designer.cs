namespace RestMyAss
{
    partial class frm_Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grb_ReminderList = new System.Windows.Forms.GroupBox();
            this.dgvReminders = new System.Windows.Forms.DataGridView();
            this.grb_AddReminder = new System.Windows.Forms.GroupBox();
            this.dtpScheduledTime = new System.Windows.Forms.DateTimePicker();
            this.chkScheduled = new System.Windows.Forms.CheckBox();
            this.chkMathChallenge = new System.Windows.Forms.CheckBox();
            this.btnCancelEdit = new System.Windows.Forms.Button();
            this.btnAddReminder = new System.Windows.Forms.Button();
            this.nudMinutes = new System.Windows.Forms.NumericUpDown();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chk_StartWithWindows = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.grb_ReminderList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).BeginInit();
            this.grb_AddReminder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // grb_ReminderList
            // 
            this.grb_ReminderList.Controls.Add(this.dgvReminders);
            this.grb_ReminderList.Location = new System.Drawing.Point(12, 12);
            this.grb_ReminderList.Name = "grb_ReminderList";
            this.grb_ReminderList.Size = new System.Drawing.Size(874, 297);
            this.grb_ReminderList.TabIndex = 0;
            this.grb_ReminderList.TabStop = false;
            this.grb_ReminderList.Text = "Reminder list";
            // 
            // dgvReminders
            // 
            this.dgvReminders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReminders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReminders.Location = new System.Drawing.Point(3, 18);
            this.dgvReminders.Name = "dgvReminders";
            this.dgvReminders.RowHeadersWidth = 51;
            this.dgvReminders.RowTemplate.Height = 24;
            this.dgvReminders.Size = new System.Drawing.Size(868, 276);
            this.dgvReminders.TabIndex = 0;
            this.dgvReminders.SelectionChanged += new System.EventHandler(this.dgvReminders_SelectionChanged);
            // 
            // grb_AddReminder
            // 
            this.grb_AddReminder.Controls.Add(this.dtpScheduledTime);
            this.grb_AddReminder.Controls.Add(this.chkScheduled);
            this.grb_AddReminder.Controls.Add(this.chkMathChallenge);
            this.grb_AddReminder.Controls.Add(this.btnCancelEdit);
            this.grb_AddReminder.Controls.Add(this.btnAddReminder);
            this.grb_AddReminder.Controls.Add(this.nudMinutes);
            this.grb_AddReminder.Controls.Add(this.lblMinutes);
            this.grb_AddReminder.Controls.Add(this.txtMessage);
            this.grb_AddReminder.Controls.Add(this.lblMessage);
            this.grb_AddReminder.Controls.Add(this.txtTitle);
            this.grb_AddReminder.Controls.Add(this.lblTitle);
            this.grb_AddReminder.Location = new System.Drawing.Point(12, 315);
            this.grb_AddReminder.Name = "grb_AddReminder";
            this.grb_AddReminder.Size = new System.Drawing.Size(874, 190);
            this.grb_AddReminder.TabIndex = 1;
            this.grb_AddReminder.TabStop = false;
            this.grb_AddReminder.Text = "Task editor";
            // 
            // dtpScheduledTime
            // 
            this.dtpScheduledTime.CustomFormat = "hh:mm tt";
            this.dtpScheduledTime.Enabled = false;
            this.dtpScheduledTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpScheduledTime.Location = new System.Drawing.Point(308, 126);
            this.dtpScheduledTime.Name = "dtpScheduledTime";
            this.dtpScheduledTime.ShowUpDown = true;
            this.dtpScheduledTime.Size = new System.Drawing.Size(121, 22);
            this.dtpScheduledTime.TabIndex = 11;
            // 
            // chkScheduled
            // 
            this.chkScheduled.AutoSize = true;
            this.chkScheduled.Location = new System.Drawing.Point(204, 127);
            this.chkScheduled.Name = "chkScheduled";
            this.chkScheduled.Size = new System.Drawing.Size(98, 21);
            this.chkScheduled.TabIndex = 10;
            this.chkScheduled.Text = "Scheduled";
            this.chkScheduled.UseVisualStyleBackColor = true;
            this.chkScheduled.CheckedChanged += new System.EventHandler(this.chkScheduled_CheckedChanged);
            // 
            // chkMathChallenge
            // 
            this.chkMathChallenge.AutoSize = true;
            this.chkMathChallenge.Location = new System.Drawing.Point(16, 127);
            this.chkMathChallenge.Name = "chkMathChallenge";
            this.chkMathChallenge.Size = new System.Drawing.Size(129, 21);
            this.chkMathChallenge.TabIndex = 9;
            this.chkMathChallenge.Text = "Math challenge";
            this.chkMathChallenge.UseVisualStyleBackColor = true;
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Enabled = false;
            this.btnCancelEdit.Location = new System.Drawing.Point(676, 148);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(89, 31);
            this.btnCancelEdit.TabIndex = 7;
            this.btnCancelEdit.Text = "Cancel";
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // btnAddReminder
            // 
            this.btnAddReminder.Location = new System.Drawing.Point(775, 148);
            this.btnAddReminder.Name = "btnAddReminder";
            this.btnAddReminder.Size = new System.Drawing.Size(89, 31);
            this.btnAddReminder.TabIndex = 6;
            this.btnAddReminder.Text = "Add New";
            this.btnAddReminder.UseVisualStyleBackColor = true;
            this.btnAddReminder.Click += new System.EventHandler(this.btnAddReminder_Click);
            // 
            // nudMinutes
            // 
            this.nudMinutes.Location = new System.Drawing.Point(95, 92);
            this.nudMinutes.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.nudMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinutes.Name = "nudMinutes";
            this.nudMinutes.Size = new System.Drawing.Size(89, 22);
            this.nudMinutes.TabIndex = 5;
            this.nudMinutes.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(13, 94);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(57, 16);
            this.lblMinutes.TabIndex = 4;
            this.lblMinutes.Text = "Minutes:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(95, 58);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(769, 22);
            this.txtMessage.TabIndex = 3;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 61);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(68, 16);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(95, 24);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(769, 22);
            this.txtTitle.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(13, 27);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(36, 16);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            // 
            // chk_StartWithWindows
            // 
            this.chk_StartWithWindows.AutoSize = true;
            this.chk_StartWithWindows.Location = new System.Drawing.Point(15, 515);
            this.chk_StartWithWindows.Name = "chk_StartWithWindows";
            this.chk_StartWithWindows.Size = new System.Drawing.Size(145, 21);
            this.chk_StartWithWindows.TabIndex = 2;
            this.chk_StartWithWindows.Text = "Start with Windows";
            this.chk_StartWithWindows.UseVisualStyleBackColor = true;
            this.chk_StartWithWindows.CheckedChanged += new System.EventHandler(this.chk_StartWithWindows_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(600, 512);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 27);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(360, 512);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(114, 27);
            this.btnDeleteSelected.TabIndex = 3;
            this.btnDeleteSelected.Text = "Delete selected";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(797, 512);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(89, 27);
            this.btnQuit.TabIndex = 5;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(480, 512);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(54, 27);
            this.btnMoveUp.TabIndex = 6;
            this.btnMoveUp.Text = "Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(540, 512);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(54, 27);
            this.btnMoveDown.TabIndex = 7;
            this.btnMoveDown.Text = "Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // frm_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 552);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnDeleteSelected);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chk_StartWithWindows);
            this.Controls.Add(this.grb_AddReminder);
            this.Controls.Add(this.grb_ReminderList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frm_Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestMyAss Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Setting_FormClosing);
            this.Load += new System.EventHandler(this.frm_Setting_Load);
            this.grb_ReminderList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).EndInit();
            this.grb_AddReminder.ResumeLayout(false);
            this.grb_AddReminder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grb_ReminderList;
        private System.Windows.Forms.DataGridView dgvReminders;
        private System.Windows.Forms.GroupBox grb_AddReminder;
        private System.Windows.Forms.Button btnAddReminder;
        private System.Windows.Forms.NumericUpDown nudMinutes;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chk_StartWithWindows;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnCancelEdit;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.DateTimePicker dtpScheduledTime;
        private System.Windows.Forms.CheckBox chkScheduled;
        private System.Windows.Forms.CheckBox chkMathChallenge;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
    }
}
