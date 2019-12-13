using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Virgo.Domain.Models;

namespace Virgo.Presentation.Models
{
    public class ValueObjectTest : ValueObject<ValueObjectTest>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Age;

        }
    }
}
