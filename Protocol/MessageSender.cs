using System;
using System.Linq;
using System.Text;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    // http://tools.ietf.org/html/rfc2812
    public class MessageSender : IMessageSender
    {
        public IClient Client { get; private set; }
        public IClientConnection Connection { get; private set; }

        public MessageSender(IClientConnection connection)
        {
            Connection = connection;
            Client = connection.Client;
        }

        public ISendMessage Pass(String password)
        {
            return Client.CreateSendMessage(Connection, "PASS " + password, SendType.Pass);
        }

        public ISendMessage Nick(String nickname)
        {
            return Client.CreateSendMessage(Connection, "NICK " + nickname, SendType.Nick);
        }

        public ISendMessage User(String username, int usermode, String realname)
        {
            return Client.CreateSendMessage(Connection, "USER " + username + " " + usermode.ToString() + " * :" + realname, 
                SendType.User);
        }

        public ISendMessage Oper(String name, String password)
        {
            return Client.CreateSendMessage(Connection, "OPER " + name + " " + password, SendType.Oper);
        }

        public ISendMessage Message(IMessageTarget receiver, String message)
        {
            return Client.CreateSendMessage(Connection, "PRIVMSG " + receiver.Name + " :" + message, 
                SendType.Privmsg, receiver);
        }

        public ISendMessage Action(IMessageTarget receiver, String action)
        {
            return Message(receiver, '\x001' + "ACTION " + action + '\x001');
        }

        public ISendMessage Notice(IMessageTarget receiver, String message)
        {
            return Client.CreateSendMessage(Connection, "NOTICE " + receiver.Name + " :" + message, 
                SendType.Notice, receiver);
        }

        public ISendMessage Join(IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "JOIN " + channel.Name, SendType.Join, channel);
        }

        public ISendMessage Join(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "JOIN " + channellist, SendType.Join, channels);
        }

        public ISendMessage Join(IChannel channel, String key)
        {
            return Client.CreateSendMessage(Connection, "JOIN " + channel.Name + " " + key, SendType.Join, 
                channel);
        }

        public ISendMessage Join(IChannel[] channels, String[] keys)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            String keylist = String.Join(",", keys);
            return Client.CreateSendMessage(Connection, "JOIN " + channellist + " " + keylist, SendType.Join, channels);
        }

        public ISendMessage Part(IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "PART " + channel.Name, SendType.Part, channel);
        }

        public ISendMessage Part(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "PART " + channellist, SendType.Part, channels);
        }

        public ISendMessage Part(IChannel channel, String partmessage)
        {
            return Client.CreateSendMessage(Connection, "PART " + channel.Name + " :" + partmessage, 
                SendType.Part, channel);
        }

        public ISendMessage Part(String partmessage, params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "PART " + channellist + " :" + partmessage, SendType.Part, 
                channels);
        }

        public ISendMessage Kick(IChannelUser channelUser)
        {
            return Client.CreateSendMessage(Connection, "KICK " + channelUser.Channel.Name + " " +
                channelUser.User.Name, SendType.Kick, channelUser);
        }

        public ISendMessage Kick(IChannelUser channelUser, String comment)
        {
            return Client.CreateSendMessage(Connection, "KICK " + channelUser.Channel.Name + " " 
                + channelUser.User.Name + " :" + comment,
                SendType.Kick, channelUser);
        }

        /*public ISendMessage Kick(String[] channels, String nickname)
        {
            String channellist = String.Join(",", channels);
            return Client.CreateSendMessage(Connection, "KICK " + channellist + " " + nickname, SendMessageType.Kick);
        }

        public ISendMessage Kick(String[] channels, String nickname, String comment)
        {
            String channellist = String.Join(",", channels);
            return Client.CreateSendMessage(Connection, "KICK " + channellist + " " + nickname + " :" + comment, 
                SendMessageType.Kick);
        }

        public ISendMessage Kick(String channel, String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return Client.CreateSendMessage(Connection, "KICK " + channel + " " + nicknamelist, SendMessageType.Kick);
        }

        public ISendMessage Kick(String channel, String[] nicknames, String comment)
        {
            String nicknamelist = String.Join(",", nicknames);
            return Client.CreateSendMessage(Connection, "KICK " + channel + " " + nicknamelist + " :" + comment, 
                SendMessageType.Kick);
        }*/

        public ISendMessage Kick(params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Name));
            return Client.CreateSendMessage(Connection, "KICK " + channellist + " " + nicknamelist, SendType.Kick, 
                channelUsers);
        }

        public ISendMessage Kick(String comment, params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Name));
            return Client.CreateSendMessage(Connection, "KICK " + channellist + " " + nicknamelist + " :" + comment,
                SendType.Kick, channelUsers);
        }

        public ISendMessage Motd()
        {
            return Client.CreateSendMessage(Connection, "MOTD", SendType.Motd);
        }

        public ISendMessage Motd(String target)
        {
            return Client.CreateSendMessage(Connection, "MOTD " + target, SendType.Motd);
        }

        public ISendMessage Lusers()
        {
            return Client.CreateSendMessage(Connection, "LUSERS", SendType.Lusers);
        }

        public ISendMessage Lusers(String mask)
        {
            return Client.CreateSendMessage(Connection, "LUSER " + mask, SendType.Lusers);
        }

        public ISendMessage Lusers(String mask, String target)
        {
            return Client.CreateSendMessage(Connection, "LUSER " + mask + " " + target, SendType.Lusers);
        }

        public ISendMessage Version()
        {
            return Client.CreateSendMessage(Connection, "VERSION", SendType.Version);
        }

        public ISendMessage Version(String target)
        {
            return Client.CreateSendMessage(Connection, "VERSION " + target, SendType.Version);
        }

        public ISendMessage Stats()
        {
            return Client.CreateSendMessage(Connection, "STATS", SendType.Stats);
        }

        public ISendMessage Stats(String query)
        {
            return Client.CreateSendMessage(Connection, "STATS " + query, SendType.Stats);
        }

        public ISendMessage Stats(String query, String target)
        {
            return Client.CreateSendMessage(Connection, "STATS " + query + " " + target, SendType.Stats);
        }

        public ISendMessage Links()
        {
            return Client.CreateSendMessage(Connection, "LINKS", SendType.Links);
        }

        public ISendMessage Links(String servermask)
        {
            return Client.CreateSendMessage(Connection, "LINKS " + servermask, SendType.Links);
        }

        public ISendMessage Links(String remoteserver, String servermask)
        {
            return Client.CreateSendMessage(Connection, "LINKS " + remoteserver + " " + servermask, SendType.Links);
        }

        public ISendMessage Time()
        {
            return Client.CreateSendMessage(Connection, "TIME", SendType.Time);
        }

        public ISendMessage Time(String target)
        {
            return Client.CreateSendMessage(Connection, "TIME " + target, SendType.Time);
        }

        public ISendMessage Connect(String targetserver, String port)
        {
            return Client.CreateSendMessage(Connection, "CONNECT " + targetserver + " " + port, SendType.Connect);
        }

        public ISendMessage Connect(String targetserver, String port, String remoteserver)
        {
            return Client.CreateSendMessage(Connection, "CONNECT " + targetserver + " " + port + " " + remoteserver, 
                SendType.Connect);
        }

        public ISendMessage Trace()
        {
            return Client.CreateSendMessage(Connection, "TRACE", SendType.Trace);
        }

        public ISendMessage Trace(String target)
        {
            return Client.CreateSendMessage(Connection, "TRACE " + target, SendType.Trace);
        }

        public ISendMessage Admin()
        {
            return Client.CreateSendMessage(Connection, "ADMIN", SendType.Admin);
        }

        public ISendMessage Admin(String target)
        {
            return Client.CreateSendMessage(Connection, "ADMIN " + target, SendType.Admin);
        }

        public ISendMessage Info()
        {
            return Client.CreateSendMessage(Connection, "INFO", SendType.Info);
        }

        public ISendMessage Info(String target)
        {
            return Client.CreateSendMessage(Connection, "INFO " + target, SendType.Info);
        }

        public ISendMessage Servlist()
        {
            return Client.CreateSendMessage(Connection, "SERVLIST", SendType.Servlist);
        }

        public ISendMessage Servlist(String mask)
        {
            return Client.CreateSendMessage(Connection, "SERVLIST " + mask, SendType.Servlist);
        }

        public ISendMessage Servlist(String mask, String type)
        {
            return Client.CreateSendMessage(Connection, "SERVLIST " + mask + " " + type, SendType.Servlist);
        }

        public ISendMessage Squery(String servicename, String servicetext)
        {
            return Client.CreateSendMessage(Connection, "SQUERY " + servicename + " :" + servicetext, SendType.Squery);
        }

        public ISendMessage List()
        {
            return Client.CreateSendMessage(Connection, "LIST", SendType.List);
        }

        public ISendMessage List(IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "LIST " + channel.Name, SendType.List, channel);
        }

        public ISendMessage List(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "LIST " + channellist, SendType.List, channels);
        }

        public ISendMessage List(IChannel channel, String target)
        {
            return Client.CreateSendMessage(Connection, "LIST " + channel.Name + " " + target, SendType.List, 
                channel);
        }

        public ISendMessage List(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "LIST " + channellist + " " + target, SendType.List, channels);
        }

        public ISendMessage Names()
        {
            return Client.CreateSendMessage(Connection, "NAMES", SendType.Names);
        }

        public ISendMessage Names(IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "NAMES " + channel.Name, SendType.Names, channel);
        }

        public ISendMessage Names(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "NAMES " + channellist, SendType.Names, channels);
        }

        public ISendMessage Names(IChannel channel, String target)
        {
            return Client.CreateSendMessage(Connection, "NAMES " + channel.Name + " " + target, SendType.Names, 
                channel);
        }

        public ISendMessage Names(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return Client.CreateSendMessage(Connection, "NAMES " + channellist + " " + target, SendType.Names, channels);
        }

        public ISendMessage Topic(IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "TOPIC " + channel.Name, SendType.Topic, channel);
        }

        public ISendMessage Topic(IChannel channel, String newtopic)
        {
            return Client.CreateSendMessage(Connection, "TOPIC " + channel.Name + " :" + newtopic, 
                SendType.Topic, channel);
        }

        public ISendMessage Mode(IMessageTarget target)
        {
            return Client.CreateSendMessage(Connection, "MODE " + target.Name, SendType.Mode, target);
        }

        public ISendMessage Mode(IMessageTarget target, String newmode)
        {
            return Client.CreateSendMessage(Connection, "MODE " + target.Name + " " + newmode, SendType.Mode, target);
        }

        public ISendMessage Mode(IMessageTarget target, String newmode, String newModeParameter)
        {
            return Client.CreateSendMessage(Connection, "MODE " + target.Name + " " + newmode + " " + newModeParameter, 
                SendType.Mode, target);
        }

        public ISendMessage Mode(IMessageTarget target, String[] newModes, String[] newModeParameters)
        {
            if(newModes == null)
            {
                throw new ArgumentNullException("newModes");
            }
            if(newModeParameters == null)
            {
                throw new ArgumentNullException("newModeParameters");
            }
            if(newModes.Length != newModeParameters.Length)
            {
                throw new ArgumentException("newModes and newModeParameters must have the same size.");
            }

            StringBuilder newMode = new StringBuilder(newModes.Length);
            StringBuilder newModeParameter = new StringBuilder();
            // as per RFC 3.2.3, maximum is 3 modes changes at once
            int maxModeChanges = 3;
            if(newModes.Length > maxModeChanges)
            {
                throw new ArgumentOutOfRangeException(
                    "newModes.Length",
                    newModes.Length,
                    String.Format("Mode change list is too large (> {0}).", maxModeChanges)
                );
            }

            for(int i = 0; i <= newModes.Length; i += maxModeChanges)
            {
                for(int j = 0; j < maxModeChanges; j++)
                {
                    if(i + j >= newModes.Length)
                    {
                        break;
                    }
                    newMode.Append(newModes[i + j]);
                }

                for(int j = 0; j < maxModeChanges; j++)
                {
                    if(i + j >= newModeParameters.Length)
                    {
                        break;
                    }
                    newModeParameter.Append(newModeParameters[i + j]);
                    newModeParameter.Append(" ");
                }
            }
            if(newModeParameter.Length > 0)
            {
                // remove trailing space
                newModeParameter.Length--;
                newMode.Append(" ");
                newMode.Append(newModeParameter.ToString());
            }

            return Mode(target, newMode.ToString());
        }

        public ISendMessage Service(String nickname, String distribution, String info)
        {
            return Client.CreateSendMessage(Connection, "SERVICE " + nickname + " * " + distribution + " * * :" + info, 
                SendType.Service);
        }

        public ISendMessage Invite(IUser user, IChannel channel)
        {
            return Client.CreateSendMessage(Connection, "INVITE " + user.Name + " " + channel.Name, 
                SendType.Invite, user);
        }

        public ISendMessage Who()
        {
            return Client.CreateSendMessage(Connection, "WHO", SendType.Who);
        }

        public ISendMessage Who(String mask)
        {
            return Client.CreateSendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public ISendMessage Who(String mask, bool ircop)
        {
            if(ircop)
                return Client.CreateSendMessage(Connection, "WHO " + mask + " o", SendType.Who);
            else
                return Client.CreateSendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public ISendMessage Whois(String mask)
        {
            return Client.CreateSendMessage(Connection, "WHOIS " + mask, SendType.Whois);
        }

        public ISendMessage Whois(String[] masks)
        {
            String masklist = String.Join(",", masks);
            return Client.CreateSendMessage(Connection, "WHOIS " + masklist, SendType.Whois);
        }

        public ISendMessage Whois(String target, String mask)
        {
            return Client.CreateSendMessage(Connection, "WHOIS " + target + " " + mask, SendType.Whois);
        }

        public ISendMessage Whois(String target, String[] masks)
        {
            String masklist = String.Join(",", masks);
            return Client.CreateSendMessage(Connection, "WHOIS " + target + " " + masklist, SendType.Whois);
        }

        public ISendMessage Whowas(String nickname)
        {
            return Client.CreateSendMessage(Connection, "WHOWAS " + nickname, SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return Client.CreateSendMessage(Connection, "WHOWAS " + nicknamelist, SendType.Whowas);
        }

        public ISendMessage Whowas(String nickname, String count)
        {
            return Client.CreateSendMessage(Connection, "WHOWAS " + nickname + " " + count + " ", SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames, String count)
        {
            String nicknamelist = String.Join(",", nicknames);
            return Client.CreateSendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " ", SendType.Whowas);
        }

        public ISendMessage Whowas(String nickname, String count, String target)
        {
            return Client.CreateSendMessage(Connection, "WHOWAS " + nickname + " " + count + " " + target, 
                SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames, String count, String target)
        {
            String nicknamelist = String.Join(",", nicknames);
            return Client.CreateSendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " " + target, 
                SendType.Whowas);
        }

        public ISendMessage Kill(String nickname, String comment)
        {
            return Client.CreateSendMessage(Connection, "KILL " + nickname + " :" + comment, SendType.Kill);
        }

        public ISendMessage Ping(String server)
        {
            return Client.CreateSendMessage(Connection, "PING " + server, SendType.Ping);
        }

        public ISendMessage Ping(String server, String server2)
        {
            return Client.CreateSendMessage(Connection, "PING " + server + " " + server2, SendType.Ping);
        }

        public ISendMessage Pong(String server)
        {
            return Client.CreateSendMessage(Connection, "PONG " + server, SendType.Pong);
        }

        public ISendMessage Pong(String server, String server2)
        {
            return Client.CreateSendMessage(Connection, "PONG " + server + " " + server2, SendType.Pong);
        }

        public ISendMessage Error(String errormessage)
        {
            return Client.CreateSendMessage(Connection, "ERROR :" + errormessage, SendType.Error);
        }

        public ISendMessage Away()
        {
            return Client.CreateSendMessage(Connection, "AWAY", SendType.Away);
        }

        public ISendMessage Away(String awaytext)
        {
            return Client.CreateSendMessage(Connection, "AWAY :" + awaytext, SendType.Away);
        }

        public ISendMessage Rehash()
        {
            return Client.CreateSendMessage(Connection, "REHASH", SendType.Rehash);
        }

        public ISendMessage Die()
        {
            return Client.CreateSendMessage(Connection, "DIE", SendType.Die);
        }

        public ISendMessage Restart()
        {
            return Client.CreateSendMessage(Connection, "RESTART", SendType.Restart);
        }

        public ISendMessage Summon(String user)
        {
            return Client.CreateSendMessage(Connection, "SUMMON " + user, SendType.Summon);
        }

        public ISendMessage Summon(String user, String target)
        {
            return Client.CreateSendMessage(Connection, "SUMMON " + user + " " + target, SendType.Summon);
        }

        public ISendMessage Summon(String user, String target, String channel)
        {
            return Client.CreateSendMessage(Connection, "SUMMON " + user + " " + target + " " + channel, 
                SendType.Summon);
        }

        public ISendMessage Users()
        {
            return Client.CreateSendMessage(Connection, "USERS", SendType.Users);
        }

        public ISendMessage Users(String target)
        {
            return Client.CreateSendMessage(Connection, "USERS " + target, SendType.Users);
        }

        public ISendMessage Wallops(String wallopstext)
        {
            return Client.CreateSendMessage(Connection, "WALLOPS :" + wallopstext, SendType.Wallops);
        }

        public ISendMessage Userhost(String nickname)
        {
            return Client.CreateSendMessage(Connection, "USERHOST " + nickname, SendType.Userhost);
        }

        public ISendMessage Userhost(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return Client.CreateSendMessage(Connection, "USERHOST " + nicknamelist, SendType.Userhost);
        }

        public ISendMessage Ison(String nickname)
        {
            return Client.CreateSendMessage(Connection, "ISON " + nickname, SendType.Ison);
        }

        public ISendMessage Ison(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return Client.CreateSendMessage(Connection, "ISON " + nicknamelist, SendType.Ison);
        }

        public ISendMessage Quit()
        {
            return Client.CreateSendMessage(Connection, "QUIT", SendType.Quit);
        }

        public ISendMessage Quit(String quitmessage)
        {
            return Client.CreateSendMessage(Connection, "QUIT :" + quitmessage, SendType.Quit);
        }

        public ISendMessage Squit(String server, String comment)
        {
            return Client.CreateSendMessage(Connection, "SQUIT " + server + " :" + comment, SendType.Squit);
        }
    }
}
