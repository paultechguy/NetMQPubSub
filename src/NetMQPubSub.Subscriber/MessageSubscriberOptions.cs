namespace NetMQPubSub.Subscriber;

using NetMQ;
using NetMQPubSub.Core.Interfaces;

/// <summary>
/// Message subscriber options.
/// </summary>
public class MessageSubscriberOptions : SocketOptions, IMessageSubscriberOptions
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MessageSubscriberOptions"/> class.
	/// </summary>
	public MessageSubscriberOptions(MessageSubscriber publisher)
		: base(publisher.socket)
	{
	}

	// I don't like this design, but NetMQ's SocketOptions has a dependency
	// on Socket which seems backwards

	// inhertited interface members
	//   ReceiveHighWatermark
}
