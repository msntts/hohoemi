using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace hohoemi.Model
{
    // ユーザ設定を持ってるクラス
    // こいつもシングルトン
    public class SettingsModel : INotifyPropertyChanged
    {
        public static SettingsModel Instance { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void Init()
        {
            try
            {
                Instance = Load();
            }
            catch
            {
                // デフォルト設定で生成
                Instance = new SettingsModel();
            }
        }

        private static readonly string path = Path.Combine(Environment.CurrentDirectory, "hohoemi.settings");

        private static SettingsModel Load()
        {
            var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));

            return new SettingsModel()
            {
                SelectedScreenIndex = 0,
                RejectNaming = bool.Parse(settings[nameof(RejectNaming)]),
                UserName = settings[nameof(UserName)],
                CommentColorR = byte.Parse(settings[nameof(CommentColorR)]),
                CommentColorG = byte.Parse(settings[nameof(CommentColorG)]),
                CommentColorB = byte.Parse(settings[nameof(CommentColorB)]),
                SelectedFontSize = int.Parse(settings[nameof(SelectedFontSize)])
            };
        }

        public void Save()
        {
            // ボタン連打されたくないので、同期で保存します。
            // そんな時間もかからないと信じて。。。
            try
            {
                var settings = new Dictionary<string, string>(){
                    {nameof(SelectedScreenIndex), SelectedScreenIndex.ToString()},
                    {nameof(RejectNaming), RejectNaming.ToString()},
                    {nameof(UserName), UserName},
                    {nameof(CommentColorR), CommentColorR.ToString()},
                    {nameof(CommentColorG), CommentColorG.ToString()},
                    {nameof(CommentColorB), CommentColorB.ToString()},
                    {nameof(SelectedFontSize), SelectedFontSize.ToString()}};

                File.WriteAllText(path, JsonSerializer.Serialize(settings));
            }
            catch
            {
                // do nothing
            }
        }


        private SettingsModel()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// 画面設定(ここは設定ファイルに落とさない)
        ////////////////////////////////////////////////////////////////////////////////////
        private int selectedScreenIndex = 0; // Index 0ってPrimaryScreenだよね。。たぶん。。
        public int SelectedScreenIndex
        {
            set
            {
                if (selectedScreenIndex != value)
                {
                    selectedScreenIndex = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedScreenIndex)));
                }
            }

            get => selectedScreenIndex;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(ユーザ名関係)
        ////////////////////////////////////////////////////////////////////////////////////
        private bool rejectNaming = false;
        public bool RejectNaming
        {
            set
            {
                if (rejectNaming != value)
                {
                    rejectNaming = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RejectNaming)));
                }
            }

            get => rejectNaming;
        }

        private string userName = Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        public string UserName
        {
            set
            {
                if (userName != value)
                {
                    userName = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                }
            }

            get => userName;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(コメント色)
        ////////////////////////////////////////////////////////////////////////////////////
        private byte commentColorR = 0;
        public byte CommentColorR
        {
            set
            {
                if (commentColorR != value)
                {
                    commentColorR = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorR)));
                }
            }

            get => commentColorR;
        }

        private byte commentColorG = 0;
        public byte CommentColorG
        {
            set
            {
                if (commentColorG != value)
                {
                    commentColorG = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorG)));
                }
            }

            get => commentColorG;
        }

        private byte commentColorB = 0;
        public byte CommentColorB
        {
            set
            {
                if (commentColorB != value)
                {
                    commentColorB = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentColorB)));
                }
            }

            get => commentColorB;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// ユーザ設定(フォントサイズ)
        ////////////////////////////////////////////////////////////////////////////////////
        private int selectedFontSize = 20;
        public int SelectedFontSize
        {
            set
            {
                if (selectedFontSize != value)
                {
                    selectedFontSize = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontSize)));
                }
            }

            get => selectedFontSize;
        }

        public IReadOnlyList<int> FontSizes
        {
            get => Enumerable.Range(6, 24).ToList(); // VSのフォントサイズ設定がこうでした
        }
    }
}