using Newtonsoft.Json;
using Shouldly;
using System;
using Virgo.Extensions;
using Xunit;

namespace Virgo.Tests.Extensions
{
    public class JsonExtensions_Tests
    {
        [Fact]
        public void Simple_Serialize_Test()
        {
            var person = new Person("AiYoCore");
            var json = person.Serialize();
            json.ShouldBe("{\"Name\":\"AiYoCore\"}");
        }
        [Fact]
        public void Simple_Deserialize_Test()
        {
            var json = "{\"Name\":\"AiYoCore\"}";
            var person = json.Deserialize<Person>();
            person.Name.ShouldBe("AiYoCore");
        }

    }

    [Serializable]
    public class Person
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        public Person()
        {
        }
        public Person(string name)
        {
            Name = name;
        }
    }
}
