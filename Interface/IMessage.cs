using System;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing an IRC message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the connection this message belongs to.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// Gets the sender of the message.
        /// </summary>
        IMessageTarget Sender { get; }

        /// <summary>
        /// Gets the receiver of the message.
        /// </summary>
        IMessageTarget Receiver { get; }

        /// <summary>
        /// Gets the message string.
        /// </summary>
        String Message { get; }
    }
}
