using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Virgo.Domain.Models;

namespace Virgo.UserInterface.Models
{
    public class EntityTest : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
