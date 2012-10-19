using System;
using System.Linq;
using System.Text;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    // http://tools.ietf.org/html/rfc2812
    public class MessageSender
    {
        public IClientConnection Connection { get; private set; }

        public MessageSender(IClientConnection connection)
        {
            Connection = connection;
        }

        public SendMessage Pass(String password)
        {
            return new SendMessage(Connection, "PASS " + password, SendType.Pass);
        }

        public SendMessage Nick(String nickname)
        {
            return new SendMessage(Connection, "NICK " + nickname, SendType.Nick);
        }

        public SendMessage User(String username, int usermode, String realname)
        {
            return new SendMessage(Connection, "USER " + username + " " + usermode.ToString() + " * :" + realname, 
                SendType.User);
        }

        public SendMessage Oper(String name, String password)
        {
            return new SendMessage(Connection, "OPER " + name + " " + password, SendType.Oper);
        }

        public SendMessage Privmsg(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "PRIVMSG " + receiver.Identity.Name + " :" + message, 
                SendType.Privmsg, receiver);
        }

        public SendMessage Notice(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "NOTICE " + receiver.Identity.Name + " :" + message, 
                SendType.Notice, receiver);
        }

        public SendMessage Join(IChannel channel)
        {
            return new SendMessage(Connection, "JOIN " + channel.Identity.Name, SendType.Join, channel);
        }

        public SendMessage Join(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "JOIN " + channellist, SendType.Join, channels);
        }

        public SendMessage Join(IChannel channel, String key)
        {
            return new SendMessage(Connection, "JOIN " + channel.Identity.Name + " " + key, SendType.Join, 
                channel);
        }

        public SendMessage Join(IChannel[] channels, String[] keys)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            String keylist = String.Join(",", keys);
            return new SendMessage(Connection, "JOIN " + channellist + " " + keylist, SendType.Join, channels);
        }

        public SendMessage Part(IChannel channel)
        {
            return new SendMessage(Connection, "PART " + channel.Identity.Name, SendType.Part, channel);
        }

        public SendMessage Part(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "PART " + channellist, SendType.Part, channels);
        }

        public SendMessage Part(IChannel channel, String partmessage)
        {
            return new SendMessage(Connection, "PART " + channel.Identity.Name + " :" + partmessage, 
                SendType.Part, channel);
        }

        public SendMessage Part(String partmessage, params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "PART " + channellist + " :" + partmessage, SendType.Part, 
                channels);
        }

        public SendMessage Kick(IChannelUser channelUser)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Identity.Name + " " +
                channelUser.User.Identity.Name, SendType.Kick, channelUser);
        }

        public SendMessage Kick(IChannelUser channelUser, String comment)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Identity.Name + " " 
                + channelUser.User.Identity.Name + " :" + comment,
                SendType.Kick, channelUser);
        }

        /*public SendMessage Kick(String[] channels, String nickname)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "KICK " + channellist + " " + nickname, SendMessageType.Kick);
        }

        public SendMessage Kick(String[] channels, String nickname, String comment)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "KICK " + channellist + " " + nickname + " :" + comment, 
                SendMessageType.Kick);
        }

        public SendMessage Kick(String channel, String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "KICK " + channel + " " + nicknamelist, SendMessageType.Kick);
        }

        public SendMessage Kick(String channel, String[] nicknames, String comment)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "KICK " + channel + " " + nicknamelist + " :" + comment, 
                SendMessageType.Kick);
        }*/

        public SendMessage Kick(params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Identity.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Identity.Name));
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist, SendType.Kick, 
                channelUsers);
        }

        public SendMessage Kick(String comment, params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Identity.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Identity.Name));
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist + " :" + comment,
                SendType.Kick, channelUsers);
        }

        public SendMessage Motd()
        {
            return new SendMessage(Connection, "MOTD", SendType.Motd);
        }

        public SendMessage Motd(String target)
        {
            return new SendMessage(Connection, "MOTD " + target, SendType.Motd);
        }

        public SendMessage Lusers()
        {
            return new SendMessage(Connection, "LUSERS", SendType.Lusers);
        }

        public SendMessage Lusers(String mask)
        {
            return new SendMessage(Connection, "LUSER " + mask, SendType.Lusers);
        }

        public SendMessage Lusers(String mask, String target)
        {
            return new SendMessage(Connection, "LUSER " + mask + " " + target, SendType.Lusers);
        }

        public SendMessage Version()
        {
            return new SendMessage(Connection, "VERSION", SendType.Version);
        }

        public SendMessage Version(String target)
        {
            return new SendMessage(Connection, "VERSION " + target, SendType.Version);
        }

        public SendMessage Stats()
        {
            return new SendMessage(Connection, "STATS", SendType.Stats);
        }

        public SendMessage Stats(String query)
        {
            return new SendMessage(Connection, "STATS " + query, SendType.Stats);
        }

        public SendMessage Stats(String query, String target)
        {
            return new SendMessage(Connection, "STATS " + query + " " + target, SendType.Stats);
        }

        public SendMessage Links()
        {
            return new SendMessage(Connection, "LINKS", SendType.Links);
        }

        public SendMessage Links(String servermask)
        {
            return new SendMessage(Connection, "LINKS " + servermask, SendType.Links);
        }

        public SendMessage Links(String remoteserver, String servermask)
        {
            return new SendMessage(Connection, "LINKS " + remoteserver + " " + servermask, SendType.Links);
        }

        public SendMessage Time()
        {
            return new SendMessage(Connection, "TIME", SendType.Time);
        }

        public SendMessage Time(String target)
        {
            return new SendMessage(Connection, "TIME " + target, SendType.Time);
        }

        public SendMessage Connect(String targetserver, String port)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port, SendType.Connect);
        }

        public SendMessage Connect(String targetserver, String port, String remoteserver)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port + " " + remoteserver, 
                SendType.Connect);
        }

        public SendMessage Trace()
        {
            return new SendMessage(Connection, "TRACE", SendType.Trace);
        }

        public SendMessage Trace(String target)
        {
            return new SendMessage(Connection, "TRACE " + target, SendType.Trace);
        }

        public SendMessage Admin()
        {
            return new SendMessage(Connection, "ADMIN", SendType.Admin);
        }

        public SendMessage Admin(String target)
        {
            return new SendMessage(Connection, "ADMIN " + target, SendType.Admin);
        }

        public SendMessage Info()
        {
            return new SendMessage(Connection, "INFO", SendType.Info);
        }

        public SendMessage Info(String target)
        {
            return new SendMessage(Connection, "INFO " + target, SendType.Info);
        }

        public SendMessage Servlist()
        {
            return new SendMessage(Connection, "SERVLIST", SendType.Servlist);
        }

        public SendMessage Servlist(String mask)
        {
            return new SendMessage(Connection, "SERVLIST " + mask, SendType.Servlist);
        }

        public SendMessage Servlist(String mask, String type)
        {
            return new SendMessage(Connection, "SERVLIST " + mask + " " + type, SendType.Servlist);
        }

        public SendMessage Squery(String servicename, String servicetext)
        {
            return new SendMessage(Connection, "SQUERY " + servicename + " :" + servicetext, SendType.Squery);
        }

        public SendMessage List()
        {
            return new SendMessage(Connection, "LIST", SendType.List);
        }

        public SendMessage List(IChannel channel)
        {
            return new SendMessage(Connection, "LIST " + channel.Identity.Name, SendType.List, channel);
        }

        public SendMessage List(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "LIST " + channellist, SendType.List, channels);
        }

        public SendMessage List(IChannel channel, String target)
        {
            return new SendMessage(Connection, "LIST " + channel.Identity.Name + " " + target, SendType.List, 
                channel);
        }

        public SendMessage List(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "LIST " + channellist + " " + target, SendType.List, channels);
        }

        public SendMessage Names()
        {
            return new SendMessage(Connection, "NAMES", SendType.Names);
        }

        public SendMessage Names(IChannel channel)
        {
            return new SendMessage(Connection, "NAMES " + channel.Identity.Name, SendType.Names, channel);
        }

        public SendMessage Names(IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "NAMES " + channellist, SendType.Names, channels);
        }

        public SendMessage Names(IChannel channel, String target)
        {
            return new SendMessage(Connection, "NAMES " + channel.Identity.Name + " " + target, SendType.Names, 
                channel);
        }

        public SendMessage Names(IChannel[] channels, String target)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "NAMES " + channellist + " " + target, SendType.Names, channels);
        }

        public SendMessage Topic(IChannel channel)
        {
            return new SendMessage(Connection, "TOPIC " + channel.Identity.Name, SendType.Topic, channel);
        }

        public SendMessage Topic(IChannel channel, String newtopic)
        {
            return new SendMessage(Connection, "TOPIC " + channel.Identity.Name + " :" + newtopic, 
                SendType.Topic, channel);
        }

        public SendMessage Mode(String target)
        {
            return new SendMessage(Connection, "MODE " + target, SendType.Mode);
        }

        public SendMessage Mode(String target, String newmode)
        {
            return new SendMessage(Connection, "MODE " + target + " " + newmode, SendType.Mode);
        }

        public SendMessage Mode(String target, String[] newModes, String[] newModeParameters)
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

        public SendMessage Service(String nickname, String distribution, String info)
        {
            return new SendMessage(Connection, "SERVICE " + nickname + " * " + distribution + " * * :" + info, 
                SendType.Service);
        }

        public SendMessage Invite(IUser user, IChannel channel)
        {
            return new SendMessage(Connection, "INVITE " + user.Identity.Name + " " + channel.Identity.Name, 
                SendType.Invite, user);
        }

        public SendMessage Who()
        {
            return new SendMessage(Connection, "WHO", SendType.Who);
        }

        public SendMessage Who(String mask)
        {
            return new SendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public SendMessage Who(String mask, bool ircop)
        {
            if(ircop)
                return new SendMessage(Connection, "WHO " + mask + " o", SendType.Who);
            else
                return new SendMessage(Connection, "WHO " + mask, SendType.Who);
        }

        public SendMessage Whois(String mask)
        {
            return new SendMessage(Connection, "WHOIS " + mask, SendType.Whois);
        }

        public SendMessage Whois(String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + masklist, SendType.Whois);
        }

        public SendMessage Whois(String target, String mask)
        {
            return new SendMessage(Connection, "WHOIS " + target + " " + mask, SendType.Whois);
        }

        public SendMessage Whois(String target, String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + target + " " + masklist, SendType.Whois);
        }

        public SendMessage Whowas(String nickname)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname, SendType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist, SendType.Whowas);
        }

        public SendMessage Whowas(String nickname, String count)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " ", SendType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames, String count)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " ", SendType.Whowas);
        }

        public SendMessage Whowas(String nickname, String count, String target)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " " + target, 
                SendType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames, String count, String target)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " " + target, 
                SendType.Whowas);
        }

        public SendMessage Kill(String nickname, String comment)
        {
            return new SendMessage(Connection, "KILL " + nickname + " :" + comment, SendType.Kill);
        }

        public SendMessage Ping(String server)
        {
            return new SendMessage(Connection, "PING " + server, SendType.Ping);
        }

        public SendMessage Ping(String server, String server2)
        {
            return new SendMessage(Connection, "PING " + server + " " + server2, SendType.Ping);
        }

        public SendMessage Pong(String server)
        {
            return new SendMessage(Connection, "PONG " + server, SendType.Pong);
        }

        public SendMessage Pong(String server, String server2)
        {
            return new SendMessage(Connection, "PONG " + server + " " + server2, SendType.Pong);
        }

        public SendMessage Error(String errormessage)
        {
            return new SendMessage(Connection, "ERROR :" + errormessage, SendType.Error);
        }

        public SendMessage Away()
        {
            return new SendMessage(Connection, "AWAY", SendType.Away);
        }

        public SendMessage Away(String awaytext)
        {
            return new SendMessage(Connection, "AWAY :" + awaytext, SendType.Away);
        }

        public SendMessage Rehash()
        {
            return new SendMessage(Connection, "REHASH", SendType.Rehash);
        }

        public SendMessage Die()
        {
            return new SendMessage(Connection, "DIE", SendType.Die);
        }

        public SendMessage Restart()
        {
            return new SendMessage(Connection, "RESTART", SendType.Restart);
        }

        public SendMessage Summon(String user)
        {
            return new SendMessage(Connection, "SUMMON " + user, SendType.Summon);
        }

        public SendMessage Summon(String user, String target)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target, SendType.Summon);
        }

        public SendMessage Summon(String user, String target, String channel)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target + " " + channel, 
                SendType.Summon);
        }

        public SendMessage Users()
        {
            return new SendMessage(Connection, "USERS", SendType.Users);
        }

        public SendMessage Users(String target)
        {
            return new SendMessage(Connection, "USERS " + target, SendType.Users);
        }

        public SendMessage Wallops(String wallopstext)
        {
            return new SendMessage(Connection, "WALLOPS :" + wallopstext, SendType.Wallops);
        }

        public SendMessage Userhost(String nickname)
        {
            return new SendMessage(Connection, "USERHOST " + nickname, SendType.Userhost);
        }

        public SendMessage Userhost(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "USERHOST " + nicknamelist, SendType.Userhost);
        }

        public SendMessage Ison(String nickname)
        {
            return new SendMessage(Connection, "ISON " + nickname, SendType.Ison);
        }

        public SendMessage Ison(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "ISON " + nicknamelist, SendType.Ison);
        }

        public SendMessage Quit()
        {
            return new SendMessage(Connection, "QUIT", SendType.Quit);
        }

        public SendMessage Quit(String quitmessage)
        {
            return new SendMessage(Connection, "QUIT :" + quitmessage, SendType.Quit);
        }

        public SendMessage Squit(String server, String comment)
        {
            return new SendMessage(Connection, "SQUIT " + server + " :" + comment, SendType.Squit);
        }
    }
}
