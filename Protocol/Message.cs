using System.Collections.Generic;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class Message : IMessage
    {
        public IConnection Connection { get; private set; }
        public ICollection<IMessageTarget> Receivers { get; private set; }

        public Message(IConnection connection, params IMessageTarget[] receivers)
        {
            Connection = connection;
            Receivers = new HashSet<IMessageTarget>(receivers);
        }
    }
}
