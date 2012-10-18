using System;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class SendMessage : Message, ISendMessage
    {
        public String Raw { get; private set; }
        public SendMessageType Type { get; private set; }

        public SendMessage(IConnection connection, String raw, SendMessageType type, params IMessageTarget[] receivers) :
            base(connection, receivers)
        {
            Raw = raw;
            Type = type;
        }
    }
}
