using Virgo.Media;
using Xunit;

namespace Virgo.Tests.Common
{
    public class QRCoderHelper_Tests
    {
        [Fact]
        public void QRCode_Test()
        {
           var bitmap= QRCoderHelper.QRCode("virgo.vanfj.com");
            bitmap.Save("D:/QRCode.bmp");
        }
    }
}
