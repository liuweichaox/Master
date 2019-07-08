using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Hardware
{
    /// <summary>
    /// 内存条模型
    /// </summary>
    public class RamInfo
    {
        public double MemoryAvailable { get; set; }
        public double PhysicalMemory { get; set; }
        public double TotalPageFile { get; set; }
        public double AvailablePageFile { get; set; }
        public double TotalVirtual { get; set; }
        public double AvailableVirtual { get; set; }
        public double MemoryUsage => (1 - MemoryAvailable / PhysicalMemory) * 100;
    }
}
