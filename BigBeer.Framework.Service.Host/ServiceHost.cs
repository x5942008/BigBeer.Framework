using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceProcess;
using BigBeer.Freamework.Service.Interface;

namespace BigBeer.Framework.Service.Host
{
    public class ServiceHost
    {
        ServiceCollections serviceCollections { get; set; } = new ServiceCollections();
        Dictionary<string, (string display, Action<string[]> action)> Commands { get; set; }
        /// <summary>
        /// windows服务名称
        /// </summary>
        public string Name { get; set; } = "BigBeerServiceFramework";
        /// <summary>
        /// windows服务描述
        /// </summary>
        public string Display { get; set; } = "windows服务框架";
        protected bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //WindowsBuiltInRole 可以枚举出很多权限,例如系统用户、User、Guest等等
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        protected Action<string, string> Logger = (n, m) =>
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrEmpty(n))
                Console.WriteLine($"{n}");
            if (!string.IsNullOrEmpty(m))
                Console.WriteLine($"{m}");
            Console.ForegroundColor = ConsoleColor.Green;
        };
        #region 设置居中
        private struct RECT { public int left, top, right, bottom; }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
        static void ToScreenCenter()
        {
            IntPtr hWin = GetConsoleWindow();
            RECT rc;
            GetWindowRect(hWin, out rc);
            Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));
            int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
            int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
            MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, true);
        }
        #endregion
        public ServiceHost()
        {
            Commands = new Dictionary<string, (string display, Action<string[]> action)>(){
                {"install",("安装成windows服务,请以管理员身份运行",ps=>{
                    try
    {
                        Logger("开始安装windows服务","");
                        var path = $"{Process.GetCurrentProcess().MainModule.FileName}service";
                        Process.Start("sc",$"create{Name}binpath\"{path}\" displayName ={Display} start = auto");
                        Logger("服务安装成功","");
    }
    catch (Exception)
    {
                        Logger("服务安装失败","");
    }
                }) },
                {"uninstall",("卸载windows服务",ps=>{
                    try
    {
                        Logger("开始卸载windows服务","");
                        Process.Start("sc","delete BigBeerServiceFramework");
                        Logger("服务卸载成功","");
    }
    catch (Exception)
    {
                        Logger("服务卸载失败","");
    }
                }) },

                {"start",("不安装服务直接启动所有服务",ps=>{
                    Logger("准备启动服务","");
                    try
    {
                    var result = serviceCollections.Start();
                    foreach(var m in result){
                            Logger($"{m }","");
                        }
                    Logger("服务启动完成","");
    }
    catch (Exception ex)
    {
                        Logger($"服务启动错误：{ex.Message}","");
    }
                }) },

                {"stop",("停止所有服务",ps=>{
                    Logger("准备停止服务","");
                    try
    {
                        var result = serviceCollections.Stop();
                        foreach (var m in result)
                        {
                            Logger($"{m}","");
                        }
                        Logger("准备停止服务完成","");
    }
    catch (Exception ex)
    {
                        Logger($"服务停止错误:{ex.Message}","");
    }
                }) },

               {"help",("显示所有命令",ps=>{
                   foreach (var m in Commands)
                    {
                       Logger($"{m.Key.PadRight(8)}->{m.Value.display}","");
                    }
                }) }
            };
        }
        public void Run(string[] args)
        {
            if (args.Any() && args[0] == "service")
                ServiceBase.Run(new[] { new StartService(serviceCollections) });
            else
            {
                ToScreenCenter();
                Console.Title = "布德软件Windows Service";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("****************版权说明******************");
                Console.WriteLine("*       email:develop@buydee.cn          *");
                Console.WriteLine("*       phone:15980892074                *");
                Console.WriteLine("*       developer:sunlutao@buydee.cn     *");
                Console.WriteLine("*       布德软件科技有限公司             *");
                Console.WriteLine("******************************************");
                Console.WriteLine("提示:所有服务以文件夹的形式存放在根目录 services 中");
                if (!IsAdministrator())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("->");
                    Console.WriteLine("警告:");
                    Console.WriteLine("请使用管理员权限运行此程序,否则无法安装服务成功");
                    Console.WriteLine("->");
                    Console.ForegroundColor = ConsoleColor.Green;

                }
                Console.WriteLine("输入:help 获取所有命令");
                Console.ForegroundColor = ConsoleColor.Gray;
                RunCommand();
            }
        }
        public virtual ServiceHost UseServiceCollection(Action<ServiceCollections> servicesAction)
        {
            servicesAction.Invoke(serviceCollections);
            return this;
        }
        public virtual ServiceHost UseCommand() {
            return this;
        }
        void RunCommand()
        {
            Console.WriteLine("请输入命令:");
            Console.ForegroundColor = ConsoleColor.Green;
            var commandLine = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(commandLine))
                RunCommand();
            var commands = commandLine.Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).ToArray();
            if (!commands.Any()) RunCommand();
            var cmd = commands[0];
            if (!Commands.ContainsKey(cmd))
            {
                Logger("","命令不存在");
                RunCommand();
            }
            try
            {
                //if(cmd!="help") Logger($"开始执行:[{cmd}] 命令", "");
                Commands[cmd].action(commands.Skip(1).ToArray());
                // if (cmd != "help") Logger($"[{cmd}] 执行完成", "");
            }
            catch (Exception ex)
            {
                Logger(commandLine, "");
                Logger("", ex.Message);
                Logger("", ex.Source);
            }
            RunCommand();
        }
    }

    public class TestService : BaseService
    {
        public override string Display => "测试的服务";

        public override string Name => "测试的服务";

        public override void Start()
        {
            Console.WriteLine("TestService Start");
        }

        public override void Stop()
        {
            Console.WriteLine("TestService Stop");
        }
    }
}
