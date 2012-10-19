using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class Network : INetwork
    {
        public IClientConnection Connection { get; set; }
        public MessageTargetType Type { get { return MessageTargetType.Network; } }
        public IObservable<String> Name { get; private set; }

        public String Key { get { return Name.FirstAsync().Wait(); } }

        public Network(IClientConnection connection, String name)
        {
            Connection = connection;
            Name = Observable.Return(name);
        }

        public int CompareTo(INetwork other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Name.FirstAsync().Wait().CompareTo(other.Name.FirstAsync().Wait());
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
                EqualityComparer<String>.Default.Equals(this.Name.FirstAsync().Wait(), other.Name.FirstAsync().Wait())
             && EqualityComparer<IClientConnection>.Default.Equals(this.Connection, other.Connection)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Name.FirstAsync().Wait());
                hash = hash * 23 + EqualityComparer<IClientConnection>.Default.GetHashCode(this.Connection);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name.FirstAsync().Wait();
        }
    }
}
