using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{

    /// <summary>
    /// データ属性クラス
    /// </summary>
    public class FTDataAttribute : Attribute, IDisposable
    {

        #region<内部変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<プロパティ>

        /// <summary>品種</summary>
        public bool IsKind { get; set; }

        #endregion

        #region<コンストラクタ>

        /// <summary>コンストラクタ</summary>
        /// <param name="Kind">品種フラグ</param>
        /// <remarks></remarks>
        public FTDataAttribute(bool Kind)
        {
            this.IsKind = Kind;
        }

        #endregion

        #region<関数　一致するデータを取得>

        /// <summary>
        /// 一致する言語データを取得する
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public bool GetString(FT.C.ENM.Data lang)
        {
            bool s;
            switch (lang)
            {
                case ENM.Data.Kind:
                    s = IsKind;
                    break;

                default:
                    s = false;
                    break;
            }
            return s;
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
    /// データ属性 拡張クラス
    /// </summary>
    public static class DataExpansion
    {

        #region <関数　クラス言語拡張 処理>

        /// <summary>
        /// 列挙値の言語名を返す（言語指定可）
        /// </summary>
        /// <param name="eValue">列挙値</param>
        /// <param name="item">言語コード</param>
        /// <returns>指定言語での言語文字列</returns>
        public static object ToLang(this Enum eValue, ENM.Data item)
        {

            var atbLang = FT.C.ENM.GetAttribute<FTDataAttribute>(eValue);
            {
                switch (item)
                {
                    case ENM.Data.Kind: return atbLang.IsKind;
                    default: return "";
                }
            }
        }

        #endregion

        #region <拡張　品種フラグ　>

        /// <summary>
        /// 品種フラグ　拡張クラス
        /// </summary>
        /// <param name="da">項目</param>
        /// <returns></returns>
        public static bool IsKind(this FTDataAttribute da)
        {
            using (var atbLang = FT.C.ENM.GetAttribute<FTDataAttribute>(ENM.Data.Kind))
            {
                if (atbLang == null)
                    return false;
                return atbLang.IsKind;
            }
        }

        #endregion

    }

}
