namespace Virgo.RabbitMQ
{
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
