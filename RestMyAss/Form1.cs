using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace RestMyAss
{
    public partial class frm_Setting : Form
    {
        private const string StartupRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string StartupValueName = "RestMyAss";

        private readonly Timer _clockTimer;
        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenuStrip _trayMenu;
        private readonly ToolStripMenuItem _mnuNextTasks;
        private readonly ToolStripMenuItem _mnuSettings;
        private readonly ToolStripMenuItem _mnuQuit;

        private AppState _state;
        private string _stateFilePath;
        private bool _isExiting;
        private string _editingTaskId;
        private bool _isGridRefreshing;
        private bool _isLoadingEditor;

        public frm_Setting()
        {
            InitializeComponent();

            Icon appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon != null)
            {
                Icon = appIcon;
            }

            _clockTimer = new Timer { Interval = 1000 };
            _clockTimer.Tick += ClockTimer_Tick;

            _trayMenu = new ContextMenuStrip();
            _trayMenu.Opening += TrayMenu_Opening;

            _mnuNextTasks = new ToolStripMenuItem("Next tasks", null, mnuNextTasks_Click);
            _mnuSettings = new ToolStripMenuItem("Settings", null, mnuSettings_Click);
            _mnuQuit = new ToolStripMenuItem("Quit", null, mnuQuit_Click);

            _trayMenu.Items.Add(_mnuNextTasks);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add(_mnuSettings);
            _trayMenu.Items.Add(_mnuQuit);

            _trayIcon = new NotifyIcon
            {
                Text = "RestMyAss",
                Icon = appIcon ?? SystemIcons.Information,
                ContextMenuStrip = _trayMenu,
                Visible = true
            };
            _trayIcon.DoubleClick += TrayIcon_DoubleClick;

            ConfigureReminderGrid();
            WireEditorEvents();
            ExitEditMode();
        }

        private void frm_Setting_Load(object sender, EventArgs e)
        {
            _stateFilePath = BuildStateFilePath();
            _state = LoadState(_stateFilePath);
            NormalizeLoadedTasks();

            chk_StartWithWindows.Checked = _state.StartWithWindows;
            RefreshReminderGrid();
            SelectFirstRowIfExists();
            UpdateNextTaskMenuText();

            _clockTimer.Start();
        }

        private void frm_Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isExiting)
            {
                SaveAll(strictValidation: false, showValidationError: false);
                _trayIcon.Visible = false;
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                HideToTray();
            }
        }

        private void WireEditorEvents()
        {
            txtTitle.TextChanged += EditorFieldChanged;
            txtMessage.TextChanged += EditorFieldChanged;
            nudMinutes.ValueChanged += EditorFieldChanged;
            chkMathChallenge.CheckedChanged += EditorFieldChanged;
            dtpScheduledTime.ValueChanged += EditorFieldChanged;
        }

        private void EditorFieldChanged(object sender, EventArgs e)
        {
            if (_isLoadingEditor || _isGridRefreshing)
            {
                return;
            }

            PersistEditorToCurrentTask();
            UpdateSelectedRowFromEditor();
            UpdateNextTaskMenuText();
        }

        private void btnAddReminder_Click(object sender, EventArgs e)
        {
            PersistEditorToCurrentTask();

            ReminderTask newTask = new ReminderTask
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = string.Empty,
                Message = string.Empty,
                IntervalMinutes = 20,
                IsMathChallenge = false,
                IsScheduled = false,
                ScheduledHour = DateTime.Now.Hour,
                ScheduledMinute = DateTime.Now.Minute
            };
            newTask.NextTriggerUtc = GetInitialNextTriggerUtc(newTask, DateTime.UtcNow);

            _state.Tasks.Add(newTask);
            RefreshReminderGrid();
            SelectTaskById(newTask.Id);
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            ReminderTask selectedTask = GetSelectedTask();
            if (selectedTask == null)
            {
                return;
            }

            DialogResult confirm = MessageBox.Show(
                this,
                "Delete selected task?",
                "Confirm delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            _state.Tasks.RemoveAll(t => t.Id == selectedTask.Id);
            if (_editingTaskId == selectedTask.Id)
            {
                _editingTaskId = null;
            }

            RefreshReminderGrid();
            SelectFirstRowIfExists();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedTask(-1);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedTask(1);
        }

        private void MoveSelectedTask(int direction)
        {
            ReminderTask selectedTask = GetSelectedTask();
            if (selectedTask == null)
            {
                return;
            }

            int index = _state.Tasks.FindIndex(t => t.Id == selectedTask.Id);
            if (index < 0)
            {
                return;
            }

            int newIndex = index + direction;
            if (newIndex < 0 || newIndex >= _state.Tasks.Count)
            {
                return;
            }

            ReminderTask temp = _state.Tasks[index];
            _state.Tasks[index] = _state.Tasks[newIndex];
            _state.Tasks[newIndex] = temp;

            RefreshReminderGrid();
            SelectTaskById(selectedTask.Id);
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ExitEditMode();
            dgvReminders.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!SaveAll(strictValidation: true, showValidationError: true))
            {
                return;
            }

            MessageBox.Show(this, "Settings saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            mnuQuit_Click(sender, e);
        }

        private void chk_StartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_state == null)
            {
                return;
            }

            _state.StartWithWindows = chk_StartWithWindows.Checked;
            TryUpdateStartupRegistration(_state.StartWithWindows);
            SaveState(_stateFilePath, _state);
        }

        private void chkScheduled_CheckedChanged(object sender, EventArgs e)
        {
            dtpScheduledTime.Enabled = chkScheduled.Checked;
            if (_isLoadingEditor)
            {
                return;
            }

            PersistEditorToCurrentTask();
            UpdateSelectedRowFromEditor();
            UpdateNextTaskMenuText();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            HandleDueReminders();
            UpdateRemainingCellsOnly();
            UpdateNextTaskMenuText();
        }

        private void HandleDueReminders()
        {
            DateTime now = DateTime.UtcNow;
            var dueTasks = _state.Tasks.Where(t => IsTaskComplete(t) && t.NextTriggerUtc <= now).ToList();
            if (dueTasks.Count == 0)
            {
                return;
            }

            _clockTimer.Stop();
            foreach (ReminderTask dueTask in dueTasks)
            {
                if (dueTask.IsMathChallenge)
                {
                    MathChallengePopup.ShowChallenge(dueTask.Title, dueTask.Message);
                }
                else
                {
                    ReminderPopup.ShowReminder(dueTask.Title, dueTask.Message);
                }

                dueTask.NextTriggerUtc = DateTime.UtcNow.AddMinutes(Math.Max(1, dueTask.IntervalMinutes));
            }
            _clockTimer.Start();

            UpdateRemainingCellsOnly();
            SaveAll(strictValidation: false, showValidationError: false);
        }

        private void ConfigureReminderGrid()
        {
            dgvReminders.AutoGenerateColumns = false;
            dgvReminders.AllowUserToAddRows = false;
            dgvReminders.AllowUserToDeleteRows = false;
            dgvReminders.ReadOnly = true;
            dgvReminders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReminders.MultiSelect = false;
            dgvReminders.RowHeadersVisible = false;
            dgvReminders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvReminders.Columns.Clear();
            DataGridViewTextBoxColumn orderCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "#",
                ReadOnly = true,
                Width = 36,
                MinimumWidth = 36,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            orderCol.DefaultCellStyle.ForeColor = Color.Red;
            orderCol.DefaultCellStyle.Font = new Font(dgvReminders.Font, FontStyle.Bold);
            dgvReminders.Columns.Add(orderCol);

            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Title", ReadOnly = true, FillWeight = 20F, MinimumWidth = 120 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Message", ReadOnly = true, FillWeight = 33F, MinimumWidth = 180 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Every (min)", ReadOnly = true, FillWeight = 11F, MinimumWidth = 75 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Math", ReadOnly = true, FillWeight = 7F, MinimumWidth = 55 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Scheduled", ReadOnly = true, FillWeight = 10F, MinimumWidth = 75 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "At", ReadOnly = true, FillWeight = 8F, MinimumWidth = 65 });
            dgvReminders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Remaining", ReadOnly = true, FillWeight = 11F, MinimumWidth = 85 });
        }

        private static string Mark(bool value)
        {
            return value ? "\u2713" : "\u2717";
        }

        private static Color MarkColor(bool value)
        {
            return value ? Color.Green : Color.Red;
        }

        private static string BuildScheduleText(ReminderTask task)
        {
            return task.IsScheduled
                ? new DateTime(2000, 1, 1, task.ScheduledHour, task.ScheduledMinute, 0).ToString("hh:mm tt")
                : "-";
        }

        private static string BuildRemainingText(ReminderTask task)
        {
            TimeSpan remaining = task.NextTriggerUtc - DateTime.UtcNow;
            if (remaining < TimeSpan.Zero)
            {
                remaining = TimeSpan.Zero;
            }

            return remaining.ToString(@"hh\:mm\:ss");
        }

        private void FillRow(DataGridViewRow row, ReminderTask task, int order)
        {
            row.Cells[0].Value = order;
            row.Cells[1].Value = task.Title;
            row.Cells[2].Value = task.Message;
            row.Cells[3].Value = task.IntervalMinutes;
            row.Cells[4].Value = Mark(task.IsMathChallenge);
            row.Cells[5].Value = Mark(task.IsScheduled);
            row.Cells[6].Value = BuildScheduleText(task);
            row.Cells[7].Value = BuildRemainingText(task);

            row.Cells[4].Style.ForeColor = MarkColor(task.IsMathChallenge);
            row.Cells[5].Style.ForeColor = MarkColor(task.IsScheduled);
            row.Cells[4].Style.Font = new Font(dgvReminders.Font, FontStyle.Bold);
            row.Cells[5].Style.Font = new Font(dgvReminders.Font, FontStyle.Bold);
            row.Tag = task;
        }

        private void RefreshReminderGrid()
        {
            if (_state == null)
            {
                return;
            }

            _isGridRefreshing = true;
            dgvReminders.Rows.Clear();

            int orderNumber = 1;
            foreach (ReminderTask task in _state.Tasks)
            {
                int rowIndex = dgvReminders.Rows.Add();
                FillRow(dgvReminders.Rows[rowIndex], task, orderNumber);
                orderNumber++;
            }

            _isGridRefreshing = false;
        }

        private void UpdateRemainingCellsOnly()
        {
            if (_state == null || dgvReminders.Rows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow row in dgvReminders.Rows)
            {
                ReminderTask task = row.Tag as ReminderTask;
                if (task == null)
                {
                    continue;
                }

                row.Cells[7].Value = BuildRemainingText(task);
            }
        }

        private void UpdateSelectedRowFromEditor()
        {
            ReminderTask selectedTask = GetSelectedTask();
            if (selectedTask == null)
            {
                return;
            }

            DataGridViewRow row = dgvReminders.SelectedRows.Count > 0 ? dgvReminders.SelectedRows[0] : null;
            if (row == null)
            {
                return;
            }

            int order = row.Index + 1;
            FillRow(row, selectedTask, order);
        }

        private void dgvReminders_SelectionChanged(object sender, EventArgs e)
        {
            if (_isGridRefreshing || _state == null)
            {
                return;
            }

            PersistEditorToCurrentTask();

            ReminderTask task = GetSelectedTask();
            if (task == null)
            {
                ExitEditMode();
                return;
            }

            LoadTaskToEditor(task);
        }

        private ReminderTask GetSelectedTask()
        {
            if (dgvReminders.SelectedRows.Count == 0)
            {
                return null;
            }

            return dgvReminders.SelectedRows[0].Tag as ReminderTask;
        }

        private void LoadTaskToEditor(ReminderTask task)
        {
            _isLoadingEditor = true;
            _editingTaskId = task.Id;
            txtTitle.Text = task.Title;
            txtMessage.Text = task.Message;

            decimal taskMinutes = task.IntervalMinutes;
            if (taskMinutes < nudMinutes.Minimum)
            {
                taskMinutes = nudMinutes.Minimum;
            }
            if (taskMinutes > nudMinutes.Maximum)
            {
                taskMinutes = nudMinutes.Maximum;
            }
            nudMinutes.Value = taskMinutes;

            chkMathChallenge.Checked = task.IsMathChallenge;
            chkScheduled.Checked = task.IsScheduled;
            dtpScheduledTime.Value = new DateTime(2000, 1, 1, task.ScheduledHour, task.ScheduledMinute, 0);
            dtpScheduledTime.Enabled = task.IsScheduled;

            btnCancelEdit.Enabled = true;
            _isLoadingEditor = false;
        }

        private void PersistEditorToCurrentTask()
        {
            if (_isLoadingEditor || string.IsNullOrWhiteSpace(_editingTaskId))
            {
                return;
            }

            ReminderTask task = _state.Tasks.FirstOrDefault(t => t.Id == _editingTaskId);
            if (task == null)
            {
                return;
            }

            int oldInterval = task.IntervalMinutes;
            bool oldScheduled = task.IsScheduled;
            int oldHour = task.ScheduledHour;
            int oldMinute = task.ScheduledMinute;

            task.Title = txtTitle.Text.Trim();
            task.Message = txtMessage.Text.Trim();
            task.IntervalMinutes = (int)nudMinutes.Value;
            task.IsMathChallenge = chkMathChallenge.Checked;
            task.IsScheduled = chkScheduled.Checked;
            task.ScheduledHour = dtpScheduledTime.Value.Hour;
            task.ScheduledMinute = dtpScheduledTime.Value.Minute;

            bool scheduleChanged = oldInterval != task.IntervalMinutes ||
                                   oldScheduled != task.IsScheduled ||
                                   oldHour != task.ScheduledHour ||
                                   oldMinute != task.ScheduledMinute;

            if (scheduleChanged)
            {
                task.NextTriggerUtc = GetInitialNextTriggerUtc(task, DateTime.UtcNow);
            }
        }

        private void ExitEditMode()
        {
            _isLoadingEditor = true;
            _editingTaskId = null;
            txtTitle.Clear();
            txtMessage.Clear();
            nudMinutes.Value = 20;
            chkMathChallenge.Checked = false;
            chkScheduled.Checked = false;
            dtpScheduledTime.Value = DateTime.Now;
            dtpScheduledTime.Enabled = false;
            btnCancelEdit.Enabled = false;
            _isLoadingEditor = false;
        }

        private void NormalizeLoadedTasks()
        {
            if (_state.Tasks == null)
            {
                _state.Tasks = new List<ReminderTask>();
                return;
            }

            DateTime nowUtc = DateTime.UtcNow;
            foreach (ReminderTask task in _state.Tasks)
            {
                if (string.IsNullOrWhiteSpace(task.Id))
                {
                    task.Id = Guid.NewGuid().ToString("N");
                }

                if (task.IntervalMinutes <= 0)
                {
                    task.IntervalMinutes = 20;
                }

                if (task.ScheduledHour < 0 || task.ScheduledHour > 23)
                {
                    task.ScheduledHour = 8;
                }
                if (task.ScheduledMinute < 0 || task.ScheduledMinute > 59)
                {
                    task.ScheduledMinute = 0;
                }

                task.NextTriggerUtc = GetInitialNextTriggerUtc(task, nowUtc);
            }
        }

        private static DateTime GetInitialNextTriggerUtc(ReminderTask task, DateTime nowUtc)
        {
            int intervalMinutes = Math.Max(1, task.IntervalMinutes);
            if (!task.IsScheduled)
            {
                return nowUtc.AddMinutes(intervalMinutes);
            }

            DateTime localNow = nowUtc.ToLocalTime();
            DateTime localAnchor = new DateTime(localNow.Year, localNow.Month, localNow.Day, task.ScheduledHour, task.ScheduledMinute, 0, DateTimeKind.Local);
            DateTime anchorUtc = localAnchor.ToUniversalTime();

            if (nowUtc <= anchorUtc)
            {
                return anchorUtc;
            }

            double elapsedMinutes = (nowUtc - anchorUtc).TotalMinutes;
            long step = (long)Math.Floor(elapsedMinutes / intervalMinutes) + 1;
            return anchorUtc.AddMinutes(step * intervalMinutes);
        }

        private static bool IsTaskComplete(ReminderTask task)
        {
            return !string.IsNullOrWhiteSpace(task.Title) &&
                   !string.IsNullOrWhiteSpace(task.Message) &&
                   task.IntervalMinutes > 0;
        }

        private bool ValidateTasksForSave(out string error)
        {
            for (int i = 0; i < _state.Tasks.Count; i++)
            {
                ReminderTask task = _state.Tasks[i];
                if (!IsTaskComplete(task))
                {
                    error = string.Format("Task #{0} is incomplete. Please fill title and message, or delete it.", i + 1);
                    return false;
                }
            }

            error = null;
            return true;
        }

        private bool SaveAll(bool strictValidation, bool showValidationError)
        {
            if (_state == null)
            {
                return true;
            }

            PersistEditorToCurrentTask();

            if (strictValidation)
            {
                string validationError;
                if (!ValidateTasksForSave(out validationError))
                {
                    if (showValidationError)
                    {
                        MessageBox.Show(this, validationError, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return false;
                }
            }

            _state.StartWithWindows = chk_StartWithWindows.Checked;
            TryUpdateStartupRegistration(_state.StartWithWindows);
            SaveState(_stateFilePath, _state);
            return true;
        }

        private void SelectTaskById(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
            {
                return;
            }

            foreach (DataGridViewRow row in dgvReminders.Rows)
            {
                ReminderTask task = row.Tag as ReminderTask;
                if (task != null && task.Id == taskId)
                {
                    row.Selected = true;
                    dgvReminders.CurrentCell = row.Cells[0];
                    LoadTaskToEditor(task);
                    return;
                }
            }
        }

        private void SelectFirstRowIfExists()
        {
            if (dgvReminders.Rows.Count == 0)
            {
                ExitEditMode();
                return;
            }

            dgvReminders.Rows[0].Selected = true;
            dgvReminders.CurrentCell = dgvReminders.Rows[0].Cells[0];
            ReminderTask firstTask = dgvReminders.Rows[0].Tag as ReminderTask;
            if (firstTask != null)
            {
                LoadTaskToEditor(firstTask);
            }
        }

        private void HideToTray()
        {
            SaveAll(strictValidation: false, showValidationError: false);
            ShowInTaskbar = false;
            Hide();
            _trayIcon.BalloonTipTitle = "RestMyAss";
            _trayIcon.BalloonTipText = "Running in system tray. Right-click the tray icon for options.";
            _trayIcon.ShowBalloonTip(2000);
        }

        private void ShowSettingsWindow()
        {
            ShowInTaskbar = true;
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            BringToFront();
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowSettingsWindow();
        }

        private void TrayMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateNextTaskMenuText();
        }

        private void UpdateNextTaskMenuText()
        {
            var runnableTasks = _state.Tasks.Where(IsTaskComplete).ToList();
            if (runnableTasks.Count == 0)
            {
                _mnuNextTasks.Text = "Next tasks: none";
                return;
            }

            DateTime now = DateTime.UtcNow;
            ReminderTask nextTask = runnableTasks.OrderBy(t => t.NextTriggerUtc).First();
            TimeSpan remaining = nextTask.NextTriggerUtc - now;
            if (remaining < TimeSpan.Zero)
            {
                remaining = TimeSpan.Zero;
            }

            _mnuNextTasks.Text = string.Format("Next: {0} in {1}", nextTask.Title, remaining.ToString(@"hh\:mm\:ss"));
        }

        private void mnuNextTasks_Click(object sender, EventArgs e)
        {
            var runnableTasks = _state.Tasks.Where(IsTaskComplete).ToList();
            if (runnableTasks.Count == 0)
            {
                MessageBox.Show("No reminders scheduled.", "Next tasks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime now = DateTime.UtcNow;
            var ordered = runnableTasks.OrderBy(t => t.NextTriggerUtc).Take(10).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (ReminderTask task in ordered)
            {
                TimeSpan remaining = task.NextTriggerUtc - now;
                if (remaining < TimeSpan.Zero)
                {
                    remaining = TimeSpan.Zero;
                }

                sb.AppendLine(string.Format("- {0}: {1}", task.Title, remaining.ToString(@"hh\:mm\:ss")));
            }

            MessageBox.Show(sb.ToString(), "Next tasks", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            ShowSettingsWindow();
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            _isExiting = true;
            _clockTimer.Stop();
            _trayIcon.Visible = false;
            Application.Exit();
        }

        private static void TryUpdateStartupRegistration(bool enabled)
        {
            try
            {
                using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
                {
                    if (runKey == null)
                    {
                        return;
                    }

                    if (enabled)
                    {
                        runKey.SetValue(StartupValueName, "\"" + Application.ExecutablePath + "\"");
                    }
                    else
                    {
                        runKey.DeleteValue(StartupValueName, false);
                    }
                }
            }
            catch
            {
                // Ignore registry write errors and keep app running.
            }
        }

        private static string BuildStateFilePath()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataFolder, "RestMyAss");
            Directory.CreateDirectory(appFolder);
            return Path.Combine(appFolder, "state.xml");
        }

        private static AppState LoadState(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new AppState();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(AppState));
                using (FileStream stream = File.OpenRead(filePath))
                {
                    AppState state = serializer.Deserialize(stream) as AppState;
                    return state ?? new AppState();
                }
            }
            catch
            {
                return new AppState();
            }
        }

        private static void SaveState(string filePath, AppState state)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppState));
            using (FileStream stream = File.Create(filePath))
            {
                serializer.Serialize(stream, state);
            }
        }
    }
}



