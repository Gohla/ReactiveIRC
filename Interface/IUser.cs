﻿using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a user on an IRC server.
    /// </summary>
    public interface IUser : IMessageTarget, IDisposable, IKeyedObject<String>, IComparable<IUser>, IEquatable<IUser>
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
        /// Gets the identity of the user.
        /// </summary>
        IIdentity Identity { get; }

        /// <summary>
        /// Gets the real name of the user. Initially set to the empty String. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<String> RealName { get; }

        /// <summary>
        /// Gets the network the user is on. Initially null. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<INetwork> Network { get; }

        /// <summary>
        /// Gets the modes of the user.
        /// </summary>
        Mode Modes { get; }

        /// <summary>
        /// Gets a value indicating whether the user is marked as away. Initially set to false. Subscribe to receive
        /// changes.
        /// </summary>
        ObservableProperty<bool> Away { get; }
    }
}
