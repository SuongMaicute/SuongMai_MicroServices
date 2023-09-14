using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Suongmai.Services.RewardAPI.Message;
using Suongmai.Services.RewardAPI.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Suongmai.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer :IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;

        private ServiceBusProcessor _rewardProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _rewardService = rewardService;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            orderCreatedTopic = _configuration.GetValue<string>("TopicQueueName:OrderCreatedTopic");
            orderCreatedRewardSubscription = _configuration.GetValue<string>("TopicQueueName:SubscriptionName");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubscription);
        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();

        }

        private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs arg)
        {
            try
            {
            //this is where you will receive message
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
                //TODO - try to log email
                await _rewardService.UpdateRewards(objMessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

        }

        

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine("ErrHandler................"+args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
