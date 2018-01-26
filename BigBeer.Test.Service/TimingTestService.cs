using BigBeer.Freamework.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;

namespace BigBeer.Test.Service
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class TimingTestService : BaseService
    {
        public override string Display => "大雄框架测试服务-定时服务";

        public override string Name => "BigBeerTestService";

        string path => $"{AppDomain.CurrentDomain.BaseDirectory}\\log";

        IScheduler scheduler;

        public override void Start()
        {
            Logger("正在启动服务", Display);
            Run();
            Logger("启动完成");
        }

        public override void Stop()
        {
            scheduler.Shutdown(true).GetAwaiter();
            scheduler = null;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <param name="appendTime"></param>
        void Logger(string msg, string title = null, bool appendTime = true)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filePath = $"{path}\\{DateTime.Now.Date.ToString("yyMMdd")}.{Name}.txt";
                var message = new StringBuilder();
                if (appendTime)
                    message.AppendLine($"-->{DateTime.Now.ToString()}");
                if (!string.IsNullOrEmpty(title))
                    message.AppendLine($"{title}:");
                message.AppendLine(msg);
                using (var stream = File.OpenWrite(filePath))
                {
                    var buffer = UTF8Encoding.UTF8.GetBytes(message.ToString());
                    stream.Position = stream.Length;
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch (Exception)
            {
            }

        }

        Task Run()
        {
            return Task.Run(async () =>
            {
                try
                {
                    scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                    ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("TestTrigger", "Test")
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(t =>
                   {
                       t.OnEveryDay();
                       t.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 30));
                       t.EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(1, 0));
                       t.WithIntervalInMinutes(1);
                       //t.WithIntervalInHours(24);
                    }).Build();
                    IJobDetail job = JobBuilder.Create(typeof(JobTest))
                    .WithIdentity("TestJob", "Test")
                    .Build();
                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                }
                catch (Exception ex)
                {
                    Logger($"{DateTime.Now.ToString()}ERROR:{ex}");
                }
            });
        }
    }
}
