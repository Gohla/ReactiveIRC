using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a user on an IRC server.
    /// </summary>
    public interface IUser : IMessageTarget, IComparable<IUser>, IEquatable<IUser>
    {
        /// <summary>
        /// Gets a value indicating whether the user is marked as away.
        /// </summary>
        bool Away { get; }

        /// <summary>
        /// Gets the channels this user has joined.
        /// </summary>
        IObservableCollection<IChannel> Channels { get; }
    }
}
