namespace Suongmai.Services.ShoppingCartAPI.RabbitMQSender
{
    public interface IRabbbitIMCartMessageSender
    {
        void SendMessage (Object message, string queueName);
    }
}
