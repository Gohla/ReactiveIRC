﻿using System;
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
        private static readonly Regex CommandRegex = new Regex("^([0-9]{3})$", RegexOptions.Compiled);

        private static readonly Regex PingRegex = new Regex("^PING :?(.*)", RegexOptions.Compiled);
        private static readonly Regex ErrorRegex = new Regex("^ERROR :?(.*)", RegexOptions.Compiled);

        private static readonly Regex ActionRegex = new Regex("^PRIVMSG ([^: ]*?) :" + "\x1" + "ACTION (.*)" + "\x1" + "$", 
            RegexOptions.Compiled);
        private static readonly Regex CtcpRequestRegex = new Regex("^PRIVMSG ([^: ]*?) :" + "\x1" + "(.*)" + "\x1" + "$", 
            RegexOptions.Compiled);
        private static readonly Regex MessageRegex = new Regex("^PRIVMSG ([^: ]*?) :(.*)$", RegexOptions.Compiled);
        private static readonly Regex CtcpReplyRegex = new Regex("^NOTICE ([^: ]*?) :" + "\x1" + "(.*)" + "\x1" + "$",
            RegexOptions.Compiled);
        private static readonly Regex NoticeRegex = new Regex("^NOTICE ([^: ]*?) :(.*)$", RegexOptions.Compiled);
        private static readonly Regex InviteRegex = new Regex("^INVITE ([^: ]*) ([^: ]*)$", RegexOptions.Compiled);
        private static readonly Regex JoinRegex = new Regex("^JOIN :?([^: ]*)$", RegexOptions.Compiled);
        private static readonly Regex TopicRegex = new Regex("^TOPIC ([^: ]*?) :?(.*)$", RegexOptions.Compiled);
        private static readonly Regex NickRegex = new Regex("^NICK :?([^: ]*)$", RegexOptions.Compiled);
        private static readonly Regex KickRegex = new Regex("^KICK ([^: ]*?) ([^: ]*?)(?: :(.*))?$", RegexOptions.Compiled);
        private static readonly Regex PartRegex = new Regex("^PART ([^: ]*?)(?: :(.*))?$", RegexOptions.Compiled);
        private static readonly Regex ModeRegex = new Regex("^MODE ([^: ]*?) :?(.*)$", RegexOptions.Compiled);
        private static readonly Regex QuitRegex = new Regex("^QUIT :?(.*)$", RegexOptions.Compiled);

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

            // Try to parse messages without prefixes.
            ReceiveMessage message = ParseNoPrefixMessage(line);
            if(message != null) return message;

            // Tokenize
            String[] tokens = line.Split(new []{' '}, 3);
            String prefix = tokens[0];
            String command = tokens[1];
            String parameters = String.Empty;
            if(tokens.Length == 3)
                parameters = tokens[2];

            // Parse sender
            IMessageTarget sender = ParseSender(prefix);

            // Parse message type
            Tuple<ReceiveType, ReplyType> type = ParseMessageType(command);

            // Parse message
            if(type.Item1 != ReceiveType.Unknown)
                return ParseInformationMessage(sender, type.Item1, type.Item2, parameters);
            else
                return ParseMessage(sender, line.Substring(prefix.Length + 1));
        }

        private IMessageTarget ParseSender(String prefix)
        {
            IIdentity identity = Identity.Parse(prefix);

            if(identity == null)
                throw new ArgumentException("Does not contain IRC prefix", "raw");

            if(!String.IsNullOrWhiteSpace(identity.Name.Value))
            {
                IUser user = Connection.GetUser(identity.Name);
                // TODO: Is setting host and ident necessary?
                user.Identity.Host.Value = identity.Host;
                user.Identity.Ident.Value = identity.Ident;
                return user;
            }
            else
                return Connection.GetNetwork(identity.Host);
        }

        private Tuple<ReceiveType, ReplyType> ParseMessageType(String command)
        {
            // Try to match a numeric code.
            Match results = CommandRegex.Match(command);
            if(results.Success && results.Groups[1].Success)
            {
                String code = results.Groups[1].Value;
                int intCode;
                if(int.TryParse(code, out intCode))
                {
                    if(Enum.IsDefined(typeof(ReplyType), intCode))
                    {
                        ReplyType reply = (ReplyType)intCode;

                        if((intCode >= 400) && (intCode <= 599))
                            return Tuple.Create(ReceiveType.Error, reply);
                        else
                            return Tuple.Create(ReceiveType.Reply, reply);
                    }
                }
            }

            // Not a numeric code.
            return Tuple.Create(ReceiveType.Unknown, ReplyType.Unknown);
        }

        private ReceiveMessage ParseNoPrefixMessage(String line)
        {
            ReceiveMessage message = null;

            message = ParseUndirectedMessage(PingRegex, ReceiveType.Ping, Connection.Me.Network.Value, line, 
                Connection.Me);
            if(message != null) return message;
            message = ParseUndirectedMessage(ErrorRegex, ReceiveType.Error, Connection.Me.Network.Value, line, 
                Connection.Me);
            if(message != null) return message;

            return message;
        }

        private ReceiveMessage ParseInformationMessage(IMessageTarget sender, ReceiveType type, ReplyType replyType, 
            String parameters)
        {
            return new ReceiveMessage(Connection, parameters, sender, type, replyType, Connection.Me);
        }

        private ReceiveMessage ParseMessage(IMessageTarget sender, String line)
        {
            ReceiveMessage message = null;

            message = ParseDirectedMessage(ActionRegex, ReceiveType.Action, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(MessageRegex, ReceiveType.Message, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(ActionRegex, ReceiveType.Notice, sender, line);
            if(message != null) return message;

            message = ParseDirectedMessage(JoinRegex, ReceiveType.Join, sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(PartRegex, ReceiveType.Part, sender, line);
            if(message != null) return message;
            message = ParseKickMessage(sender, line);
            if(message != null) return message;
            message = ParseDirectedMessage(TopicRegex, ReceiveType.TopicChange, sender, line);
            if(message != null) return message;

            message = ParseUndirectedMessage(NickRegex, ReceiveType.NickChange, sender, line, 
                Connection.Me.Network.Value);
            if(message != null) return message;
            message = ParseUndirectedMessage(QuitRegex, ReceiveType.Quit, sender, line, Connection.Me.Network.Value);
            if(message != null) return message;

            message = ParseModeMessage(sender, line);
            if(message != null) return message;
            message = ParseInviteMessage(sender, line);
            if(message != null) return message;

            return new ReceiveMessage(Connection, line, sender, ReceiveType.Unknown, ReplyType.Unknown);
        }

        private ReceiveMessage ParseUndirectedMessage(Regex regex, ReceiveType type, IMessageTarget sender, String line,
            IMessageTarget receiver)
        {
            Match results = regex.Match(line);
            if(results.Success)
            {
                String message = String.Empty;
                if(results.Groups[1].Success)
                    message = results.Groups[1].Value;

                return new ReceiveMessage(Connection, message, sender, type, ReplyType.Unknown,
                    receiver);
            }
            return null;
        }

        private ReceiveMessage ParseDirectedMessage(Regex regex, ReceiveType type, IMessageTarget sender, String line)
        {
            Match results = regex.Match(line);
            if(results.Success && results.Groups[1].Success)
            {
                String receiverName = results.Groups[1].Value;
                IMessageTarget receiver = ParseReceiver(receiverName);
                String message = String.Empty;
                if(results.Groups[2].Success)
                    message = results.Groups[2].Value;

                return new ReceiveMessage(Connection, message, sender, type, ReplyType.Unknown, receiver);
            }
            return null;
        }

        private ReceiveMessage ParseKickMessage(IMessageTarget sender, String line)
        {
            Match results = KickRegex.Match(line);
            if(results.Success && results.Groups[1].Success && results.Groups[2].Success)
            {
                String channelName = results.Groups[1].Value;
                String userName = results.Groups[2].Value;
                String message = String.Empty;
                if(results.Groups[3].Success)
                    message = results.Groups[3].Value;
                IChannel channel = Connection.GetChannel(channelName);
                IChannelUser channelUser = channel.GetUser(userName);

                return new ReceiveMessage(Connection, message, sender, ReceiveType.Kick, ReplyType.Unknown, 
                    channelUser);
            }
            return null;
        }

        private ReceiveMessage ParseModeMessage(IMessageTarget sender, String line)
        {
            Match results = ModeRegex.Match(line);
            if(results.Success && results.Groups[1].Success)
            {
                String receiverName = results.Groups[1].Value;
                IMessageTarget receiver = ParseReceiver(receiverName);
                String message = String.Empty;
                if(results.Groups[2].Success)
                    message = results.Groups[2].Value;

                ReceiveType type = receiver.Type == MessageTargetType.Channel ? ReceiveType.ChannelModeChange : 
                    ReceiveType.UserModeChange;
                return new ReceiveMessage(Connection, message, sender, type, ReplyType.Unknown, receiver);
            }
            return null;
        }

        private ReceiveMessage ParseInviteMessage(IMessageTarget sender, String line)
        {
            Match results = InviteRegex.Match(line);
            if(results.Success && results.Groups[1].Success && results.Groups[2].Success)
            {
                String userName = results.Groups[1].Value;
                String channelName = results.Groups[2].Value;
                IChannel channel = Connection.GetChannel(channelName);

                return new ReceiveMessage(Connection, userName, sender, ReceiveType.Invite, ReplyType.Unknown, channel);
            }
            return null;
        }

        private IMessageTarget ParseReceiver(String name)
        {
            IMessageTarget receiver = null;
            switch(name[0])
            {
                case '#':
                case '!':
                case '&':
                case '+':
                    receiver = Connection.GetChannel(name);
                    break;
                default:
                    receiver = Connection.GetUser(name);
                    break;
            }
            return receiver;
        }
    }
}
