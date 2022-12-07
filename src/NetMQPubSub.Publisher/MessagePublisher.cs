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

	/// <summary>
	/// Gets the <see cref="IMessagePublisherOptions"/> for this class.
	/// </summary>
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

	/// <summary>
	/// Bind the publisher to the address.
	/// </summary>
	/// <param name="address">The address to bind to. Any address scheme supported by NetMQ.</param>
	public virtual void Bind(string address)
	{
		this.socket.Bind(address);
	}

	/// <summary>
	/// Send a string topic along with a string message.
	/// </summary>
	/// <param name="topic">The topic used to associate the message with. See NetMQ for topic semantics.</param>
	/// <param name="message">The message to send.</param>
	public virtual void SendTopicMessage(string topic, string message)
	{
		this.socket.SendMoreFrame(topic).SendFrame(message);
	}

	/// <summary>
	/// Send a string topic along with an entity object. The entity must have a default constructor.
	/// </summary>
	/// <param name="topic">The topic used to associate the message with. See NetMQ for topic semantics.</param>
	/// <param name="entity">The entity object to send.</param>
	public virtual void SendTopicMessage<T>(string topic, T entity) where T : class, new()
	{
		var json = JsonSerializer.Serialize(entity);
		this.socket.SendMoreFrame(topic).SendFrame(json);
	}

	/// <summary>
	/// Unbind this publsher from address.
	/// </summary>
	/// <param name="address">The address to unbind from. Any address scheme supported by NetMQ.</param>
	public virtual void Unbind(string address)
	{
		this.socket.Unbind(address);
	}

	/// <summary>
	/// Close this publisher, rendering it unusable. Equivalent to calling <see cref="MessagePublisher.Dispose()"/>.
	/// </summary>
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
