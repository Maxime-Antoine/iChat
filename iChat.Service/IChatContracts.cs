using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace iChat.Service
{
    [ServiceContract(Namespace = "iChat", 
                     CallbackContract = typeof(IChatCallbackContracts),
                     SessionMode = SessionMode.Required)]
    public interface IChatContracts
    {
        [OperationContract(IsInitiating = true, IsTerminating = false, IsOneWay = true)]
        void StartSession(string name);

        [OperationContract(IsInitiating = false, IsTerminating = true, IsOneWay = true)]
        void StopSession();

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SendMessage(string message);
    }
}
