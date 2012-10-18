using System;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a network on an IRC server.
    /// </summary>
    public interface INetwork : IMessageTarget, IComparable<INetwork>, IEquatable<INetwork>
    {

    }
}
