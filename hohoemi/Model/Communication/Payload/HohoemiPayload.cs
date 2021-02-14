using System;
using System.Text;
using System.Text.Json;

namespace hohoemi.Model.Communication.Payload
{
    public class HohoemiPayload
    {
        public string Sender { set; get; }

        public string Message { set; get; }
    }
}
