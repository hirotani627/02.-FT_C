using System;

namespace FT.C
{

    /// <summary>
    /// システム共通例外クラス
    /// </summary>
    /// 
    /// <remarks>
    /// システム共通例外処理群
    /// 
    /// Ver1.00  2012-04-05  リリース    H.Sato
    /// 
    /// </remarks>
    /// 
    public class EXP : ApplicationException
	{

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <param name="Message">基準となるメッセージ</param>
		/// <param name="ClassName">発生したクラス名</param>
		/// <param name="MessageAdd">追加するメッセージ</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public EXP(string Message, string ClassName, string MessageAdd) : base(Message + "\n" + ClassName + ":" + MessageAdd)
        {
        }


        

	}

    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public class EXP_MSG
    {
        /// <summary>エラー</summary>
        public bool IsErr;

        /// <summary>タイトル</summary>
        public string Title;

        /// <summary>内容</summary>
        public string Message;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EXP_MSG()
        {
            IsErr = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EXP_MSG(string Title)
        {
            IsErr = false;
            this.Title = Title;
        }


        /// <summary>
        /// エラー内容の表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "【" + Title + "】" + Message;
        }
    }


}
