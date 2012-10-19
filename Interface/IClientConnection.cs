using System;
using System.Reactive;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a connection to an IRC server.
    /// </summary>
    public interface IClientConnection : IEquatable<IClientConnection>, IComparable<IClientConnection>, IDisposable
    {
        /// <summary>
        /// Gets the server address.
        /// </summary>
        String Address { get; }

        /// <summary>
        /// Gets the server port.
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        IUser Me { get; }

        /// <summary>
        /// Gets the channels the current user has joined.
        /// </summary>
        IObservableCollection<IChannel> Channels { get; }

        /// <summary>
        /// Gets the users the current user can see.
        /// </summary>
        IObservableCollection<IUser> Users { get; }

        /// <summary>
        /// Gets an observable stream of all received and sent messages.
        /// </summary>
        IObservable<IMessage> Messages { get; }

        /// <summary>
        /// Gets an observable stream of all received messages from the server.
        /// </summary>
        IObservable<IReceiveMessage> ReceivedMessages { get; }

        /// <summary>
        /// Gets an observable stream of all sent messages by the client.
        /// </summary>
        IObservable<ISendMessage> SentMessages { get; }

        /// <summary>
        /// Connects to the server.
        /// </summary>
        ///
        /// <returns>
        /// An observable stream that notifies when connected or when failed to connect.
        /// </returns>
        IObservable<Unit> Connect();

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        ///
        /// <returns>
        /// An observable stream that notifies when disconnected or when failed to disconnect.
        /// </returns>
        IObservable<Unit> Disconnect();

        /// <summary>
        /// Send given messages to the server.
        /// </summary>
        ///
        /// <param name="messages">An observable stream that notifies when message is sent or has failed to send.</param>
        IObservable<Unit> Send(params ISendMessage[] messages);

        /// <summary>
        /// Logins in on the IRC server.
        /// </summary>
        ///
        /// <param name="nickname">The nickname.</param>
        /// <param name="username">The user name.</param>
        /// <param name="realname">The real name.</param>
        ///
        /// <param name="messages">An observable stream that notifies when message is sent or has failed to send.</param>
        IObservable<Unit> Login(String nickname, String username, String realname);

        /// <summary>
        /// Logins in on the password protected IRC server.
        /// </summary>
        ///
        /// <param name="nickname">The nickname.</param>
        /// <param name="username">The user name.</param>
        /// <param name="realname">The real name.</param>
        /// <param name="password">The password.</param>
        ///
        /// <param name="messages">An observable stream that notifies when message is sent or has failed to send.</param>
        IObservable<Unit> Login(String nickname, String username, String realname, String password);

        /// <summary>
        /// Gets the network with given name. 
        /// </summary>
        ///
        /// <param name="name">The network name.</param>
        INetwork GetNetwork(String network);

        /// <summary>
        /// Gets the channel with given name. Returns null if the channel cannot be found.
        /// </summary>
        ///
        /// <param name="channel">The channel name.</param>
        ///
        /// <returns>
        /// The channel or null if it cannot be found.
        /// </returns>
        IChannel GetChannel(String channel);

        /// <summary>
        /// Gets the user with given nickname. Returns null if the user cannot be found.
        /// </summary>
        ///
        /// <param name="nickname">The nickname of the user.</param>
        ///
        /// <returns>
        /// The user or null if they cannot be found.
        /// </returns>
        IUser GetUser(String nickname);
    }
}
