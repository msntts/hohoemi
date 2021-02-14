using System.Windows;

namespace hohoemi.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HohoemiSenderView : Window
    {
        private readonly HohoemiView view = new HohoemiView();

        private HohoemiCommentListView commentView = null;

        public HohoemiSenderView()
        {
            InitializeComponent();

            view.Show();
        }

        private void SettingsImageClick(object sender, RoutedEventArgs e)
        {
            new SettingsView().ShowDialog();
        }

        private void CommentViewerImageClick(object sender, RoutedEventArgs e)
        {
            if(commentView == null || !commentView.IsVisible)
            {
                commentView = new  HohoemiCommentListView();

                commentView.Show();
            }
            else
            {
                commentView.Activate();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            view.Close();

            if(commentView != null)
            {
                commentView.Close();
            }
        }
    }
}
