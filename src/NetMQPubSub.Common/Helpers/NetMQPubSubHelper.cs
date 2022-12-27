namespace NetMQPubSub.Common.Helpers;

using NetMQ;

/// <summary>
/// A helper class for supporting NetMQ specific features.
/// </summary>
public static class NetMQPubSubHelper
{
	/// <summary>
	/// Cleanup library resources. Call this method when your process is shutting down.
	/// </summary>
	public static void Cleanup(bool block = true)
	{
		NetMQConfig.Cleanup(block); // the the docs for final cleanup
	}
}
