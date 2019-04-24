using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;           // トレース機能

namespace FT.C
{
    /// <summary>
    /// フォーテクノス製コントロール
    /// </summary>
    class FTControl
    {

        // --- 自社オブジェクト関連 ---

        /// <summary>ボタンコントロール名</summary>
        public const string MY_BTN = "C_BTN";										// 

        /// <summary>チェックボックスコントロール名</summary>
        public const string MY_CKBX = "C_CKBX";										// 

        /// <summary>コンボボックスコントロール名</summary>
        public const string MY_COBX = "C_COBX";										// 

        /// <summary>フレームコントロール名</summary>
        public const string MY_FRAM = "C_FRAM";										// 

        /// <summary>ラベルコントロール名</summary>
        public const string MY_LABL = "C_LABL";										// 

        /// <summary>数値コントロール名</summary>
        public const string MY_NUM = "C_NUM";										// 

        /// <summary>オプションボタンコントロール名</summary>
        public const string MY_OPT = "C_OPT";										// 

        /// <summary>タブコントロール名</summary>
        public const string MY_TAB = "C_TAB";                                       // 



    }

    /// <summary>
    /// コントロールクラス
    /// </summary>
    public class Controls
    {

        #region<デリゲート>

        /// <summary>パラメータを変更したデリゲート</summary>
        public delegate void ParamChange_EventHandler(object sender, EventArgs e);

        /// <summary>テキストボックスをクリックしたデリゲート</summary>
        public delegate void TextBoxClick_EventHandler(object sender, EventArgs e);

        /// <summary>子コントロールクリックデリゲート</summary>
        public delegate void ChildClikEventHandler(object sender, ChildClickEventArgs e);

        #endregion

        #region<コントロールの描画制御>

        /// <summary>
        /// メッセージ送信（win32）
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary></summary>
        public const int WM_SETREDRAW = 0x000B;

        /// <summary>False</summary>
        public const int Win32False = 0;

        /// <summary>True</summary>
        public const int Win32True = 1;

        /// <summary>
        /// EndUpdate メソッドが呼ばれるまで、コントロールを再描画しないようにします。
        /// </summary>
        /// <remarks>•System.Windows.Forms.Control から派生しているクラスが対象</remarks>
        public static void BeginUpdate(IntPtr handle )
        {
            SendMessage(handle, WM_SETREDRAW, Win32False, 0);
        }

        /// <summary>
        /// BeginUpdate メソッドにより中断されていた描画を再開します。
        /// </summary>
        /// <remarks>•System.Windows.Forms.Control から派生しているクラスが対象</remarks>
        public static void EndUpdate(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, Win32True, 0);
            //this.Invalidate();
        }

        #endregion

        #region<コントロールをすべて列挙>

        /// <summary>
        /// 指定コントロール内のすべてのコントロールを列挙
        /// </summary>
        /// <param name="cReferent">参照先コントロール</param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control cReferent)
        {
            ArrayList buf = new ArrayList();
            foreach (Control c in cReferent.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return (Control[])buf.ToArray(typeof(Control));
        }

        #endregion

        #region<コントロールのロケーションを計算>

        /// <summary>
        /// コントロールの中心位置を取得
        /// </summary>
        /// <param name="StartPoint">スタートの座標</param>
        /// <param name="width">全体幅</param>
        /// <param name="division">指定幅を何個に分けるかを指定</param>
        /// <param name="selectNo">分けた何番目の値を返すか(スタート位置が０番目)</param>
        /// <returns>コントロールの中心座標</returns>
        /// <remarks>
        /// (例）
        /// 0--1--2--3
        /// ・StartPointが0の開始ピクセル
        /// ・widthが0から3の幅（ピクセル）
        /// ・division この例では4
        /// ・selectNo何個目の位置を取得するか？
        /// </remarks>
        public static int GetLocation( int StartPoint, int width, int division, int selectNo )
        {
            return GetLocation(StartPoint, width, division, selectNo ,0);
        }

        /// <summary>
        /// コントロールの中心位置を取得
        /// </summary>
        /// <param name="StartPoint">スタートの座標</param>
        /// <param name="width">全体幅</param>
        /// <param name="division">指定幅を何個に分けるかを指定</param>
        /// <param name="selectNo">分けた何番目の値を返すか(スタート位置が０番目)</param>
        /// <param name="Margin">マージン</param>
        /// <returns>コントロールの中心座標</returns>
        /// <remarks>
        /// (例）
        /// 0--1--2--3
        /// ・StartPointが0の開始ピクセル
        /// ・widthが0から3の幅（ピクセル）
        /// ・division この例では4
        /// ・selectNo何個目の位置を取得するか？
        /// </remarks>
        public static int GetLocation(int StartPoint, int width, int division, int selectNo , int Margin)
        {
            if (selectNo > division)
                throw new FormatException("指定の位置が分割数を超えています");

            int マージンを考慮した幅 = width - (Margin * 2);
            if (マージンを考慮した幅 < 0)
                throw new FormatException("マージンが指定幅よりも大きくなりました。");

            int 分割したX幅 = マージンを考慮した幅 / division;

            int PointControlSenter;
            PointControlSenter = StartPoint + (分割したX幅 * selectNo);

            return PointControlSenter;

        }


        /// <summary>
        /// コントロールのロケーションを取得
        /// </summary>
        /// <param name="targetControl">コントロールのターゲット</param>
        /// <param name="コントロールの中心位置"></param>
        /// <returns></returns>
        public static Point GetControlLocation( Control targetControl, Point コントロールの中心位置 )
        {
            Point p = new Point();
            p.X = コントロールの中心位置.X - (targetControl.Width / 2);
            p.Y = コントロールの中心位置.Y - (targetControl.Height / 2);
            return p;
        }

        /// <summary>
        /// コントロールのロケーションを取得
        /// </summary>
        /// <returns></returns>
        public static Point GetLocation(Rectangle 表示範囲, Control 中心に表示するコントロール)
        {
            Point p = new Point();
            p.X = (表示範囲.Width / 2 - 中心に表示するコントロール.Width / 2);
            p.Y = (表示範囲.Height / 2 - 中心に表示するコントロール.Height / 2);
            Trace.WriteLine(p);
            return p;
        }

        #endregion

        #region<コントロール配列の子コントロールロケーションを設定>

        /// <summary>
        /// コントロール配列の子コントロールロケーションを設定
        /// </summary>
        /// <param name="algin">子コントロールの配置</param>
        /// <param name="conts">子コントロール配列</param>
        /// <param name="childControlSize">子コントロールひとつのサイズ</param>
        /// <param name="No">コントロール配列の番号</param>
        public static void SetChildControlLocation(ArrayControlAlginConstants algin, Control[] conts, Size childControlSize, int No)
        {

            switch (algin)
            {
                case ArrayControlAlginConstants.Horizontal:
                    conts[No].Location = new Point(childControlSize.Width * No, 0);
                    break;

                case ArrayControlAlginConstants.HorizontalReverse:
                    int HorizontalReverseNo = (conts.Length - 1 - No);
                    if (HorizontalReverseNo >= 0)
                        conts[No].Location = new Point(childControlSize.Width * HorizontalReverseNo, 0);
                    break;

                case ArrayControlAlginConstants.Horizontal_Return2:
                    // 水平に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Horizontal(childControlSize, No, 2);
                    break;

                case ArrayControlAlginConstants.Horizontal_Return3:
                    // 水平に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Horizontal(childControlSize, No, 3);
                    break;

                case ArrayControlAlginConstants.Horizontal_Return4:
                    // 水平に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Horizontal(childControlSize, No, 4);
                    break;

                case ArrayControlAlginConstants.Vertical:
                    conts[No].Location = new Point(0, childControlSize.Height * No);
                    break;

                case ArrayControlAlginConstants.VerticalReverse:
                    int VerticalReverseNo = conts.Length - 1 - No;
                    if (VerticalReverseNo >= 0)
                        conts[No].Location = new Point(0, childControlSize.Height * VerticalReverseNo);
                    break;

                case ArrayControlAlginConstants.Vertical_Return2:
                    // 垂直に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Vertical(childControlSize, No, 2);
                    break;

                case ArrayControlAlginConstants.Vertical_Return3:
                    // 垂直に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Vertical(childControlSize, No, 3);
                    break;

                case ArrayControlAlginConstants.Vertical_Return4:
                    // 垂直に配置の子ロケーションの取得
                    conts[No].Location = GetChildLocation_Vertical(childControlSize, No, 4);
                    break;


                default:
                    string errText = "U_LEDButtonArrayのCreateControl():" + algin.ToString() + "は未実装です。　コードを追加してください";
                    Console.WriteLine(errText);
                    //throw new NotImplementedException(errText);
                    break;
            }
        }

        /// <summary>
        /// 水平に配置の子ロケーションの取得
        /// </summary>
        /// <param name="childControlSize">子コントロールひとつのサイズ</param>
        /// <param name="No">コントロール配列の番号</param>
        /// <param name="returnCout">改行位置</param>
        /// <returns></returns>
        private static Point GetChildLocation_Horizontal(Size childControlSize, int No, int returnCout)
        {
            int lx = No % returnCout;
            int ly = No / returnCout;
            return new Point(childControlSize.Width * lx, childControlSize.Height * ly);
        }

        /// <summary>
        /// 垂直に配置の子ロケーションの取得
        /// </summary>
        /// <param name="childControlSize">子コントロールひとつのサイズ</param>
        /// <param name="No">コントロール配列の番号</param>
        /// <param name="returnCout">改行位置</param>
        /// <returns></returns>
        private static Point GetChildLocation_Vertical(Size childControlSize, int No, int returnCout)
        {
            int lx = No / returnCout;
            int ly = No % returnCout;
            return new Point(childControlSize.Width * lx, childControlSize.Height * ly);
        }

        #endregion

        #region<コントロール配列からコントロール全体のサイズを取得>
        
        /// <summary>
        /// コントロール配列からコントロール全体のサイズを取得
        /// </summary>
        /// <param name="controlCout">子コントロールの総数</param>
        /// <param name="childControlSize">子コントロールひとつのサイズ</param>
        /// <param name="algin">子コントロールの配置</param>
        public static Size GetControlSize(FT.C.ArrayControlAlginConstants algin, Size childControlSize, int controlCout)
        {
            return GetControlSize(algin, childControlSize, controlCout, new Padding(0));
        }

        /// <summary>
        /// コントロール配列からコントロール全体のサイズを取得
        /// </summary>
        /// <param name="controlCout">子コントロールの総数</param>
        /// <param name="childControlSize">子コントロールひとつのサイズ</param>
        /// <param name="algin">子コントロールの配置</param>
        /// <param name="childPaddin">子コントロールのパディング</param>
        public static Size GetControlSize(FT.C.ArrayControlAlginConstants algin, Size childControlSize, int controlCout, Padding childPaddin)
        {
            Size contlorSize = new Size(0, 0);

            // コントロールのサイズ変更
            switch (algin)
            {
                case FT.C.ArrayControlAlginConstants.Horizontal:
                case FT.C.ArrayControlAlginConstants.HorizontalReverse:
                    contlorSize.Width = childControlSize.Width * controlCout + childPaddin.Left * controlCout + childPaddin.Right * controlCout;
                    contlorSize.Height = childControlSize.Height + childPaddin.Top + childPaddin.Bottom;
                    break;

                case FT.C.ArrayControlAlginConstants.Horizontal_Return2:
                    // 水平に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeHorizontal_Return(childControlSize, controlCout, 2);
                    break;

                case FT.C.ArrayControlAlginConstants.Horizontal_Return3:
                    // 水平に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeHorizontal_Return(childControlSize, controlCout,3);
                    break;

                case FT.C.ArrayControlAlginConstants.Horizontal_Return4:
                    // 水平に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeHorizontal_Return(childControlSize, controlCout, 4);
                    break;

                case FT.C.ArrayControlAlginConstants.Vertical:
                case FT.C.ArrayControlAlginConstants.VerticalReverse:
                    contlorSize.Width = childControlSize.Width;
                    contlorSize.Height = childControlSize.Height * controlCout;
                    break;

                case FT.C.ArrayControlAlginConstants.Vertical_Return2:
                    // 垂直に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeVertical_Return(childControlSize, controlCout, 2);
                    break;

                case FT.C.ArrayControlAlginConstants.Vertical_Return3:
                    // 垂直に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeVertical_Return(childControlSize, controlCout, 3);
                    break;

                case FT.C.ArrayControlAlginConstants.Vertical_Return4:
                    // 垂直に配置したコントロール全体のサイズを取得
                    contlorSize = GetControlSizeVertical_Return(childControlSize, controlCout, 4);
                    break;
            }

            return contlorSize;
        }

        /// <summary>
        /// 垂直に配置したコントロール全体のサイズを取得
        /// </summary>
        /// <param name="childSize">子コントロールサイズ</param>
        /// <param name="controlCout">コントロール数</param>
        /// <param name="returnCout">改行位置</param>
        public static Size GetControlSizeVertical_Return(Size childSize, int controlCout, int returnCout)
        {
            Size MainSize = new Size();      // コントロール全体のサイズ
            
            // 幅
            MainSize.Width = childSize.Width * (controlCout / returnCout);

            // 高さ
            if ((controlCout / returnCout) > 0)
                MainSize.Height = childSize.Height * returnCout;
            else
                MainSize.Height = childSize.Height % returnCout;
            return MainSize;
        }


        /// <summary>
        /// 水平に配置したコントロール全体のサイズを取得
        /// </summary>
        /// <param name="childSize">子コントロールサイズ</param>
        /// <param name="controlCout">コントロール数</param>
        /// <param name="returnCout">改行数</param>
        public static Size GetControlSizeHorizontal_Return(Size childSize, int controlCout, int returnCout)
        {
            Size MainSize = new Size();      // コントロール全体のサイズ

            // 幅
            if (controlCout >= returnCout)
                MainSize.Width = childSize.Width * returnCout;
            else
                MainSize.Width = childSize.Width * controlCout;

            // 高さ
            MainSize.Height = childSize.Height * ((controlCout / returnCout) + 1);
            return MainSize;
        }

        #endregion

    }

    /// <summary>
    /// コントロール配列の配置定義
    /// </summary>
    public enum ArrayControlAlginConstants
    {
        /// <summary>水平（左から右）に配置</summary>
        /// <remarks>
        /// ①②③④⑤⑥⑦⑧⑨⑩
        /// </remarks>
        Horizontal,

        /// <summary>水平（右から左）に配置</summary>
        /// <remarks>
        /// ⑩⑨⑧⑦⑥⑤④③②①
        /// </remarks>
        HorizontalReverse,

        /// <summary>水平（左から右）に配置2列で改行</summary>
        /// /// <remarks>
        /// ①②
        /// ③④
        /// ⑤⑥
        /// ⑦⑧
        /// ⑨⑩
        /// </remarks>
        Horizontal_Return2,

        /// <summary>水平（左から右）に配置3列で改行</summary>
        /// /// <remarks>
        /// ①②③
        /// ④⑤⑥
        /// ⑦⑧⑨
        /// ⑩
        /// </remarks>
        Horizontal_Return3,

        /// <summary>水平（左から右）に配置4列で改行</summary>
        /// /// /// <remarks>
        /// ①②③④
        /// ⑤⑥⑦⑧
        /// ⑨⑩
        /// </remarks>
        Horizontal_Return4,

        /// <summary>垂直（上から下）に配置</summary>
        /// <remarks>
        /// ①
        /// ②
        /// ③
        /// ④
        /// ⑤
        /// ⑥
        /// ⑦
        /// ⑧
        /// ⑨
        /// ⑩
        /// </remarks>
        Vertical,

        /// <summary>垂直（下から上）に配置</summary>
        /// <remarks>
        /// ⑩
        /// ⑨
        /// ⑧
        /// ⑦
        /// ⑥
        /// ⑤
        /// ④
        /// ③
        /// ②
        /// ①
        /// </remarks>
        VerticalReverse,

        /// <summary>垂直（上から下）に配置2行で改行</summary>
        /// /// <remarks>
        /// ①③⑤⑦⑨
        /// ②④⑥⑧⑩
        /// </remarks>
        Vertical_Return2,

        /// <summary>垂直（上から下）に配置3行で改行</summary>
        /// <remarks>
        /// ①④⑦⑩
        /// ②⑤⑧
        /// ③⑥⑨
        /// </remarks>
        Vertical_Return3,

        /// <summary>垂直（上から下）に配置4行で改行</summary>
        /// <remarks>
        /// ①⑤⑨
        /// ②⑥⑩
        /// ③⑦
        /// ④⑧
        /// </remarks>
        Vertical_Return4,

    }

    /// <summary>
    /// コントロール インターフェイス
    /// </summary>
    public interface IControl :IDisposable
    {
        /// <summary>コントロール初期化</summary>
        void Initialize();

    }

    /// <summary>
    /// ポップコントロールインターフェイス（指定時間で消えるコントロール）
    /// </summary>
    public interface IPopControl : IDisposable
    {
        /// <summary>表示を重ねるターゲット　プロパティ</summary>
        Control Target { get; set; }

        /// <summary>ターゲットコントロールからのオフセット　プロパティ</summary>
        System.Drawing.Point LocationOffset { get; set; }

        /// <summary>消えるまでの時間　プロパティ</summary>
        int PopDelay { get; set; }

        /// <summary>
        /// Pop表示(Invoke対応)
        /// </summary>
        void ShowPopInvoke();
    }

    /// <summary>
    /// コントロール配列　子コントロールクリック　イベントハンドラ
    /// </summary>
    public class ChildClickEventArgs : EventArgs
    {
        /// <summary>ボタン番号</summary>
        public int mControlNo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChildClickEventArgs(int controlNo)
        {
            mControlNo = controlNo;
        }

        /// <summary>
        /// コントロール番号　プロパティ
        /// </summary>
        public int ControlNo
        {
            get { return mControlNo; }
        }
    }

}
