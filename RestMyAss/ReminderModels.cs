using System;
using System.Collections.Generic;

namespace RestMyAss
{
    public class AppState
    {
        public bool StartWithWindows { get; set; }

        public List<ReminderTask> Tasks { get; set; }

        public AppState()
        {
            Tasks = new List<ReminderTask>();
        }
    }

    public class ReminderTask
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public int IntervalMinutes { get; set; }

        public DateTime NextTriggerUtc { get; set; }

        public bool IsMathChallenge { get; set; }

        public bool IsScheduled { get; set; }

        public int ScheduledHour { get; set; }

        public int ScheduledMinute { get; set; }
    }
}
