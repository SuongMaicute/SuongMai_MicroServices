namespace Suongmai.Services.AuthAPI.RabbitMQSender
{
    public interface IRabbbitIMAuthMessageSender
    {
        void SendMessage (Object message, string queueName);
    }
}
