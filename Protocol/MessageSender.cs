using System;
using System.Linq;
using System.Text;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    // http://tools.ietf.org/html/rfc2812
    public class MessageSender : IMessageSender
    {
        public IClientConnection Connection { get; private set; }

        public MessageSender(IClientConnection connection)
        {
            Connection = connection;
        }

        public ISendMessage Pass(String password)
        {
            return new SendMessage(Connection, "PASS " + password, SendType.Pass);
        }

        public ISendMessage Nick(String nickname)
        {
            return new SendMessage(Connection, "NICK " + nickname, SendType.Nick);
        }

        public ISendMessage User(String username, int usermode, String realname)
        {
            return new SendMessage(Connection, "USER " + username + " " + usermode.ToString() + " * :" + realname, 
                SendType.User);
        }

        public ISendMessage Oper(String name, String password)
        {
            return new SendMessage(Connection, "OPER " + name + " " + password, SendType.Oper);
        }

        public ISendMessage Privmsg(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "PRIVMSG " + receiver.Name + " :" + message, 
                SendType.Privmsg, receiver);
        }

        public ISendMessage Notice(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "NOTICE " + receiver.Name + " :" + message, 
                SendType.Notice, receiver);
        }

        public ISendMessage Join(IChannel channel)
        {
            return new SendMessage(Connection, "JOIN " + channel.Name, SendType.Join, channel);
        }

        public ISendMessage Join(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "JOIN " + channellist, SendType.Join, channels);
        }

        public ISendMessage Join(IChannel channel, String key)
        {
            return new SendMessage(Connection, "JOIN " + channel.Name + " " + key, SendType.Join, 
                channel);
        }

        public ISendMessage Join(IChannel[] channels, String[] keys)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            String keylist = String.Join(",", keys);
            return new SendMessage(Connection, "JOIN " + channellist + " " + keylist, SendType.Join, channels);
        }

        public ISendMessage Part(IChannel channel)
        {
            return new SendMessage(Connection, "PART " + channel.Name, SendType.Part, channel);
        }

        public ISendMessage Part(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "PART " + channellist, SendType.Part, channels);
        }

        public ISendMessage Part(IChannel channel, String partmessage)
        {
            return new SendMessage(Connection, "PART " + channel.Name + " :" + partmessage, 
                SendType.Part, channel);
        }

        public ISendMessage Part(String partmessage, params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "PART " + channellist + " :" + partmessage, SendType.Part, 
                channels);
        }

        public ISendMessage Kick(IChannelUser channelUser)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Name + " " +
                channelUser.User.Name, SendType.Kick, channelUser);
        }

        public ISendMessage Kick(IChannelUser channelUser, String comment)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Name + " " 
                + channelUser.User.Name + " :" + comment,
                SendType.Kick, channelUser);
        }

        /*public ISendMessage Kick(String[] channels, String nickname)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "KICK " + channellist + " " + nickname, SendMessageType.Kick);
        }

        public ISendMessage Kick(String[] channels, String nickname, String comment)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "KICK " + channellist + " " + nickname + " :" + comment, 
                SendMessageType.Kick);
        }

        public ISendMessage Kick(String channel, String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "KICK " + channel + " " + nicknamelist, SendMessageType.Kick);
        }

        public ISendMessage Kick(String channel, String[] nicknames, String comment)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "KICK " + channel + " " + nicknamelist + " :" + comment, 
                SendMessageType.Kick);
        }*/

        public ISendMessage Kick(params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Name));
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist, SendType.Kick, 
                channelUsers);
        }

        public ISendMessage Kick(String comment, params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Name));
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist + " :" + comment,
                SendType.Kick, channelUsers);
        }

        public ISendMessage Motd()
        {
            return new SendMessage(Connection, "MOTD", SendType.Motd);
        }

        public ISendMessage Motd(String target)
        {
            return new SendMessage(Connection, "MOTD " + target, SendType.Motd);
        }

        public ISendMessage Lusers()
        {
            return new SendMessage(Connection, "LUSERS", SendType.Lusers);
        }

        public ISendMessage Lusers(String mask)
        {
            return new SendMessage(Connection, "LUSER " + mask, SendType.Lusers);
        }

        public ISendMessage Lusers(String mask, String target)
        {
            return new SendMessage(Connection, "LUSER " + mask + " " + target, SendType.Lusers);
        }

        public ISendMessage Version()
        {
            return new SendMessage(Connection, "VERSION", SendType.Version);
        }

        public ISendMessage Version(String target)
        {
            return new SendMessage(Connection, "VERSION " + target, SendType.Version);
        }

        public ISendMessage Stats()
        {
            return new SendMessage(Connection, "STATS", SendType.Stats);
        }

        public ISendMessage Stats(String query)
        {
            return new SendMessage(Connection, "STATS " + query, SendType.Stats);
        }

        public ISendMessage Stats(String query, String target)
        {
            return new SendMessage(Connection, "STATS " + query + " " + target, SendType.Stats);
        }

        public ISendMessage Links()
        {
            return new SendMessage(Connection, "LINKS", SendType.Links);
        }

        public ISendMessage Links(String servermask)
        {
            return new SendMessage(Connection, "LINKS " + servermask, SendType.Links);
        }

        public ISendMessage Links(String remoteserver, String servermask)
        {
            return new SendMessage(Connection, "LINKS " + remoteserver + " " + servermask, SendType.Links);
        }

        public ISendMessage Time()
        {
            return new SendMessage(Connection, "TIME", SendType.Time);
        }

        public ISendMessage Time(String target)
        {
            return new SendMessage(Connection, "TIME " + target, SendType.Time);
        }

        public ISendMessage Connect(String targetserver, String port)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port, SendType.Connect);
        }

        public ISendMessage Connect(String targetserver, String port, String remoteserver)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port + " " + remoteserver, 
                SendType.Connect);
        }

        public ISendMessage Trace()
        {
            return new SendMessage(Connection, "TRACE", SendType.Trace);
        }

        public ISendMessage Trace(String target)
        {
            return new SendMessage(Connection, "TRACE " + target, SendType.Trace);
        }

        public ISendMessage Admin()
        {
            return new SendMessage(Connection, "ADMIN", SendType.Admin);
        }

        public ISendMessage Admin(String target)
        {
            return new SendMessage(Connection, "ADMIN " + target, SendType.Admin);
        }

        public ISendMessage Info()
        {
            return new SendMessage(Connection, "INFO", SendType.Info);
        }

        public ISendMessage Info(String target)
        {
            return new SendMessage(Connection, "INFO " + target, SendType.Info);
        }

        public ISendMessage Servlist()
        {
            return new SendMessage(Connection, "SERVLIST", SendType.Servlist);
        }

        public ISendMessage Servlist(String mask)
        {
            return new SendMessage(Connection, "SERVLIST " + mask, SendType.Servlist);
        }

        public ISendMessage Servlist(String mask, String type)
        {
            return new SendMessage(Connection, "SERVLIST " + mask + " " + type, SendType.Servlist);
        }

        public ISendMessage Squery(String servicename, String servicetext)
        {
            return new SendMessage(Connection, "SQUERY " + servicename + " :" + servicetext, SendType.Squery);
        }

        public ISendMessage List()
        {
            return new SendMessage(Connection, "LIST", SendType.List);
        }

        public ISendMessage List(IChannel channel)
        {
            return new SendMessage(Connection, "LIST " + channel.Name, SendType.List, channel);
        }

        public ISendMessage List(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "LIST " + channellist, SendType.List, channels);
        }

        public ISendMessage List(IChannel channel, String target)
        {
            return new SendMessage(Connection, "LIST " + channel.Name + " " + target, SendType.List, 
                channel);
        }

        public ISendMessage List(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "LIST " + channellist + " " + target, SendType.List, channels);
        }

        public ISendMessage Names()
        {
            return new SendMessage(Connection, "NAMES", SendType.Names);
        }

        public ISendMessage Names(IChannel channel)
        {
            return new SendMessage(Connection, "NAMES " + channel.Name, SendType.Names, channel);
        }

        public ISendMessage Names(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "NAMES " + channellist, SendType.Names, channels);
        }

        public ISendMessage Names(IChannel channel, String target)
        {
            return new SendMessage(Connection, "NAMES " + channel.Name + " " + target, SendType.Names, 
                channel);
        }

        public ISendMessage Names(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Name));
            return new SendMessage(Connection, "NAMES " + channellist + " " + target, SendType.Names, channels);
        }

        public ISendMessage Topic(IChannel channel)
        {
            return new SendMessage(Connection, "TOPIC " + channel.Name, SendType.Topic, channel);
        }

        public ISendMessage Topic(IChannel channel, String newtopic)
        {
            return new SendMessage(Connection, "TOPIC " + channel.Name + " :" + newtopic, 
                SendType.Topic, channel);
        }

        public ISendMessage Mode(String target)
        {
            return new SendMessage(Connection, "MODE " + target, SendType.Mode);
        }

        public ISendMessage Mode(String target, String newmode)
        {
            return new SendMessage(Connection, "MODE " + target + " " + newmode, SendType.Mode);
        }

        public ISendMessage Mode(String target, String[] newModes, String[] newModeParameters)
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
            return new SendMessage(Connection, "SERVICE " + nickname + " * " + distribution + " * * :" + info, 
                SendType.Service);
        }

        public ISendMessage Invite(IUser user, IChannel channel)
        {
            return new SendMessage(Connection, "INVITE " + user.Name + " " + channel.Name, 
                SendType.Invite, user);
        }

        public ISendMessage Who()
        {
            return new SendMessage(Connection, "WHO", SendType.Who);
        }

        public ISendMessage Who(String mask)
        {
            return new SendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public ISendMessage Who(String mask, bool ircop)
        {
            if(ircop)
                return new SendMessage(Connection, "WHO " + mask + " o", SendType.Who);
            else
                return new SendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public ISendMessage Whois(String mask)
        {
            return new SendMessage(Connection, "WHOIS " + mask, SendType.Whois);
        }

        public ISendMessage Whois(String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + masklist, SendType.Whois);
        }

        public ISendMessage Whois(String target, String mask)
        {
            return new SendMessage(Connection, "WHOIS " + target + " " + mask, SendType.Whois);
        }

        public ISendMessage Whois(String target, String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + target + " " + masklist, SendType.Whois);
        }

        public ISendMessage Whowas(String nickname)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname, SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist, SendType.Whowas);
        }

        public ISendMessage Whowas(String nickname, String count)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " ", SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames, String count)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " ", SendType.Whowas);
        }

        public ISendMessage Whowas(String nickname, String count, String target)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " " + target, 
                SendType.Whowas);
        }

        public ISendMessage Whowas(String[] nicknames, String count, String target)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " " + target, 
                SendType.Whowas);
        }

        public ISendMessage Kill(String nickname, String comment)
        {
            return new SendMessage(Connection, "KILL " + nickname + " :" + comment, SendType.Kill);
        }

        public ISendMessage Ping(String server)
        {
            return new SendMessage(Connection, "PING " + server, SendType.Ping);
        }

        public ISendMessage Ping(String server, String server2)
        {
            return new SendMessage(Connection, "PING " + server + " " + server2, SendType.Ping);
        }

        public ISendMessage Pong(String server)
        {
            return new SendMessage(Connection, "PONG " + server, SendType.Pong);
        }

        public ISendMessage Pong(String server, String server2)
        {
            return new SendMessage(Connection, "PONG " + server + " " + server2, SendType.Pong);
        }

        public ISendMessage Error(String errormessage)
        {
            return new SendMessage(Connection, "ERROR :" + errormessage, SendType.Error);
        }

        public ISendMessage Away()
        {
            return new SendMessage(Connection, "AWAY", SendType.Away);
        }

        public ISendMessage Away(String awaytext)
        {
            return new SendMessage(Connection, "AWAY :" + awaytext, SendType.Away);
        }

        public ISendMessage Rehash()
        {
            return new SendMessage(Connection, "REHASH", SendType.Rehash);
        }

        public ISendMessage Die()
        {
            return new SendMessage(Connection, "DIE", SendType.Die);
        }

        public ISendMessage Restart()
        {
            return new SendMessage(Connection, "RESTART", SendType.Restart);
        }

        public ISendMessage Summon(String user)
        {
            return new SendMessage(Connection, "SUMMON " + user, SendType.Summon);
        }

        public ISendMessage Summon(String user, String target)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target, SendType.Summon);
        }

        public ISendMessage Summon(String user, String target, String channel)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target + " " + channel, 
                SendType.Summon);
        }

        public ISendMessage Users()
        {
            return new SendMessage(Connection, "USERS", SendType.Users);
        }

        public ISendMessage Users(String target)
        {
            return new SendMessage(Connection, "USERS " + target, SendType.Users);
        }

        public ISendMessage Wallops(String wallopstext)
        {
            return new SendMessage(Connection, "WALLOPS :" + wallopstext, SendType.Wallops);
        }

        public ISendMessage Userhost(String nickname)
        {
            return new SendMessage(Connection, "USERHOST " + nickname, SendType.Userhost);
        }

        public ISendMessage Userhost(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "USERHOST " + nicknamelist, SendType.Userhost);
        }

        public ISendMessage Ison(String nickname)
        {
            return new SendMessage(Connection, "ISON " + nickname, SendType.Ison);
        }

        public ISendMessage Ison(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "ISON " + nicknamelist, SendType.Ison);
        }

        public ISendMessage Quit()
        {
            return new SendMessage(Connection, "QUIT", SendType.Quit);
        }

        public ISendMessage Quit(String quitmessage)
        {
            return new SendMessage(Connection, "QUIT :" + quitmessage, SendType.Quit);
        }

        public ISendMessage Squit(String server, String comment)
        {
            return new SendMessage(Connection, "SQUIT " + server + " :" + comment, SendType.Squit);
        }
    }
}
