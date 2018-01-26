using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace BigBeer.Test.Service
{
    public class JobTest : JobBase
    {
        public override Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    var  date = context.JobDetail.Key.Name;
                    Logger(new Random().Next(1,99999).ToString() + date);
                }
                catch (Exception ex)
                {
                    Logger($"{DateTime.Now}{ex.Message}");
                }
            });
        }
    }
}
