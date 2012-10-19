using System;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing an IRC message that can be sent.
    /// </summary>
    public interface ISendMessage : IMessage
    {
        /// <summary>
        /// Gets the raw contents of the message.
        /// </summary>
        String Raw { get; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        SendType Type { get; }
    }
}
