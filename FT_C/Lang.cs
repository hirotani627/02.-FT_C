using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{

    /// <summary>
    /// 言語クラス
    /// </summary>
    public static class FT_Lang
    {

        #region <関数　変更フラグ>

        /// <summary>
        /// 数値からバイリンガル列挙値にキャスト
        /// </summary>
        /// <param name="Value">バイリンガル値</param>
        /// <returns>バイリンガル列挙値</returns>
        public static ENM.Lang CBilingual(int Value)
        {

            ENM.Lang eBilin = ENM.Lang.Jpn;

            switch (Value)
            {
                case (int)0: eBilin = ENM.Lang.Jpn; break;
                case (int)1: eBilin = ENM.Lang.Eng; break;
                case (int)2: eBilin = ENM.Lang.Oth; break;
            }

            return eBilin;

        }

        /// <summary>
        /// バイリンガル列挙値に対する文字列を返す
        /// </summary>
        /// <param name="eBilin"></param>
        /// <returns></returns>
        public static string BilingualStr(ENM.Lang eBilin)
        {
            string strBilin = "";

            switch (eBilin)
            {
                case ENM.Lang.Jpn: strBilin = "Jpn"; break;
                case ENM.Lang.Eng: strBilin = "Eng"; break;
                case ENM.Lang.Oth: strBilin = "Oth"; break;
            }

            return strBilin;
        }

        #endregion

        #region<関数　変換　コンバート>

        /// <summary>
        /// 文字列からバイリンガル列挙値にキャスト
        /// </summary>
        /// <param name="Value">バイリンガル値</param>
        /// <returns>バイリンガル列挙値</returns>
        public static ENM.Lang CBilingual(string Value)
        {

            ENM.Lang eBilin = ENM.Lang.Jpn;

            switch (Value)
            {
                case "0": eBilin = ENM.Lang.Jpn; break;
                case "1": eBilin = ENM.Lang.Eng; break;
                case "2": eBilin = ENM.Lang.Oth; break;
            }

            return eBilin;

        }


        /// <summary>
        /// バイリンガル値からint値に変換
        /// </summary>
        /// <param name="Value">バイリンガル値</param>
        /// <returns>int値</returns>
        public static int CInt(ENM.Lang Value)
        {

            int iValue = 0;

            switch (Value)
            {
                case ENM.Lang.Jpn: iValue = 0; break;
                case ENM.Lang.Eng: iValue = 1; break;
                case ENM.Lang.Oth: iValue = 2; break;
            }

            return iValue;

        }

        #endregion

    }

    /// <summary>
    /// 言語文字列構造
    /// </summary>
    public class LangString
    {

        #region<内部変数>

        /// <summary>日本語</summary>
        public string Jpn;

        /// <summary>英語</summary>
        public string Eng;

        /// <summary>その他</summary>
        public string Oth;

        /// <summary>中国語(台湾)</summary>
        public string ChineseTW;

        #endregion

        #region<コンストラクタ>

        /// <summary>コンストラクタ</summary>
        /// <remarks></remarks>
        public LangString()
        {
            Jpn = Eng = Oth = ChineseTW = String.Empty;
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="JpnStr">日本語</param>
        /// <param name="EngStr">英語</param>
        /// <param name="OthStr">その他</param>
        /// <remarks></remarks>
        public LangString(string JpnStr, string EngStr, string OthStr)
        {
            this.Jpn = JpnStr;
            this.Eng = EngStr;
            this.Oth = OthStr;
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="JpnStr">日本語</param>
        /// <param name="EngStr">英語</param>
        /// <param name="OthStr">中国語(台湾)</param>
        /// <param name="ChineseTW"></param>
        /// <remarks></remarks>
        public LangString(string JpnStr, string EngStr, string OthStr, string ChineseTW)
        {
            this.Jpn = JpnStr;
            this.Eng = EngStr;
            this.Oth = OthStr;
            this.ChineseTW = ChineseTW;
        }


        #endregion

        #region<関数　一致する言語データを取得>

        /// <summary>
        /// 一致する言語データを取得する
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public string GetString(FT.C.ENM.Lang lang)
        {
            string s;
            switch (lang)
            {
                case ENM.Lang.Jpn:
                    s = Jpn;
                    break;

                case ENM.Lang.Eng:
                    s = Eng;
                    break;

                case ENM.Lang.Oth:
                    s = Oth;
                    break;

                case ENM.Lang.ChineseTW:
                    s = ChineseTW;
                    break;

                default:
                    s = "";
                    break;
            }
            return s;
        }

        #endregion

    }

    /// <summary>
    /// 言語　カスタム属性クラス
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// </remarks>
    /// 
    public class LANG : Attribute, IDisposable
    {

        #region<内部変数>

        /// <summary>言語情報</summary>
        private LangString mLang;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<プロパティ>

        /// <summary>日本語プロパティ</summary>
        /// <remarks></remarks>
        public string Jpn { get { return mLang.Jpn; } }

        /// <summary>英語プロパティ</summary>
        /// <remarks></remarks>
        public string Eng { get { return mLang.Eng; } }

        /// <summary>その他プロパティ</summary>
        /// <remarks></remarks>
        public string Oth { get { return mLang.Oth; } }

        /// <summary>全言語プロパティ</summary>
        /// <remarks></remarks>
        public LangString Langs { get { return mLang; } }

        #endregion

        #region<コンストラクタ>

        /// <summary>コンストラクタ</summary>
        /// <param name="JpnStr">日本語</param>
        /// <param name="EngStr">英本語</param>
        /// <param name="OthStr">その他</param>
        /// <remarks></remarks>
        public LANG(string JpnStr, string EngStr, string OthStr)
        {
            mLang = new LangString(JpnStr, EngStr, OthStr);
        }

        #endregion

        #region<Dispose>

        /// <summary>
        /// リソースの解放
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected virtual void Dispose(bool disposing)
        {
            // 既にリソースが破棄されている場合は何もしない
            if (disposed) return;

            try
            {
                mLang = null;
            }
            finally
            {
                // リソースは破棄されている
                disposed = true;
            }
        }

        #endregion

    }

    /// <summary>
    /// 言語プロパティ　インターフェース
    /// </summary>
    public interface ILang
    {
        /// <summary>言語 プロパティ</summary>
        FT.C.ENM.Lang Lang { get; set; }

        /// <summary>言語変更有効 プロパティ</summary>
        bool LangCange { get; set; }
    }

    /// <summary>
    /// 言語詳細設定　インターフェース
    /// </summary>
    public interface ILangProperty : ILang
    {

        /// <summary>日本語</summary>
        string Jpn { get; set; }

        /// <summary>英語</summary>
        string Eng { get; set; }

        /// <summary>その他</summary>
        string Oth { get; set; }

        /// <summary>言語情報の一括設定</summary>
        void SetLangs(FT.C.LangString langString);

    }

}
