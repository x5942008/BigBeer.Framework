using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Test.Service
{
    public abstract class JobBase : IJob
    {
        string Path => $"{AppDomain.CurrentDomain.BaseDirectory}\\log";

        public virtual Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <param name="appendTime"></param>
        public void Logger(string msg, string title = null, bool appendTime = true)
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                var filePath = $"{Path}\\{DateTime.Now.Date.ToString("yyMMdd")}.TimelyService.txt";
                var message = new StringBuilder();
                if (appendTime)
                    message.AppendLine($"-->{DateTime.Now.ToString()}");
                if (!string.IsNullOrEmpty(title))
                    message.AppendLine($"{title}:");
                message.AppendLine(msg);
                using (var stream = File.OpenWrite(filePath))
                {
                    var buffer = Encoding.UTF8.GetBytes(message.ToString());
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
    }

    #region 数据库存储过程调用
    //using System.Data.Entity
    //public class MyDB : DbContext
    //{
    //public CusumerDb()
    //{
    //    Database.CreateIfNotExists();
    //}
    //public CusumerDb(string nameOrConnectonString) : base(nameOrConnectonString)
    //{
    //    Database.CreateIfNotExists();
    //}
    ///// <summary>
    ///// 默认的数据库对象
    ///// </summary>
    //public static CusumerDb Create()
    //{
    //    return new CusumerDb();
    //}
    //调用使用
    //public void command()
    //{
    //    using (var db = MyDb.Create())
    //    {
    //        var result = db.Database.ExecuteSqlCommand("declare @num int;declare @status bit;exec Day_Bank_Property_Interest @num,@status");
    //    }
    //}
    //}
    #endregion
}
