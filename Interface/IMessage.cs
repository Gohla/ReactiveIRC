using System;
using System.Collections.Generic;

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
        /// Gets the receivers of the message. If empty, receiver is global.
        /// </summary>
        ICollection<IMessageTarget> Receivers { get; }
    }
}
