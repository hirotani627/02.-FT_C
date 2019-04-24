using System;
using System.IO.Ports;
using System.Text;
using System.Diagnostics;                   // トレース機能

namespace FT.C
{
    /// <summary>
    /// RS_232C制御クラス
    /// </summary>
    public class RS_232C : IDisposable
    {

        #region<内部変数>

        /// <summary>ポート番号</summary>
        private int mPortNo;

        /// <summary>COMポート制御クラス</summary>
        private System.IO.Ports.SerialPort serialPort;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        /// <summary>ボーレート</summary>
        private int mBaudRate = 9600;

        /// <summary>パリティ</summary>
        private Parity mParity = Parity.None;

        /// <summary>ストップビット</summary>
        private StopBits mStopBits = StopBits.One;

        /// <summary>フロー制御</summary>
        private Handshake mHandshake = Handshake.None;

        /// <summary>ターミナルコマンド（送信時）</summary>
        string sEndComanndSend = "";

        /// <summary>ターミナルコマンド（受信時）</summary>
        string sEndComanndRecive;

        #endregion

        #region<イベント定義・デリゲート・バックグラウンドワーカー>

        /// <summary>データ受信デリゲート</summary>
        public delegate void DataReceivedEventHandler(object sender, RS_232C_EventArgs e);

        /// <summary>データ受信したイベント(※Formとは別スレッド)</summary>
        /// <remarks>
        /// 【フォームで使う時のサンプル】
        /// if (this.InvokeRequired)
        /// {
        ///     this.Invoke((MethodInvoker)delegate()
        ///     {
        ///          this.txtRacive.Text = e;
        ///     });
        /// }
        /// else
        /// {
        ///      this.txtRacive.Text = e;
        /// }
        /// </remarks>
        public event DataReceivedEventHandler DataReceived = null;

        #endregion

        #region<プロパティ>

        /// <summary>
        /// プロパティ　接続中？
        /// </summary>
        public bool IsConnect
        {
            get {
                if (serialPort != null)
                    return serialPort.IsOpen;
                else
                    return false;
            }
        }

        /// <summary>
        /// プロパティ　エラーメッセージ
        /// </summary>
        public string ErrMesseage
        {
            get;
            private set;
        }

        /// <summary>
        /// バッファー有無モード 
        /// </summary>
        public bool IsNonBufferMode { get; set; }

        #endregion

        #region<イベント　受信>
        object oRecive = new object();
        string sBuf = string.Empty;

        /// <summary>
        /// 受信処理。本メソッドはFormとは別スレッドで処理されるため、
        /// Formの値の書き換えを行う場合にはdelegateを利用してForm側のスレッドで処理させる。
        /// </summary>
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // 受信文字列の取得
            try
            {

                lock (oRecive)
                {
                    string receivedData = this.serialPort.ReadExisting();
                    receivedData = FT.C.FTString.DeleteTerminal(receivedData);
                    Trace.WriteLine("RS-232C受信:" + receivedData);

                    if (IsNonBufferMode)
                    {
                        //　受信イベント
                        if (DataReceived != null)
                            DataReceived(this, new C.RS_232C_EventArgs(this.mPortNo, receivedData));
                    }
                    else
                    {
                        // 終了コマンドある？
                        int i = receivedData.IndexOf(sEndComanndRecive);
                        if (i == -1)
                        {
                            // 見つからない場合【結合処理】
                            // ターミネータコマンド受信まで保存する
                            sBuf = sBuf + receivedData;
                        }
                        else
                        {
                            //　受信イベント
                            if (DataReceived != null)
                            {
                                // 送信コマンドの整理
                                string sSend;
                                if (string.Empty == sBuf)
                                    sSend = receivedData;
                                else
                                    sSend = sBuf;

                                // 終了コマンドを \n に統一する
                                sSend = FTString.GetString_Front(sSend, sEndComanndRecive);
                                sSend += "\n";

                                // イベントの送信
                                if (DataReceived != null)
                                {
                                    DataReceived(this, new C.RS_232C_EventArgs(this.mPortNo, sSend));
                                }
                                System.Threading.Thread.Sleep(10);
                            }
                            sBuf = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMesseage = ex.Message;
            }

        }

        #endregion

        #region<イベント　ポートでエラーが発生>

        /// <summary>
        /// ポートでエラーが発生イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
        }

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RS_232C()
        {
            serialPort = new SerialPort();
            IsNonBufferMode = false;    
        }

        #endregion

        #region <Dispose>

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected void Dispose(bool disposing)
        {
            // 既にリソースが破棄されている場合は何もしない
            if (disposed) return;

            try
            {
                // 破棄されていないアンマネージリソースの解放処理
                Close();    //切断
                if (serialPort != null)
                    serialPort.Dispose();
            }
            finally
            {

            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<関数　初期化>

        /// <summary>
        /// 初期化
        /// </summary>
        public void Ini(int BaudRate, Parity parity ,StopBits stopBits)
        {
            mBaudRate = BaudRate;
            mParity = parity;
            mStopBits = stopBits;
        }

        #endregion

        #region<関数　接続>

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="port">使用するポート番号</param>
        /// <returns>True:接続完了 False:失敗</returns>
        public bool Connect(int port)
        {
            mPortNo = port;
            try
            {
                if (serialPort.IsOpen)
                {
                    ErrMesseage = "COM番号" + mPortNo.ToString() + " はすでに開かれています。";
                    return false;
                }
                else
                {
                    this.serialPort.PortName = "COM" + mPortNo;        // COMｘで表す
                    this.serialPort.BaudRate = mBaudRate;
                    this.serialPort.Parity = mParity;
                    this.serialPort.DataBits = 8;
                    this.serialPort.StopBits = mStopBits;
                    this.serialPort.Handshake = mHandshake;
                    this.serialPort.Encoding = Encoding.UTF8;
                    this.serialPort.NewLine = sEndComanndRecive;
                    serialPort.Open();
                    this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                    this.serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort1_ErrorReceived);
                }
            }
            catch (Exception ex)
            {
                ErrMesseage = ex.Message;
                return false;
            }
             return true;
        }

        #endregion

        #region<関数　切断>

        /// <summary>
        /// 切断
        /// </summary>
        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                this.serialPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                this.serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(SerialPort1_ErrorReceived);
            }
        }

        #endregion

        #region<関数　COM設定>


        /// <summary>
        /// ターミナルコマンド（受信時）
        /// </summary>
        /// <param name="v"></param>
        public void SetEndComanndRecive(string v)
        {
            sEndComanndRecive = v;
        }

        /// <summary>
        /// ターミナルコマンド（送信時）
        /// </summary>
        /// <param name="v"></param>
        public void SetEndComanndSend(string v)
        {
            sEndComanndSend = v;
        }

        /// <summary>ボーレートの設定</summary>
        public void SetBaudRate(int value)
        {
            mBaudRate = value;
        }

        /// <summary>
        /// パリティーの設定 0:Non 1:Odd 2:Even 3:Mark 4:Space
        /// </summary>
        /// <param name="value"></param>
        public void SetParity(Parity value)
        {
            mParity = (Parity)value;
        }

        /// <summary>フロー制御の設定</summary>
        public void SetHandshake(Handshake value)
        {
            mHandshake = value;
        }

        /// <summary>ストップビットの設定</summary>
        public void SetStopBits(StopBits value)
        {
            mStopBits = value;
        }

        #endregion

        #region<関数　バッファーからデータを破棄>

        /// <summary>
        /// シリアル ドライバーの受信バッファーからデータを破棄
        /// </summary>
        public void DiscardInBuffer()
        {
            this.serialPort.DiscardInBuffer();
        }

        /// <summary>
        /// シリアル ドライバーの送信バッファーからデータを破棄
        /// </summary>
        public void DiscardOutBuffer()
        {
            this.serialPort.DiscardOutBuffer();
        }

        #endregion

        #region<関数　送信>

        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="text">送信文字列</param>
        public void SendStringLine(string text)
        {
            //SendString(text + NewLine);   //@@@
            SendString(text);
        }


        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="text">送信文字列</param>
        public bool SendString(string text)
        {
            try
            {
                this.serialPort.Write(text + sEndComanndSend);
                return true;
            }
            catch (Exception ex)
            {
                ErrMesseage = ex.Message;
                return false;
            }
        }

        #endregion

        #region<関数　有効なシリアルポート名一覧取得>

        /// <summary>
        /// 有効なシリアルポート名一覧取得
        /// </summary>
        /// <returns></returns>
        public string[] GetEnablePort()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }


        #endregion

    }


    /// <summary>
    /// イベントデータクラス
    /// </summary>
    public class RS_232C_EventArgs
    {
        /// <summary>
        /// COMNo
        /// </summary>
        public int COMNo { get; set; }

        /// <summary>
        /// 受信コマンド
        /// </summary>
        public string Messeage { get; set; }

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="COMNo">COM番号</param>
        /// <param name="msg">受信コマンド</param>
        public RS_232C_EventArgs(int COMNo, string msg)
        {
            this.COMNo = COMNo;
            Messeage = msg;
        }

        #endregion

    }

}
