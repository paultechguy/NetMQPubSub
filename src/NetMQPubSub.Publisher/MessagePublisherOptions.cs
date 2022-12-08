namespace NetMQPubSub.Publisher;

using NetMQ;
using NetMQPubSub.Core.Interfaces;

/// <summary>
/// Message publisher options.
/// </summary>
public class MessagePublisherOptions : SocketOptions, IMessagePublisherOptions
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MessagePublisherOptions"/> class.
	/// </summary>
	public MessagePublisherOptions(MessagePublisher publisher)
		: base(publisher.socket)
	{
	}

	// I don't like this design, but NetMQ's SocketOptions has a dependency
	// on Socket which seems backwards.

	// inhertited interface members
	//   SendHighWatermark
}
