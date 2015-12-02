using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace iChat.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Single)]
    public class ChatService : IChatContracts
    {
        private static IList<string> _userList = new List<string>();
        private string _userName = String.Empty;
        private IChatCallbackContracts _callback;

        #region Events

        internal static event ChatMessageEventHandler ChatMessageEvent;
        internal delegate void ChatMessageEventHandler(object sender, ChatMessageEventArgs e);

        internal static event UserConnectionEventHandler UserConnectionEvent;
        internal delegate void UserConnectionEventHandler(object sender, UserConnectionEventArgs e);

        internal static event UserDisconnectionEventHandler UserDisconnectionEvent;
        internal delegate void UserDisconnectionEventHandler(object sender, UserDisconnectionEventArgs e);

        #endregion

        #region IChatContracts implementation

        public void StartSession(string name)
        {
            _callback = OperationContext.Current.GetCallbackChannel<IChatCallbackContracts>();

            lock (_userList)
            {
                _userName = name;
                _userList.Add(_userName);

                ChatMessageEvent += ChatMessageHandler;
                UserConnectionEvent += UserConnectionHandler;
                UserDisconnectionEvent += UserDisconnectionHandler;

                UserConnectionEventArgs cnxEventArgs = new UserConnectionEventArgs { User = _userName };
                UserConnectionEvent(this, cnxEventArgs);

                Console.WriteLine("User {0} connected", _userName);
            }
        }

        public void StopSession()
        {
            lock (_userList)
            {
                ChatMessageEvent -= ChatMessageHandler;
                UserConnectionEvent -= UserConnectionHandler;
                UserDisconnectionEvent -= UserDisconnectionHandler;

                _userList.Remove(_userName);

                UserDisconnectionEventArgs discnxEventArgs = new UserDisconnectionEventArgs { User = _userName };
                UserDisconnectionEvent(this, discnxEventArgs);

                Console.WriteLine("User {0} disconnected", _userName);
            }
        }

        public void SendMessage(string message)
        {
            ChatMessageEventArgs msgEventARgs = new ChatMessageEventArgs { Message = message, User = _userName };
            ChatMessageEvent(this, msgEventARgs);
        }

        #endregion

        #region Private Methods

        private void UserConnectionHandler(object sender, UserConnectionEventArgs e)
        {
            _callback.UserConnected(e.User);
        }

        private void ChatMessageHandler(object sender, ChatMessageEventArgs e)
        {
            _callback.MessageReceived(e.User, e.Message);
        }

        private void UserDisconnectionHandler(object sender, UserDisconnectionEventArgs e)
        {
            _callback.UserDisconnected(e.User);
        }

        #endregion
    }

    #region EventArgs classes

    internal class ChatMessageEventArgs : EventArgs
    {
        public string User { get; set; }
        public string Message { get; set; }
    }

    internal class UserConnectionEventArgs : EventArgs
    {
        public string User { get; set; }
    }

    internal class UserDisconnectionEventArgs : EventArgs
    {
        public string User { get; set; }
    }

    #endregion
}
