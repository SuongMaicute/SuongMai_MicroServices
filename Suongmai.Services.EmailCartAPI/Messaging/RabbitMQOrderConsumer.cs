﻿
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Suongmai.Services.EmailCartAPI.Message;
using Suongmai.Services.EmailCartAPI.Services;
using System.Text;

namespace Suongmai.Services.EmailCartAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;
         string queueName = "";
        private  string ExchangeName = "";
        private  string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";



        public RabbitMQOrderConsumer( IConfiguration config, EmailService service)
        {
            _configuration = config;
            _emailService = service;
            ExchangeName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName ,ExchangeType.Direct);
            _channel.QueueDeclare(OrderCreated_EmailUpdateQueue, false, false, false, null);
            _channel.QueueBind(OrderCreated_EmailUpdateQueue, ExchangeName, "EmailUpdate");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardsMessage reward = JsonConvert.DeserializeObject<RewardsMessage>(content);
                HandleMessage(reward).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(OrderCreated_EmailUpdateQueue, false, consumer);

            return Task.CompletedTask;

        }

        private async Task HandleMessage(RewardsMessage reward)
        {
            _emailService.LogOrderPlaced(reward).GetAwaiter().GetResult();
        }
    }
}
