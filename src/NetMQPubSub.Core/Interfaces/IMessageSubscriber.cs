namespace NetMQPubSub.Core.Interfaces;

using System;

/// <summary>
/// Interface for a message subscriber.
/// </summary>
public interface IMessageSubscriber : IDisposable
{
	/// <summary>
	/// Connect the subscriber to the address.
	/// </summary>
	/// <param name="address">The address to connect to. Any address scheme supported by NetMQ.</param>
	void Connect(string address);

	/// <summary>
	/// Disconnect this subscriber from address.
	/// </summary>
	/// <param name="address">The address to disconnect from. Any address scheme supported by NetMQ.</param>
	void Disconnect(string address);

	/// <summary>
	/// Close this subscriber, rendering it unusable. Equivalent to calling Dispose.
	/// </summary>
	void Close();

	/// <summary>
	/// Receive a string message, blocking until one arrives.
	/// </summary>
	/// <returns>The message received.</returns>
	string ReceiveStringMessage();

	/// <summary>
	/// Receive an entity object, blocking until one arrives.
	/// </summary>
	/// <returns>The enity object received.</returns>
	T ReceiveMessage<T>() where T : class, new();

	/// <summary>
	/// Attempt to receive a string message, blocking until one arrives. If no message is available within <paramref name="timeout"/>, returns false;
	/// </summary>
	/// <param name="timeout">The amount of time to wait for the arrival of a message. See <see cref="TimeSpan"/>.</param>
	/// <param name="topic">If a message was avaiable, the received <paramref name="topic"/> of the message.</param>
	/// <param name="message">If a message was available, the message received.</param>
	/// <returns>true if a message was available, otherwise false.</returns>
	bool TryReceiveStringMessage(TimeSpan timeout, out string? topic, out string? message);

	/// <summary>
	/// Attempt to receive an enity message, blocking until one arrives. If no message is available within <paramref name="timeout"/>, returns false;
	/// </summary>
	/// <param name="timeout">The amount of time to wait for the arrival of a message. See <see cref="TimeSpan"/>.</param>
	/// <param name="topic">If a message was avaiable, the received <paramref name="topic"/> of the message.</param>
	/// <param name="entity">If a message was available, the  message received.</param>
	/// <returns>true if a message was available, otherwise false.</returns>
	bool TryReceiveMessage<T>(TimeSpan timeout, out string? topic, out T? entity) where T : class, new();

	/// <summary>
	/// Subscribe this subscriber to receive a topic.
	/// </summary>
	/// <param name="topic">The topic that the subscriber wants to subscribe to. See NetMQ for topic semantics.</param>
	void TopicSubscribe(string topic);

	/// <summary>
	/// Subscribe this subscriber to receive a collection of topics.
	/// </summary>
	/// <param name="topics">The topics that the subscriber wants to subscribe to. See NetMQ for topic semantics.</param>
	void TopicSubscribe(IEnumerable<string> topics);

	/// <summary>
	/// Gets the <see cref="IMessageSubscriberOptions"/> for this class.
	/// </summary>
	IMessageSubscriberOptions Options { get; }
}
