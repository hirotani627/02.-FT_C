using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;           // トレース機能

namespace FT.C
{
    /// <summary>
    /// システム共通属性クラス
    /// </summary>
    public static class FTAttribute
    {

        #region<関数　クラス内のプロパティの属性を取得する>

        /// <summary>
        /// クラス内のプロパティの属性を取得する
        /// </summary>
        /// <typeparam name="T">クラス</typeparam>
        /// <typeparam name="G">プロパティにつけている情報クラス</typeparam>
        /// <returns></returns>
        public static MemberInfo[] GetPropatyAttribute<T, G>()
        {

            //TestClassクラスのTypeオブジェクトを取得する
            Type t = typeof(T);

            //メンバを取得する
            MemberInfo[] members = t.GetMembers(
                BindingFlags.Public |
                //BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            List < MemberInfo> l = new List<MemberInfo>();
            foreach (MemberInfo m in members)
            {
                var d = m.GetCustomAttributes(false);
                if (m.MemberType != MemberTypes.Constructor && m.MemberType != MemberTypes.Method)
                {
                    // クラス内のプロパティを取得する
                    G ra = (G)FTAttribute.GetAttribute<T, G>(m);
                    l.Add(m);
                }
            }

            return l.ToArray();
        }

        #endregion

        #region<関数　クラス内の属性を取得する>

        /// <summary>
        /// クラス内の属性を取得する
        /// </summary>
        /// <param name="eValue">プロパティ名</param>
        /// <typeparam name="T">クラス</typeparam>
        /// <typeparam name="G">プロパティにつけている情報クラス</typeparam>
        /// <returns>
        /// Attribute情報
        /// (Attribute)でキャストして下さい。
        /// </returns>
        public static object GetAttribute<T,G>(MemberInfo eValue)
        {

            try
            {
                // プロパティに付いている属性を取得する
                G[] authors =
                        Attribute.GetCustomAttributes(
                            //typeof(T).GetProperty(eValue.ToString()),
                            //typeof(T).GetProperty(eValue.Name),
                            typeof(G)) as G[];

                // 取得内容の確認
                if (authors != null)
                {
                    foreach (var author in authors)
                    {
                        //Console.WriteLine(author.Name);
                        return (object)author;
                    }
                }
                return null;
            }
            catch
            {
                throw (new Exception(eValue + "のクラス内の属性が読み込めませんでした\n" ));
            }
            //return authors.FirstOrDefault();																			// FirstOrDefault : 配列の先頭を返すreturn atbLang;
            
        }

        #endregion

        #region<関数　実装サンプル>

        ///// <summary>
        ///// クラスデータにカスタム属性を追加　サンプル
        ///// </summary>
        //public class FTAttributeSample
        //{

        //    public FTAttributeSample()
        //    {
        //        FTAttribute.GetPropatyAttribute<Foo>();
        //    }

        //}

        ///// <summary>
        ///// カスタム属性　サンプル
        ///// </summary>
        //public class RecipeAttribute : Attribute
        //{
        //    /// <summary>名前</summary>
        //    public string Name { get; set; }

        //    /// <summary>品種</summary>
        //    public bool IsRecipe { get; set; }

        //    /// <summary>
        //    /// コンストラクタ
        //    /// </summary>
        //    /// <param name="name"></param>
        //    /// <param name="Recipe"></param>
        //    public RecipeAttribute(string name, bool Recipe)
        //    {
        //        this.Name = name;
        //        this.IsRecipe = Recipe;
        //    }
        //}

        //// 対象クラス
        //public class Foo
        //{
        //    // プロパティに属性を付ける
        //    [RecipeAttribute("だん吉", true)]
        //    public string Bar { get; set; }

        //    // プロパティに属性を付ける
        //    [RecipeAttribute("ぽん太", false)]
        //    public string Aer { get; set; }

        //    public Foo()
        //    {

        //        Bar = "abc";
        //    }

        //}

        #endregion
    }
    
    
}
