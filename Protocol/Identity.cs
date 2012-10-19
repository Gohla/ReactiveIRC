using System;
using System.Collections.Generic;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class Identity : IIdentity
    {
        private String _name;
        private String _ident;
        private String _host;

        public String Name
        {
            get
            {
                return _name == null ? "unknown" : _name;
            }
            set
            {
                _name = value;
            }
        }

        public String Ident
        {
            get
            {
                return _ident == null ? "unknown" : _ident;
            }
        }

        public String Host
        {
            get
            {
                return _host == null ? "unknown" : _host;
            }
        }

        public bool HasName { get { return _name != null; } }

        public bool HasIdent { get { return _ident != null; } }

        public bool CompareByName
        {
            get
            {
                //return _ident == null || _host == null;
                return true;
            }
        }

        public Identity(String name, String ident, String host)
        {
            _name = name;
            _ident = ident;
            _host = host;
        }

        public int CompareTo(IIdentity other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            if(this.CompareByName || other.CompareByName)
            {
                int result = 0;
                result = this.Name.CompareTo(other.Name);
                return result;
            }
            else
            {
                int result = 0;
                result = this.Ident.CompareTo(other.Ident);
                if(result == 0)
                    result = this.Host.CompareTo(other.Host);
                return result;
            }
        }

        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return Equals(other as IIdentity);
        }

        public bool Equals(IIdentity other)
        {
            if(ReferenceEquals(other, null))
                return false;

            if(this.CompareByName || other.CompareByName)
            {
                return
                    EqualityComparer<String>.Default.Equals(this.Name, other.Name)
                 ;
            }
            else
            {
                return
                    EqualityComparer<String>.Default.Equals(this.Ident, other.Ident)
                 && EqualityComparer<String>.Default.Equals(this.Host, other.Host)
                 ;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                if(this.CompareByName)
                {
                    hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Name);
                }
                else
                {
                    hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Ident);
                    hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Host);
                }
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
