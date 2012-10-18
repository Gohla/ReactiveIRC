using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing channels on an IRC server.
    /// </summary>
    public interface IChannel : IMessageTarget, IComparable<IChannel>, IEquatable<IChannel>
    {
        /// <summary>
        /// Gets an observable stream of topics. Sends value on subscribe.
        /// </summary>
        IObservable<String> Topic { get; }

        /// <summary>
        /// Gets the users in this channel.
        /// </summary>
        IObservableCollection<IChannelUser> Users { get; }
    }
}
