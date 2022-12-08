namespace NetMQPubSub.Core.Interfaces;

using System;

/// <summary>
/// Interface for a message publisher.
/// </summary>
public interface IMessagePublisher : IDisposable
{
	/// <summary>
	/// Bind the publisher to the address.
	/// </summary>
	/// <param name="address">The address to bind to. Any address scheme supported by NetMQ.</param>
	void Bind(string address);

	/// <summary>
	/// Unbind this publsher from address.
	/// </summary>
	/// <param name="address">The address to unbind from. Any address scheme supported by NetMQ.</param>
	void Unbind(string address);

	/// <summary>
	/// Close this publisher, rendering it unusable. Equivalent to calling Dispose.
	/// </summary>
	void Close();

	/// <summary>
	/// Send a string topic along with a string message.
	/// </summary>
	/// <param name="topic">The topic used to associate the message with. See NetMQ for topic semantics.</param>
	/// <param name="message">The message to send.</param>
	void SendTopicMessage(string topic, string message);

	/// <summary>
	/// Send a string topic along with an entity object. The entity must have a default constructor.
	/// </summary>
	/// <param name="topic">The topic used to associate the message with. See NetMQ for topic semantics.</param>
	/// <param name="entity">The entity object to send.</param>
	void SendTopicMessage<T>(string topic, T entity) where T : class, new();

	/// <summary>
	/// Gets the <see cref="IMessagePublisherOptions"/> for this class.
	/// </summary>
	IMessagePublisherOptions Options { get; }
}
