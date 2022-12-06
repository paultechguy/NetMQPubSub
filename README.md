# NetMQPubSub
NetMQPubSub is a general IPC publish-subcribe library written in NET Core. The underlying IPC transport layer is provided by
[NetMQ](https://github.com/zeromq/netmq).

## Overview

NetMQPubSub provides support for features such as:
* Multiple subscribers, subscribing to different topics.
* Cancellation of publishers and subscribers using a .NET Core CancellationToken.
* Multiple language use
* Can be used by both .NET Core Web Applications and Console Applications.

The number of
[NetMQ](https://github.com/zeromq/netmq) features supported in NetMQPubSub is limited to those needed
by the example applications, found in the
[GitHub repository](https://github.com/paultechguy/NetMQPubSub).

## Example Applications

* Demonstrates the use of a timeout when reading IPC messages (i.e. NET Core CancellationToken).
