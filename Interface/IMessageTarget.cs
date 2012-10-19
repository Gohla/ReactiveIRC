using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Values that represent message target types.
    /// </summary>
    public enum MessageTargetType
    {
        Unknown,
        Network,
        Channel,
        User,
        ChannelUser
    }

    /// <summary>
    /// Interface representing a message target (sender or receiver)
    /// </summary>
    public interface IMessageTarget : IKeyedObject<IIdentity>
    {
        /// <summary>
        /// Gets the connection this target belongs to.
        /// </summary>
        IClientConnection Connection { get; }

        /// <summary>
        /// Gets the identity.
        /// </summary>
        IIdentity Identity { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        MessageTargetType Type { get; }
    }
}
