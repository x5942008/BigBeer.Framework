using BigBeer.Service.Host.App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigBeer.Service.Host
{
    partial class Program
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
        static ServiceLoader loader { get; set; }
        static void Main(string[] args)
        {
            ToScreenCenter();
            Console.Title = "大雄软件服务框架";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("************大雄软件服务框架***********");
            Console.WriteLine("*       email:549590615@qq.com        *");
            Console.WriteLine("*       phone:15980892074             *");
            Console.WriteLine("*       WebSite:www.wujixiong.com     *");
            Console.WriteLine("*          软件科技有限公司           *");
            Console.WriteLine("***************************************");
            Console.ForegroundColor = ConsoleColor.White;
            commands.First().Value.action(null);
            if (loader == null)
                loader = new ServiceLoader();
            StartCommand();
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public static Action<string, string> Logger = (n, m) =>
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrEmpty(n))
                Console.WriteLine($"{n}");
            if (!string.IsNullOrEmpty(m))
                Console.WriteLine($"=> {m}");
            Console.ForegroundColor = ConsoleColor.Green;
        };
        /// <summary>
        /// 开始运行服务
        /// </summary>
        /// <returns></returns>
        static Task RunService()
        {

            return Task.CompletedTask;
        }
    }
}
