using System;

namespace hohoemi.Model.Communication
{
    public class MessageArrivedArgs : EventArgs
    {
        public string Message { get; private set; } = string.Empty;

        public string Sender { get; private set; } = string.Empty; 

        public MessageArrivedArgs(string sender, string message)
        {
            Message = message;
            Sender = sender;
        }
    }
}   
