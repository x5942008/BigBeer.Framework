using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Host
{
    /// <summary>
    /// 命令
    /// </summary>
    partial class Program
    {
        /// <summary>
        /// 所有命令
        /// </summary>
        static IDictionary<string, (string discription, Action<string[]> action)> commands => new Dictionary<string, (string discription, Action<string[]> action)>() {
{ "help",("显示所有命令",(k)=>{
    foreach(var c in commands){
        Logger("",$"{c.Key}:{c.Value.discription}");
    }
 })},
{ "run",("启动服务,启动指定服务请输入[appdomain]",async (key)=>{
    string appdomain=null;
    if(key.Length>0)
        appdomain=key[0];
   await loader.Run(appdomain);
 })},

{ "stop",("停止服务,停止指定服务请输入[appdomain]",(key)=>{
    string appdomain=null;
     if(key.Length>0)
        appdomain=key[0];
     loader.Stop(appdomain);
 })},
{ "rerun",("重新启动,启动指定服务请输入[appdomain]",(key)=>{
     string appdomain=null;
     if(key.Length>0)
        appdomain=key[0];
    loader.Rerun(appdomain);
 })},

{ "status",("获取服务状态,指定服务请输入[appdomain]",(key)=>{
    string appdomain=null;
     if(key.Length>0)
        appdomain=key[0];
     loader.GetStatus(appdomain);
 })},
{ "clear",("清除屏幕",(k)=>{
     Console.Clear();
 })}
,
            { "all",("显示所有加载完成的服务",(key)=>{
     foreach(var app in loader.ServiceBag){
                    Logger($"------------{app.AppDomanName}--------------","");
                    Logger("",$"Name:{app.Name}");
                    Logger("",$"AppDomanName:{app.AppDomanName}");
                    Logger("",$"Assembly:{app.Assembly}");
                    Logger("",$"TypeName:{app.TypeName}");
                }
 })},
{ "exit",("退出程序",(k)=>{
     Environment.Exit(0);
 })}
                };
        /// <summary>
        /// 开始命令循环
        /// </summary>
        static void StartCommand()
        {
            var cmd = Console.ReadLine();
            if (string.IsNullOrEmpty(cmd)) StartCommand();
            var key = cmd.Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).ToArray();
            if (!key.Any()) StartCommand();
            (string discription, Action<string[]> action) conmmand;

            if (commands.TryGetValue(key[0], out conmmand))
                try
                {
                    conmmand.action(key.Skip(1).ToArray());
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Error:{ex.Message}");
                    Console.WriteLine($"->:{ex.Source}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            else
            {
                Logger("", "命令不存在!请输入 [help] 查看所有命令");
            }
            StartCommand();
        }
    }
}
