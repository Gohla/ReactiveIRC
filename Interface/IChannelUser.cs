using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Bitfield of flags for specifying user modes.
    /// </summary>
    [Flags]
    public enum UserMode
    {
        None = 0,
        Voice = 1,
        HalfOp = 2,
        Op = 4,
        Protected = 8,
        Owner = 16,
        ServerOp = 32
    }

    /// <summary>
    /// Interface representing a user on a certain channel.
    /// </summary>
    public interface IChannelUser : IMessageTarget, IKeyedObject<String>, IComparable<IChannel>, IEquatable<IChannel>, IComparable<IUser>,
        IEquatable<IUser>, IComparable<IChannelUser>, IEquatable<IChannelUser>
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        IChannel Channel { get; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Gets a the user modes that the user has on the channel. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<UserMode> UserModes { get; }
    }
}
