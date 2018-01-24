using BigBeer.Service.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Host.App
{
    public class ServiceLoader
    {
        /// <summary>
        /// 默认文件夹
        /// </summary>
        public string ServiceDirectory { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\Services";
        private string AsseblyName { get; } = "ServiceLoader";
        /// <summary>
        /// 准备上下文对象
        /// </summary>
        private IContext Context { get; } = new DefaultContext()
        {
            AppDomanName = AppDomain.CurrentDomain.FriendlyName,
            ComputerName = Environment.MachineName,
            Context = new Dictionary<string, string>() {
                { "AsseblyName","ServiceLoader"},
                { "IpAddress", Dns.GetHostAddresses("localhost").Select(t=>t.MapToIPv4().ToString()).ToJson()},
                { ".Net Version",Environment.Version.ToString()},
                { "TotalPhysicalMemory",(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory/1024/1024).ToString()}
            }
        };
        /// <summary>
        /// 所有服务
        /// </summary>
        public List<Service> ServiceBag { get; set; } = new List<Service>();
        public ServiceLoader(string servicePath = null)
        {
            if (servicePath != null)
                ServiceDirectory = servicePath;
            if (!Directory.Exists(ServiceDirectory))
                Directory.CreateDirectory(ServiceDirectory);

        }
        /// <summary>
        /// 加载所有服务
        /// </summary>
        /// <returns></returns>
        public async Task LoadServices()
        {
            Program.Logger("", "加载服务中...");
            var i = 0;
            var Directorys = Directory.GetDirectories(ServiceDirectory);
            foreach (var directory in Directorys)
            {
                var configPath = Path.Combine(directory, "service.json");
                if (!File.Exists(configPath))
                    continue;
                Service service = null;
                try
                {
                    using (var reader = new StreamReader(configPath))
                    {
                        var config = await reader.ReadToEndAsync();
                        service = config.ToObject<Service>();
                        if (ServiceBag.Any(t => t.AppDomanName == service.AppDomanName)) continue;
                        try
                        {
                            await CreateService(service, appSetup(directory));
                            service.Status = AppStatus.Preparing;
                            ServiceBag.Add(service);
                        }
                        catch (Exception ex)
                        {
                            Program.Logger("", ex.Message);
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Logger("", ex.Message);
                    continue;
                }
                await Task.Delay(500);
                i++;
            }
            Program.Logger("", $"总计:{i} 个服务");
            Program.Logger("", "服务加载完成");
        }
        AppDomainSetup appSetup(string baseDirectory)
        {
            var configs = Directory.GetFiles(baseDirectory, "*.config");
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (configs.Length > 0)
                configFile = configs[0];
            return new AppDomainSetup()
            {
                ApplicationBase = baseDirectory,
                DisallowBindingRedirects = false,
                DisallowCodeDownload = true,
                ConfigurationFile = configFile
            };
        }
        /// <summary>
        /// 创建一个服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public async Task CreateService(Service service, AppDomainSetup setup)
        {
            //准备权限
            PermissionSet ps = new PermissionSet(PermissionState.Unrestricted);
            SecurityPermission secps = new SecurityPermission(SecurityPermissionFlag.AllFlags);
            ps.AddPermission(secps);
            //创建域
            var appdomain = AppDomain.CreateDomain(service.AppDomanName, AppDomain.CurrentDomain.Evidence, setup, ps);//, null, setup);


            var obj = appdomain.CreateInstanceAndUnwrap(service.Assembly, service.TypeName);
            service.App = (IApp)obj;
            service.AppDomain = appdomain;
            await Task.CompletedTask;
        }



        /// <summary>
        /// 获取所有状态
        /// </summary>
        public void GetStatus(string appdomain = null)
        {
            if (!string.IsNullOrEmpty(appdomain))
            {
                if (ServiceBag.Any(t => t.AppDomanName == appdomain))
                    try
                    {
                        var service = ServiceBag.First(t => t.AppDomanName == appdomain);
                        var status = service.App.RunStatus;
                        showRunStatus(status, service.Name);
                    }
                    catch (Exception ex)
                    {
                        Program.Logger("", ex.Message);
                    }
                return;
            }
            foreach (var service in ServiceBag)
            {
                try
                {
                    var status = service.App.RunStatus;
                    showRunStatus(status, service.Name);

                }
                catch (Exception ex)
                {
                    Program.Logger($"ServiceLoader:GetStatus():{service.AppDomanName}", ex.Message);
                    continue;
                }
            }
        }
        private void showRunStatus(IRunStatus status, string domain)
        {
            Program.Logger($"------------{domain}------------", "");
            Program.Logger("", status.Status.ToString());
            Program.Logger("", $"StartTime:{ status.StartTime.ToString()}");
            Program.Logger("Error", "");
            foreach (var e in status.Error)
            {
                Program.Logger("", $"{e.Key}:{e.Value}");
            }
            Program.Logger("Message", "");
            foreach (var e in status.Message)
            {
                Program.Logger("", $"{e.Key}:{e.Value}");
            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="appdomain"></param>
        public void Stop(string appdomain = null)
        {
            Service service = null;
            if (appdomain != null)
            {
                Program.Logger("", $"停止:{appdomain}");
                if (ServiceBag.Any(t => t.AppDomanName == appdomain))
                    try
                    {
                        service = ServiceBag.First(t => t.AppDomanName == appdomain);
                        service.App.Stop();
                        service.Status = AppStatus.Stop;
                        AppDomain.Unload(service.AppDomain);
                        ServiceBag.Remove(service);
                        //强制垃圾回收
                        service = null;
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        Program.Logger($"", ex.Message);
                    }
                    finally
                    {
                        Program.Logger($"", "已停止");
                    }

                return;
            }
            if (!ServiceBag.Any())
            {
                Program.Logger("", "没有运行的服务");
                return;
            }
            var serviceNumber = ServiceBag.Count();
            for (var i = 0; i < serviceNumber; i++)
            {
                service = ServiceBag[0];
                try
                {
                    Program.Logger("", $"停止:{service.AppDomanName}");
                    service.App.Stop();
                    service.Status = AppStatus.Stop;
                    AppDomain.Unload(service.AppDomain);
                    ServiceBag.Remove(service);
                    //强制垃圾回收
                    service = null;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    Program.Logger($"", ex.Message);
                    continue;
                }

            }
            Program.Logger($"", "全部停止完成");
        }
        /// <summary>
        /// 运行程序
        /// </summary>
        /// <param name="appdomain"></param>
        /// <returns></returns>
        public async Task Run(string appdomain = null)
        {
            try
            {
                await LoadServices();
            }
            catch (Exception ex)
            {
                Program.Logger(AsseblyName, ex.Message);
            }
            if (appdomain != null)
            {
                Program.Logger("", $"启动:{appdomain}");
                if (ServiceBag.Any(t => t.AppDomanName == appdomain))
                    try
                    {
                        var service = ServiceBag.First(t => t.AppDomanName == appdomain);
                        if (service.Status == AppStatus.Running)
                        {
                            Program.Logger("", "服务已经运行,请勿重复运行");
                            return;
                        }
                        service.App.Run(Context);
                    }
                    catch (Exception ex)
                    {
                        Program.Logger("", ex.Message);
                    }
                await Task.CompletedTask;
                return;
            }
            foreach (var service in ServiceBag)
            {
                Program.Logger("", $"启动:{service.AppDomanName}");
                if (service.Status == AppStatus.Running)
                {
                    Program.Logger("", "服务已经运行,请勿重复运行");
                    continue;
                }
                try
                {
                    service.App.Run(Context);
                    service.Status = service.App.RunStatus.Status;
                    Program.Logger("", $"{service.AppDomanName}:{service.Status.ToString()}");
                }
                catch (Exception ex)
                {
                    Program.Logger("", ex.Message);
                    continue;
                }
            }

        }

        public void Rerun(string appdomain = null)
        {
            if (appdomain != null)
            {
                Program.Logger("", $"重启:{appdomain}");
                if (ServiceBag.Any(t => t.AppDomanName == appdomain))
                    try
                    {
                        var service = ServiceBag.First(t => t.AppDomanName == appdomain);
                        service.App.ReStart();
                    }
                    catch (Exception ex)
                    {
                        Program.Logger("", ex.Message);
                    }
                return;
            }
            foreach (var service in ServiceBag)
            {
                Program.Logger("", $"重启:{service.AppDomanName}");

                try
                {
                    service.App.ReStart();
                    service.Status = (AppStatus)service.AppDomain.GetData("status");
                    Program.Logger("", $"{service.AppDomanName}:{service.Status.ToString()}");
                }
                catch (Exception ex)
                {
                    Program.Logger("", ex.Message);
                    continue;
                }
            }
        }
    }
}
