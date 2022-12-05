namespace NetMQPubSub.Core.Interfaces;

public interface IMessageSubscriberOptions
{
	int ReceiveHighWatermark { get; set; }
}
