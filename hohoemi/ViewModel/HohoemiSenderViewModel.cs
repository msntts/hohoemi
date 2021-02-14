using hohoemi.Model;
using hohoemi.Model.Comment;
using System.ComponentModel;
using System.Windows.Input;

namespace hohoemi.ViewModel
{
    public class HohoemiSenderViewModel : INotifyPropertyChanged
    {
        private readonly SettingsModel settings = SettingsModel.Instance;
        private readonly CommentManager commentMgr = CommentManager.Instance;

        public HohoemiSenderViewModel()
        {
            commentMgr.PropertyChanged += OnCommentManagerPropertyChanged;

            SendCommand = new Command(
                (cmnt) => (cmnt is string) && !string.IsNullOrEmpty(cmnt as string),
                (cmnt) =>
                {
                    commentMgr.SendMessage(settings.UserName, settings.RejectNaming, Comment);
                    Comment = string.Empty;
                },
                (e) => { /* do nothing */ });

            ChangeVisibilityCommand = new Command(
                (param) => true,
                (param) => CommentVisible = !(bool)param,
                (e) => { });
        }

        private void OnCommentManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(commentMgr.CommentVisible):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentVisible)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentOnOffIconPath)));
                    break;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント送信関連
        ////////////////////////////////////////////////////////////////////////////////////
        public ICommand SendCommand { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string comment = null;
        public string Comment
        {
            set
            {
                if ((value != null) && comment != value)
                {
                    comment = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));

                    CommandManager.InvalidateRequerySuggested();
                }
            }

            get => comment;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント表示/非表示関連
        ////////////////////////////////////////////////////////////////////////////////////
        public ICommand ChangeVisibilityCommand { set; get; }

        public bool CommentVisible
        {
            set => commentMgr.CommentVisible = value;
            get => commentMgr.CommentVisible;
        }

        public string CommentOnOffIconPath
        {
            get => CommentVisible ? "../Images/commentOn.png" : "../Images/commentOff.png";
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ///  コメントビューア関係
        ////////////////////////////////////////////////////////////////////////////////////
        public string CommentViewerIconPath
        {
            get => "../Images/view.png";
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ///  設定画面関係
        ////////////////////////////////////////////////////////////////////////////////////
        public string SettingIconPath
        {
            get => "../Images/settings.png";
        }
    }
}
