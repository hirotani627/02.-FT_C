using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;   // DllImportのために必要
using System.Windows.Forms;


namespace FT.C
{
    /// <summary>
    /// フォームクラス
    /// </summary>
    public class From
    {
        /// <summary>
        /// ユーザーコントロールを貼り付けたフォームの作成
        /// </summary>
        /// <param name="control">貼り付けるコントロール</param>
        /// <param name="position">フォームの表示位置</param>
        /// <param name="style">フォームのスタイル</param>
        public static Form MakeForm(UserControl control, FormBorderStyle style, FormStartPosition position)
        {
            Form frm = new Form();
            frm.Width = control.Width;
            frm.Height = control.Height;
            frm.Controls.Add(control);
            frm.FormBorderStyle = style;
            frm.StartPosition = position;
            return frm;
        }

        #region<ウインドウ点滅>

        [DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

        /// <summary>
        /// フォームフラッシュ構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            /// <summary>FLASHWINFO構造体のサイズ</summary>
            public UInt32 cbSize;

            /// <summary>点滅対象のウィンドウ・ハンドル</summary>
            public IntPtr hwnd;

            /// <summary>以下の「FLASHW_XXX」のいずれか</summary>
            public UInt32 dwFlags;

            /// <summary>点滅する回数</summary>
            public UInt32 uCount;

            /// <summary>点滅する間隔（ミリ秒単位）</summary>
            public UInt32 dwTimeout;
        }

        /// <summary>
        /// フォームフラッシュモード
        /// </summary>
        public enum FormFlashMode
        {
            /// <summary>点滅を止める</summary>
            Stop = 0 ,

            /// <summary>タイトルバーを点滅させる</summary>
            CAPTION = 1,

            /// <summary>タスクバー・ボタンを点滅させる</summary>
            TRAY = 2,

            /// <summary>タスクバー・ボタンとタイトルバーを点滅させる</summary>
            ALL = 3,

            /// <summary>FLASHW_STOPが指定されるまでずっと点滅させる</summary>
            TIMER = 4,

            /// <summary>ウィンドウが最前面に来るまでずっと</summary>
            TIMERNOFG = 12,

        }


        /// <summary>
        /// ウインドウ点滅
        /// </summary>
        /// <param name="fm">対象フォーム</param>
        /// <remarks>点滅後はタイトルが強調表示されたままになります</remarks>
        public static void FlashWindow( Form fm )
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = fm.Handle;
            fInfo.dwFlags = (UInt32)FormFlashMode.ALL;
            fInfo.uCount = 5;               // 点滅回数
            fInfo.dwTimeout = 0;
            FlashWindowEx(ref fInfo);
        }

        #endregion

    }

    /// <summary>
    /// フォームクローズイベントクラス
    /// </summary>
    public class FormDialogResultEventArgs : EventArgs
    {
        #region<内部変数>

        /// <summary>フォームクローズ値</summary>
        private System.Windows.Forms.DialogResult mResult;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">フォームクローズ値</param>
        public FormDialogResultEventArgs(System.Windows.Forms.DialogResult result)
        {
            this.mResult = result;
        }

        #endregion

        #region<プロパティ>

        /// <summary>
        /// フォームクローズ値　プロパティ
        /// </summary>
        public System.Windows.Forms.DialogResult Result
        {
            get { return mResult; }
        }

        #endregion

    }

}
