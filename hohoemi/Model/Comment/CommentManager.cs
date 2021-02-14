using hohoemi.Model.Communication;
using System.ComponentModel;
using System.Collections.Generic;

namespace hohoemi.Model.Comment
{
    // コメントにまつわるすべてを管理
    // コメントを動かすのも実態はココ
    // ただ、初期位置とか、画面から切れたかとかはControlの世界なのでここではノータッチ
    // しんぐるとん
    public class CommentManager : INotifyPropertyChanged
    {
        private readonly ICommunicator communicator;

        public event PropertyChangedEventHandler PropertyChanged;

        public static CommentManager Instance { private set; get; }

        public static void Init()
        {
            Instance = new CommentManager();
        }

        public static void Destory()
        {
            Instance.communicator.Disconnect();
        }

        private CommentManager()
        {
            communicator = CommunicatorFactory.Create();
            communicator.OnMessageArrived += OnMessageArrived;
            communicator.Connect();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント送信
        ////////////////////////////////////////////////////////////////////////////////////
        public void SendMessage(string sender, bool rejectNaming, string message)
        {
            communicator.Send(rejectNaming ? string.Empty : sender, message);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント受信
        ////////////////////////////////////////////////////////////////////////////////////
        private readonly List<CommentModel> comments = new List<CommentModel>();
        public IList<CommentModel> Comments => comments;

        private int lastCommentIndex = -1;
        public int LastCommentIndex
        {
            set
            {
                if (lastCommentIndex != value)
                {
                    lastCommentIndex = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastCommentIndex)));
                }
            }

            get => lastCommentIndex;
        }

        private readonly List<CommentModel> images = new List<CommentModel>();
        public IList<CommentModel> Images => images;

        private int lastImageIndex = -1;
        public int LastImageIndex
        {
            set
            {
                if (lastImageIndex != value)
                {
                    lastImageIndex = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastImageIndex)));
                }
            }

            get => lastImageIndex;
        }

        private void OnMessageArrived(object sender, MessageArrivedArgs e)
        {
            foreach (var cmntmdl in CommentParser.Parse(e.Sender, e.Message))
            {
                switch(cmntmdl.Type)
                {
                    case CommentType.TEXT:
                        comments.Add(cmntmdl);
                        LastCommentIndex = comments.Count - 1;
                        break;
                    case CommentType.IMAGE:
                        images.Add(cmntmdl);
                        lastImageIndex = images.Count - 1;
                        break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント表示
        ////////////////////////////////////////////////////////////////////////////////////
        private bool commentVisible = true;
        public bool CommentVisible
        {
            set
            {
                if (commentVisible != value)
                {
                    commentVisible = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentVisible)));
                }
            }

            get => commentVisible;
        }
    }
}
