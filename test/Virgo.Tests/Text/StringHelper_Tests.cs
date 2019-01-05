using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Text;
using Xunit;

namespace Virgo.Tests.Text
{
   public class StringHelper_Tests
    {
        [Fact]
        public void Simple_SimplifiedToTraditional_Test()
        {
           var text= StringHelper.SimplifiedToTraditional("刘伟超");
            text.ShouldBe("劉偉超");
        }
        [Fact]
        public void Simple_TraditionalToSimplified_Test()
        {
           var text= StringHelper.TraditionalToSimplified("劉偉超");
            text.ShouldBe("刘伟超");
        }
    }
}
