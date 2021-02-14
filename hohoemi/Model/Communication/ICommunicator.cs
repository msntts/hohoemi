using System;
using System.Collections.Generic;

namespace hohoemi.Model.Communication
{
    public interface ICommunicator
    {
        void Init(Dictionary<string, string> properties);

        void Connect();

        void Disconnect();

        int Send(string sender, string message);

        event EventHandler<MessageArrivedArgs> OnMessageArrived;
    }
}
