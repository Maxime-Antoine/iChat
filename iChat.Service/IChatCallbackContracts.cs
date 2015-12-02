using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iChat.Service
{
    public interface IChatCallbackContracts
    {
        [OperationContract(IsOneWay = true)]
        void UserConnected(string name);

        [OperationContract(IsOneWay = true)]
        void UserDisconnected(string name);

        [OperationContract(IsOneWay = true)]
        void MessageReceived(string pseudo, string message);
    }
}
