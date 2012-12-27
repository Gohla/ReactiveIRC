using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ReactiveIRC.Interface
{
    public class IdentityMask
    {
        private static readonly Regex PrefixRegex = new Regex("(?:([^!@]*)!)?(?:([^!@]*)@)?([^!@]*)", 
            RegexOptions.Compiled);

        private readonly Regex _name;
        private readonly Regex _ident;
        private readonly Regex _host;

        public IdentityMask(String name, String ident, String host)
        {
            if(name != null)
                _name = new Regex(Regex.Escape(name).Replace(@"\*", @"[^!@]*"), RegexOptions.Compiled);
            if(ident != null)
                _ident = new Regex(Regex.Escape(ident).Replace(@"\*", @"[^!@]*"), RegexOptions.Compiled);
            if(host != null)
                _host = new Regex(Regex.Escape(host).Replace(@"\*", @"[^!@]*"), RegexOptions.Compiled);
        }

        public static IdentityMask Parse(String str)
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

            return new IdentityMask(name, ident, host);
        }

        public bool Match(IIdentity identity)
        {
            bool match = true;

            if(_name != null)
                if(identity.Name.Value != null)
                    match &= _name.IsMatch(identity.Name.Value);
                else
                    return false;

            if(_ident != null)
                if(identity.Ident.Value != null)
                    match &= _ident.IsMatch(identity.Ident.Value);
                else
                    return false;

            if(_host != null)
                if(identity.Host.Value != null)
                    match &= _host.IsMatch(identity.Host.Value);
                else
                    return false;

            return match;
        }
    }
}
