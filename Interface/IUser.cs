using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a user on an IRC server.
    /// </summary>
    public interface IUser : IMessageTarget, IComparable<IUser>, IEquatable<IUser>, IKeyedObject<String>
    {
        /// <summary>
        /// Gets an observable stream of received and sent messages for this user.
        /// </summary>
        IObservable<IMessage> Messages { get; }

        /// <summary>
        /// Gets an observable stream of received messages for this user from the server.
        /// </summary>
        IObservable<IReceiveMessage> ReceivedMessages { get; }

        /// <summary>
        /// Gets an observable stream of all sent messages for this user by the client.
        /// </summary>
        IObservable<ISendMessage> SentMessages { get; }

        /// <summary>
        /// Gets the channels this user has joined.
        /// </summary>
        IObservableCollection<IChannel> Channels { get; }

        /// <summary>
        /// Gets an observable stream of values indicating whether the user is marked as away. Sends current value on
        /// subscribe.
        /// </summary>
        IObservable<bool> Away { get; }
    }
}
