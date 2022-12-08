namespace NetMQPubSub.Publisher;

using NetMQ;
using NetMQ.Sockets;
using NetMQPubSub.Core.Interfaces;
using System;
using System.Text.Json;

/// <summary>
/// IPC publisher to send topic messages.
/// </summary>
public class MessagePublisher : BaseMessagePublisher, IMessagePublisher, IDisposable
{
	internal readonly PublisherSocket socket;
	private bool isDisposed;

	/// <inheritdoc/>
	public IMessagePublisherOptions Options { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MessagePublisher"/> class.
	/// </summary>
	public MessagePublisher()
		: base()
	{
		this.socket = new PublisherSocket();
		this.Options = new MessagePublisherOptions(this);
	}

	/// <summary>
	/// Finalizes a <see cref="MessagePublisher"/> object.
	/// </summary>
	~MessagePublisher()
	{
		// do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	/// <inheritdoc/>
	public virtual void Bind(string address)
	{
		this.socket.Bind(address);
	}

	/// <inheritdoc/>
	public virtual void SendTopicMessage(string topic, string message)
	{
		this.socket.SendMoreFrame(topic).SendFrame(message);
	}

	/// <inheritdoc/>
	public virtual void SendTopicMessage<T>(string topic, T entity) where T : class, new()
	{
		var json = JsonSerializer.Serialize(entity);
		this.socket.SendMoreFrame(topic).SendFrame(json);
	}

	/// <inheritdoc/>
	public virtual void Unbind(string address)
	{
		this.socket.Unbind(address);
	}

	/// <inheritdoc/>
	public virtual void Close()
	{
		this.socket.Close();
	}

	/// <summary>
	/// Dispose of the <see cref="MessagePublisher"/>, rendering it unusable.  Equivalent to calling <see cref="MessagePublisher.Close()"/>.
	/// </summary>
	public virtual void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose the <see cref="MessagePublisher"/>.
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
