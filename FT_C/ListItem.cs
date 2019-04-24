using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FT.C;

namespace FT.Cntrs
{
	/// <summary>
	/// リストアイテムクラス
	/// </summary>
	/// 
	/// <remarks>
	/// リストアイテム制御処理群
	/// 
	/// Ver1.00  2013-08-06  リリース    H.Sato
	/// 
	/// </remarks>
	///
	public class ListItem
	{

		private int	mItemNo;					// アイテム番号
		private LangString mItemString;		// アイテム文字列

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		/// <param name="No">アイテム値（0～)</param>
		/// <param name="Jpn">日本語</param>
		/// <param name="Eng">英語</param>
		/// <param name="Oth">その他</param>
		/// 
		public ListItem( int No, string Jpn, string Eng, string Oth )
		{
			mItemNo = No;
			
			mItemString = new LangString(Jpn, Eng, Oth);
		}

		/// <summary>
		/// アイテム値プロパティ
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public int ItemNo
		{
			get { return mItemNo; }
			set {
					if( value < 0 ) return;
					
					mItemNo = value;
				}
		}

		/// <summary>
		/// アイテム文字列プロパティ
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public LangString ItemString
		{
			get { return mItemString; }
			set { mItemString = value; }
		}

	}
}
