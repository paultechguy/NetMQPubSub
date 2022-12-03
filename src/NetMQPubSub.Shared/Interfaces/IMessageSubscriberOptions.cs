namespace NetMQPubSub.Shared.Interfaces;

public interface IMessageSubscriberOptions
{
	int ReceiveHighWatermark { get; set; }
}
