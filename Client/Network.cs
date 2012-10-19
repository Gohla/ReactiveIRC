using System.Collections.Generic;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class Network : INetwork
    {
        public IClientConnection Connection { get; set; }
        public IIdentity Identity { get; set; }
        public MessageTargetType Type { get { return MessageTargetType.Network; } }

        public IIdentity Key { get { return Identity; } }

        public Network(IClientConnection connection, IIdentity identity)
        {
            Connection = connection;
            Identity = identity;
        }

        public int CompareTo(INetwork other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Identity.CompareTo(other.Identity);
            if(result == 0)
                result = this.Connection.CompareTo(other.Connection);
            return result;
        }

        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return Equals(other as INetwork);
        }

        public bool Equals(INetwork other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return
                EqualityComparer<IIdentity>.Default.Equals(this.Identity, other.Identity)
             && EqualityComparer<IClientConnection>.Default.Equals(this.Connection, other.Connection)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<IIdentity>.Default.GetHashCode(this.Identity);
                hash = hash * 23 + EqualityComparer<IClientConnection>.Default.GetHashCode(this.Connection);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Identity.Host;
        }
    }
}
