using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Common;
using Xunit;

namespace Virgo.Tests.Common
{
   public class QRCoderUtils_Tests
    {
        [Fact]
        public void QRCode_Test()
        {
           var bitmap= QRCoderUtils.QRCode("http://virgo.vanfj.com");
            bitmap.Save("C:/dddd.bmp");
        }
    }
}
