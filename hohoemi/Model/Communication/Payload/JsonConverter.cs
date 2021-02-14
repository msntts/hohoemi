using System.Text;
using System.Text.Json;

namespace hohoemi.Model.Communication.Payload
{
    public static class JsonConverter
    {
        public static HohoemiPayload FromBytes(byte[] payload)
        {
            HohoemiPayload hp;

            try
            {
                hp = JsonSerializer.Deserialize<HohoemiPayload>(Encoding.UTF8.GetString(payload));
            }
            catch
            {
                // よく知らないフォーマットのデータが来ている
                // そのまま流す
                hp = new HohoemiPayload()
                {
                    Sender = "unkown",
                    Message = Encoding.UTF8.GetString(payload)
                };
            }

            return hp;
        }

        public static byte[] ToJsonBytes(string sender, string message)
        {
            var payload = new HohoemiPayload()
            {
                Sender = sender,
                Message = message
            };

            // 入力によっては例外飛びそうな気もするけど、基本的にないと信じているので例外を上げる
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        }
    }
}
