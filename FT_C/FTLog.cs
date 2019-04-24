using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;                   // トレース機能

namespace FT.C
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    public class FTLog
    {

        #region<関数　エラーの文字　作成>

        /// <summary>
        /// Nullエラーの文字を作成
        /// </summary>
        /// <param name="e">エラー情報</param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static string GetString(Exception e)
        {
            string outString = e.Message;
            outString += "\n【クラス名】\n" + e.Source;
            outString += "\n\n【関数名】\n" + e.Message;
            outString += "\n\n【関数の呼び出し履歴】\n" + e.StackTrace;
            return outString;
        }

        /// <summary>
        /// Nullエラーの文字を作成
        /// </summary>
        /// <param name="eNull">エラー情報</param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static string GetString(NullReferenceException eNull)
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

        #region<関数　ログを記録>

        /// <summary>
        /// ログを記録
        /// </summary>
        /// <param name="fullPass">ファイルパス(拡張子付き)</param>
        /// <param name="contents">書き込むログ</param>
        public static void Write(string fullPass, string contents)
        {
            FT.C.TXT.WriteText(fullPass, contents);
        }


        /// <summary>
        /// エラーログの書き出し(exeのパスに出力)
        /// ※すでにファイルが存在する場合は削除
        /// </summary>
        /// <param name="contents">文字列</param>
        /// <returns></returns>
        public static void WriteText_ErrLog_ToExe(string contents)
        {
            Trace.WriteLine(contents);

            string s日付 = DateTime.Now.ToString() + Environment.NewLine;

            System.IO.File.WriteAllText(FT.C.FTSystem.GetExeFilePass() + @"\ErrLog.txt", s日付 + contents);
        }


        /// <summary>
        /// エラーログの書き出し(exeのパスに出力)
        /// ※すでにファイルが存在する場合は削除
        /// </summary>
        /// <param name="e">エラー情報</param>
        /// <returns></returns>
        public static string WriteText_ErrLog_ToExe(Exception e)
        {
            string contents = GetString(e);

            // ログ書き出し
            WriteText_ErrLog_ToExe(contents);

            return contents;
        }

        /// <summary>
        /// エラーログの書き出し(exeのパスに出力)
        /// ※すでにファイルが存在する場合は削除
        /// </summary>
        /// <param name="e">エラー情報</param>
        /// <returns></returns>
        public static string WriteText_ErrLog_ToExe(NullReferenceException e)
        {
            string contents = GetString(e);

            // ログ書き出し
            WriteText_ErrLog_ToExe(contents);

            return contents;
        }

        /// <summary>
        /// エラーログの書き出し(exeのパスに出力)
        /// ※すでにファイルが存在する場合は削除
        /// </summary>
        /// <param name="e">エラー情報</param>
        /// <returns></returns>
        public static string WriteText_ErrLog_ToExe(MissingMethodException e)
        {
            string contents = GetString(e);

            // ログ書き出し
            WriteText_ErrLog_ToExe(contents);

            return contents;
        }

        #endregion

    }
}
