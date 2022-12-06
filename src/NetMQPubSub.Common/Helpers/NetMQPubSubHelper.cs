namespace NetMQPubSub.Common.Helpers;

using NetMQ;

public static class NetMQPubSubHelper
{
	public static void Cleanup()
	{
		NetMQConfig.Cleanup(); // the the docs for final cleanup
	}
}
