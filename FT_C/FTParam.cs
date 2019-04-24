using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{


    ///// <summary>
    ///// パラメータクラスカスタム属性
    ///// </summary>
    //public class ParaDataAttribute : Attribute
    //{

    //    #region<プロパティ>

    //    /// <summary>No</summary>
    //    public int No { get; set; }

    //    /// <summary>品種</summary>
    //    public bool IsKind { get; set; }

    //    /// <summary>オペレータ変更可</summary>
    //    public bool IsChangeOpe { get; set; }

    //    #endregion

    //    #region<コンストラクタ>

    //    /// <summary>
    //    /// コンストラクタ
    //    /// </summary>
    //    /// <param name="JpnStr">日本語</param>
    //    /// <param name="EngStr">英本語</param>
    //    /// <param name="OthStr">その他</param>
    //    /// <param name="Kind">品種</param>
    //    /// <param name="IsChangeOpe">オペレータ変更可</param>
    //    /// <remarks></remarks>
    //    public ParaDataAttribute(string JpnStr, string EngStr, string OthStr, bool Kind, bool IsChangeOpe)
    //    {
    //        this.IsKind = Kind;
    //        this.IsChangeOpe = IsChangeOpe;
    //    }

    //    #endregion

    //}

    // /// <summary>
    // /// WBPmac　共通クラス
    // /// ※言語とアドレスをまとめて定義
    // /// </summary>
    // public static class C_WB
    // {

    //     #region <関数　クラス言語拡張 言語>

    //     /// <summary>
    //     /// 列挙値の日本語名を返す
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>日本語文字列</returns>
    //     ///
    //     /// <remarks></remarks>
    //     /// 
    //     public static string ToJpn(this Enum eValue)
    //     {

    //         using (var atbLang = GetAttribute(eValue))
    //         {
    //             if (atbLang == null)
    //                 return "";
    //             return atbLang.Jpn;
    //         }

    //     }

    //     /// <summary>
    //     /// 列挙値の英語名を返す
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>英語文字列</returns>
    //     /// 
    //     /// <remarks></remarks>
    //     /// 
    //     public static string ToEng(this Enum eValue)
    //     {

    //         using (var atbLang = GetAttribute(eValue))
    //         {
    //             if (atbLang == null)
    //                 return "";
    //             return atbLang.Eng;
    //         }

    //     }

    //     /// <summary>
    //     /// 列挙値のその他の言語名を返す
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>その他の言語文字列</returns>
    //     /// 
    //     /// <remarks></remarks>
    //     /// 
    //     public static string ToOth(this Enum eValue)
    //     {

    //         using (var atbLang = GetAttribute(eValue))
    //         {
    //             if (atbLang == null)
    //                 return "";
    //             return atbLang.Oth;
    //         }

    //     }

    //     #endregion

    //     #region <関数　クラス言語拡張 処理>

    //     /// <summary>
    //     /// 列挙値のアドレスを返す
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>アドレス</returns>
    //     ///
    //     /// <remarks></remarks>
    //     /// 
    //     public static string ToAddres(this Enum eValue)
    //     {

    //         using (var atbLang = GetAttribute(eValue))
    //         {
    //             if (atbLang == null)
    //                 return "";
    //             return atbLang.Jpn;
    //         }

    //     }

    //     /// <summary>
    //     /// 内容を取得 
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>言語属性値</returns>
    //     /// 
    //     /// <remarks></remarks>
    //     /// 
    //     private static LANG GetAttribute(Enum eValue)
    //     {

    //         Type enmType = eValue.GetType();                                                                            // 列挙値のタイプ
    //         string strName = Enum.GetName(enmType, eValue);                                                             // 選択しているEnumのメンバー名を取得する

    //         var atbLang = enmType.GetField(strName).GetCustomAttributes(typeof(LANG), false);    // 言語属性群取得 : ※GetCustomAttributesはMemberInfoクラスでカスタム属性の配列を返すメソッドです
    //         foreach (LANG n in atbLang)
    //         {
    //             return n;
    //         }

    //         return null;
    //         //return ((T[])(atbLang)).FirstOrDefault();                                                                            // FirstOrDefault : 配列の先頭を返す

    //     }

    //     /// <summary>全言語を一括で返す</summary>
    //     /// <remarks></remarks>
    //     public static LangString ToLangs(this Enum eValue)
    //     {
    //         LANG atbLang = GetAttribute(eValue);
    //         return atbLang.Langs;
    //     }

    //     #endregion

    //     #region <関数　クラス言語拡張 追加分>

    //     /// <summary>
    //     /// 列挙値の言語名を返す（言語指定可）
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <param name="eLang">言語コード</param>
    //     /// <returns>指定言語での言語文字列</returns>
    //     /// 
    //     /// <remarks></remarks>
    //     /// 
    //     public static string ToLang(this Enum eValue, ENM.Lang eLang)
    //     {

    //         using (LANGWB atbLang = (LANGWB)GetAttribute<LANGWB>(eValue))
    //         {
    //             if (atbLang == null)
    //                 throw (new NotImplementedException("呼び出したENUMに【LANG2】が定義されていません"));

    //             switch (eLang)
    //             {
    //                 case ENM.Lang.Jpn: return atbLang.Jpn;
    //                 case ENM.Lang.Eng: return atbLang.Eng;
    //                 case ENM.Lang.Oth: return atbLang.Oth;
    //                 default: return atbLang.Addres;
    //             }
    //         }
    //     }

    //     /// <summary>
    //     /// 内容を取得 
    //     /// </summary>
    //     /// 
    //     /// <param name="eValue">列挙値</param>
    //     /// <returns>言語属性値</returns>
    //     /// 
    //     /// <remarks></remarks>
    //     /// 
    //     private static object GetAttribute<T>(Enum eValue)
    //     {

    //         Type enmType = eValue.GetType();                                                                            // 列挙値のタイプ
    //         string strName = Enum.GetName(enmType, eValue);                                                             // 選択しているEnumのメンバー名を取得する

    //         var atbLang = enmType.GetField(strName).GetCustomAttributes(typeof(T), false);    // 言語属性群取得 : ※GetCustomAttributesはMemberInfoクラスでカスタム属性の配列を返すメソッドです
    //         foreach (T n in atbLang)
    //         {
    //             return n;
    //         }

    //         return null;
    //         //return ((T[])(atbLang)).FirstOrDefault();                                                                            // FirstOrDefault : 配列の先頭を返す

    //     }

    //     #endregion

    //     /// <summary>
    //     /// 動作パラメータ定義
    //     /// </summary>
    //     public enum eAddresConstants
    //     {

    //         /// <summary>
    //         /// 1st ワークタッチ位置　X(P1000)
    //         /// </summary>
    //         [LANGWB("1st ワークタッチ位置 X", "1st ワークタッチ位置 X", "1st ワークタッチ位置 X", "P1000")]
    //         P_1stWorkTouchX = 0,

    //         /// <summary>
    //         /// 1st ワークタッチ位置　Y(P1001)
    //         /// </summary>
    //         [LANGWB("1st ワークタッチ位置 Y", "1st ワークタッチ位置 Y", "1st ワークタッチ位置 Y", "P1001")]
    //         P_1stWorkTouchY,

    //         /// <summary>
    //         /// 1st ワークタッチ位置　Z(P1002)
    //         /// </summary>
    //         [LANGWB("1st ワークタッチ位置 Z", "1st ワークタッチ位置 Z", "1st ワークタッチ位置 Z", "P1002")]
    //         P_1stWorkTouchZ,

    //         /// <summary>エンドマーク</summary>
    //EndMark,
    //     }


}
