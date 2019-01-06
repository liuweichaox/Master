using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Extensions;
using Xunit;

namespace Virgo.Tests.Extensions
{
    public class DateTimeExtensions_Tests
    {
        [Fact]
        public void Simple_ToUnixTimestamp_Test()
        { 
            var unixTime = DateTime.Parse("2019/1/1 0:00:00").ToUnixTimestamp();
            unixTime.ShouldBe(1546272000L);
        }
        [Fact]
        public void Simple_FromUnixTimestamp_Test()
        {
            var unixTime = 1546272000L.FromUnixTimestamp();
            unixTime.ShouldBe(DateTime.Parse("2019/1/1 0:00:00"));
        }
    }
}
