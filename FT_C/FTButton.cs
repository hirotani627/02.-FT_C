using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// ボタンクラス
    /// </summary>
    class FTButton
    {

    }

    /// <summary>
    /// ボタンクリックイベント(ボタンコードを追加)
    /// </summary>
    public class ButtonClickEventArgs : EventArgs
    {
        #region<内部変数>

        /// <summary>ボタンコード</summary>
        private FT.C.ENM.MbxButton mButtonCode;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ButtonClickEventArgs(FT.C.ENM.MbxButton mbxButton)
        {
            this.mButtonCode = mbxButton;
        }

        #endregion

        #region<プロパティ>

        /// <summary>
        /// ボタンコード　プロパティ
        /// </summary>
        public FT.C.ENM.MbxButton ButtonCode
        {
            get { return mButtonCode; }
        }

        #endregion

    }

}
