using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Virgo.Hardware
{
    /// <summary>
    /// 定义系统时间的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemtimeInfo
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}
