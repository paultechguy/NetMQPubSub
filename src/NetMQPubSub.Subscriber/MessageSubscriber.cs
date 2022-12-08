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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
	public virtual void Connect(string address)
	{
		this.socket.Connect(address);
	}

	/// <inheritdoc/>
	public virtual void Disconnect(string address)
	{
		this.socket.Disconnect(address);
	}

	/// <inheritdoc/>
	public virtual void Close()
	{
		this.socket.Close();
	}

	/// <inheritdoc/>
	public virtual string ReceiveStringMessage()
	{
		return this.socket.ReceiveFrameString();
	}

	/// <inheritdoc/>
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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
	public virtual void TopicSubscribe(string topic)
	{
		this.socket.Subscribe(topic);
	}

	/// <inheritdoc/>
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

	/// <summary>
	/// Attempt to receive a string message, blocking until one arrives. If no message is available within <paramref name="timeout"/>, returns false;
	/// </summary>
	/// <param name="timeout">The amount of time to wait for the arrival of a message. See <see cref="TimeSpan"/>.</param>
	/// <param name="message">If a message was avaiable, the received message.</param>
	/// <param name="moreFrames">true if another frame of the same message follows, otherwise false.</param>
	/// <returns>true if a message was available, otherwise false.</returns>
	protected virtual bool TryReceiveStringMessage(TimeSpan timeout, out string? message, out bool moreFrames)
	{
		return this.socket.TryReceiveFrameString(timeout, out message, out moreFrames);
	}

	/// <summary>
	/// Dispose the <see cref="MessageSubscriber"/>.
	/// </summary>
	/// <param name="disposing">true if called during <see cref="Dispose()"/>, or false if called during the object finalizer.</param>
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
