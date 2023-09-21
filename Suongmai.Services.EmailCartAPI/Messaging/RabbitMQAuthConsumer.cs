﻿
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Suongmai.Services.EmailCartAPI.Models.Dto;
using Suongmai.Services.EmailCartAPI.Services;
using System.Text;

namespace Suongmai.Services.EmailCartAPI.Messaging
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;


        public RabbitMQAuthConsumer( IConfiguration config, EmailService service)
        {
            _configuration = config;
            _emailService = service;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")
                , false, false, false, null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CartDto cartDTO = JsonConvert.DeserializeObject<CartDto>(content);
                HandleMessage(cartDTO).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"), false, consumer);

            return Task.CompletedTask;

        }

        private async Task HandleMessage(CartDto cart)
        {
            _emailService.EmailCartAndLog(cart).GetAwaiter().GetResult();
        }
    }
}