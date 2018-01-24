using BigBeer.Framework.Command.Frame.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.Command.Frame
{
    /// <summary>
    /// 入口
    /// </summary>
    public class Gateway
    {
        Gateway()
        {
        }
        private IDictionary<(string commandkey, string display), Func<string[], string[]>> commandParser { get; }
        = new Dictionary<(string commanlkey, string display), Func<string[], string[]>>();

        private static Gateway gateway { get; } = new Gateway();

        private bool hasCommandKey(string commandKey)
        {
            return gateway.commandParser.Keys.Any(t => t.commandkey == commandKey);
        }


        #region 基础方法
        /// <summary>
        /// 获取所有命令
        /// </summary>
        public static IDictionary<string, string> Displays
        {
            get
            {
                return gateway.commandParser.ToDictionary((k) => k.Key.commandkey, v => v.Key.display);
            }
        }
        /// <summary>
        /// 是否含有某个命令
        /// </summary>
        /// <param name="commandKey">命令</param>
        /// <returns></returns>
        public static bool HasCommandKey(string commandKey)
        {
            return gateway.hasCommandKey(commandKey);
        }
        /// <summary>
        /// 根据命令获取执行函数
        /// </summary>
        /// <param name="commandKey">命令</param>
        /// <returns></returns>
        public static Func<string[], string[]> Command(string commandKey)
        {
            if (gateway.commandParser.Keys.Any(t => t.commandkey == commandKey))
                return gateway.commandParser.FirstOrDefault(t => t.Key.commandkey == commandKey).Value;
            return (ps) => { return new string[] { "没有找到命令" }; };
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string[] Excute(string commandKey, string[] parameters = null)
        {
            if (!gateway.commandParser.Keys.Any(t => t.commandkey == commandKey))
                return new string[] { "没有找到执行命令" };
            try
            {
                return gateway.commandParser.FirstOrDefault(t => t.Key.commandkey == commandKey).Value(parameters);
            }
            catch (Exception ex)
            {
                return new string[] { "执行错误", ex.Message };
            }

        }
        /// <summary>
        /// 清除所有命令
        /// </summary>
        public static void Clear()
        {
            gateway.commandParser.Clear();
            Parallel.ForEach(gateway.commandAppDomain, (d) => {
                try
                {
                    AppDomain.Unload(d);
                }
                catch (Exception)
                {

                }
            });
            gateway.commandAppDomain.Clear();
        }
        #endregion

        #region 注册命令
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="commandKey">命令</param>
        /// <param name="command">执行函数</param>
        /// <returns></returns>
        public static Gateway Register(string commandKey, ICommand command)
        {
            if (!gateway.hasCommandKey(commandKey))
                gateway.commandParser.Add((commandKey, command.Display), (ps) => command.Execute(commandKey, ps));
            return gateway;
        }
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="commandKey">命令</param>
        /// <param name="func">执行函数</param>
        /// <param name="display">命令描述</param>
        /// <returns></returns>
        public static Gateway Register(string commandKey, Func<string[], string[]> func, string display)
        {
            if (!gateway.hasCommandKey(commandKey))
                gateway.commandParser.Add((commandKey, display), func);
            return gateway;
        }
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="commandKey">命令</param>
        /// <param name="func">执行函数</param>
        /// <returns></returns>
        public static Gateway Register(string commandKey, Func<string[], string[]> func)
        {
            return Register(commandKey, func, null);
        }
        #endregion

        #region 加载命令
        private List<AppDomain> commandAppDomain { get; } = new List<AppDomain>();
        private string commandDirectory { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Commands";
        private AppDomainSetup appSetup(string baseDirectory)
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
        /// 从文件夹加载命令
        /// </summary>
        /// <returns></returns>
        public static List<string> LoadAssblyCommands()
        {
            var result = new List<string>();
            if (!Directory.Exists(gateway.commandDirectory))
            {
                result.Add($"请先创建 Commands 文件夹,将所有命令以文件夹形式放入");
                return result;
            }
            var Directorys = Directory.GetDirectories(gateway.commandDirectory);
            Parallel.ForEach(Directorys, (d) => {
                var cmdFiles = Directory.GetFiles(d, "*.dll");
                Parallel.ForEach(cmdFiles, (f, s) => {
                    var cmdType = typeof(CommandBase);
                    var assembly = Assembly.LoadFile(f);
                    var types = assembly.GetTypes().Where(t => t.IsSubclassOf(cmdType));
                    if (!types.Any()) return;

                    Parallel.ForEach(types, (t) =>
                    {
                        try
                        {
                            var command = (ICommand)Activator.CreateInstance(t);
                            //准备权限
                            PermissionSet ps = new PermissionSet(PermissionState.Unrestricted);
                            SecurityPermission secps = new SecurityPermission(SecurityPermissionFlag.AllFlags);
                            ps.AddPermission(secps);
                            var setup = gateway.appSetup(d);
                            //创建域
                            var appdomain = AppDomain.CreateDomain(command.UniqueId, AppDomain.CurrentDomain.Evidence, setup, ps);
                            var domainCommand = (ICommand)appdomain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName);
                            Register(domainCommand.Command, domainCommand);

                            result.Add($"{domainCommand.Command} 加载成功");
                            gateway.commandAppDomain.Add(appdomain);
                            command = null;
                            GC.Collect();
                        }
                        catch (Exception ex)
                        {
                            result.Add($"error :{t.Name} {ex.Message}");
                        }
                    });

                });
            });
            return result;
        }
        #endregion
    }
    public static partial class Extensions
    {

    }
}
