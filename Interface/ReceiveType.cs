namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Values that represent the type of a received message.
    /// </summary>
    public enum ReceiveType
    {
        Unknown
      , Reply               // {INetwork}                       -> {Me}
      , Error               // {INetwork}                       -> {Me}
      , Ping                // {INetwork}                       -> {Me}
      , Echo                // {Me}                             -> {INetwork, IUser, IChannel, IChannelUser}
      , Message	            // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Action              // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Notice              // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Invite              // {IChannelUser, IUser, INetwork}  -> {IChannel}.
      , Join                // {IChannelUser}                   -> {IChannel}. 
      , Part                // {IChannelUser}                   -> {IChannel}. 
      , Kick                // {IChannelUser, INetwork}         -> {IChannelUser}. 
      , Quit                // {IUser}                          -> {INetwork}.
      , TopicChange         // {IChannelUser, IUser, INetwork}  -> {IChannel}. 
      , NickChange          // {IUser}                          -> {INetwork}. 
      , UserModeChange      // {IUser, INetwork}                -> {IUser}
      , ChannelModeChange   // {IChannelUser, IUser, INetwork}  -> {IChannel}
    }
}
