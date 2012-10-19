using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class MessageReceiver
    {
        private static readonly Regex PrefixRegex = new Regex("(?:([^!@]*)!)?(?:([^!@]*)@)?([^!@]*)", 
            RegexOptions.Compiled);
        private static readonly Regex CommandRegex = new Regex("^([0-9]{3})$", RegexOptions.Compiled);

        private static readonly Regex PingRegex = new Regex("^PING :(.*)", RegexOptions.Compiled);
        private static readonly Regex ErrorRegex = new Regex("^ERROR :(.*)", RegexOptions.Compiled);
        private static readonly Regex ActionRegex = new Regex("^PRIVMSG ([^:]*?) :" + "\x1" + "ACTION (.*)" + "\x1" + "$", 
            RegexOptions.Compiled);
        private static readonly Regex CtcpRequestRegex = new Regex("^PRIVMSG ([^:]*?) :" + "\x1" + "(.*)" + "\x1" + "$", 
            RegexOptions.Compiled);
        private static readonly Regex MessageRegex = new Regex("^PRIVMSG ([^:]*?) :(.*)$", RegexOptions.Compiled);
        private static readonly Regex CtcpReplyRegex = new Regex("^NOTICE ([^:]*?) :" + "\x1" + "(.*)" + "\x1" + "$",
            RegexOptions.Compiled);
        private static readonly Regex NoticeRegex = new Regex("^NOTICE ([^:]*?) :(.*)$", RegexOptions.Compiled);
        private static readonly Regex InviteRegex = new Regex("^INVITE (.*) (.*)$", RegexOptions.Compiled);
        private static readonly Regex JoinRegex = new Regex("^JOIN (.*)$", RegexOptions.Compiled);
        private static readonly Regex TopicRegex = new Regex("^TOPIC ([^:]*?) :(.*)$", RegexOptions.Compiled);
        private static readonly Regex NickRegex = new Regex("^NICK (.*)$", RegexOptions.Compiled);
        private static readonly Regex KickRegex = new Regex("^KICK (.*) (.*)$", RegexOptions.Compiled);
        private static readonly Regex PartRegex = new Regex("^PART (.*)$", RegexOptions.Compiled);
        private static readonly Regex ModeRegex = new Regex("^MODE (.*) (.*)$", RegexOptions.Compiled);
        private static readonly Regex QuitRegex = new Regex("^QUIT :(.*)$", RegexOptions.Compiled);

        public IClientConnection Connection { get; private set; }

        public MessageReceiver(IClientConnection connection)
        {
            Connection = connection;
        }

        public ReceiveMessage Receive(String raw)
        {
            if(raw == null)
                throw new ArgumentNullException("raw");
            if(String.IsNullOrWhiteSpace(raw))
                throw new ArgumentException("Not an IRC line", "raw");

            // Remove first :
            String line = null;
            if(raw[0] == ':')
                line = raw.Substring(1);
            else
                line = raw;

            // Tokenize
            String[] tokens = line.Split(new []{' '}, 3);
            String prefix = tokens[0];
            String command = tokens[1];
            String parameters = tokens[2];

            // Parse sender
            IMessageTarget sender = ParseSender(prefix);

            // Parse message type
            ReceiveType type = ParseMessageType(command);

            // Parse message
            if(type != ReceiveType.Unknown)
                return ParseInformationMessage(sender, type, parameters);
            else
                return ParseMessage(sender, line.Substring(prefix.Length + 1));
        }

        private IMessageTarget ParseSender(String prefix)
        {
            Match results = PrefixRegex.Match(prefix);
            String name = null;
            String ident = null;
            String host = null;

            if(!results.Success)
                throw new ArgumentException("Does not contain IRC prefix", "raw");
            if(results.Groups[1].Success)
                name = results.Groups[1].Value;
            if(results.Groups[2].Success)
                ident = results.Groups[2].Value;
            if(results.Groups[3].Success)
                host = results.Groups[3].Value;

            Identity identity = new Identity(name, ident, host);
            if(identity.HasName)
                return Connection.GetUser(identity);
            else
                return Connection.GetNetwork(identity.Host);
        }

        private ReceiveType ParseMessageType(String command)
        {
            // Try to match a numeric code.
            Match results = CommandRegex.Match(command);
            if(results.Success && results.Groups[1].Success)
            {
                String code = results.Groups[1].Value;
                int intCode;
                if(int.TryParse(code, out intCode))
                {
                    ReplyType reply = (ReplyType)intCode;
                    switch(reply)
                    {
                        case ReplyType.Welcome:
                        case ReplyType.YourHost:
                        case ReplyType.Created:
                        case ReplyType.MyInfo:
                        case ReplyType.Bounce:
                            return ReceiveType.Login;
                        case ReplyType.LuserClient:
                        case ReplyType.LuserOp:
                        case ReplyType.LuserUnknown:
                        case ReplyType.LuserMe:
                        case ReplyType.LuserChannels:
                            return ReceiveType.Info;
                        case ReplyType.MotdStart:
                        case ReplyType.Motd:
                        case ReplyType.EndOfMotd:
                            return ReceiveType.Motd;
                        case ReplyType.NamesReply:
                        case ReplyType.EndOfNames:
                            return ReceiveType.Name;
                        case ReplyType.WhoReply:
                        case ReplyType.EndOfWho:
                            return ReceiveType.Who;
                        case ReplyType.ListStart:
                        case ReplyType.List:
                        case ReplyType.ListEnd:
                            return ReceiveType.List;
                        case ReplyType.BanList:
                        case ReplyType.EndOfBanList:
                            return ReceiveType.BanList;
                        case ReplyType.Topic:
                        case ReplyType.NoTopic:
                            return ReceiveType.Topic;
                        case ReplyType.WhoIsUser:
                        case ReplyType.WhoIsServer:
                        case ReplyType.WhoIsOperator:
                        case ReplyType.WhoIsIdle:
                        case ReplyType.WhoIsChannels:
                        case ReplyType.EndOfWhoIs:
                            return ReceiveType.WhoIs;
                        case ReplyType.WhoWasUser:
                        case ReplyType.EndOfWhoWas:
                            return ReceiveType.WhoWas;
                        case ReplyType.UserModeIs:
                            return ReceiveType.UserMode;
                        case ReplyType.ChannelModeIs:
                            return ReceiveType.ChannelMode;
                        default:
                            if((intCode >= 400) && (intCode <= 599))
                                return ReceiveType.Error;
                            else
                                return ReceiveType.Unknown;
                    }
                }
            }

            // Not a numeric code.
            return ReceiveType.Unknown;
        }

        private ReceiveMessage ParseInformationMessage(IMessageTarget sender, ReceiveType type, String parameters)
        {
            return new ReceiveMessage(Connection, parameters, sender, type, Connection.Me);
        }

        private ReceiveMessage ParseMessage(IMessageTarget sender, String line)
        {
            ReceiveMessage message = null;

            message = ParseUndirectedMessage(PingRegex, ReceiveType.Ping, sender, line);
            if(message != null) return message;
            message = ParseUndirectedMessage(ErrorRegex, ReceiveType.Ping, sender, line);
            if(message != null) return message;

            message = ParseDirectedMessage(ActionRegex, ReceiveType.Action, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(MessageRegex, ReceiveType.Message, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(ActionRegex, ReceiveType.Notice, sender, line);
            if(message != null) return message;

            //message = ParseDirectedMessage(InviteRegex, ReceiveType.Invite, sender, line, offset);
            //if(message != null) return message;

            message = ParseDirectedMessage(JoinRegex, ReceiveType.Join, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(PartRegex, ReceiveType.Part, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(KickRegex, ReceiveType.Kick, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(TopicRegex, ReceiveType.TopicChange, sender, line);
            if(message != null) return message;
            //message = ParseDirectedMessage(ModeRegex, ReceiveType.Mode, sender, line, offset);
            //if(message != null) return message;

            message = ParseUndirectedMessage(NickRegex, ReceiveType.NickChange, sender, line);
            if(message != null) return message;
            message = ParseUndirectedMessage(QuitRegex, ReceiveType.Quit, sender, line);
            if(message != null) return message;

            return new ReceiveMessage(Connection, line, sender, ReceiveType.Unknown);
        }

        private ReceiveMessage ParseUndirectedMessage(Regex regex, ReceiveType type, IMessageTarget sender, String line)
        {
            Match results = PingRegex.Match(line);
            if(results.Success)
            {
                String message = String.Empty;
                if(results.Groups[1].Success)
                    message = results.Groups[1].Value;

                return new ReceiveMessage(Connection, message, sender, ReceiveType.Ping, Connection.Me);
            }
            return null;
        }

        private ReceiveMessage ParseDirectedMessage(Regex regex, ReceiveType type, IMessageTarget sender, String line)
        {
            Match results = regex.Match(line);
            if(results.Success && results.Groups[1].Success)
            {
                String receiverName = results.Groups[1].Value;
                IMessageTarget receiver;
                switch(receiverName[0])
                {
                    case '#':
                    case '!':
                    case '&':
                    case '+':
                        receiver = Connection.GetChannel(receiverName);
                        break;
                    default:
                        receiver = Connection.GetUser(receiverName);
                        break;
                }

                String message = String.Empty;
                if(results.Groups[2].Success)
                    message = results.Groups[2].Value;

                return new ReceiveMessage(Connection, message, sender, type, receiver);
            }
            return null;
        }
    }
}
