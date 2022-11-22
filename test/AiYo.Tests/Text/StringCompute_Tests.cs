using Shouldly;
using Virgo.Text;
using Xunit;

namespace Virgo.Tests.Text
{
    public class StringCompute_Tests
    {
        [Fact]
        public void Simple_Compute_Test()
        {
            var compute = new StringCompute("世界，你好。", "你好，世界。");
            compute.Compute();
            var computeResult = compute.ComputeResult;
            computeResult.Rate.ShouldBe(0.4285714285714285714285714286M);
            computeResult.Difference.ShouldBe(4);
            computeResult.UseTime.ShouldNotBeNull();
            computeResult.ComputeTimes.ShouldBe("36");
        }
        [Fact]
        public void Simple_SpeedyCompute_Test()
        {
            var compute = new StringCompute("世界，你好。", "你好，世界。");
            compute.SpeedyCompute();
            var computeResult = compute.ComputeResult;
            computeResult.Rate.ShouldBe(0.4285714285714285714285714286M);
            computeResult.Difference.ShouldBe(4);
            computeResult.UseTime.ShouldBeNull();
            computeResult.ComputeTimes.ShouldBe("36");
        }
    }
}
