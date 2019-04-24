using System;
using System.Linq;

namespace FT.C
{

    /// <summary>
    /// システム共通列挙値クラス
    /// </summary>
    /// 
    /// <remarks>
    /// システム共通列挙値群
    /// 
    /// Ver1.00  2012-04-05  リリース    H.Sato
    /// 
    /// ※列挙値の最後のメンバに必ずEndMarkを設ける事！！
    /// ※各列挙値に対してLANG属性を設定する事！！（但し、EndMarkには不要）
    /// 
    /// </remarks>
    /// 
    public static class ENM
    {

        #region <関数　列挙型の個数を取得>

        /// <summary> 
        /// 列挙型の個数を取得
        /// </summary> 
        public static int GetLength<T>()
        { 
            return Enum.GetValues(typeof(T)).Length; 
        }

        #endregion

        #region <関数　クラス言語拡張 言語>

        /// <summary>
        /// 列挙値の日本語名を返す
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>日本語文字列</returns>
        ///
        /// <remarks></remarks>
        /// 
        public static string ToJpn(this Enum eValue)
        {

            using (var atbLang = GetAttribute<LANG>(eValue))
            {
                if (atbLang == null)
                    return "";
                return atbLang.Jpn;
            }

        }

        /// <summary>
        /// 列挙値の英語名を返す
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>英語文字列</returns>
        /// 
        /// <remarks></remarks>
        /// 
        public static string ToEng(this Enum eValue)
        {

            using (var atbLang = GetAttribute<LANG>(eValue))
            {
                if (atbLang == null)
                    return "";
                return atbLang.Eng;
            }

        }

        /// <summary>
        /// 列挙値のその他の言語名を返す
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>その他の言語文字列</returns>
        /// 
        /// <remarks></remarks>
        /// 
        public static string ToOth(this Enum eValue)
        {

            using (var atbLang = GetAttribute<LANG>(eValue))
            {
                if (atbLang == null)
                    return "";
                return atbLang.Oth;
            }

        }

        #endregion

        #region <関数　クラス言語拡張 処理>

        /// <summary>
        /// 列挙値のアドレスを返す
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <returns>アドレス</returns>
        ///
        /// <remarks></remarks>
        /// 
        public static string ToAddres(this Enum eValue)
        {

            using (var atbLang = GetAttribute<LANG>(eValue))
            {
                if (atbLang == null)
                    return "";
                return atbLang.Jpn;
            }

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
        public static T GetAttribute<T>(Enum eValue)
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
        ///// 内容を取得 
        ///// </summary>
        ///// 
        ///// <param name="eValue">列挙値</param>
        ///// <returns>言語属性値</returns>
        ///// 
        ///// <remarks></remarks>
        ///// 
        //private static LANG GetAttribute(Enum eValue)
        //{

        //    Type enmType = eValue.GetType();                            // 列挙値のタイプ
        //    string strName = Enum.GetName(enmType, eValue);             // メンバー名を取得する

        //    // 言語属性群取得 : ※GetCustomAttributesはMemberInfoクラスでカスタム属性の配列を返すメソッドです
        //    var atbLang = enmType.GetField(strName).GetCustomAttributes(typeof(LANG), false);
        //    foreach (LANG n in atbLang)
        //    {
        //        return n;
        //    }

        //    return null;
        //    //return ((T[])(atbLang)).FirstOrDefault();                                                                            // FirstOrDefault : 配列の先頭を返す

        //}


        /// <summary>
        /// 列挙値の言語名を返す（言語指定可）
        /// </summary>
        /// 
        /// <param name="eValue">列挙値</param>
        /// <param name="eLang">言語コード</param>
        /// <returns>指定言語での言語文字列</returns>
        /// 
        /// <remarks></remarks>
        /// 
        public static string ToLang(this Enum eValue, Lang eLang)
        {

            using (LANG atbLang = GetAttribute<LANG>(eValue))
            {
                if (atbLang == null)
                    throw (new NotImplementedException("呼び出したENUMに【ATB.LANG】が定義されていません"));

                switch (eLang)
                {
                    case Lang.Jpn: return atbLang.Jpn;
                    case Lang.Eng: return atbLang.Eng;
                    case Lang.Oth: return atbLang.Oth;
                    default: return "";
                }
            }
        }

        /// <summary>全言語を一括で返す</summary>
        /// <remarks></remarks>
        public static LangString ToLangs(this Enum eValue)
        {
            LANG atbLang = GetAttribute<LANG>(eValue);
            return atbLang.Langs;
        }
        
        #endregion

        /// <summary>プロセス処理コード</summary>
        public enum Process : int
		{
			/// <summary>失敗</summary>
			[LANG( "NG", "NG", "NG" )]
			NG = 0,

			/// <summary>成功</summary>
			[LANG( "OK", "OK", "OK" )]
			OK,

			/// <summary>処理中</summary>
			[LANG( "Busy", "Busy", "Busy" )]
			Busy,

            /// <summary>エンドマーク</summary>
            [LANG( "", "", "" )]
            EndMark,
		}

        /// <summary>コード制御</summary>
        public enum CodeJudgment : int
		{
			/// <summary>失敗</summary>
			[LANG( "NG", "NG", "NG" )]
			NG = 0,

			/// <summary>成功</summary>
			[LANG( "OK", "OK", "OK" )]
			OK,

            /// <summary>再トライ</summary>
            [LANG("Retry", "Retry", "Retry")]
            Retry,

   //         /// <summary>スキップ</summary>
			//[LANG("PASS", "PASS", "PASS")]
   //         PASS,

   //         /// <summary>廃棄</summary>
			//[LANG("SCRP", "SCRP", "SCRP")]
   //         SCRP,

        }

        /// <summary>ユーザー権限コード</summary>
        public enum Authority : int
		{
			/// <summary>オペレータ</summary>
			[LANG( "オペレータ", "Operator", "操作員" )]
			Operator = 0,

			/// <summary>エンジニア</summary>
			[LANG( "エンジニア", "Engineer", "保全" )]
			Engineer,

			/// <summary>管理者</summary>
			[LANG( "管理者", "Administrator", "管理者" )]
			Admin,

			/// <summary>フォーテクノス</summary>
			[LANG( "フォーテクノス", "FourTechnos", "FourTechnos" )]
			FT,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>言語コード</summary>
		public enum Lang : int
		{

			/// <summary>日本語</summary>
			[LANG( "日本語", "Japanese", "日本語" )]
			Jpn = 0,

			/// <summary>英語</summary>
			[LANG( "英語", "English", "英語" )]
			Eng,

			/// <summary>その他</summary>
            [LANG("中国語", "Chinese", "中国語")]
			Oth,

            /// <summary>中国語(台湾)</summary>
            [LANG("中国語(台湾)", "Chinese(TW)", "中国語(台湾)")]
            ChineseTW,


            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

        /// <summary>データコード</summary>
		public enum Data : int
        {

            /// <summary>品種</summary>
            [LANG("", "", "")]
            Kind = 0,


            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
        }

        /// <summary>
        /// 画面表示位置コード
        /// 
        /// ※２画面のうちどちらに表示するか
        /// </summary>
        public enum WindowPosi : int
		{
			/// <summary>左画面</summary>
			[LANG( "左画面", "Left screen", "左画面" )]
			Left = 0,

			/// <summary>右画面</summary>
			[LANG( "右画面", "Right screen", "右画面" )]
			Right,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>メッセージボックスタイプコード</summary>
		public enum MbxType : int
		{
			/// <summary>情報</summary>
			[LANG( "情報", "Information", "情報" )]
			Information = 0,

			/// <summary>異常</summary>
			[LANG( "異常", "Error", "異常" )]
			Error,

			/// <summary>警告</summary>
			[LANG( "警告", "Warning", "警告" )]
			Warning,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>メッセージボックスのボタンタイプコード</summary>
		public enum MbxButType : int
		{
			/// <summary>'了解'ボタンだけ</summary>
			[LANG( "OK", "OK", "OK" )]
			OKOnly = 0,

			/// <summary>'はい' 'いいえ'ボタン</summary>
			[LANG( "YesNo", "YesNo", "YesNo" )]
			YesNo,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>
		/// メッセージボックスのボタンコード
		/// 
		/// ※どのボタンが押されたか
		/// </summary>
		public enum MbxButton : int
		{
			/// <summary>ボタンが押されていない</summary>
			[LANG( "Non", "Non", "Non" )]
			Non = 0,

			/// <summary>'了解'ボタン</summary>
			[LANG( "OK", "OK", "OK" )]
			OK,

			/// <summary>'はい'ボタン</summary>
			[LANG( "Yes", "Yes", "Yes" )]
			Yes,

			/// <summary>'いいえ'ボタン</summary>
			[LANG( "No", "No", "No" )]
			No,

            /// <summary>'キャンセル'ボタン</summary>
            [LANG("キャンセル", "Cancel", "Cancel")]
            Cancel,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>移動単位コード</summary>
		public enum MovUnit : int
		{
			/// <summary>μm</summary>
			[LANG( "μｍ", "μｍ", "μｍ" )]
			Um = 0,

			/// <summary>deg</summary>
			[LANG( "ｄｅｇ", "ｄｅｇ", "ｄｅｇ" )]
			Deg,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>操作パネル SWコード</summary>
		public enum OpSW : int
		{
			/// <summary>Ｘスイッチ</summary>
			[LANG( "X", "X", "X" )]
			X = 0,

			/// <summary>Ｙスイッチ</summary>
			[LANG( "Y", "Y", "Y" )]
			Y,

			/// <summary>Ｚ１スイッチ</summary>
			[LANG( "Z1", "Z1", "Z1" )]
			Z1,

			/// <summary>Ｚ２スイッチ</summary>
			[LANG( "Z2", "Z2", "Z2" )]
			Z2,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>軸移動方向コード</summary>
		public enum MovDir : int
		{
			/// <summary>プラス方向</summary>
			[LANG( "+", "+", "+" )]
			Plus = 0,

			/// <summary>マイナス方向</summary>
			[LANG( "-", "-", "-" )]
			Minus,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}

		/// <summary>ソート（並べ替え）コード</summary>
		public enum Sort : int
		{
			/// <summary>昇順</summary>
			[LANG( "昇順", "Ascending order", "昇順" )]
			Asc = 0,

			/// <summary>降順</summary>
			[LANG( "降順", "Descending order", "降順" )]
			Desc,

            /// <summary>エンドマーク</summary>
            [LANG("", "", "")]
            EndMark,
		}


        /// <summary>
        /// フラグ定義
        /// </summary>
        public enum Frg
        {
            /// <summary>OFF</summary>
            [LANG("", "", "")]
            OFF = 0,

            /// <summary>ON</summary>
            [LANG("", "", "")]
            ON = 1,

            /// <summary>TGL</summary>
            [LANG("", "", "")]
            TGL = 2
        }

        /// <summary>
        /// 時間項目
        /// </summary>
        [Flags]
        public enum DateTimeContens
        {

            /// <summary>日にち</summary>
            [LANG("日", "d", "d")]
            Days = 0x01,

            /// <summary>時間</summary>
            [LANG("時間", "h", "h")]
            Hours = 0x02,

            /// <summary>分</summary>
            [LANG("分", "m", "m")]
            Minutes = 0x04,

            /// <summary>秒</summary>
            [LANG("秒", "s", "s")]
            Seconds = 0x08,

            /// <summary>ミリ秒</summary>
            [LANG("ミリ", "ms", "ms")]
            Milliseconds = 0x10,

        }



    }


    /// <summary>方向</summary>
    public enum DirectionContens : int
    {
        /// <summary>縦</summary>
        [LANG("縦", "縦", "縦")]
        Vertical = 0,

        /// <summary>横</summary>
        [LANG("横", "横", "横")]
        Horizontal,
    }

    /// <summary>
    /// テーマ色
    /// </summary>
    public enum FTWindowColorContens
    {
        ///// <summary></summary>
        //noml,

        /// <summary></summary>
        Steel,

        /// <summary></summary>
        Sienna,

        /// <summary></summary>
        Brown,
        
        /// <summary></summary>
        Black,

        /// <summary></summary>
        Blue,

        /// <summary></summary>
        Green,

        /// <summary></summary>
        Yellow,

        /// <summary></summary>
        Violet,

        /// <summary></summary>
        Purple,

        /// <summary></summary>
        Pink,

        /// <summary></summary>
        Red,

    }


    /// <summary>
    /// ベース色
    /// </summary>
    public enum FTWindowBaseColorContens
    {
        /// <summary></summary>
        BaseWhite,

        /// <summary></summary>
        BaseLight,

        /// <summary></summary>
        BaseDark,
    }
}
