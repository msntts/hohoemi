using hohoemi.Model;
using hohoemi.Model.Comment;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace hohoemi.ViewModel
{
    public class HohoemiViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly SettingsModel settings = SettingsModel.Instance;

        private readonly CommentManager commentManager = CommentManager.Instance;

        public HohoemiViewModel()
        {
            settings.PropertyChanged += OnSettingsChanged;

            commentManager.PropertyChanged += OnCommentArrived;

            // 初期値を設定する
            UpdateWindowRectangle();
        }

        private void UpdateWindowRectangle()
        {
            var current = Screen.AllScreens[settings.SelectedScreenIndex];

            WindowWidth = current.WorkingArea.Width;
            WindowHeight = current.WorkingArea.Height;
            WindowTop = current.WorkingArea.Y;
            WindowLeft = current.WorkingArea.X;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// 設定関係
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(settings.SelectedScreenIndex):
                    try
                    {
                        UpdateWindowRectangle();

                        PropertyChanged?.Invoke(this,
                            new PropertyChangedEventArgs(nameof(WindowWidth)));
                        PropertyChanged?.Invoke(this,
                            new PropertyChangedEventArgs(nameof(WindowHeight)));
                        PropertyChanged?.Invoke(this,
                            new PropertyChangedEventArgs(nameof(WindowTop)));
                        PropertyChanged?.Invoke(this,
                            new PropertyChangedEventArgs(nameof(WindowLeft)));
                    }
                    catch
                    {
                        // ここに来たということは。。。
                        // 自分でディスプレイ設定をして刹那のタイミングでディスプレイを抜いたと。。。
                        // なにもしない
                    }

                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// 表示サイズ
        ////////////////////////////////////////////////////////////////////////////////////
        public double WindowWidth { set; get; }
        public double WindowHeight { set; get; }

        ////////////////////////////////////////////////////////////////////////////////////
        /// 表示位置
        ////////////////////////////////////////////////////////////////////////////////////
        public double WindowTop { set; get; }
        public double WindowLeft { set; get; }


        ////////////////////////////////////////////////////////////////////////////////////
        /// コメント表示関係
        ////////////////////////////////////////////////////////////////////////////////////
        // この手のロジックはModelなのか悩んだけど、表示にかかわることなのでViewModelに集約
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnCommentArrived(object sender, PropertyChangedEventArgs e)
        {
            IControl ctrl = default;

            switch (e.PropertyName)
            {
                case nameof(commentManager.LastCommentIndex):
                    var comment = commentManager.Comments[commentManager.LastCommentIndex];

                    var cmntVm = new CommentViewModel()
                    {
                        Comment = comment.Resource,
                        FontColor = Color.FromArgb(settings.CommentColorR, settings.CommentColorG, settings.CommentColorB),
                        FontSize = settings.SelectedFontSize
                    };

                    CommentGenerationRequired?.Invoke(cmntVm);

                    ctrl = cmntVm;

                    break;
                case nameof(commentManager.LastImageIndex):
                    var image = commentManager.Images[commentManager.LastImageIndex];

                    var imageVm = new ImageViewModel()
                    {
                        ImagePath = image.Resource
                    };

                    ImageGenerationRequired?.Invoke(imageVm);

                    ctrl = imageVm;
                    break;
            }

            // ここでデータバインディングされてWidth/Heigtが入ってくるはず
        }

        public event Action<ImageViewModel> ImageGenerationRequired;
        public event Action<CommentViewModel> CommentGenerationRequired;
    }

    public interface IControl
    {
        int Top { set; get; }
        int Left { set; get; }

        double Width { set; get; }
        double Height { set; get; }
    }

    public class ImageViewModel : IControl
    {
        public string ImagePath { set; get; }

        public int Top { set; get; }
        public int Left { set; get; }

        public double Width { set; get; }
        public double Height { set; get; }
    }
    
    public class CommentViewModel : IControl
    {
        public string Comment { set; get; }
 
        public int Top { set; get; }
        public int Left { set; get; }

        public double Width { set; get; }
        public double Height { set; get; }

        public int FontSize { set; get; }
        public Color FontColor { set; get; }
    }
}
