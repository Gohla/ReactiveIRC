using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gohla.Shared;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class Identity : IIdentity
    {
        private static readonly Regex PrefixRegex = new Regex("(?:([^!@]*)!)?(?:([^!@]*)@)?([^!@]*)", 
            RegexOptions.Compiled);

        public ObservableProperty<String> Name { get; private set; }
        public ObservableProperty<String> Ident { get; private set; }
        public ObservableProperty<String> Host { get; private set; }

        public Identity(String name, String ident, String host)
        {
            Name = new ObservableProperty<String>(name);
            Ident = new ObservableProperty<String>(ident);
            Host = new ObservableProperty<String>(host);
        }

        public void Dispose()
        {
            if(Host == null)
                return;

            Host.Dispose();
            Host = null;
            Ident.Dispose();
            Ident = null;
            Name.Dispose();
            Name = null;
        }

        public static IIdentity Parse(String str)
        {
            Match results = PrefixRegex.Match(str);

            if(!results.Success)
                return null;

            String name = null;
            String ident = null;
            String host = null;

            if(results.Groups[1].Success)
                name = results.Groups[1].Value;
            if(results.Groups[2].Success)
                ident = results.Groups[2].Value;
            if(results.Groups[3].Success)
                host = results.Groups[3].Value;

            return new Identity(name, ident, host);
        }

        public static bool TryParse(String str, out IIdentity identity)
        {
            identity = Parse(str);
            if(identity == null)
                return false;
            else
                return true;
        }

        public int CompareTo(IIdentity other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Name.Value.CompareTo(other.Name);
            if(result == 0)
                result = this.Ident.Value.CompareTo(other.Ident);
            if(result == 0)
                result = this.Host.Value.CompareTo(other.Host);
            return result;
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

            return
                EqualityComparer<String>.Default.Equals(this.Name, other.Name)
             && EqualityComparer<String>.Default.Equals(this.Ident, other.Ident)
             && EqualityComparer<String>.Default.Equals(this.Host, other.Host)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Name);
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Ident);
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Host);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
