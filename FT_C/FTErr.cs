using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// エラー処理
    /// </summary>
    public static class FTErr
    {
        #region<関数　エラーの文字　作成>

        /// <summary>
        /// Nullエラーの文字を作成
        /// </summary>
        /// <param name="eNull"></param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static  string GetString(NullReferenceException eNull)
        {
            string outString = "Nullのデータを呼び出しました";
            outString += "\n【クラス名】\n" + eNull.Source;
            outString += "\n\n【関数名】\n" + eNull.Message;
            outString += "\n\n【関数の呼び出し履歴】\n" + eNull.StackTrace;
            return outString;
        }

        /// <summary>
        /// メソッドが見つからなかったエラーの文字を作成
        /// </summary>
        /// <param name="err"> メソッドが見つからなかったエラー</param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static string GetString(MissingMethodException err)
        {
            string outString = "DLL内の関数が見つかりません";
            outString += "\n【クラス名】\n" + err.Source;
            outString += "\n\n【関数名】\n" + err.Message;
            outString += "\n\n【関数の呼び出し履歴】\n" + err.StackTrace;

            //MessageBox.Show(err.Message);                // 名前空間付関数名の取得
            //MessageBox.Show(err.Source.ToString());      // アプリケーションのオブジェクト名の取得
            //MessageBox.Show(err.StackTrace.ToString());  // 呼び出し履歴の取得(at 改行で表示)
            //MessageBox.Show(err.TargetSite.ToString());  // 例外を出したメソッド名の表示
            return outString;
        }

        #endregion

    }
}
