using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FT.C
{
    /// <summary>
    /// UDP通信クラス(未完成）
    /// </summary>
    public class UDP : IDisposable
    {

        #region<内部変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        /// <summary>接続ポート</summary>
        int mlocalPort = 9600;

        /// <summary>ソケット</summary>
        System.Net.Sockets.UdpClient objsck;

        /// <summary>ネットワークエンドポイント</summary>
        System.Net.IPEndPoint ipAny;

        /// <summary>データ受信スレッドＯＮＯＦＦ制御フラグ</summary>
        private bool ONOFFRun = false;

        /// <summary>データ受信スレッド</summary>
        private Thread threadRun = null;

        #endregion

        #region<イベント定義>

        /// <summary>データ受信のイベント</summary>
        public event LanBase.ReceiveLanDataDelegate ReceiveData;

        #endregion

        #region<プロパティ>

        /// <summary>
        /// データを受信完了フラグ　プロパティ
        /// </summary>
        public bool Receive
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 受信した文字列　プロパティ
        /// </summary>
        public string Data
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="localPort"></param>
        public UDP(int localPort)
        {
            mlocalPort = localPort;
            objsck = new System.Net.Sockets.UdpClient(localPort);
            ipAny = new System.Net.IPEndPoint(System.Net.IPAddress.Any, mlocalPort);
            //objsck.Connect("10.5.5.100", localPort);
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
                DisConect();
                ipAny = null;
            }
            finally
            {
            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<接続 切断>

        /// <summary>
        /// 接続  （受信スレッドも動作開始） 
        /// </summary>
        public void Conect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 切断  （受信スレッドを終了）
        /// </summary>
        public void DisConect()
        {
            if (objsck != null)
            {
                objsck.Close();
                objsck = null;
            }
        }

        #endregion

        #region<データ送信>

        /// <summary>
        /// データ送信（文字列） 
        /// </summary>
        /// <param name="Data">1:送信文字列（コマンドなど）</param>
        /// <returns></returns>
        /// <remarks>エンコードはSHIFT-JIS</remarks>
        public bool Send_String(string Data)
        {
            string sendcmd = "MEASURE";
            byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(sendcmd);
            if (objsck == null) return false;
            objsck.Send(dat, dat.GetLength(0));
            return true;
        }

        #endregion

        #region<データ受信>

        /// <summary>
        /// 受信データ有無確認
        /// </summary>
        /// <returns></returns>
        public bool PresenceReceveDara()
        {
            bool Um = false;
            if ((objsck != null) && (objsck.Available > 0))
            {
                Um = true;
            }
            return Um;
        }

        /// <summary>
        /// データ受信
        /// </summary>
        /// <returns></returns>
        public string[] Receve()
        {
            string[] strOUT = new string[0];

            if (objsck.Available > 0)
            {
                byte[] dat = objsck.Receive(ref ipAny);
                string soutText = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);
                strOUT = soutText.Split(',');
            }

            return strOUT;
        }

        #endregion
     
        #region<スレッド  データ受信  スレッド操作>

        /// <summary>
        /// データ受信スレッドスタート
        /// </summary>
        public void ReceveThread_Start()
        {
            if (ReceveThread_State())                    // すでにスレッドが起動していればスタートしない
                return;

            threadRun = new Thread(new ThreadStart(ReceveThreadLoop));
            threadRun.Priority = ThreadPriority.Lowest;
            ONOFFRun = true;
            threadRun.Start();
        }

        /// <summary>
        /// データ受信スレッドが起動しているかを返す
        /// </summary>
        /// <returns>true:起動　False:停止</returns>
        public bool ReceveThread_State()
        {
            bool b = false;

            if (threadRun != null)
            {
                b = threadRun.IsAlive;
            }
            return b;
        }

        /// <summary>
        /// データ受信スレッド終了 
        /// </summary>
        public void ReceveThread_End()
        {
            ONOFFRun = false;
        }

        #endregion

        #region<スレッド  データ受信  ループ処理>

        /// <summary>
        /// データ受信ループ
        /// </summary>
        private void ReceveThreadLoop()
        {
            while (ONOFFRun)
            {
                if(PresenceReceveDara())
                {
                    if (null != ReceiveData)
                        ReceiveData( this, new ReceiveLanDataEventArgs(""));                      // 実行完了イベント発生
                }
                Thread.Sleep(10);
            }

            threadRun.Abort();
        }

        #endregion

    }
}
