using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.Service.Host
{
    public partial class StartService : ServiceBase
    {
        ServiceCollections Service { get; set; }
        public StartService(ServiceCollections service)
        {
            Service = service;
            InitializeComponent();
        }

        string path => $"{AppDomain.CurrentDomain.BaseDirectory}\\log";

        protected override void OnStart(string[] args)
        {
            try
            {
                var result = Service.Start();
                foreach (var m in result)
                {
                    logger(m);
                }
            }
            catch (Exception ex)
            {
                logger($"service error:{ex.Message}");
            }
        }

        protected override void OnStop()
        {
            try
            {
                var result = Service.Stop();
                foreach (var m in result)
                {
                    logger(m);
                }
            }
            catch (Exception ex)
            {
                logger($"service error:{ex.Message}");
            }
        }

        void logger(string msg)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filePath = $"{path}\\{DateTime.Now.Date.ToString("yyMMdd")}.txt";
            var message = new StringBuilder();
            message.AppendLine($"-->{DateTime.Now.ToString()}");
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
    }
}
