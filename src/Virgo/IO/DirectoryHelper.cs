using System.IO;

namespace Virgo.IO
{
    /// <summary>
    /// <see cref="Directory"/>帮助类
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
