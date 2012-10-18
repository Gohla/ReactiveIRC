using System;
using System.Reactive;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a connection to an IRC server.
    /// </summary>
    public interface IConnection : IEquatable<IConnection>, IComparable<IConnection>, IDisposable
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
        /// Gets an observable stream of messages.
        /// </summary>
        IObservable<IMessage> Messages { get; }

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
