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
        private readonly string orderCreatedrRewardsSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;

        private ServiceBusProcessor _rewardProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardService reward)
        {

            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<String>("ServiceBusConnectionString");

            orderCreatedTopic = _configuration.GetValue<String>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedrRewardsSubscription = _configuration.GetValue<String>("TopicAndQueueNames:SubscriptionName");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _rewardProcessor = client.CreateProcessor(orderCreatedTopic,orderCreatedrRewardsSubscription);
            _rewardService = reward;
        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardstRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();

           
        }

      
        

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnNewOrderRewardstRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where u received a message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _rewardService.UpdateRewards(objMessage);
                await args.CompleteMessageAsync(args.Message);
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
    }
}
