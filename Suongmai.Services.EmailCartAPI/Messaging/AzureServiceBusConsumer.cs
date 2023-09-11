using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Suongmai.Services.EmailCartAPI.Message;
using Suongmai.Services.EmailCartAPI.Models.Dto;
using Suongmai.Services.EmailCartAPI.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Suongmai.Services.EmailCartAPI.Messaging
{
    public class AzureServiceBusConsumer :IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private readonly string OrderCreated_Topic;
        private readonly string OrderCreated_Email_Supsciption;
        private readonly ServiceBusProcessor _emailOrderPlacedProceesor;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {

            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<String>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<String>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = _configuration.GetValue<String>("TopicAndQueueNames:RegisterUserQueue");

            OrderCreated_Topic = _configuration.GetValue<String>("TopicAndQueueNames:OrderCreatedTopic");
            OrderCreated_Email_Supsciption = _configuration.GetValue<String>("TopicAndQueueNames:SubscriptionName");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerUserProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProceesor = client.CreateProcessor(OrderCreated_Topic, OrderCreated_Email_Supsciption);
            _emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();


            _registerUserProcessor.ProcessMessageAsync += OnUserRequestRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProceesor.ProcessMessageAsync += OnorderPlacedRequestReceived;
            _emailOrderPlacedProceesor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProceesor.StartProcessingAsync();
        }

        private async Task OnorderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
             var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserRequestRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where u received a message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where u received a message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();


            await _emailOrderPlacedProceesor.StopProcessingAsync();
            await _emailOrderPlacedProceesor.DisposeAsync();
        }
    }
}
