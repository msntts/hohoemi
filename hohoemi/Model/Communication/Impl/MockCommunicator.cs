using System;
using System.Collections.Generic;
using System.Timers;

namespace hohoemi.Model.Communication.Impl
{
    public class MockCommunicator : ICommunicator
    {
        // 受信スレッドを模擬
        // タイマーのスレッドから通知させることで、GUIスレッドの操作有無をデバッグ中に確認できるようにする
        private readonly Timer _pollingTimer = new Timer();

        private readonly List<Tuple<string, string>> _messageBuff = new List<Tuple<string, string>>();

        public event EventHandler<MessageArrivedArgs> OnMessageArrived;

        public void Connect()
        {
            _pollingTimer.AutoReset = true;
            _pollingTimer.Interval = 100; /* ms */
            _pollingTimer.Enabled = true;
            _pollingTimer.Elapsed += Notify;
        }

        private void Notify(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                foreach(var msg in _messageBuff)
                {
                    OnMessageArrived(this, new MessageArrivedArgs(msg.Item1, msg.Item2));
                }

                _messageBuff.Clear();
            }
        }

        public void Disconnect()
        {
            _pollingTimer.Dispose();
        }

        public void Init(Dictionary<string, string> properties)
        {
        }

        public int Send(string sender, string message)
        {
            lock(this)
            {
                _messageBuff.Add(new Tuple<string, string>(sender, message));
            }

            return 1;
        }
    }
}
