using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref="Stream"/>的扩展方法
    /// </summary>
    public static class StreamExtensions
    {
        public static async Task<byte[]> GetAllBytesAsync(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<MemoryStream> GetStreamAsync(this byte[] buffer)
        {
            using var stream = new MemoryStream();
            await stream.WriteAsync(buffer, 0, buffer.Length);
            return stream;
        }
    }
}
