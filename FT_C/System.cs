using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// FTシステムクラス
    /// </summary>
    public class FTSystem
    {

        /// <summary>マシンデータフォルダ名</summary>
        public static string PassMachineData = @"\DATA_Machine";

        /// <summary>品種データフォルダ名</summary>
        public static string PassPecipeData = @"\DATA_Recipie";

        /// <summary>WBマシンデータフォルダ名</summary>
        public static string PassMachineDataWB = @"\MachineWB";

        /// <summary>開発用フォーテクノスフォルダーパス</summary>
        public const string PassFTPass_CS = @"\FourTechnos(C#)";

        /// <summary>EXEフォルダーパス</summary>
        public const string Pass_EXE = @"\EXE";

        /// <summary>認識ルートフォルダ</summary>
        public const string Pass_RECOG = @"\Recognition";

        /// <summary>認識アプリフォルダ</summary>
        public const string Pass_FT_REC_App = @"\FT_REC";

        /// <summary>認識ルートフォルダ</summary>
        public const string Pass_FT_REC_PARA = @"\PARA";

        /// <summary>認識ルートフォルダ</summary>
        public const string Pass_FT_REC_Image = @"\Images";

        
        /// <summary>認識アプリフォルダ</summary>
        public const string Pass_RecWPF_APP = @"\FT_RecoWPF";

        /// <summary>DATA_Machineフォルダ名</summary>
        public const string FOLDER_MACHINE = @"\DATA_Machine";

        /// <summary>DATA_Recipieフォルダ名</summary>
        public const string FOLDER_RECIPIE = @"\DATA_Recipie";

        /// <summary>\Machine.iniファイル名</summary>
        public const string MASHINE_INI = @"\Machine.ini";


        #region<関数 EXEを実行しているファイルパスを取得>

        /// <summary>
        /// EXEを実行しているファイルパスを取得 (GetExeFilePassを使ってください)
        /// </summary>
        /// <returns></returns>
        [System.ObsoleteAttribute]
        public static string GetEXELootPass()
        { 
            string pass = System.Windows.Forms.Application.ExecutablePath;
            pass = FTString.GetString_Front(pass, @"\");                      // ファイルパスのみにする
            return pass;
        }

        /// <summary>
        /// EXEを実行しているファイルパスを取得 (GetExeFilePassを使ってください)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [System.ObsoleteAttribute]
        public static string GetEXELootPass(string p)
        {
            string pass = System.Windows.Forms.Application.ExecutablePath;
            return FTString.GetDirectoryName(pass);
        }

        /// <summary>
        /// EXEを実行しているファイルパスを取得
        /// </summary>
        /// <returns></returns>
        public static string GetExeFilePass()
        {
            string pass = System.Windows.Forms.Application.ExecutablePath;
            return FTString.GetDirectoryName(pass);
        }

        #endregion

        #region<関数 マイドキュメントパスを取得>

        /// <summary>
        /// マイドキュメントパスを取得
        /// </summary>
        /// <returns></returns>
        public static string GetMyDocmentPass()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);      // マイドキュメントを取得
        }

        #endregion

        #region<関数 デスクトップパスを取得>

        /// <summary>
        /// デスクトップパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetDesktopPass()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return desktop;
        }


        #endregion

        #region<関数 FourTechnosパスを取得>

        /// <summary>
        /// FourTechnosパスを取得(C#)開発用
        /// </summary>
        /// <returns></returns>
        public static string GetFTPass_CS()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + PassFTPass_CS;
        }

        #endregion

        #region<関数　exeを起動>

        /// <summary>
        /// アプリケーションをキック（exeを起動）
        /// </summary>
        /// <param name="Path">ファイルパス</param>
        /// <returns></returns>
        public static bool ApriExec(string Path)
        {

            // ファイルパスがない？
            if ("" == Path) return false;

            try
            {
                System.Diagnostics.Process.Start(Path);
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region<関数　年月日時分秒を取得>

        /// <summary>
        /// 年月日時分秒を取得
        /// </summary>
        /// <returns></returns>
        public static string GetTime()
        {
            System.DateTime tim = System.DateTime.Now;
            return tim.Year.ToString() + tim.Month + tim.Day + tim.Hour + tim.Minute + tim.Second;
        }

        #endregion

        #region<関数　エラーの文字　作成>

        /// <summary>
        /// Nullエラーの文字を作成
        /// </summary>
        /// <param name="eNull"></param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static string GetException(NullReferenceException eNull)
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
        public static string GetException(MissingMethodException err)
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

        /// <summary>
        /// メソッドが見つからなかったエラーの文字を作成
        /// </summary>
        /// <param name="e"> メソッドが見つからなかったエラー</param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        public static string GetException(Exception e)
        {
            string outString = e.Message;
            outString += "\n【クラス名】\n" + e.Source;
            outString += "\n\n【関数名】\n" + e.Message;
            outString += "\n\n【関数の呼び出し履歴】\n" + e.StackTrace;
            return outString;
        }

        #endregion

        #region<関数　定型文>

        /// <summary>定型文の取得　終了文字</summary>
        public static LangString GetMessege_Close()
        {
            return new LangString(
            "終了してもよろしいですか？",
            "Are you sure you want to exit the program", "");
        }

        /// <summary>定型文の取得　終了文字</summary>
        public static LangString GetMessege_CloseChek()
        {
            return new LangString(
            "プログラム確認",
            "Program confirmation", "");
        }

        #endregion

    }
}
