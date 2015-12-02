using System;

namespace iChat.Client.Services
{
    public interface IChatService
    {
        event EventHandler<UserConnectionEventArgs> UserConnected;

        event EventHandler<UserConnectionEventArgs> UserDisconnected;

        event EventHandler<MessageEventArgs> MessageReceived;

        void Connect(string name);

        void Disconnect();
    }
}
