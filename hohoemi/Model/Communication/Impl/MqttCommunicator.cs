using hohoemi.Model.Communication.Payload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace hohoemi.Model.Communication.Impl
{
    public class MqttCommunicator : ICommunicator
    {
        private const string KEY_PORT = "Port";
        private const string KEY_IP = "IP";
        private const string KEY_CHANNEL = "Channel";

        public event EventHandler<MessageArrivedArgs> OnMessageArrived = delegate { };

        private MqttClient _client;
        private string _channerl;

        public void Connect()
        {
            if (!_client.IsConnected)
            {
                // PCのユーザ名とtopicをclient idに使う
                string id = Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                id += _channerl;

                _client.Connect(id);

                while(!_client.IsConnected)
                {
                    Task.Delay(100 /* ms */);
                }

                _client.MqttMsgPublishReceived += Receve;
                _client.Subscribe(new string[] { _channerl }, new byte[] { 2 });
            }
        }

        public void Disconnect()
        {
            _client.Unsubscribe(new string[] { _channerl });

            _client.Disconnect();
        }

        [Obsolete]
        public void Init(Dictionary<string, string> properties)
        {
            int port = Convert.ToInt32(properties[KEY_PORT]);
            IPAddress addr = IPAddress.Parse(properties[KEY_IP]);
            _channerl = properties[KEY_CHANNEL];

            _client = new MqttClient(addr, port, false, null, null, MqttSslProtocols.None);
        }

        public int Send(string sender, string message)
        {
            var payload = JsonConverter.ToJsonBytes(sender, message);
            // ユーザ名、コメントに何入れられてるかわからないのでBASE64エンコードして送る
            payload = Encoding.UTF8.GetBytes(Convert.ToBase64String(payload));

            return _client.Publish(_channerl, payload);
        }

        private void Receve(object sender, MqttMsgPublishEventArgs e)
        {
            // BASE64エンコードされてるはずなのででコードする
            var message = Convert.FromBase64String(Encoding.UTF8.GetString(e.Message));
            HohoemiPayload paylod = JsonConverter.FromBytes(message);

            OnMessageArrived(this, new MessageArrivedArgs(paylod.Sender, paylod.Message));
        }
    }
}
