using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using hohoemi.Model.Comment;
using hohoemi.Model;

namespace hohoemi
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Init(object sender, StartupEventArgs e)
        {
            // アプリの起動にまつわるものはココで一回初期化しておく
            // シングルトンは全部ここかな
            try
            {
                CommentManager.Init();
                SettingsModel.Init();
            }
            catch
            {
                MessageBox.Show($"初期化に失敗しました。{Environment.NewLine}{Environment.CurrentDirectory}\\hohoemi.configを確認してください。");
                Current.Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                CommentManager.Destory();
            }
            catch
            {
                // もう終わりなので気にしない
            }
        }
    }
}
