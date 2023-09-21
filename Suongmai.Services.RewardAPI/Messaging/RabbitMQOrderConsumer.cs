
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Suongmai.Services.RewardAPI.Message;
using Suongmai.Services.RewardAPI.Services;
using System.Text;

namespace Suongmai.Services.RewardAPI.Messaging
{
    public class RabbitMQCartConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;
        private IConnection _connection;
        private IModel _channel;
        string queuename = "";
        private  string OrderCreated_RewardsUpdateQueue = "RewardsUpdateQueue";
        private  string ExchangeName = "";


        public RabbitMQCartConsumer( IConfiguration config, RewardService service)
        {
            _configuration = config;
            _rewardService = service;
            ExchangeName= _configuration.GetValue<string>("TopicQueueName:OrderCreatedTopic");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            queuename = _channel.QueueDeclare(OrderCreated_RewardsUpdateQueue, false, false,false, null);
            _channel.QueueBind(OrderCreated_RewardsUpdateQueue, ExchangeName, "RewardsUpdate");

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardsMessage message  = JsonConvert.DeserializeObject<RewardsMessage>(content);
                HandleMessage(message).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(OrderCreated_RewardsUpdateQueue, false, consumer);

            return Task.CompletedTask;

        }

        private async Task HandleMessage(RewardsMessage email)
        {
            _rewardService.UpdateRewards(email).GetAwaiter().GetResult();
        }
    }
}
