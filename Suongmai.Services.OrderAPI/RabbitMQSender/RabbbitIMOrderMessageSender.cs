using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Suongmai.Services.OrderAPI.RabbitMQSender
{
    public class RabbbitIMOrderMessageSender : IRabbbitIMOrderMessageSender
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;
        private  string OrderCreated_RewardsUpdateQueue = "RewardsUpdateQueue";
        private  string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";

        public RabbbitIMOrderMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";

        }
        public void SendMessage(object message, string exchangeName)
        {
            if (ConnectionExists())
            {

                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable:false);

                channel.QueueDeclare(OrderCreated_EmailUpdateQueue, false, false, false, null);
                channel.QueueDeclare(OrderCreated_RewardsUpdateQueue, false, false, false, null);

                channel.QueueBind(OrderCreated_EmailUpdateQueue, exchangeName, "EmailUpdate");
                channel.QueueBind(OrderCreated_RewardsUpdateQueue, exchangeName, "RewardsUpdate");

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: exchangeName, "EmailUpdate", null, body: body);
                channel.BasicPublish(exchange: exchangeName, "RewardsUpdate", null, body: body);
            }
        }
        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    Password = _password,
                    UserName = _username,
                };
                // connect to rabbitMQ
                _connection = factory.CreateConnection();

            }
            catch(Exception ex)
            {

            }
        }
        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return true;
        }
    }
}
