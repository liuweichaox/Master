using System.IO;

namespace Virgo.IO
{
    /// <summary>
    /// 文件辅助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 删除文件如果存在
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
