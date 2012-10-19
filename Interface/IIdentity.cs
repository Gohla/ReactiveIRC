using System;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing an identity on IRC.
    /// </summary>
    public interface IIdentity : IEquatable<IIdentity>, IComparable<IIdentity>
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        String Ident { get; }

        /// <summary>
        /// Gets the host.
        /// </summary>
        String Host { get; }

        /// <summary>
        /// Gets a value indicating whether this identity has a name.
        /// </summary>
        bool HasName { get; }

        /// <summary>
        /// Gets a value indicating whether this identity has an ident.
        /// </summary>
        bool HasIdent { get; }

        /// <summary>
        /// Queries if this identity will be compared by Name instead of Ident and Host.
        /// </summary>
        bool CompareByName { get; }
    }
}
