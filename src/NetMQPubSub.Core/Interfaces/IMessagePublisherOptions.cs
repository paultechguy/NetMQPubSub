namespace NetMQPubSub.Core.Interfaces;

public interface IMessagePublisherOptions
{
	int SendHighWatermark { get; set; }
}
