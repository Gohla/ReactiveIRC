using System;
using System.Collections.Generic;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class SendMessage : ISendMessage
    {
        public IClientConnection Connection { get; private set; }
        public String Contents { get; private set; }
        public ICollection<IMessageTarget> Receivers { get; private set; }
        public SendType Type { get; private set; }

        public SendMessage(IClientConnection connection, String contents, SendType type, 
            params IMessageTarget[] receivers)
        {
            Connection = connection;
            Contents = contents;
            Receivers = new HashSet<IMessageTarget>(receivers);
            Type = type;
        }

        public override string ToString()
        {
            return Contents;
        }
    }
}
