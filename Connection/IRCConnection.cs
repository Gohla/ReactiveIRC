using System;
using System.Reactive;
using ReactiveIRC.Command;

namespace ReactiveIRC.Connection
{
    public class IRCConnection : RawConnection
    {
        public IRCConnection(String address, ushort port)
            : base(address, port)
        {

        }

        public IObservable<Unit> Login(String nickname)
        {
            return Login(nickname, nickname, nickname);
        }

        public IObservable<Unit> Login(String nickname, String username)
        {
            return Login(nickname, username, username);
        }

        public IObservable<Unit> Login(String nickname, String username, String realname)
        {
            return WriteRaw
            (
                Rfc2812.Nick(nickname)
              , Rfc2812.User(username, 0, realname)
            );
        }

        public IObservable<Unit> Login(String nickname, String username, String realname, String password)
        {
            return WriteRaw
            (
                Rfc2812.Pass(password)
              , Rfc2812.Nick(nickname)
              , Rfc2812.User(username, 0, realname)
            );
        }
    }
}
