using System;

namespace MoneyNote.Core.Schedules
{
    public class SchedulerOption
    {
        public string Id { get; set; }

        public enum ScheduleInstanceType
        {
            SingleProcess,
            MultipleProcess,
        }
        public enum ScheduleRunType
        {
            Interval,
            Yearly,
            Monthly,
            Weekly,
            Daily,
            Hourly,
            Minutely,
            Onlyone,
            OnlyoneAtYearMonthDayMinute
        }

        public ScheduleRunType RunType { get; set; } = ScheduleRunType.Interval;

        public ScheduleInstanceType InstanceType { get; set; } = ScheduleInstanceType.MultipleProcess;

        public int IntervalMiliseconds { get; set; } = 10000;

        public int YearOfLife { get; set; } = 1;
        public int MonthOfYear { get; set; } = 1;
        public int DayOfMonth { get; set; } = 1;
        public int HourOfDay { get; set; } = 0;
        public int MinuteOfHour { get; set; } = 0;
        public int SecondOfHour { get; set; } = 0;

        public DayOfWeek Dayofweek { get; set; } = DayOfWeek.Sunday;

        public bool RunFirstTime { get; set; } = false;

    }

}
