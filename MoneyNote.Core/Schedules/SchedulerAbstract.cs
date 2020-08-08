using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading;

namespace MoneyNote.Core.Schedules
{
    public abstract class SchedulerAbstract : IScheduler
    {

        bool _isDispose = false;

        string _id;

        public string ScheduleName { get; }

        public string _KeySchedule
        {
            get
            {
                return "MoneyNote:ScheduleNameSpace:" + ScheduleName;
            }
        }

        public IScheduler.SchedulerState State { get; private set; }

        public SchedulerOption Option { get { return _option; } }

        SchedulerOption _option;

        Thread _thread;

        bool _runingFirstTimeDone = true;
        protected SchedulerAbstract(SchedulerOption option)
        {           

            if (option == null)
            {
                option = new SchedulerOption();
            }
            _option = option;

            if (string.IsNullOrEmpty(_option.Id))
            {
                _option.Id = Guid.NewGuid().ToString();
            }

            ScheduleName = $"{this.GetType().FullName}:{_option.Id}";

            _thread = _thread ?? new Thread(() =>
            {
                LoopCheckScheduleOptionsToCallMainJob();
            });

            State = IScheduler.SchedulerState.New;

            if (option.RunFirstTime)
            {
                _runingFirstTimeDone = false;

                ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
                {
                    try
                    {

                        State = IScheduler.SchedulerState.Started;

                        JobToDo();

                    }
                    finally
                    {
                        _runingFirstTimeDone = true;
                    }
                }));
            }
            else
            {
                _runingFirstTimeDone = true;
            }
        }

        void Release()
        {

           

        }

        public void Dispose()
        {
            _isDispose = true;
            Release();
        }


        /// <summary>
        /// Phương thức xử lý cho mỗi job
        /// </summary>
        public abstract void JobToDo();

        public void Start()
        {
            if (string.IsNullOrEmpty(ScheduleName)) throw new Exception("ScheduleName do not allow null or empty");
            if (State != IScheduler.SchedulerState.Started)
            {
                _isDispose = false;
                _thread.Start();

                State = IScheduler.SchedulerState.Started;

            }
        }


        void LoopCheckScheduleOptionsToCallMainJob()
        {
            var currentNumOfProcess = string.Empty;
                       

            bool isRunning = false;

            while (!_isDispose)
            {
                try
                {
                    if (!_runingFirstTimeDone)
                    {
                        continue;
                    }

                    var dtNow = DateTime.Now;
                    switch (_option.RunType)
                    {
                        case SchedulerOption.ScheduleRunType.Yearly:
                            if (dtNow.Month != _option.MonthOfYear
                                || dtNow.Day != _option.DayOfMonth
                                || dtNow.Hour != _option.HourOfDay
                                || dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.Monthly:
                            if (dtNow.Day != _option.DayOfMonth
                                || dtNow.Hour != _option.HourOfDay
                                || dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.Weekly:
                            if (dtNow.DayOfWeek != _option.Dayofweek
                               || dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.Daily:
                            if (dtNow.Hour != _option.HourOfDay
                               || dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.Hourly:
                            if (dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.Minutely:
                            if (dtNow.Second != _option.SecondOfHour)
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                            break;
                        case SchedulerOption.ScheduleRunType.OnlyoneAtYearMonthDayMinute:
                            // idea is that: Run one at specific day then stop
                            if (dtNow.Month != _option.MonthOfYear
                                || dtNow.Day != _option.DayOfMonth
                                || dtNow.Hour != _option.HourOfDay
                                || dtNow.Minute != _option.MinuteOfHour)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }
                            break;
                    }

                    if (_option.RunType != SchedulerOption.ScheduleRunType.Interval)
                    {
                        if (isRunning)
                        {
                            for (var i = 1; i < 60; i++)
                            {
                                Thread.Sleep(1000);
                            }

                            isRunning = false;
                            continue;
                        }
                    }

                   
                    isRunning = true;

                    JobToDo();

                    if (_option.RunType == SchedulerOption.ScheduleRunType.Onlyone
                        || _option.RunType == SchedulerOption.ScheduleRunType.OnlyoneAtYearMonthDayMinute)
                    {
                        isRunning = false;
                        _isDispose = true;
                    }

                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    Console.WriteLine(msg);
                    LogError($"{_KeySchedule} {ex.Message}", ex);
                }
                finally
                {
                    if (_option.RunType == SchedulerOption.ScheduleRunType.Interval)
                    {
                        int seconds = _option.IntervalMiliseconds / 1000;

                        var remain = _option.IntervalMiliseconds;

                        for (var i = 1; i <= seconds; i++)
                        {
                            remain = remain - 1000;
                            Thread.Sleep(1000);
                        }
                        if (remain > 0)
                        {
                            Thread.Sleep(remain);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }

            }

            Release();
        }

        public void LogToConsole(long itemsCount)
        {
            Console.WriteLine($"{ScheduleName}: Processing records: {itemsCount}");
        }

        public void LogItemToConsole(object itemToProcess)
        {
            try
            {
                Console.WriteLine($"{ScheduleName} Processing item:\r\n");
                Console.WriteLine(JsonConvert.SerializeObject(itemToProcess));
            }
            catch { }
        }

        public void LogError(string msg, Exception ex)
        {
            Console.WriteLine($"{ScheduleName}: Error: {msg} {ex.StackTrace}");
        }


    }

}
