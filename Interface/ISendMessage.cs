using System;
using System.Collections.Generic;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing an IRC message that can be sent.
    /// </summary>
    public interface ISendMessage : IMessage
    {
        /// <summary>
        /// Gets the receivers of the message. If empty, receiver is global.
        /// </summary>
        ICollection<IMessageTarget> Receivers { get; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        SendType Type { get; }
    }
}
