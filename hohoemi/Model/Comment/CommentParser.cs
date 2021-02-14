using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hohoemi.Model.Comment
{
    public static class CommentParser
    {
        static readonly Dictionary<string /* command string*/,
                                   Func<string /* sender */, string/* comment */, IEnumerable<CommentModel> /* parser */ >> pasers;

        static CommentParser()
        {
            pasers = new Dictionary<string, Func<string, string, IEnumerable<CommentModel>>>()
            {   // コマンドを追加するときはここにコマンド名とパーサを追加していく 
                { "@top", ParseTopCommand },
                { "@bottom", ParseBottomCommand }
            };
        }

        public static IEnumerable<CommentModel> Parse(string sender, string cmnt)
        {
            // コマンドはコメントの先頭に来ないとダメ
            // 半角/全角スペースぐらいはとってあげるけど。。
            foreach (var command in pasers.Keys)
            {
                var comment = Regex.Replace(cmnt, $"^[ 　]*{command}[ 　]", "");

                // 正規表現で置換されていたら(コマンドを含んでいたら)差異が出るはず
                if (comment != cmnt)
                {
                    return pasers[command](sender, comment);
                }
            }

            // コマンドなし。普通のコメントを生成
            return new CommentModel[] { new CommentModel(CommentType.TEXT, BehaviorType.LeftToRight, sender, cmnt) };
        }

        private static IEnumerable<CommentModel> ParseTopCommand(string sender, string comment)
        {
            return new CommentModel[] { new CommentModel(CommentType.TEXT, BehaviorType.TopCenterStaying, sender, comment) };
        }

        private static IEnumerable<CommentModel> ParseBottomCommand(string sender, string comment)
        {
            return new CommentModel[] { new CommentModel(CommentType.TEXT, BehaviorType.BottomCenterStaying, sender, comment) };
        }
    }
}
