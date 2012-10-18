using System;

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
        ServerOp = 8
    }

    /// <summary>
    /// Interface representing a user on a certain channel.
    /// </summary>
    public interface IChannelUser : IMessageTarget, IComparable<IChannel>, IEquatable<IChannel>, IComparable<IUser>,
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
        /// Gets a the user modes that the user has on the channel.
        /// </summary>
        UserMode UserModes { get; }
    }
}
