using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FT.C
{

    /// <summary>
    /// 単位定義クラス
    /// </summary>
    public static class Unit
    {

        /// <summary>
        /// 単位定義
        /// </summary>
        public enum Item
        {

            /// <summary>単位なし</summary>
            Non,

            /// <summary>回数</summary>
            Count,

            /// <summary>[長さ]ミリメートル(10^-3)</summary>
            mm,

            /// <summary>[長さ]マイクロメートル(10^-6)</summary>
            um,

            /// <summary>[速度]mm/s</summary>
            mm_s,

            /// <summary>[比率]パーセント</summary>
            Percent,
        }

        /// <summary>
        /// 列挙値の単位を返す
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>単位</returns>
        ///
        /// <remarks></remarks>
        /// 
        public static Unit.Item GetUnit(this Unit.Item eValue)
        {
            return GetAttribute<Unit.Item>(eValue);
        }

        /// <summary>
        /// 内容を取得 
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>言語属性値</returns>
        /// 
        /// <remarks></remarks>
        /// 
        public static T GetAttribute<T>(Unit.Item eValue)
        {

            Type enmType = eValue.GetType();                            // 列挙値のタイプ
            string strName = Enum.GetName(enmType, eValue);             // メンバー名を取得する

            // 言語属性群取得 : ※GetCustomAttributesはMemberInfoクラスでカスタム属性の配列を返すメソッドです
            var atbLang = enmType.GetField(strName).GetCustomAttributes(typeof(T), false);
            foreach (T n in atbLang)
            {
                return n;
            }

            return default(T);
        }

        ///// <summary>
        ///// クラス自体とクラス中の public メソッドの作者情報を取得する。
        ///// </summary>
        ///// <param name="t">クラスの Type</param>
        //public static void GetAllAuthors(FT_Unit.i t)
        //{
        //    GetAllAuthors(typeof(t));
        //}

        /// <summary>
        /// クラス自体とクラス中の public メソッドの作者情報を取得する。
        /// </summary>
        /// <param name="t">クラスの Type</param>
        public static void GetAllAuthors(Type t)
        {
            Console.Write("type name: {0}\n", t.Name);
            GetAuthors(t);

            foreach (MethodInfo info in t.GetMethods())
            {
                Console.Write("  method name: {0}\n", info.Name);
                GetAuthors(info);
            }
        }

        /// <summary>
        /// クラスやメソッドの作者情報を取得する。
        /// </summary>
        /// <param name="info">クラスやメソッドの MemberInfo</param>
        public static void GetAuthors(MemberInfo info)
        {
            Attribute[] authors = Attribute.GetCustomAttributes(
                info, typeof(Item));
            foreach (FT_Unit att in authors)
            {
                var a = att.Unit;
            }
        }
    }

    /// <summary>
    /// 単位　カスタム属性クラス
    /// </summary>
    public class FT_Unit: Attribute, IDisposable
    {

        #region<内部変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<プロパティ>

        /// <summary>値</summary>
        /// <remarks></remarks>
        public Unit.Item Unit { get { return mValue; } }
        /// <summary>値</summary>
        private Unit.Item mValue;

        #endregion

        #region<コンストラクタ>

        /// <summary>コンストラクタ</summary>
        /// <param name="value">単位</param>
        /// <remarks></remarks>
        public FT_Unit(Unit.Item value)
        {
            mValue = value;
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


}
