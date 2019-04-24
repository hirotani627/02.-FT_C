using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;

namespace FT.C
{
    /// <summary>
    /// ウインドウコントロールクラス
    /// </summary>
    public class FTWindow
    {

        #region<関数 Win32 ウィンドウハンドルの取得>

        /// <summary>
        /// Windowハンドルの取得（Win32）
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow( string lpClassName, string lpWindowName);

        /// <summary>
        /// Windowハンドルの取得（Win32）
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId( IntPtr hWnd, out int lpdwProcessId);

        

        /// <summary>
        /// Windowハンドルの取得
        /// </summary>
        /// <param name="name">ソフト名</param>
        /// <param name="handle">取得したWindowハンドル</param>
        public static bool GetWindowHandle(string name, out IntPtr handle)
        {
            handle = new IntPtr();

            //すべてのプロセスを列挙する
            foreach (System.Diagnostics.Process p
                in System.Diagnostics.Process.GetProcesses())
            {
                //"メモ帳"がメインウィンドウのタイトルに含まれているか調べる
                if (0 <= p.MainWindowTitle.IndexOf(name))
                {
                    handle = p.MainWindowHandle;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 実行しているウインドウのハンドルを取得する
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetWindowHandle()
        {
            var x = HwndSource.FromHwnd(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            var main = x.RootVisual as System.Windows.Window;
            WindowInteropHelper wih = new WindowInteropHelper(main);
            return wih.Handle;
        }

        #endregion

        #region<関数 Win32 ウィンドウをアクティブにする（前に出す）>

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// ウィンドウをアクティブにする（前に出す）
        /// </summary>
        /// <param name="handle">取得したWindowハンドル</param>
        public static void ForegroundWindow(IntPtr handle)
        {
            SetForegroundWindow(handle);
        }

        #endregion

        #region<関数 Win32 ウィンドウ操作>

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        /// <summary></summary>
        public const int SWP_NOSIZE = 0x0001;

        /// <summary>ウィンドウの現在位置を保持</summary>
        public const int SWP_NOMOVE = 0x0002;

        /// <summary></summary>
        public const int SWP_SHOWWINDOW = 0x0040;

        /// <summary>ウィンドウをウィンドウリストの一番上に配置</summary>
        public const int HWND_TOPMOST = -1;

        /// <summary></summary>
        public const int HWND_NOTOPMOST = -2;

        /// <summary></summary>
        public const int SWP_ASYNCWINDOWPOS = 0x4000;

        /// <summary>手前に移動</summary>
        public const int HWND_TOP = 0;

        /// <summary>奥に移動</summary>
        public const int HWND_BOTTOM = 1;

        /// <summary>
        /// ウィンドウ操作
        /// </summary>
        /// <param name="handle">取得したWindowハンドル</param>
        /// <param name="hWndInsertAfter"></param>
        /// <returns></returns>
        public static bool SetWindowPos(IntPtr handle, int hWndInsertAfter)
        {
            return SetWindowPos(handle, hWndInsertAfter, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        /// <summary>
        /// ウィンドウ操作
        /// </summary>
        /// <param name="handle">取得したWindowハンドル</param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static bool SetWindowPos(IntPtr handle, int hWndInsertAfter, int x, int y, int h , int w)
        {
            return SetWindowPos(handle, hWndInsertAfter, x, y, w, h, 0);
        }

        #endregion

        /// <summary>
        /// 選択中のWindowを取得する
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// ウィンドウの実行ファイル名の取得(フルパスが取得される)
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpszFileName"></param>
        /// <param name="cchFileNameMax"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowModuleFileName(IntPtr hwnd, StringBuilder lpszFileName, uint cchFileNameMax);

        #region<関数 タイトル取得>

        /// <summary>
        /// Windowタイトルを取得する
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// タイトル取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String GetWindowText(IntPtr id)
        {
            StringBuilder sb = new StringBuilder(_No);
            GetWindowModuleFileName(id, sb, _No);

            GetWindowText(id, sb, _No);
            return sb.ToString();
        }

        /// <summary>
        /// Windouタイトルの取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWindowText(out IntPtr ID)
        {
            ID = GetForegroundWindow();
            return GetWindowText(ID);
        }

        private const int _No = 65535;

        #endregion

        #region<関数 アプリケーションが起動している?>

        /// <summary>
        /// アプリケーションが起動している？
        /// </summary>
        /// <returns></returns>
        public static string GetProsessName(string target)
        {

            //タイトルが"無題 - メモ帳"のウィンドウを探す
            IntPtr hWnd = FindWindow(null, target);
            if (hWnd != IntPtr.Zero)
            {
                //ウィンドウを作成したプロセスのIDを取得する
                int processId;
                GetWindowThreadProcessId(hWnd, out processId);
                //Processオブジェクトを作成する
                var p = System.Diagnostics.Process.GetProcessById(processId);
                return p.ProcessName;
            }
            else
            {
                return null;
            }

        }

        #endregion

        #region<関数 アプリケーションが起動している?>

        /// <summary>
        /// 自分自身がすでに起動している？
        /// </summary>
        /// <returns></returns>
        public static bool IsAppStert()
        {
            //二重起動をチェックする
            var a = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var t = System.Diagnostics.Process.GetProcessesByName(a);
            if (t.Length > 1)
            {
                //すでに起動していると判断する
                return true;
            }
            return false;
        }

        /// <summary>
        /// アプリケーションが起動している？
        /// </summary>
        /// <returns></returns>
        public static bool IsAppStert(string target)
        {
            //二重起動をチェックする
            //var a = System.Diagnostics.Process.GetProcesses();
            var t = System.Diagnostics.Process.GetProcessesByName(target);
            foreach (var item in t)
            {
                if (item.ProcessName == target)
                    return true;
            }
            return false;
        }

        #endregion

    }

    /// <summary>
    /// Windowデータ
    /// </summary>
    [XmlType("WindowData")]
    public class WindowData
    {
        /// <summary>表示名前</summary>
        [XmlElement("DispName")]
        public string DispName = "";

        /// <summary>Window表示位置 X</summary>
        [XmlElement("WindowPosi_X")]
        public int WindowPosi_X = 0;

        /// <summary>Window表示位置 Y</summary>
        [XmlElement("WindowPosi_Y")]
        public int WindowPosi_Y = 0;

        /// <summary>Window幅</summary>
        [XmlElement("Window_Width")]
        public double Window_Width = 1000;

        /// <summary>Window高さ</summary>
        [XmlElement("Window_Height")]
        public double Window_Height = 800;

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowData(int x,int y ,double w ,double h)
        {
            WindowPosi_X = x;
            WindowPosi_Y = y;
            Window_Width = w;
            Window_Height = h;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="copy"></param>
        public WindowData(WindowData copy)
        {
            this.DispName = copy.DispName;
            this.WindowPosi_X = copy.WindowPosi_X;
            this.WindowPosi_Y = copy.WindowPosi_Y;
            this.Window_Width = copy.Window_Width;
            this.Window_Height = copy.Window_Height;
        }

        #endregion
    }

    /// <summary>
    /// アプリ画面設定データクラス
    /// ※派生して使用する
    /// </summary>
    public class WindowDataModel
    {
        /// <summary>ファイルパス</summary>
        public static string csFileName = @"\WindowData.xml";

        /// <summary>データ</summary>
        private WindowData mXML;

        /// <summary>
        /// データ
        /// </summary>
        public WindowData DATA
        {
            get { return mXML; }
            set { mXML = value; }
        }

        #region<関数　ロード>

        /// <summary>
        /// ロード
        /// </summary>
        public static WindowData Load_DataWindow(string pass)
        {
            WindowData mdata;
            try
            {
                mdata = (WindowData)FT.C.INIXML.LoadObjectMake<WindowData>(pass, new WindowData());
                //mdata.ini();
            }
            catch (System.InvalidOperationException)
            {
                mdata = new WindowData();
            }

            return mdata;
        }

        #endregion

        #region<関数　セーブ>

        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="pass">保存先</param>
        /// <param name="data"></param>
        public static void Save_DataWindow(string pass, WindowData data)
        {
            // セーブ(マシンデータフォルダー)
            FT.C.INIXML.SaveObject<WindowData>(pass, data);
        }

        #endregion
    }

    /// <summary>
    /// 表示方向
    /// </summary>
    public enum WindowDirectionContens
    {
        /// <summary>垂直（縦）</summary>
        Vertical,

        /// <summary>水平（横）</summary>
        Horizontal,
    }

}
