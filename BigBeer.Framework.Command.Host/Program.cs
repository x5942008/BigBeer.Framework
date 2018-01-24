using BigBeer.Framework.Command.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;


namespace BigBeer.Framework.Command.Host
{
    class Program
    {
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
        public static void Main(string[] args)
        {
            ToScreenCenter();
            Console.Title = "大雄软件服务命令工具";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*************大雄软件命令工具*************");
            Console.WriteLine("*       email:549590615@qq.com           *");
            Console.WriteLine("*         phone:15980892074              *");
            Console.WriteLine("*         www.wujixiong.com              *");
            Console.WriteLine("*            大雄服务框架                *");
            Console.WriteLine("******************************************");
            Console.WriteLine("输入:help 获取所有命令");
            //创建系统命令
            CreateSysCommands();
            //测试的命令
            Gateway.Register("reg", (ps) => {
                var result = new List<string>();
                for (var i = 0; i < ps.Length; i++)
                {
                    result.Add($"第 [{i}] 个参数为 : {ps[i]}");
                }
                return result.ToArray();
            }, "案例 :注册用户,接收参数.格式 reg username password [phone] [email]");

            RunCommand();
        }

        protected static void RunCommand()
        {
            Console.WriteLine("请输入命令:");
            Console.ForegroundColor = ConsoleColor.Green;
            var commandLine = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(commandLine))
                RunCommand();
            var commands = commandLine.Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).ToArray();
            if (!commands.Any()) RunCommand();
            if (!Gateway.HasCommandKey(commands[0]))
            {
                Logger("", "命令不存在");
                RunCommand();
            }
            try
            {
                var result = Gateway.Excute(commands[0], commands.Skip(1).ToArray());
                foreach (var r in result)
                {
                    Logger("", r);
                }
            }
            catch (Exception ex)
            {
                Logger(commandLine, "");
                Logger("", ex.Message);
                Logger("", ex.Source);
            }
            RunCommand();
        }
        /// <summary>
        /// 创建系统命令
        /// </summary>
        protected static void CreateSysCommands()
        {
            Gateway.Register("help", (ps) =>
            {
                foreach (var m in Gateway.Displays)
                {
                    Logger($"  {m.Key.PadRight(8)} -> {m.Value}", "");
                }
                return new string[] { };
            }, "显示所有命令");
            Gateway.Register("clear", (ps) =>
            {
                Console.Clear();
                return new string[] { };
            }, "清除当前屏幕");
            Gateway.Register("sys", (ps) =>
            {
                var cm = new Microsoft.VisualBasic.Devices.ComputerInfo();
                Logger("", $"MachineName:{Environment.MachineName}");
                Logger("", $"OSVersion:{Environment.OSVersion}");
                Logger("", $"ProcessorCount:{Environment.ProcessorCount}");
                Logger("", $"SystemDirectory:{Environment.SystemDirectory}");
                Logger("", $"SystemPageSize:{Environment.SystemPageSize}");
                Logger("", $"Is64BitProcess:{Environment.Is64BitProcess}");
                Logger("", $"CurrentDirectory:{Environment.CurrentDirectory}");
                Logger("", $"OSVersion:{cm.OSVersion}");
                Logger("", $"OSPlatform:{cm.OSPlatform}");
                Logger("", $".NET Version:{Environment.Version}");
                Logger("", $"Memery:{Environment.WorkingSet / 1024.00 / 1024.00} M");
                Logger("", $"PhysicalMemery:{cm.TotalPhysicalMemory / 1024.00 / 1024.00 / 1024} G");
                Logger("", $"VirtualMemery:{cm.TotalVirtualMemory / 1024.00 / 1024.00 / 1024} G");
                return new string[] { };
            }, "显示当前系统信息");
            Gateway.Register("load", (ps) =>
            {
                return Gateway.LoadAssblyCommands().ToArray();
            }, "加载 commands 文件夹下所有的命令");
            Gateway.Register("reload", (ps) =>
            {
                Gateway.Clear();
                CreateSysCommands();
                return new string[] { "重新加载成功" };
            }, "清除所有命令并重新加载(不自动加载文件夹命令)");
        }
        public static Action<string, string> Logger = (n, m) => {
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrEmpty(n))
                Console.WriteLine($"{n}");
            if (!string.IsNullOrEmpty(m))
                Console.WriteLine($"=> {m}");
            Console.ForegroundColor = ConsoleColor.Green;
        };
    }
}
