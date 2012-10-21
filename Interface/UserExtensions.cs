using System;

namespace ReactiveIRC.Interface
{
    public static class UserExtensions
    {
        /// <summary>
        /// Sends a message to this user.
        /// </summary>
        ///
        /// <param name="user">   The user to act on.</param>
        /// <param name="message">The message.</param>
        public static void SendMessage(this IUser user, String message)
        {
            IClientConnection connection = user.Connection;
            connection.Send(connection.MessageSender.Message(user, message));
        }

        /// <summary>
        /// Sends an action to this user.
        /// </summary>
        ///
        /// <param name="user">  The user to act on.</param>
        /// <param name="action">The action.</param>
        public static void SendAction(this IUser user, String action)
        {
            IClientConnection connection = user.Connection;
            connection.Send(connection.MessageSender.Action(user, action));
        }

        /// <summary>
        /// Sends a notice to this user.
        /// </summary>
        ///
        /// <param name="user">  The user to act on.</param>
        /// <param name="notice">The notice.</param>
        public static void SendNotice(this IUser user, String notice)
        {
            IClientConnection connection = user.Connection;
            connection.Send(connection.MessageSender.Notice(user, notice));
        }
    }
}
