namespace NetMQPubSub.Subscriber;

using NetMQ;
using NetMQ.Sockets;
using NetMQPubSub.Core.Interfaces;
using System;
using System.Text.Json;

/// <summary>
/// IPC subscriber to receive to topic messages.
/// </summary>
public class MessageSubscriber : BaseMessageSubscriber, IMessageSubscriber, IDisposable
{
	internal readonly SubscriberSocket socket;
	private bool isDisposed;

	/// <summary>
	/// Gets the <see cref="IMessageSubscriberOptions"/> for this class.
	/// </summary>
	public IMessageSubscriberOptions Options { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MessageSubscriber"/> class.
	/// </summary>
	public MessageSubscriber()
		: base()
	{
		this.socket = new SubscriberSocket();
		this.Options = new MessageSubscriberOptions(this);
	}

	/// <summary>
	/// Finalizes a <see cref="MessageSubscriber"/> object.
	/// </summary>
	~MessageSubscriber()
	{
		// do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	/// <summary>
	/// Connect the subscriber to the address.
	/// </summary>
	/// <param name="address">The address to connect to. Any address scheme supported by NetMQ.</param>
	public virtual void Connect(string address)
	{
		this.socket.Connect(address);
	}

	/// <summary>
	/// Disconnect this subscriber from address.
	/// </summary>
	/// <param name="address">The address to disconnect from. Any address scheme supported by NetMQ.</param>
	public virtual void Disconnect(string address)
	{
		this.socket.Disconnect(address);
	}

	/// <summary>
	/// Close this subscriber, rendering it unusable. Equivalent to calling <see cref="MessageSubscriber.Dispose()"/>.
	/// </summary>
	public virtual void Close()
	{
		this.socket.Close();
	}

	/// <summary>
	/// Receive a string message, blocking until one arrives.
	/// </summary>
	/// <returns>The message received.</returns>
	public virtual string ReceiveStringMessage()
	{
		return this.socket.ReceiveFrameString();
	}

	/// <summary>
	/// Receive an entity object, blocking until one arrives.
	/// </summary>
	/// <returns>The enity object received.</returns>
	public virtual T ReceiveMessage<T>() where T : class, new()
	{
		var text = this.ReceiveStringMessage();
		if (text is null)
		{
			throw new ArgumentNullException("Empty message received");
		}

		T entity = JsonSerializer.Deserialize<T>(text) ?? throw new ArgumentNullException($"Message received not type {typeof(T)}");

		return entity;
	}

	/// <summary>
	/// Attempt to receive a string message, blocking until one arrives. If no message is available within <paramref name="timeout"/>, returns false;
	/// </summary>
	/// <param name="timeout">The amount of time to wait for the arrival of a message. See <see cref="TimeSpan"/>.</param>
	/// <param name="topic">If a message was avaiable, the received <paramref name="topic"/> of the message.</param>
	/// <param name="message">If a message was available, the message received.</param>
	/// <returns>true if a message was available, otherwise false.</returns>
	public virtual bool TryReceiveStringMessage(TimeSpan timeout, out string? topic, out string? message)
	{
		message = null;
		if (this.TryReceiveStringMessage(timeout, out topic, out bool moreFrames))
		{
			if (moreFrames)
			{
				message = this.ReceiveStringMessage();
			}
		}

		return message is not null;
	}

	/// <summary>
	/// Attempt to receive an enity message, blocking until one arrives. If no message is available within <paramref name="timeout"/>, returns false;
	/// </summary>
	/// <param name="timeout">The amount of time to wait for the arrival of a message. See <see cref="TimeSpan"/>.</param>
	/// <param name="topic">If a message was avaiable, the received <paramref name="topic"/> of the message.</param>
	/// <param name="message">If a message was available, the  message received.</param>
	/// <returns>true if a message was available, otherwise false.</returns>
	public virtual bool TryReceiveMessage<T>(TimeSpan timeout, out string? topic, out T? entity) where T : class, new()
	{
		entity = null;
		if (this.TryReceiveStringMessage(timeout, out topic, out bool moreFrames))
		{
			if (moreFrames)
			{
				entity = this.ReceiveMessage<T>();
			}
		}

		return entity is not null;
	}

	/// <summary>
	/// Subscribe this subscriber to receive a topic.
	/// </summary>
	/// <param name="topic">The topic that the subscriber wants to subscribe to. See NetMQ for topic semantics.</param>
	public virtual void TopicSubscribe(string topic)
	{
		this.socket.Subscribe(topic);
	}

	/// <summary>
	/// Subscribe this subscriber to receive a collection of topics.
	/// </summary>
	/// <param name="topics">The topics that the subscriber wants to subscribe to. See NetMQ for topic semantics.</param>
	public virtual void TopicSubscribe(IEnumerable<string> topics)
	{
		foreach (var topic in topics)
		{
			this.socket.Subscribe(topic);
		}
	}

	/// <summary>
	/// Dispose of the <see cref="MessageSubscriber"/>, rendering it unusable.  Equivalent to calling <see cref="MessageSubscriber.Close()"/>.
	/// </summary>
	public virtual void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual bool TryReceiveStringMessage(TimeSpan timeout, out string? message, out bool moreFrames)
	{
		return this.socket.TryReceiveFrameString(timeout, out message, out moreFrames);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			if (disposing)
			{
				// dispose managed state (managed objects)
			}

			// free unmanaged resources (unmanaged objects) and override finalizer;
			// set large fields to null
			this.socket.Dispose();
			isDisposed = true;
		}
	}
}
