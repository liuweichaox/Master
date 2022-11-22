using Shouldly;
using Virgo.Text;
using Xunit;

namespace Virgo.Tests.Text
{
    public class StringHelper_Tests
    {
        [Fact]
        public void Simple_ConvertToTraditional_Test()
        {
            var text = StringHelper.ConvertToTraditional("刘伟超");
            text.ShouldBe("劉偉超");
        }
        [Fact]
        public void Simple_ConvertToSimplified_Test()
        {
            var text = StringHelper.ConvertToSimplified("劉偉超");
            text.ShouldBe("刘伟超");
        }

        [Fact]
        public void Simple_ConvertToSBC_Test()
        {
            var text = StringHelper.ConvertToSbc("2019 AiYoCore");
            text.ShouldBe("２０１９　Ｖｉｒｇｏ");
        }

        [Fact]
        public void Simple_ConvertToDBC_Test()
        {
            var text = StringHelper.ConvertToDbc("２０１９　Ｖｉｒｇｏ");
            text.ShouldBe("2019 AiYoCore");
        }

        [Fact]
        public void Simple_StringToUnicode_Test()
        {
            var text = StringHelper.StringToUnicode("2019 AiYoCore");
            text.ShouldBe(@"\u0032\u0030\u0031\u0039\u0020\u0056\u0069\u0072\u0067\u006F");
        }
        [Fact]
        public void Simple_UnicodeToString_Test()
        {
            var text = StringHelper.UnicodeToString(@"\u0032\u0030\u0031\u0039\u0020\u0056\u0069\u0072\u0067\u006F");
            text.ShouldBe("2019 AiYoCore");
        }
    }
}
