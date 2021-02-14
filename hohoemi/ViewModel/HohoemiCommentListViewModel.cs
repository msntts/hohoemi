using hohoemi.Model.Comment;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace hohoemi.ViewModel
{
    public class HohoemiCommentListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CommentManager commentMgr = CommentManager.Instance;

        public HohoemiCommentListViewModel()
        {
            commentMgr.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(commentMgr.LastCommentIndex):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comments)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
                    break;
            }
        }

        public ObservableCollection<CommentModel> Comments => new ObservableCollection<CommentModel>(commentMgr.Comments);

        public int SelectedIndex => commentMgr.Comments.Count;
    }
}
