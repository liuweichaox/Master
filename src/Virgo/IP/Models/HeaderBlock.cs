using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.IP.Models
{
    internal class HeaderBlock
    {
        public long IndexStartIp
        {
            get;
            private set;
        }

        public int IndexPtr
        {
            get;
            private set;
        }

        public HeaderBlock(long indexStartIp, int indexPtr)
        {
            IndexStartIp = indexStartIp;
            IndexPtr = indexPtr;
        }

        /// <summary>
        /// Get the bytes for total storage
        /// </summary>
        /// <returns>
        /// Bytes gotten.
        /// </returns>
        public byte[] GetBytes()
        {
            /*
             * +------------+-----------+
             * | 4bytes     | 4bytes    |
             * +------------+-----------+
             *  start ip      index ptr
            */
            byte[] b = new byte[8];
            IpTool.WriteIntLong(b, 0, IndexStartIp);
            IpTool.WriteIntLong(b, 4, IndexPtr);
            return b;
        }
    }
}
