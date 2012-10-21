﻿using System;

namespace ReactiveIRC.Interface
{
    public interface IMessageSender
    {
        ISendMessage Action(IMessageTarget receiver, String message);
        ISendMessage Admin();
        ISendMessage Admin(String target);
        ISendMessage Away();
        ISendMessage Away(String awaytext);
        ISendMessage Connect(String targetserver, String port);
        ISendMessage Connect(String targetserver, String port, String remoteserver);
        ISendMessage Die();
        ISendMessage Error(String errormessage);
        ISendMessage Info();
        ISendMessage Info(String target);
        ISendMessage Invite(IUser user, IChannel channel);
        ISendMessage Ison(String nickname);
        ISendMessage Ison(String[] nicknames);
        ISendMessage Join(params IChannel[] channels);
        ISendMessage Join(IChannel channel);
        ISendMessage Join(IChannel channel, String key);
        ISendMessage Join(IChannel[] channels, String[] keys);
        ISendMessage Kick(params IChannelUser[] channelUsers);
        ISendMessage Kick(IChannelUser channelUser);
        ISendMessage Kick(IChannelUser channelUser, String comment);
        ISendMessage Kick(String comment, params IChannelUser[] channelUsers);
        ISendMessage Kill(String nickname, String comment);
        ISendMessage Links();
        ISendMessage Links(String remoteserver, String servermask);
        ISendMessage Links(String servermask);
        ISendMessage List();
        ISendMessage List(IChannel channel);
        ISendMessage List(IChannel channel, String target);
        ISendMessage List(IChannel[] channels);
        ISendMessage List(IChannel[] channels, String target);
        ISendMessage Lusers();
        ISendMessage Lusers(String mask);
        ISendMessage Lusers(String mask, String target);
        ISendMessage Mode(IMessageTarget target);
        ISendMessage Mode(IMessageTarget target, String newmode);
        ISendMessage Mode(IMessageTarget target, String newmode, String newModeParameter);
        ISendMessage Mode(IMessageTarget target, String[] newModes, String[] newModeParameters);
        ISendMessage Motd();
        ISendMessage Motd(String target);
        ISendMessage Names();
        ISendMessage Names(IChannel channel);
        ISendMessage Names(IChannel channel, String target);
        ISendMessage Names(IChannel[] channels);
        ISendMessage Names(IChannel[] channels, String target);
        ISendMessage Nick(String nickname);
        ISendMessage Notice(IMessageTarget receiver, String message);
        ISendMessage Oper(String name, String password);
        ISendMessage Part(params IChannel[] channels);
        ISendMessage Part(IChannel channel);
        ISendMessage Part(IChannel channel, String partmessage);
        ISendMessage Part(String partmessage, params IChannel[] channels);
        ISendMessage Pass(String password);
        ISendMessage Ping(String server);
        ISendMessage Ping(String server, String server2);
        ISendMessage Pong(String server);
        ISendMessage Pong(String server, String server2);
        ISendMessage Message(IMessageTarget receiver, String message);
        ISendMessage Quit();
        ISendMessage Quit(String quitmessage);
        ISendMessage Rehash();
        ISendMessage Restart();
        ISendMessage Service(String nickname, String distribution, String info);
        ISendMessage Servlist();
        ISendMessage Servlist(String mask);
        ISendMessage Servlist(String mask, String type);
        ISendMessage Squery(String servicename, String servicetext);
        ISendMessage Squit(String server, String comment);
        ISendMessage Stats();
        ISendMessage Stats(String query);
        ISendMessage Stats(String query, String target);
        ISendMessage Summon(String user);
        ISendMessage Summon(String user, String target);
        ISendMessage Summon(String user, String target, String channel);
        ISendMessage Time();
        ISendMessage Time(String target);
        ISendMessage Topic(IChannel channel);
        ISendMessage Topic(IChannel channel, String newtopic);
        ISendMessage Trace();
        ISendMessage Trace(String target);
        ISendMessage User(String username, int usermode, String realname);
        ISendMessage Userhost(String nickname);
        ISendMessage Userhost(String[] nicknames);
        ISendMessage Users();
        ISendMessage Users(String target);
        ISendMessage Version();
        ISendMessage Version(String target);
        ISendMessage Wallops(String wallopstext);
        ISendMessage Who();
        ISendMessage Who(String mask);
        ISendMessage Who(String mask, bool ircop);
        ISendMessage Whois(String mask);
        ISendMessage Whois(String target, String mask);
        ISendMessage Whois(String target, String[] masks);
        ISendMessage Whois(String[] masks);
        ISendMessage Whowas(String nickname);
        ISendMessage Whowas(String nickname, String count);
        ISendMessage Whowas(String nickname, String count, String target);
        ISendMessage Whowas(String[] nicknames);
        ISendMessage Whowas(String[] nicknames, String count);
        ISendMessage Whowas(String[] nicknames, String count, String target);
    }
}
