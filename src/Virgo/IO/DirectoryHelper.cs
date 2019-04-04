using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Virgo.IO
{
    public static class DirectoryHelper
    {
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
