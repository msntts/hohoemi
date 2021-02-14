using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using hohoemi.Model;

namespace hohoemi.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ICommand ApplyCommand { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly SettingsModel settings;

        public SettingsViewModel()
        {
            settings = SettingsModel.Instance;

            // プロパティをsettingsで初期化
            SelectedScreen = Screen.AllScreens[settings.SelectedScreenIndex];
            RejectNaming = settings.RejectNaming;
            UserName = settings.UserName;
            CommentColorR = settings.CommentColorR;
            CommentColorG = settings.CommentColorG;
            CommentColorB = settings.CommentColorB;
            SelectedFontSize = settings.SelectedFontSize;

            // 変更通知を受ける
            settings.PropertyChanged += OnSettingsPropertyChanged;

            ApplyCommand = new Command(
                (param) => hasSettingChanged,
                (param) => { settings.Save(); hasSettingChanged = false; },
                (e) => { });
        }

        private bool hasSettingChanged = false;

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(settings.SelectedScreenIndex):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedScreen)));
                    break;
                case nameof(settings.RejectNaming):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RejectNaming)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnableNameSetting)));
                    break;
                case nameof(settings.UserName):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                    break;
                case nameof(settings.CommentColorR):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColor)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorR)));
                    break;
                case nameof(settings.CommentColorG):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColor)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorG)));
                    break;
                case nameof(settings.CommentColorB):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColor)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorB)));
                    break;
                case nameof(settings.SelectedFontSize):
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontSize)));
                    break;
            }

            // 一回でも変更あったら(その後デフォルトに戻したとか走らない)
            hasSettingChanged = true;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// 画面設定
        ////////////////////////////////////////////////////////////////////////////////////
        public IReadOnlyList<Screen> Screens
        {
            get => Screen.AllScreens.ToList();
        }

        public Screen SelectedScreen
        {
            set
            {
                if (Screen.AllScreens.Contains(value))
                {
                    // 今のindexをモデルに通知
                    settings.SelectedScreenIndex = Screen.AllScreens.ToList().IndexOf(value);
                }
                else
                {
                    // 設定中にディスプレイ抜き差しした?マジで言ってる?
                    // モデルにindex 0をたたきつけておく
                    settings.SelectedScreenIndex = 0;
                }
            }

            get
            {
                try
                {
                    return Screen.AllScreens[settings.SelectedScreenIndex];
                }
                catch
                {
                    // ここに来たということは、ディスプレイが抜き差しされている。。。
                    // とりあえず、0返しておく
                    return Screen.AllScreens[0];
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(ユーザ名)
        ////////////////////////////////////////////////////////////////////////////////////
        public bool RejectNaming
        {
            set => settings.RejectNaming = value;
            get => settings.RejectNaming;
        }

        public bool EnableNameSetting => !RejectNaming;

        public string UserName
        {
            set => settings.UserName = value;
            get => settings.UserName;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(コメント色)
        ////////////////////////////////////////////////////////////////////////////////////
        public SolidColorBrush CommentColor
        {
            get => new SolidColorBrush(Color.FromRgb(
                settings.CommentColorR,
                settings.CommentColorG,
                settings.CommentColorB));
        }

        public byte CommentColorR
        {
            set => settings.CommentColorR = value;
            get => settings.CommentColorR;
        }

        public byte CommentColorG
        {
            set => settings.CommentColorG = value;
            get => settings.CommentColorG;
        }

        public byte CommentColorB
        {
            set => settings.CommentColorB = value;
            get => settings.CommentColorB;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(フォントサイズ)
        ////////////////////////////////////////////////////////////////////////////////////
        public int SelectedFontSize
        {
            set => settings.SelectedFontSize = value;
            get => settings.SelectedFontSize;
        }

        public IReadOnlyList<int> FontSizes
        {
            get => settings.FontSizes;
        }
    }
}
