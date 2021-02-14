using hohoemi.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace hohoemi.View
{
    /// <summary>
    /// HohoemiView.xaml の相互作用ロジック
    /// </summary>
    public partial class HohoemiView : Window
    {
        public HohoemiView()
        {
            InitializeComponent();

            var vm = DataContext as HohoemiViewModel;

            vm.CommentGenerationRequired += Vm_CommentGenerationRequired;
            vm.ImageGenerationRequired += Vm_ImageGenerationRequired;
        }

        private void Vm_ImageGenerationRequired(ImageViewModel obj)
        {
            throw new System.NotImplementedException();
        }

        private void Vm_CommentGenerationRequired(CommentViewModel comment)
        {
            Dispatcher.Invoke(() => 
            {
                var label = new Label()
                {
                    Content = comment.Comment,
                    FontSize = comment.FontSize,
                };

                // 上書きしてしまうので一回commentに必要なものをコピー
                comment.Width = label.Width;
                comment.Height = label.Height;
                label.DataContext = comment;
                label.SetBinding(WidthProperty, new Binding(nameof(comment.Width)));
                label.SetBinding(HeightProperty, new Binding(nameof(comment.Height)));
 //               label.SetBinding(TopProperty, new Binding(nameof(comment.Top)));
 //               label.SetBinding(LeftProperty, new Binding(nameof(comment.Left)));
            });
        }
    }
}
