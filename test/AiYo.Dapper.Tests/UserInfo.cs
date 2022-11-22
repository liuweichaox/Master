using System;
using System.ComponentModel.DataAnnotations.Schema;
using Virgo.Domain.Entities;

namespace Virgo.Dapper.Tests
{
    [Table("Users")]
    public class UserInfo : Entity<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }
}
