using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Snowflake;
using Xunit;

namespace Virgo.Tests.Snowflake
{
    public class IdGenerator_Tests
    {
        [Fact]
        public void Simple_IdGen_Test()
        {
            IdGenerator.Instance.NextId().ToString().Length.ShouldBe(19);
        }
    }
}
