using System;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a received IRC message.
    /// </summary>
    public interface IReceiveMessage : IMessage
    {
        /// <summary>
        /// Gets the contents of the message.
        /// </summary>
        String Contents { get; }

        /// <summary>
        /// Gets the sender of the message.
        /// </summary>
        IMessageTarget Sender { get; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        ReceiveType Type { get; }

        /// <summary>
        /// Gets the reply type of the message, if applicable.
        /// </summary>
        ReplyType ReplyType { get; }
    }
}
