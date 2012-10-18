namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Values that represent the type of a received message.
    /// </summary>
    public enum ReceiveMessageType
    {
        Unknown             // Sender types                     -> Receiver types

      , Message	            // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Action              // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Notice              // {IChannelUser, IUser, INetwork}  -> {IUser, IChannel}. 
      , Join                // {IChannelUser}                   -> {IChannel}. 
      , Part                // {IChannelUser}                   -> {IChannel}. 
      , Kick                // {IChannelUser, INetwork}         -> {IChannelUser}. 
      , Quit                // {IUser}                          -> {INetwork}.
      , TopicChange         // {IChannelUser, IUser, INetwork}  -> {IChannel}. 
      , NickChange          // {IUser}                          -> {INetwork}. 
      , UserModeChange      // {IUser, INetwork}                -> {IUser}
      , ChannelModeChange   // {IChannelUser, IUser, INetwork}  -> {IChannel}
      , Voice               // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
      , DeVoice             // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
      , HalfOp              // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
      , DeHalfOp            // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
      , Op                  // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
      , DeOp                // {IChannelUser, IUser, INetwork}  -> {IChannelUser}
    }
}
