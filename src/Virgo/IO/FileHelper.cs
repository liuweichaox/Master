using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Virgo.IO
{
    public static class FileHelper
    {
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
