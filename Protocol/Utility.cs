using System;
using System.Text.RegularExpressions;

namespace ReactiveIRC.Protocol
{
    public static class Utility
    {
        private static Regex _nicknameRegex = new Regex(@"^[A-Za-z\[\]\\`_^{|}][A-Za-z0-9\[\]\\`_\-^{|}]+$",
            RegexOptions.Compiled);

        public static bool IsValidNickname(String nickname)
        {
            if((nickname != null) && (nickname.Length > 0) && (_nicknameRegex.Match(nickname).Success))
                return true;

            return false;
        }
    }
}
