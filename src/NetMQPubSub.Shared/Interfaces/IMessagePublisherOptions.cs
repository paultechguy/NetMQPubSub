namespace NetMQPubSub.Shared.Interfaces;

using NetMQ;

public interface IMessagePublisherOptions
{
    int SendHighWatermark { get; set; }
}
