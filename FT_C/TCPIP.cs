using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace FT.C
{
    /// <summary>
    /// TCP-IP通信                								
    /// </summary>
    public class TCPIP : LanBase ,IDisposable
    {

        /// <summary>タイムアウト×1</summary>
        private static int _Timeout = 2000;       // 2S

        #region<内部定数>

        ///// <summary>接続先ID</summary>
        //public const string localIP = "10.5.5.100";   

        ///// <summary>接続先ポート</summary>
        //public const int localPort = 9876;

        #endregion

        #region<内部変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool IsSted = true;

        /// <summary>データを受信したフラグ</summary>
        private bool mReceiveFlg = false;

        /// <summary>接続アドレス</summary>
        private TCPConnectAddres mConnectAdd;

        /// <summary>受信したデータ保存用</summary>
        private string sRec = "";

        /// <summary>送信したデータ保存用</summary>
        private string sSend = "";


        /// <summary>受信スレッド</summary>
        private Thread mReceiveThread;

        /// <summary>ソケット</summary>
        private System.Net.Sockets.TcpClient objSck;

        /// <summary>データストリーム</summary>
        private System.Net.Sockets.NetworkStream objStm;     

        #endregion

        #region<イベント定義・デリゲート>

        /// <summary>データ受信のイベント</summary>
        public event LanBase.ReceiveLanDataDelegate ReceiveData;

        #endregion

        #region<プロパティ>

        /// <summary>
        /// データを受信完了フラグ　プロパティ
        /// </summary>
        public bool Receive
        {
            get { return mReceiveFlg; }
            //set { mReception = value; }
        }

        /// <summary>
        /// 受信した文字列　プロパティ
        /// </summary>
        public string Data      
        {
            get 
            {
                mReceiveFlg = false;            //データを受信完了フラグをクリアする
                return sRec;
            }
        }

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ	
        /// </summary>
        /// <param name="ConAdd">接続先のアドレス</param>
        public TCPIP(TCPConnectAddres ConAdd)
        {
            //接続情報を保存する
            mConnectAdd = ConAdd;
        }

        /// <summary>
        /// コンストラクタ	
        /// </summary>
        /// <param name="IP">接続先のアドレス</param>
        /// <param name="Port">ポート番号</param>
        public TCPIP(string IP,  int Port)
        {
            //接続情報を保存する
            mConnectAdd = new TCPConnectAddres(IP, Port);
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
                mReceiveThread = null;

                DisConect();                //切断
            }
            finally
            {
            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<関数 TCP接続>

        /// <summary>
        /// TCP接続  （受信スレッドも動作開始） 
        /// </summary>
        public bool Conect()
        {
            try
            {
                objSck = new System.Net.Sockets.TcpClient();
                
                objSck.SendTimeout = _Timeout;
                objSck.ReceiveTimeout = _Timeout;
                objSck.Connect(mConnectAdd.IP, mConnectAdd.Port);     //アドレス設定

                objStm = objSck.GetStream();
                objStm.WriteTimeout = _Timeout;                       // タイムアウトの設定
                objStm.ReadTimeout = _Timeout;                        // タイムアウトの設定


                //スレッドが起動されて、正常終了していないまたは中止されていない場合は true 。それ以外の場合は false 
                if ((mReceiveThread == null) || (!mReceiveThread.IsAlive))
                {
                    mReceiveThread = new Thread(new ThreadStart(ReceiveThread));     //受信スレッドを作成する
                    mReceiveThread.Start();

                }

                return objSck.Connected;
            }
            catch (System.Exception exp)
            {
                throw (new Exception("TCP接続:" + exp.Message));
            }
        }

        #endregion

        #region<関数 TCP切断>

        /// <summary>
        /// TCP切断  （受信スレッドを終了）
        /// </summary>
        public void DisConect()
        {
            try
            {
                IsSted = false;
                if (mReceiveThread != null) 
                    mReceiveThread.Abort();                 //受信スレッドを終了させる

                if (objStm != null)
                {
                    objStm.Close();
                    objStm = null;
                }

                if (objSck != null)
                {
                    objSck.Close();
                    objSck = null;
                }
            }
            catch (System.Exception exp)
            {
                throw (new  Exception("TCP切断:"+ exp.Message));
            }
        }

        #endregion

        #region<関数 ターミナルコマンド>

        /// <summary>
        /// ターミナルコマンド
        /// </summary>
        public string EndTrminal = "\r\n";

        /// <summary>
        /// ターミナルコマンドの設定
        /// </summary>
        /// <param name="s"></param>
        public void SetEndTrminal(string s)
        {
            EndTrminal = s;
        }

        #endregion

        #region<関数 データ送信>

        /// <summary>
        /// データ送信（文字列） 
        /// </summary>
        /// <param name="Data">1:送信文字列（コマンドなど）</param>
        /// <returns></returns>
        /// <remarks>エンコードはSHIFT-JIS</remarks>
        public bool Send_String(string Data)
        {
            bool b = false;

            sSend = Data;

            if (Data != null)
            {
                try
                {
                    Data = Data + EndTrminal;
                    if ((objSck != null) && (objStm.CanWrite))                     //書き込みが可能でないなら抜ける
                    {
                        byte[] sdat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(Data);
                        objStm.Write(sdat, 0, (int)sdat.GetLongLength(0));          //データ送信
                        b = true;
                    }
                }
                catch (System.Exception exp)
                {
                    throw (new  Exception("データ送信:" + exp.Message));
                }
            }
            return b;
        }


        #endregion

        #region<関数 データ受信>

        /// <summary>
        /// データ受信接続待ちスレッド
        /// </summary>
        private void ReceiveThread()
        {
            try
            {
                while (IsSted)
                {
                    if (objSck != null)
                    {
                        if (objSck.Available > 0)
                        {
                            byte[] rdat = new byte[objSck.Available];
                            objStm.Read(rdat, 0, rdat.GetLength(0));
                            sRec = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(rdat);
                            mReceiveFlg = true;         //受信フラグ
                            if(ReceiveData != null)
                            {
                                var a = new ReceiveLanDataEventArgs(sRec);
                                a.SendData = sSend;
                                ReceiveData(this,a);        //イベントを発生させる
                            }
                                
                        }
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            catch (System.Exception exp)
            {
                if (IsSted)
                {
                    throw (new Exception("データ受信接続待ちスレッド:" + exp.Message));
                }
            }
        }

        #endregion

        #region<デバック>

        /// <summary>
        /// デバック用】受信イベントを発生させる
        /// </summary>
        public void Debugg_OnTCP_Receive()
        {
            if(ReceiveData != null)
                ReceiveData( this, new ReceiveLanDataEventArgs("【デバック】受信イベントを発生させました"));      
        }

        #endregion
      
    }

    #region<インターフェイス　LAN通信>

    /// <summary>
    /// LAN通信インターフェイス
    /// </summary>
    public interface ILAN
    {
        /// <summary>データ受信のイベント</summary>
        event LanBase.ReceiveLanDataDelegate ReceiveData;

        /// <summary>
        /// 受信フラグ
        /// </summary>
        bool Receive { get; }

        /// <summary>
        /// 受信データ
        /// </summary>
        string Data { get; }

        /// <summary>
        /// 接続
        /// </summary>
        void Conect();

        /// <summary>
        /// 切断
        /// </summary>
        void DisConect();

        /// <summary>
        /// 文字送信
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        bool Send_String(string Data);
    }

    #endregion

    /// <summary>
    /// データ受信イベントハンドラ
    /// </summary>
    public class ReceiveLanDataEventArgs : EventArgs
    {
        /// <summary>受信データ</summary>
        private string mReciveData;

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReceiveLanDataEventArgs(string sData)
        {
            mReciveData = sData;
        }

        #endregion

        #region<プロパティ>

        /// <summary>
        /// 受信データ　プロパティ
        /// </summary>
        public string ReciveData
        {
            get { return mReciveData; }
        }

        /// <summary>
        /// 送信データ　プロパティ
        /// </summary>
        public string SendData
        {
            get; set;
        }

        /// <summary>
        /// パラメータ
        /// </summary>
        public bool Para
        {
            get; set;
        }

        #endregion

    }


    /// <summary>
    /// TCP-IP 接続アドレス
    /// </summary>
    public struct TCPConnectAddres
    {
        /// <summary>IPアドレス</summary>
        public string IP;

        /// <summary>ポート番号</summary>
        public int Port;

        /// <summary>接続完了フラグ</summary>
        public bool IsConnected;

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ip">接続IPアドレス</param>
        /// <param name="port">接続ポート番号</param>
        public TCPConnectAddres(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;

            IsConnected = false;
        }

        #endregion

    }

    /// <summary>
    /// LAN通信制御クラス
    /// </summary>
    public abstract class LanBase
    {
        /// <summary>データ受信のイベント</summary>
        public delegate void ReceiveLanDataDelegate(object sender, ReceiveLanDataEventArgs e);

        #region<関数 ホスト名からIPアドレスを取得する>

        /// <summary>
        /// ホスト名からIPアドレスを取得する
        /// </summary>
        /// <param name="hostname">調べたいホスト名(例)"www.yahoo.co.jp"</param>
        public static string GetHostIPAdres(string hostname)
        {
            // IPHostEntry取得
            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(hostname);

            // 最初のアドレスを取得
            System.Net.IPAddress ip = hostEntry.AddressList[0];

            // xxx.xxx.xxx.xxx形式の文字列を取得
            return ip.ToString();
        }

        #endregion

        #region<関数 IPアドレスの取得>

        /// <summary>
        /// IPアドレスの取得
        /// </summary>
        /// <returns></returns>
        public string GetIPAdress()
        {
            // ホスト名を取得
            string hostname = Dns.GetHostName();

            // ホスト名からエントリー取得
            IPHostEntry ipentry = Dns.GetHostEntry(hostname);

            // IPアドレスは２つ以上のケースもあるのですべて列挙（有線＋無線など）
            string msg = string.Empty;
            foreach (IPAddress ip in ipentry.AddressList)
            {
                msg += ip.ToString() + "\r\n";
            }

            return msg;
        }

        #endregion

        #region<関数 ホスト名の取得>

        /// <summary>
        /// ホスト名の取得
        /// </summary>
        /// <returns></returns>
        public string GetHost()
        {
            return Dns.GetHostName();
        }

        #endregion

    }

}
