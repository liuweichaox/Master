using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Common;
using Xunit;

namespace Virgo.Tests.Common
{
   public class QRCoderHelper_Tests
    {
        [Fact]
        public void QRCode_Test()
        {
           var bitmap= QRCoderHelper.QRCode("http://virgo.vanfj.com");
            bitmap.Save("C:/dddd.bmp");
        }
    }
}
