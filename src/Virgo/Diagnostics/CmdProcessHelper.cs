using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Diagnostics
{
    /// <summary>
    /// C#.Net后台执行CMD命令辅助类
    /// <para>RunCmdAsync方法可以在程序中一次执行多个CMD命令</para>
    /// <para>使用示例：</para>
    /// <para>分别在CMD执行 systeminfo、ipconfig/all 命令</para>
    /// <para>执行：var result= await CmdHelper.RunCmdAsync( new string[] { "systeminfo", "ipconfig/all" });</para>
    /// </summary>   
    public static class CmdProcessHelper
    {
        /// <summary>
        /// 执行CMD命令
        /// </summary>
        /// <param name="CmdText">CMD执行命令</param>
        /// <returns>命令执行结果</returns>
        public static async Task<List<string[]>> RunCmdAsync(string[] CmdText)
        {
            List<string[]> list = new List<string[]>();
            string cmd = string.Join("&", CmdText) + " &exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状
            using (var p = new Process())
            {
                p.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";//设置要启动的应用程序
                p.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动  
                p.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息  
                p.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息  
                p.StartInfo.RedirectStandardError = true; //重定向标准错误输出  
                p.StartInfo.CreateNoWindow = true; //不显示程序窗口  
                p.Start(); //启动程序                     
                p.StandardInput.WriteLine(cmd); //向cmd窗口写入命令                               
                p.StandardInput.AutoFlush = true;//true：刷新缓冲区；否则，为false
                string strOutput = await p.StandardOutput.ReadToEndAsync();//获取CMD窗口的输出信息          
                list = await ParsesCmdContentAsync(strOutput);//解析CMD窗口的输出信息
                p.WaitForExit(); //等待程序执行完退出进程  
                p.Close();//关闭程序
            }
            return list;
        }

        /// <summary>
        /// 解析CMD窗口的输出信息
        /// </summary>
        /// <param name="strOutput">CMD窗口的输出信息</param>
        /// <returns>CMD窗口输出信息集合</returns>
        private static async Task<List<string[]>> ParsesCmdContentAsync(string strOutput)
        {
            var result = await Task.Run(() =>
            {
                List<string[]> list = new List<string[]>();
                string str = strOutput;
                string[] lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] temp = line.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length == 2)
                        list.Add(new string[] { temp[0].Trim(), temp[1].Trim() });
                    else if (temp.Length == 1)
                        list.Add(new string[] { temp[0].Trim() });
                }
                return list;
            });
            return result;
        }
    }
}
