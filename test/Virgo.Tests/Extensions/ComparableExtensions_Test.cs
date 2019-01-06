using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Extensions;
using Xunit;

namespace Virgo.Tests.Extensions
{
    public class ComparableExtensions_Test
    {
        [Fact]
        public void Simple_IsBetween_Test()
        {
            var flag1 = 2.IsBetween(1, 3);
            flag1.ShouldBe(true);
            var flag2= 4.IsBetween(1, 3);
            flag2.ShouldBe(false);
        }
    }
}
