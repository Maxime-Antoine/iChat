using iChat.Client.ServiceReference;
using System;
using System.ServiceModel;

namespace iChat.Client.Services
{
    public class ChatService : IChatService, IChatContractsCallback
    {
        private ChatContractsClient _svcClient;

        public event EventHandler<UserConnectionEventArgs> UserConnected;

        public event EventHandler<UserConnectionEventArgs> UserDisconnected;

        public event EventHandler<MessageEventArgs> MessageReceived;

        public ChatService()
        {
            _svcClient = new ChatContractsClient(new InstanceContext(this));
        }

        #region Public Methods

        public void Connect(string name)
        {
            _svcClient.Open();
            _svcClient.StartSession(name);
        }

        public void Disconnect()
        {
            _svcClient.StopSession();
            _svcClient.Close();
        }

        #endregion Public Methods

        #region IChatContractsCallback implementation

        void IChatContractsCallback.UserConnected(string name)
        {
            throw new NotImplementedException();
        }

        void IChatContractsCallback.UserDisconnected(string name)
        {
            throw new NotImplementedException();
        }

        void IChatContractsCallback.MessageReceived(string pseudo, string message)
        {
            throw new NotImplementedException();
        }

        #endregion IChatContractsCallback implementation
    }

    public class UserConnectionEventArgs : EventArgs
    {
        public string Username { get; set; }
    }

    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}