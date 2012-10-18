using System;
using System.Text;
using ReactiveIRC.Interface;
using System.Linq;

namespace ReactiveIRC.Protocol
{
    public class MessageSender
    {
        public IConnection Connection { get; private set; }

        public SendMessage Pass(String password)
        {
            return new SendMessage(Connection, "PASS " + password, SendMessageType.Pass);
        }

        public SendMessage Nick(String nickname)
        {
            return new SendMessage(Connection, "NICK " + nickname, SendMessageType.Nick);
        }

        public SendMessage User(String username, int usermode, String realname)
        {
            return new SendMessage(Connection, "USER " + username + " " + usermode.ToString() + " * :" + realname, 
                SendMessageType.User);
        }

        public SendMessage Oper(String name, String password)
        {
            return new SendMessage(Connection, "OPER " + name + " " + password, SendMessageType.Oper);
        }

        public SendMessage Privmsg(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "PRIVMSG " + receiver.Identity.Name + " :" + message, 
                SendMessageType.Privmsg, receiver);
        }

        public SendMessage Notice(IMessageTarget receiver, String message)
        {
            return new SendMessage(Connection, "NOTICE " + receiver.Identity.Name + " :" + message, 
                SendMessageType.Notice, receiver);
        }

        public SendMessage Join(IChannel channel)
        {
            return new SendMessage(Connection, "JOIN " + channel.Identity.Name, SendMessageType.Join, channel);
        }

        public SendMessage Join(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "JOIN " + channellist, SendMessageType.Join, channels);
        }

        public SendMessage Join(IChannel channel, String key)
        {
            return new SendMessage(Connection, "JOIN " + channel.Identity.Name + " " + key, SendMessageType.Join, 
                channel);
        }

        public SendMessage Join(IChannel[] channels, String[] keys)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            String keylist = String.Join(",", keys);
            return new SendMessage(Connection, "JOIN " + channellist + " " + keylist, SendMessageType.Join, channels);
        }

        public SendMessage Part(IChannel channel)
        {
            return new SendMessage(Connection, "PART " + channel.Identity.Name, SendMessageType.Part, channel);
        }

        public SendMessage Part(params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "PART " + channellist, SendMessageType.Part, channels);
        }

        public SendMessage Part(IChannel channel, String partmessage)
        {
            return new SendMessage(Connection, "PART " + channel.Identity.Name + " :" + partmessage, 
                SendMessageType.Part, channel);
        }

        public SendMessage Part(String partmessage, params IChannel[] channels)
        {
            String channellist = String.Join(",", channels.Select(c => c.Identity.Name));
            return new SendMessage(Connection, "PART " + channellist + " :" + partmessage, SendMessageType.Part, 
                channels);
        }

        public SendMessage Kick(IChannelUser channelUser)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Identity.Name + " " +
                channelUser.User.Identity.Name, SendMessageType.Kick, channelUser);
        }

        public SendMessage Kick(IChannelUser channelUser, String comment)
        {
            return new SendMessage(Connection, "KICK " + channelUser.Channel.Identity.Name + " " 
                + channelUser.User.Identity.Name + " :" + comment,
                SendMessageType.Kick, channelUser);
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
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist, SendMessageType.Kick, 
                channelUsers);
        }

        public SendMessage Kick(String comment, params IChannelUser[] channelUsers)
        {
            String channellist = String.Join(",", channelUsers.Select(c => c.Channel.Identity.Name));
            String nicknamelist = String.Join(",", channelUsers.Select(c => c.User.Identity.Name));
            return new SendMessage(Connection, "KICK " + channellist + " " + nicknamelist + " :" + comment,
                SendMessageType.Kick, channelUsers);
        }

        public SendMessage Motd()
        {
            return new SendMessage(Connection, "MOTD", SendMessageType.Motd);
        }

        public SendMessage Motd(String target)
        {
            return new SendMessage(Connection, "MOTD " + target, SendMessageType.Motd);
        }

        public SendMessage Lusers()
        {
            return new SendMessage(Connection, "LUSERS", SendMessageType.Lusers);
        }

        public SendMessage Lusers(String mask)
        {
            return new SendMessage(Connection, "LUSER " + mask, SendMessageType.Lusers);
        }

        public SendMessage Lusers(String mask, String target)
        {
            return new SendMessage(Connection, "LUSER " + mask + " " + target, SendMessageType.Lusers);
        }

        public SendMessage Version()
        {
            return new SendMessage(Connection, "VERSION", SendMessageType.Version);
        }

        public SendMessage Version(String target)
        {
            return new SendMessage(Connection, "VERSION " + target, SendMessageType.Version);
        }

        public SendMessage Stats()
        {
            return new SendMessage(Connection, "STATS", SendMessageType.Stats);
        }

        public SendMessage Stats(String query)
        {
            return new SendMessage(Connection, "STATS " + query, SendMessageType.Stats);
        }

        public SendMessage Stats(String query, String target)
        {
            return new SendMessage(Connection, "STATS " + query + " " + target, SendMessageType.Stats);
        }

        public SendMessage Links()
        {
            return new SendMessage(Connection, "LINKS", SendMessageType.Links);
        }

        public SendMessage Links(String servermask)
        {
            return new SendMessage(Connection, "LINKS " + servermask, SendMessageType.Links);
        }

        public SendMessage Links(String remoteserver, String servermask)
        {
            return new SendMessage(Connection, "LINKS " + remoteserver + " " + servermask, SendMessageType.Links);
        }

        public SendMessage Time()
        {
            return new SendMessage(Connection, "TIME", SendMessageType.Time);
        }

        public SendMessage Time(String target)
        {
            return new SendMessage(Connection, "TIME " + target, SendMessageType.Time);
        }

        public SendMessage Connect(String targetserver, String port)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port, SendMessageType.Connect);
        }

        public SendMessage Connect(String targetserver, String port, String remoteserver)
        {
            return new SendMessage(Connection, "CONNECT " + targetserver + " " + port + " " + remoteserver, 
                SendMessageType.Connect);
        }

        public SendMessage Trace()
        {
            return new SendMessage(Connection, "TRACE", SendMessageType.Trace);
        }

        public SendMessage Trace(String target)
        {
            return new SendMessage(Connection, "TRACE " + target, SendMessageType.Trace);
        }

        public SendMessage Admin()
        {
            return new SendMessage(Connection, "ADMIN", SendMessageType.Admin);
        }

        public SendMessage Admin(String target)
        {
            return new SendMessage(Connection, "ADMIN " + target, SendMessageType.Admin);
        }

        public SendMessage Info()
        {
            return new SendMessage(Connection, "INFO", SendMessageType.Info);
        }

        public SendMessage Info(String target)
        {
            return new SendMessage(Connection, "INFO " + target, SendMessageType.Info);
        }

        public SendMessage Servlist()
        {
            return new SendMessage(Connection, "SERVLIST", SendMessageType.Servlist);
        }

        public SendMessage Servlist(String mask)
        {
            return new SendMessage(Connection, "SERVLIST " + mask, SendMessageType.Servlist);
        }

        public SendMessage Servlist(String mask, String type)
        {
            return new SendMessage(Connection, "SERVLIST " + mask + " " + type, SendMessageType.Servlist);
        }

        public SendMessage Squery(String servicename, String servicetext)
        {
            return new SendMessage(Connection, "SQUERY " + servicename + " :" + servicetext, SendMessageType.Squery);
        }

        public SendMessage List()
        {
            return new SendMessage(Connection, "LIST", SendMessageType.List);
        }

        public SendMessage List(String channel)
        {
            return new SendMessage(Connection, "LIST " + channel, SendMessageType.List);
        }

        public SendMessage List(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "LIST " + channellist, SendMessageType.List);
        }

        public SendMessage List(String channel, String target)
        {
            return new SendMessage(Connection, "LIST " + channel + " " + target, SendMessageType.List);
        }

        public SendMessage List(String[] channels, String target)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "LIST " + channellist + " " + target, SendMessageType.List);
        }

        public SendMessage Names()
        {
            return new SendMessage(Connection, "NAMES", SendMessageType.Names);
        }

        public SendMessage Names(String channel)
        {
            return new SendMessage(Connection, "NAMES " + channel, SendMessageType.Names);
        }

        public SendMessage Names(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "NAMES " + channellist, SendMessageType.Names);
        }

        public SendMessage Names(String channel, String target)
        {
            return new SendMessage(Connection, "NAMES " + channel + " " + target, SendMessageType.Names);
        }

        public SendMessage Names(String[] channels, String target)
        {
            String channellist = String.Join(",", channels);
            return new SendMessage(Connection, "NAMES " + channellist + " " + target, SendMessageType.Names);
        }

        public SendMessage Topic(String channel)
        {
            return new SendMessage(Connection, "TOPIC " + channel, SendMessageType.Topic);
        }

        public SendMessage Topic(String channel, String newtopic)
        {
            return new SendMessage(Connection, "TOPIC " + channel + " :" + newtopic, SendMessageType.Topic);
        }

        public SendMessage Mode(String target)
        {
            return new SendMessage(Connection, "MODE " + target, SendMessageType.Mode);
        }

        public SendMessage Mode(String target, String newmode)
        {
            return new SendMessage(Connection, "MODE " + target + " " + newmode, SendMessageType.Mode);
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
                SendMessageType.Service);
        }

        public SendMessage Invite(String nickname, String channel)
        {
            return new SendMessage(Connection, "INVITE " + nickname + " " + channel, SendMessageType.Invite);
        }

        public SendMessage Who()
        {
            return new SendMessage(Connection, "WHO", SendMessageType.Who);
        }

        public SendMessage Who(String mask)
        {
            return new SendMessage(Connection, "WHO " + mask, SendMessageType.Who);
        }

        public SendMessage Who(String mask, bool ircop)
        {
            if(ircop)
                return new SendMessage(Connection, "WHO " + mask + " o", SendMessageType.Who);
            else
                return new SendMessage(Connection, "WHO " + mask, SendMessageType.Who);
        }

        public SendMessage Whois(String mask)
        {
            return new SendMessage(Connection, "WHOIS " + mask, SendMessageType.Whois);
        }

        public SendMessage Whois(String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + masklist, SendMessageType.Whois);
        }

        public SendMessage Whois(String target, String mask)
        {
            return new SendMessage(Connection, "WHOIS " + target + " " + mask, SendMessageType.Whois);
        }

        public SendMessage Whois(String target, String[] masks)
        {
            String masklist = String.Join(",", masks);
            return new SendMessage(Connection, "WHOIS " + target + " " + masklist, SendMessageType.Whois);
        }

        public SendMessage Whowas(String nickname)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname, SendMessageType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist, SendMessageType.Whowas);
        }

        public SendMessage Whowas(String nickname, String count)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " ", SendMessageType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames, String count)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " ", SendMessageType.Whowas);
        }

        public SendMessage Whowas(String nickname, String count, String target)
        {
            return new SendMessage(Connection, "WHOWAS " + nickname + " " + count + " " + target, 
                SendMessageType.Whowas);
        }

        public SendMessage Whowas(String[] nicknames, String count, String target)
        {
            String nicknamelist = String.Join(",", nicknames);
            return new SendMessage(Connection, "WHOWAS " + nicknamelist + " " + count + " " + target, 
                SendMessageType.Whowas);
        }

        public SendMessage Kill(String nickname, String comment)
        {
            return new SendMessage(Connection, "KILL " + nickname + " :" + comment, SendMessageType.Kill);
        }

        public SendMessage Ping(String server)
        {
            return new SendMessage(Connection, "PING " + server, SendMessageType.Ping);
        }

        public SendMessage Ping(String server, String server2)
        {
            return new SendMessage(Connection, "PING " + server + " " + server2, SendMessageType.Ping);
        }

        public SendMessage Pong(String server)
        {
            return new SendMessage(Connection, "PONG " + server, SendMessageType.Pong);
        }

        public SendMessage Pong(String server, String server2)
        {
            return new SendMessage(Connection, "PONG " + server + " " + server2, SendMessageType.Pong);
        }

        public SendMessage Error(String errormessage)
        {
            return new SendMessage(Connection, "ERROR :" + errormessage, SendMessageType.Error);
        }

        public SendMessage Away()
        {
            return new SendMessage(Connection, "AWAY", SendMessageType.Away);
        }

        public SendMessage Away(String awaytext)
        {
            return new SendMessage(Connection, "AWAY :" + awaytext, SendMessageType.Away);
        }

        public SendMessage Rehash()
        {
            return new SendMessage(Connection, "REHASH", SendMessageType.Rehash);
        }

        public SendMessage Die()
        {
            return new SendMessage(Connection, "DIE", SendMessageType.Die);
        }

        public SendMessage Restart()
        {
            return new SendMessage(Connection, "RESTART", SendMessageType.Restart);
        }

        public SendMessage Summon(String user)
        {
            return new SendMessage(Connection, "SUMMON " + user, SendMessageType.Summon);
        }

        public SendMessage Summon(String user, String target)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target, SendMessageType.Summon);
        }

        public SendMessage Summon(String user, String target, String channel)
        {
            return new SendMessage(Connection, "SUMMON " + user + " " + target + " " + channel, 
                SendMessageType.Summon);
        }

        public SendMessage Users()
        {
            return new SendMessage(Connection, "USERS", SendMessageType.Users);
        }

        public SendMessage Users(String target)
        {
            return new SendMessage(Connection, "USERS " + target, SendMessageType.Users);
        }

        public SendMessage Wallops(String wallopstext)
        {
            return new SendMessage(Connection, "WALLOPS :" + wallopstext, SendMessageType.Wallops);
        }

        public SendMessage Userhost(String nickname)
        {
            return new SendMessage(Connection, "USERHOST " + nickname, SendMessageType.Userhost);
        }

        public SendMessage Userhost(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "USERHOST " + nicknamelist, SendMessageType.Userhost);
        }

        public SendMessage Ison(String nickname)
        {
            return new SendMessage(Connection, "ISON " + nickname, SendMessageType.Ison);
        }

        public SendMessage Ison(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return new SendMessage(Connection, "ISON " + nicknamelist, SendMessageType.Ison);
        }

        public SendMessage Quit()
        {
            return new SendMessage(Connection, "QUIT", SendMessageType.Quit);
        }

        public SendMessage Quit(String quitmessage)
        {
            return new SendMessage(Connection, "QUIT :" + quitmessage, SendMessageType.Quit);
        }

        public SendMessage Squit(String server, String comment)
        {
            return new SendMessage(Connection, "SQUIT " + server + " :" + comment, SendMessageType.Squit);
        }
    }
}
