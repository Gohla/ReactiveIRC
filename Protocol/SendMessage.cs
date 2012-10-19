using System;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class SendMessage : Message, ISendMessage
    {
        public String Raw { get; private set; }
        public SendType Type { get; private set; }

        public SendMessage(IClientConnection connection, String raw, SendType type, 
            params IMessageTarget[] receivers) :
            base(connection, receivers)
        {
            Raw = raw;
            Type = type;
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}
