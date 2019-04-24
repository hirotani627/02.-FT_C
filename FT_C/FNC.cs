using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// 機能クラス
    /// </summary>
    public class FNC
    {

        #region<関数　自社制作オブジェクト>

        ///// <summary>
        ///// 自社制作オブジェクトかを返す
        ///// </summary>
        ///// <param name="ctrObj">確認するオブジェクトのインスタンス</param>
        ///// <returns>自社制作オブジェクト番号</returns>
        //public static ENM.MyObj CheckMyObj(Control ctrObj)
        //{

        //    ENM.MyObj eMyObj = ENM.MyObj.NON;
        //    string[] ObjPath = ctrObj.GetType().ToString().Split('.');	// オブジェクトパスを"."で分解

        //    if ("FT_C" == ObjPath[0] || "FT_MC" == ObjPath[0])
        //    {

        //        string[] SplitObj = ObjPath[1].Split('_');				// オブジェクト名を"_"で分解
        //        string ObjName = SplitObj[0] + "_" + SplitObj[1];		// 基底オブジェクト名を生成

        //        switch (ObjName)
        //        {
        //            // ボタンコントロール
        //            case CST.MY_BTN: eMyObj = ENM.MyObj.BTN; break;
        //            // チェックボックスコントロール
        //            case CST.MY_CKBX: eMyObj = ENM.MyObj.CKBX; break;
        //            // コンボボックスコントロール
        //            case CST.MY_COBX: eMyObj = ENM.MyObj.COBX; break;
        //            // フレームコントロール
        //            case CST.MY_FRAM: eMyObj = ENM.MyObj.FRAM; break;
        //            // ラベルコントロール
        //            case CST.MY_LABL: eMyObj = ENM.MyObj.LABL; break;
        //            // 数値コントロール
        //            case CST.MY_NUM: eMyObj = ENM.MyObj.NUM; break;
        //            // オプションボタンコントロール
        //            case CST.MY_OPT: eMyObj = ENM.MyObj.OPT; break;
        //            // タブコントロール
        //            case CST.MY_TAB: eMyObj = ENM.MyObj.TAB; break;
        //        }

        //    }

        //    return eMyObj;

        //}


        ///// <summary>
        ///// 自社制作オブジェクトのバイリンガル設定
        ///// </summary>
        ///// <param name="ctrObj">設定しようとしているオブジェクトのインスタンス</param>
        ///// <param name="eBilingual">バイリンガルコード</param>
        //public static void Set_MyObj_Bilingual(Control ctrObj, ENM.Lang eBilingual)
        //{

        //    // 自社制作のオブジェクトか確認
        //    ENM.MyObj eMyObj = CheckMyObj(ctrObj);

        //    if (ENM.MyObj.NON == eMyObj) return;	// 自社製でなければ抜ける

        //    switch (eMyObj)
        //    {
        //        // ボタンコントロール
        //        case ENM.MyObj.BTN: if (ENM.Lang.EndMark != ((C_BTN)ctrObj).Bilingual)
        //            {
        //                ((C_BTN)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //        // チェックボックスコントロール
        //        case ENM.MyObj.CKBX: if (ENM.Lang.EndMark != ((C_CKBX)ctrObj).Bilingual)
        //            {
        //                ((C_CKBX)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //        // フレームコントロール
        //        case ENM.MyObj.FRAM: if (ENM.Lang.EndMark != ((C_FRAM)ctrObj).Bilingual)
        //            {
        //                ((C_FRAM)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //        // ラベルコントロール
        //        case ENM.MyObj.LABL: if (ENM.Lang.EndMark != ((C_LABL)ctrObj).Bilingual)
        //            {
        //                ((C_LABL)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //        // オプションボタンコントロール
        //        case ENM.MyObj.OPT: if (ENM.Lang.EndMark != ((C_OPT)ctrObj).Bilingual)
        //            {
        //                ((C_OPT)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //        // タブコントロール
        //        case ENM.MyObj.TAB: if (ENM.Lang.EndMark != ((C_TAB)ctrObj).Bilingual)
        //            {
        //                ((C_TAB)ctrObj).Bilingual = eBilingual;
        //            }
        //            break;
        //    }

        //}



        ///// <summary>
        ///// 自社制作オブジェクトの使用可否を設定
        ///// </summary>
        ///// <param name="ctrObj">設定しようとしているオブジェクトのインスタンス</param>
        ///// <param name="eAuthority">ユーザ権限コード</param>
        ///// <param name="blLotStart">ロットスタートフラグ</param>
        //public void Set_MyObj_Lock(Control ctrObj, ENM.Authority eAuthority, bool blLotStart)
        //{

        //    // 自社制作のオブジェクトか確認
        //    ENM.MyObj eMyObj = CheckMyObj(ctrObj);

        //    if (ENM.MyObj.NON == eMyObj) return;	// 自社製でなければ抜ける

        //    switch (eMyObj)
        //    {
        //        // ボタンコントロール
        //        case ENM.MyObj.BTN: if (ENM.Authority.EndMark != ((C_BTN)ctrObj).EditLevel)
        //            {
        //                if (((C_BTN)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_BTN)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_BTN)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_BTN)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_BTN)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_BTN)ctrObj).Authority = eAuthority;
        //            break;
        //        // チェックボックスコントロール
        //        case ENM.MyObj.CKBX: if (ENM.Authority.EndMark != ((C_CKBX)ctrObj).EditLevel)
        //            {
        //                if (((C_CKBX)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_CKBX)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_CKBX)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_CKBX)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_CKBX)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_CKBX)ctrObj).Authority = eAuthority;
        //            break;
        //        // コンボボックスコントロール
        //        case ENM.MyObj.COBX: if (ENM.Authority.EndMark != ((C_COBX)ctrObj).EditLevel)
        //            {
        //                if (((C_COBX)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_COBX)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_COBX)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_COBX)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_COBX)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_COBX)ctrObj).Authority = eAuthority;
        //            break;
        //        // フレームコントロール
        //        case ENM.MyObj.FRAM: if (ENM.Authority.EndMark != ((C_FRAM)ctrObj).EditLevel)
        //            {
        //                if (((C_FRAM)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_FRAM)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_FRAM)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_FRAM)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_FRAM)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_FRAM)ctrObj).Authority = eAuthority;
        //            break;
        //        // 数値コントロール
        //        case ENM.MyObj.NUM: if (ENM.Authority.EndMark != ((C_NUM)ctrObj).EditLevel)
        //            {
        //                if (((C_NUM)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_NUM)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_NUM)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_NUM)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_NUM)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_NUM)ctrObj).Authority = eAuthority;
        //            break;
        //        // オプションボタンコントロール
        //        case ENM.MyObj.OPT: if (ENM.Authority.EndMark != ((C_OPT)ctrObj).EditLevel)
        //            {
        //                if (((C_OPT)ctrObj).EditLevel <= eAuthority)
        //                {
        //                    if (true == ((C_OPT)ctrObj).SLock && true == blLotStart)
        //                    {
        //                        ((C_OPT)ctrObj).Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((C_OPT)ctrObj).Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    ((C_OPT)ctrObj).Enabled = false;
        //                }
        //            }
        //            ((C_OPT)ctrObj).Authority = eAuthority;
        //            break;
        //    }

        //}

        #endregion

        #region<関数 マイドキュメントのパスを返す>

        /// <summary>
        /// マイドキュメントのパスを返す
        /// </summary>
        /// <returns></returns>
        public static string MyDocDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        #endregion

        #region<関数　フォーテクノス定義ファイルパス>

        /// <summary>
        /// 本システムフォルダパスを返す	
        /// </summary>
        /// <returns></returns>
        public static string MySystemDir()
        {
            return MyDocDir() + @"\FourTechnos";
        }


        /// <summary>
        /// メカデータフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McDataRootDir()
        {
            return MySystemDir() + @"\DATA_Machine";
        }


        /// <summary>
        /// 品種データフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string RecipeDir()
        {
            return MySystemDir() + @"\DATA_Recipie";
        }


        /// <summary>
        /// メカデータフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McDataDir()
        {
            return McDataRootDir() + @"\DATA";
        }

        
        /// <summary>
        /// メカデータの情報部フォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McInfoDir()
        {
            return McDataRootDir() + @"\INFO";
        }

        /// <summary>
        /// 動画フォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McMovieDir()
        {
            return McDataRootDir() + @"\Movie";
        }


        /// <summary>
        /// 画像フォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McPictureDir()
        {
            return McDataRootDir() + @"\Picture";
        }


        /// <summary>
        /// ユニット画像フォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McUnitPictureDir()
        {
            return McPictureDir() + @"\Unit";
        }

        /// <summary>
        /// 音声フォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McVoiceDir()
        {
            return McDataRootDir() + @"\Voice";
        }


        /// <summary>
        /// メカＥｘｅフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McExeDir()
        {
            return MySystemDir() + @"\Exe\Machine";
        }


        /// <summary>
        /// メカＥｘｅ設定ファイルフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McExeIniDir()
        {
            return McExeDir() + @"\INI";
        }


        /// <summary>
        /// ＲＴＡファイルフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McExeRtaDir()
        {
            return McExeDir() + @"\RTA";
        }

        /// <summary>
        /// Windowsアプリケーションファイルフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string McExeWinDir()
        {
            return McExeDir() + @"\WIN";
        }

        /// <summary>
        /// 認識Ｅｘｅフォルダパスを返す
        /// </summary>
        /// <returns></returns>
        public static string RecExeDir()
        {
            return MySystemDir() + @"\Exe\Recognition";
        }

        #endregion

        #region<関数 ファイルから品種名を取得する>

        /// <summary>
        /// ファイルから品種名を取得する
        /// </summary>
        /// <returns></returns>
        public static bool GetRecipeNames(out string[]s)
        {
            string pass = FT.C.FNC.RecipeDir();

            return DIR.GetFolderNames(pass, out s);
        }

        #endregion

        #region<関数　エラーの文字　作成>

        /// <summary>
        /// Nullエラーの文字を作成
        /// </summary>
        /// <param name="e"></param>
        private static string GetErrString(Exception e)
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
        /// <param name="eNull"></param>
        /// <returns>
        /// この関数はMainプログラム内に配置することをお勧めします
        /// </returns>
        private string GetErrString(NullReferenceException eNull)
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
        public string GetErrString(MissingMethodException err)
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
