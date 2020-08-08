using System;
using System.Text;

namespace MoneyNote.Core.Schedules
{
    public interface IScheduler : IDisposable
    {
        string _KeySchedule { get; }
        string ScheduleName { get; }

        public SchedulerState State { get; }

        void Start();

        public enum SchedulerState
        {
            New,
            Started,
        }

        public SchedulerOption Option { get; }
    }

}
