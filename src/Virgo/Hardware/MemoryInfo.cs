using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Virgo.Hardware
{
    /// <summary>
    /// 定义内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
}
