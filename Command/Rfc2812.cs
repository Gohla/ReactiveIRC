using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ReactiveIRC.Command
{
    public static class Rfc2812
    {
        private static Regex _nicknameRegex = new Regex(@"^[A-Za-z\[\]\\`_^{|}][A-Za-z0-9\[\]\\`_\-^{|}]+$",
            RegexOptions.Compiled);

        public static bool IsValidNickname(String nickname)
        {
            if((nickname != null) && (nickname.Length > 0) && (_nicknameRegex.Match(nickname).Success))
                return true;

            return false;
        }

        public static String Pass(String password)
        {
            return "PASS " + password;
        }

        public static String Nick(String nickname)
        {
            return "NICK " + nickname;
        }

        public static String User(String username, int usermode, String realname)
        {
            return "USER " + username + " " + usermode.ToString() + " * :" + realname;
        }

        public static String Oper(String name, String password)
        {
            return "OPER " + name + " " + password;
        }

        public static String Privmsg(String destination, String message)
        {
            return "PRIVMSG " + destination + " :" + message;
        }

        public static String Notice(String destination, String message)
        {
            return "NOTICE " + destination + " :" + message;
        }

        public static String Join(String channel)
        {
            return "JOIN " + channel;
        }

        public static String Join(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return "JOIN " + channellist;
        }

        public static String Join(String channel, String key)
        {
            return "JOIN " + channel + " " + key;
        }

        public static String Join(String[] channels, String[] keys)
        {
            String channellist = String.Join(",", channels);
            String keylist = String.Join(",", keys);
            return "JOIN " + channellist + " " + keylist;
        }

        public static String Part(String channel)
        {
            return "PART " + channel;
        }

        public static String Part(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return "PART " + channellist;
        }

        public static String Part(String channel, String partmessage)
        {
            return "PART " + channel + " :" + partmessage;
        }

        public static String Part(String[] channels, String partmessage)
        {
            String channellist = String.Join(",", channels);
            return "PART " + channellist + " :" + partmessage;
        }

        public static String Kick(String channel, String nickname)
        {
            return "KICK " + channel + " " + nickname;
        }

        public static String Kick(String channel, String nickname, String comment)
        {
            return "KICK " + channel + " " + nickname + " :" + comment;
        }

        public static String Kick(String[] channels, String nickname)
        {
            String channellist = String.Join(",", channels);
            return "KICK " + channellist + " " + nickname;
        }

        public static String Kick(String[] channels, String nickname, String comment)
        {
            String channellist = String.Join(",", channels);
            return "KICK " + channellist + " " + nickname + " :" + comment;
        }

        public static String Kick(String channel, String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return "KICK " + channel + " " + nicknamelist;
        }

        public static String Kick(String channel, String[] nicknames, String comment)
        {
            String nicknamelist = String.Join(",", nicknames);
            return "KICK " + channel + " " + nicknamelist + " :" + comment;
        }

        public static String Kick(String[] channels, String[] nicknames)
        {
            String channellist = String.Join(",", channels);
            String nicknamelist = String.Join(",", nicknames);
            return "KICK " + channellist + " " + nicknamelist;
        }

        public static String Kick(String[] channels, String[] nicknames, String comment)
        {
            String channellist = String.Join(",", channels);
            String nicknamelist = String.Join(",", nicknames);
            return "KICK " + channellist + " " + nicknamelist + " :" + comment;
        }

        public static String Motd()
        {
            return "MOTD";
        }

        public static String Motd(String target)
        {
            return "MOTD " + target;
        }

        public static String Lusers()
        {
            return "LUSERS";
        }

        public static String Lusers(String mask)
        {
            return "LUSER " + mask;
        }

        public static String Lusers(String mask, String target)
        {
            return "LUSER " + mask + " " + target;
        }

        public static String Version()
        {
            return "VERSION";
        }

        public static String Version(String target)
        {
            return "VERSION " + target;
        }

        public static String Stats()
        {
            return "STATS";
        }

        public static String Stats(String query)
        {
            return "STATS " + query;
        }

        public static String Stats(String query, String target)
        {
            return "STATS " + query + " " + target;
        }

        public static String Links()
        {
            return "LINKS";
        }

        public static String Links(String servermask)
        {
            return "LINKS " + servermask;
        }

        public static String Links(String remoteserver, String servermask)
        {
            return "LINKS " + remoteserver + " " + servermask;
        }

        public static String Time()
        {
            return "TIME";
        }

        public static String Time(String target)
        {
            return "TIME " + target;
        }

        public static String Connect(String targetserver, String port)
        {
            return "CONNECT " + targetserver + " " + port;
        }

        public static String Connect(String targetserver, String port, String remoteserver)
        {
            return "CONNECT " + targetserver + " " + port + " " + remoteserver;
        }

        public static String Trace()
        {
            return "TRACE";
        }

        public static String Trace(String target)
        {
            return "TRACE " + target;
        }

        public static String Admin()
        {
            return "ADMIN";
        }

        public static String Admin(String target)
        {
            return "ADMIN " + target;
        }

        public static String Info()
        {
            return "INFO";
        }

        public static String Info(String target)
        {
            return "INFO " + target;
        }

        public static String Servlist()
        {
            return "SERVLIST";
        }

        public static String Servlist(String mask)
        {
            return "SERVLIST " + mask;
        }

        public static String Servlist(String mask, String type)
        {
            return "SERVLIST " + mask + " " + type;
        }

        public static String Squery(String servicename, String servicetext)
        {
            return "SQUERY " + servicename + " :" + servicetext;
        }

        public static String List()
        {
            return "LIST";
        }

        public static String List(String channel)
        {
            return "LIST " + channel;
        }

        public static String List(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return "LIST " + channellist;
        }

        public static String List(String channel, String target)
        {
            return "LIST " + channel + " " + target;
        }

        public static String List(String[] channels, String target)
        {
            String channellist = String.Join(",", channels);
            return "LIST " + channellist + " " + target;
        }

        public static String Names()
        {
            return "NAMES";
        }

        public static String Names(String channel)
        {
            return "NAMES " + channel;
        }

        public static String Names(String[] channels)
        {
            String channellist = String.Join(",", channels);
            return "NAMES " + channellist;
        }

        public static String Names(String channel, String target)
        {
            return "NAMES " + channel + " " + target;
        }

        public static String Names(String[] channels, String target)
        {
            String channellist = String.Join(",", channels);
            return "NAMES " + channellist + " " + target;
        }

        public static String Topic(String channel)
        {
            return "TOPIC " + channel;
        }

        public static String Topic(String channel, String newtopic)
        {
            return "TOPIC " + channel + " :" + newtopic;
        }

        public static String Mode(String target)
        {
            return "MODE " + target;
        }

        public static String Mode(String target, String newmode)
        {
            return "MODE " + target + " " + newmode;
        }

        public static String Mode(String target, String[] newModes, String[] newModeParameters)
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

        public static String Service(String nickname, String distribution, String info)
        {
            return "SERVICE " + nickname + " * " + distribution + " * * :" + info;
        }

        public static String Invite(String nickname, String channel)
        {
            return "INVITE " + nickname + " " + channel;
        }

        public static String Who()
        {
            return "WHO";
        }

        public static String Who(String mask)
        {
            return "WHO " + mask;
        }

        public static String Who(String mask, bool ircop)
        {
            if(ircop)
            {
                return "WHO " + mask + " o";
            }
            else
            {
                return "WHO " + mask;
            }
        }

        public static String Whois(String mask)
        {
            return "WHOIS " + mask;
        }

        public static String Whois(String[] masks)
        {
            String masklist = String.Join(",", masks);
            return "WHOIS " + masklist;
        }

        public static String Whois(String target, String mask)
        {
            return "WHOIS " + target + " " + mask;
        }

        public static String Whois(String target, String[] masks)
        {
            String masklist = String.Join(",", masks);
            return "WHOIS " + target + " " + masklist;
        }

        public static String Whowas(String nickname)
        {
            return "WHOWAS " + nickname;
        }

        public static String Whowas(String[] nicknames)
        {
            String nicknamelist = String.Join(",", nicknames);
            return "WHOWAS " + nicknamelist;
        }

        public static String Whowas(String nickname, String count)
        {
            return "WHOWAS " + nickname + " " + count + " ";
        }

        public static String Whowas(String[] nicknames, String count)
        {
            String nicknamelist = String.Join(",", nicknames);
            return "WHOWAS " + nicknamelist + " " + count + " ";
        }

        public static String Whowas(String nickname, String count, String target)
        {
            return "WHOWAS " + nickname + " " + count + " " + target;
        }

        public static String Whowas(String[] nicknames, String count, String target)
        {
            String nicknamelist = String.Join(",", nicknames);
            return "WHOWAS " + nicknamelist + " " + count + " " + target;
        }

        public static String Kill(String nickname, String comment)
        {
            return "KILL " + nickname + " :" + comment;
        }

        public static String Ping(String server)
        {
            return "PING " + server;
        }

        public static String Ping(String server, String server2)
        {
            return "PING " + server + " " + server2;
        }

        public static String Pong(String server)
        {
            return "PONG " + server;
        }

        public static String Pong(String server, String server2)
        {
            return "PONG " + server + " " + server2;
        }

        public static String Error(String errormessage)
        {
            return "ERROR :" + errormessage;
        }

        public static String Away()
        {
            return "AWAY";
        }

        public static String Away(String awaytext)
        {
            return "AWAY :" + awaytext;
        }

        public static String Rehash()
        {
            return "REHASH";
        }

        public static String Die()
        {
            return "DIE";
        }

        public static String Restart()
        {
            return "RESTART";
        }

        public static String Summon(String user)
        {
            return "SUMMON " + user;
        }

        public static String Summon(String user, String target)
        {
            return "SUMMON " + user + " " + target;
        }

        public static String Summon(String user, String target, String channel)
        {
            return "SUMMON " + user + " " + target + " " + channel;
        }

        public static String Users()
        {
            return "USERS";
        }

        public static String Users(String target)
        {
            return "USERS " + target;
        }

        public static String Wallops(String wallopstext)
        {
            return "WALLOPS :" + wallopstext;
        }

        public static String Userhost(String nickname)
        {
            return "USERHOST " + nickname;
        }

        public static String Userhost(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return "USERHOST " + nicknamelist;
        }

        public static String Ison(String nickname)
        {
            return "ISON " + nickname;
        }

        public static String Ison(String[] nicknames)
        {
            String nicknamelist = String.Join(" ", nicknames);
            return "ISON " + nicknamelist;
        }

        public static String Quit()
        {
            return "QUIT";
        }

        public static String Quit(String quitmessage)
        {
            return "QUIT :" + quitmessage;
        }

        public static String Squit(String server, String comment)
        {
            return "SQUIT " + server + " :" + comment;
        }
    }
}
