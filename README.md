# NetMQPubSub
Written in NET Core 6 (C#) his is a basic publisher-subscriber IPC example.  It uses
[NetMQ](https://github.com/zeromq/netmq) as the IPC transport and provides support for features such as:
* Multiple subscribers, subscribing to different topics.
* Global method to cancel both publisher and all subscribers.
* Demonstrates the use of a timeout when reading IPC messages (i.e. NET Core CancellationToken).

Although there is some separation of function (i.e. interfaces, classes) his is fairly bare-bones example.  However, it should provide
enough detail to allow the concepts to be placed into a more formal application.  The number of
[NetMQ](https://github.com/zeromq/netmq) features provided in the
Publisher and Subscriber objects is only what is required for the example code; it can be extended fairly easily.
