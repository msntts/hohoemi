using System.Windows;

namespace hohoemi.View
{
    /// <summary>
    /// HohoemoCommentViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class HohoemiCommentListView : Window
    {
        public HohoemiCommentListView()
        {
            InitializeComponent();
        }

        private void ScrollIntoView(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(autoScroll.IsChecked ?? false) // なぜnullable...
            {
                commentList.ScrollIntoView(commentList.SelectedItem);
            }
        }
    }
}
