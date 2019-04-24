using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FT.C
{

    /// <summary>
    /// ソケット通信（サーバー）クラス
    /// </summary>
    public class SoketSVR : IDisposable
    {

        #region<デリゲート>

        /// <summary>クライアントが接続した</summary>
        public delegate void dlgConnect(SoketEventArgs e);

        /// <summary>データを受信した</summary>
        public delegate void dlgRev(ReceiveSoketDataEventArgs e);

        /// <summary>クライアントが切断した</summary>
        public delegate void dlgDisConnect(SoketEventArgs e);

        /// <summary>システムエラーが発生した</summary>
        public delegate void dlgError(ReceiveSoketDataEventArgs e);

        #endregion

        #region<イベント>

        /// <summary>クライアントが接続した</summary>
		public event dlgConnect onConnect = null;

        /// <summary>データを受信した</summary>
        public event dlgRev onRecive = null;

        /// <summary>クライアントが切断した</summary>
        public event dlgDisConnect onDisConnect = null;

        /// <summary>システムエラーが発生した</summary>
        public event dlgError onError = null;

        #endregion

        #region<変数>

        /// <summary>
        /// ポートクラス
        /// </summary>
        private List<SVR> mSVR;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int[] mOpenPort;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="OpenPort">開放ポート</param>
        /// <param name="IP">IPアドレス</param>
        public SoketSVR(int[] OpenPort , string IP = Socket.SCK_IP_MYSELF)
        {
            mSVR = new List<SVR>();
            mOpenPort = OpenPort;       // ポート番号

            for (int i = 0; i < OpenPort.Length; i++)
            {
                var s = new SVR(OpenPort[i], IP);
                s.onRecive += S_onRecive;            // 受信
                s.onConnect += S_onConnect;
                s.onError += S_onError;
                s.onDisConnect += S_onDisConnect;
                mSVR.Add(s);
            }
        }

        #endregion

        #region<イベント>

        /// <summary>
        /// 受信イベント
        /// </summary>
        /// <param name="e"></param>
        private void S_onRecive(ReceiveSoketDataEventArgs e)
        {
            if (null != onRecive)
                onRecive(e);
        }

        /// <summary>
        /// 接続エラー
        /// </summary>
        /// <param name="e"></param>
        private void S_onError(ReceiveSoketDataEventArgs e)
        {
            if (null != onError)
                onError(e);
        }

        /// <summary>
        /// 接続した
        /// </summary>
        /// <param name="e"></param>
        private void S_onConnect(SoketEventArgs e)
        {
            if (null != onConnect)
                onConnect(e);
        }

        /// <summary>
        /// 切断した
        /// </summary>
        /// <param name="e"></param>
        private void S_onDisConnect(SoketEventArgs e)
        {
            if (null != onDisConnect)
                onDisConnect(e);
        }

        #endregion

        #region<Dispose>

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {

            // 既にリソースが破棄されている場合は何もしない
            if (disposed) return;

            try
            {
                // 破棄されていないアンマネージリソースの解放処理
                if (mSVR != null)
                {
                    foreach (var item in mSVR)
                    {
                        item.Dispose();
                    }
                    this.mSVR = null;
                }

             

            }
            finally
            {
                // リソースは破棄されている
                disposed = true;
            }
        }

        #endregion

        #region<関数 ポート番号から配列番号を取得>

        /// <summary>
        /// ポート番号から配列番号を取得
        /// </summary>
        /// <param name="PortNo"></param>
        /// <returns></returns>
        private int GetRegista(int PortNo)
        {
            for (int i = 0; i < mOpenPort.Length; i++)
            {
                if (mOpenPort[i] == PortNo)
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion

        #region<関数 ソケット 接続処理>

        /// <summary>
        /// 接続処理
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            foreach (var item in mSVR)
            {
                item.Connect();
            }
        }

        #endregion

        #region<関数 ソケット 終了処理>

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Close()
        {
            foreach (var item in mSVR)
            {
                item.Close();
            }
        }

        #endregion

        #region<関数 ソケット データ送信>

        /// <summary>
		/// データ送信メソッド（ポート番号）
		/// </summary>
		/// <param name="Message">送信メッセージ</param>
        /// <param name="portNo">ポート番号</param>
		public void Send(string Message , int portNo)
        {
            mSVR[GetRegista(portNo)].Send(Message);
        }

        /// <summary>
		/// データ送信メソッド(配列番号)
		/// </summary>
		/// <param name="Message">送信メッセージ</param>
        /// <param name="AppNo">アプリケーション番号 ポート番号</param>
		public void Send_AppNp(string Message, int AppNo)
        {
            mSVR[AppNo].Send(Message);
        }

        #endregion

    }

    /// <summary>
    /// ソケット通信（サーバー）クラス
    /// </summary>
    /// 
    /// <remarks>
    /// ソケット通信処理群
    /// 
    /// Ver1.00  2012-04-05  リリース    H.Sato
    /// Ver1.01  2018-01-26  拡張        Y.Hirotani
    /// </remarks>
    /// 
    public class SVR : IDisposable
	{

        #region<デリゲート>

        /// <summary>クライアントが接続した</summary>
        public delegate void dlgConnect(SoketEventArgs e);

		/// <summary>データを受信した</summary>
		public delegate void dlgRev( ReceiveSoketDataEventArgs e);

		/// <summary>クライアントが切断した</summary>
		public delegate void dlgDisConnect(SoketEventArgs e);

		/// <summary>システムエラーが発生した</summary>
		public delegate void dlgError(ReceiveSoketDataEventArgs ErrStr);

        #endregion

        #region<イベント>

        /// <summary>クライアントが接続した</summary>
		public event dlgConnect		onConnect = null;

		/// <summary>データを受信した</summary>
		public event dlgRev			onRecive = null;

		/// <summary>クライアントが切断した</summary>
		public event dlgDisConnect	onDisConnect = null;

		/// <summary>システムエラーが発生した</summary>
		public event dlgError		onError = null;

        #endregion

        #region<変数>

        /// <summary>ソケット情報</summary>
        private SoketInfo mSoketInfo;

        /// <summary>自クラス名</summary>
        private string MY_CLASS;

        /// <summary>サーバーオブジェクト</summary>
		private TcpClient mServer = null;

        /// <summary>サーバーのリスナーオブジェクト</summary>
		private TcpListener mListener = null;

        /// <summary>サーバーのスレッドオブジェクト</summary>
		private Thread mThreadServer = null;

        /// <summary>Uniコード</summary>
		private Encoding UniCode = Encoding.GetEncoding("utf-16");

        /// <summary>Shif-jisコード</summary>
		private Encoding SJisCode = Encoding.GetEncoding("shift-jis");

        #endregion

        #region<プロパティ>

        /// <summary>
        /// 接続中？
        /// </summary>
        public bool IsConnect
        {
            get; private set;
        }

        #endregion

        #region<コンストラクタ>

        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ApriNo">アプリケーション番号(0～)</param>
        /// <param name="IP">IPアドレス</param>
		public SVR( int ApriNo, string IP)
		{
			MY_CLASS = typeof(SVR).Name;
            mSoketInfo = new C.SoketInfo();
            mSoketInfo.Port = ApriNo;
            mSoketInfo.IP = IP;
        }

        #endregion

        #region<スレッド　受信処理>

        /// <summary>
        /// 受信スレッド
        /// </summary>
        private void ThreadRecive()
        {

            Byte[] Rev1Byte = new Byte[1];
            Byte[] RevByte = new Byte[Socket.SCK_REV_BUFSIZE];
            Byte[] RevNeed;
            Byte[] RevUni;
            int iLp1;
            int nPt = 0;
            string RevStr;

            try
            {
                //クライアントの要求があったら、接続を確立する
                //クライアントの要求が有るまでここで待機する 
                mServer = mListener.AcceptTcpClient();
            }
            catch (System.Exception exp)
            {
                /*--- その他のエラー ---*/

                // システムエラーイベント発行
                if (null != onError)
                    onError(new C.ReceiveSoketDataEventArgs(exp.Message, mServer.Client.RemoteEndPoint, mSoketInfo));
                return;
            }

            // クライアントが接続した事がわかる様にイベントを発行
            if (null != onConnect)
                onConnect( new C.SoketEventArgs( mServer.Client.RemoteEndPoint,mSoketInfo));

            //クライアントとの間の通信に使用するストリームを取得
            NetworkStream nStream = mServer.GetStream();

            // 受信用ループ
            while (true)
            {

                try
                {
                    //受信が無い場合はここで待機する
                    //文字受信が有った場合とクライアントが接続を切った場合に
                    //次のステップに進む
                    int RevByteCount = nStream.Read(Rev1Byte, 0, Rev1Byte.Length);

                    if (0 < RevByteCount)
                    {
                        /*--- １バイト受信 ---*/

                        if (0x0D == Rev1Byte[0])
                        {
                            /*--- 改行コード迄、受信した ---*/

                            // 必要な分だけ切り出す
                            RevNeed = new Byte[nPt];

                            for (iLp1 = 0; iLp1 < nPt; iLp1++)
                            {
                                RevNeed[iLp1] = RevByte[iLp1];
                            }

                            // Shift-jisからUniコードに変換
                            RevUni = Encoding.Convert(SJisCode, UniCode, RevNeed);

                            // Uniコードのバイト配列から文字列に変換する
                            RevStr = UniCode.GetString(RevUni);

                            // 受信イベント発行
                            if (null != onRecive)
                                onRecive( new ReceiveSoketDataEventArgs( RevStr, mServer.Client.RemoteEndPoint,mSoketInfo));

                            nPt = 0;

                        }
                        else
                        {
                            RevByte[nPt] = Rev1Byte[0];
                            nPt++;
                        }

                    }
                    else
                    {
                        /*--- クライアントが切断 ---*/

                        // クライアントが切断した事がわかる様にイベントを発行
                        if (null != onDisConnect)
                            onDisConnect( new SoketEventArgs( mServer.Client.RemoteEndPoint,mSoketInfo));
                        return;

                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    /*--- スレッドが破棄された ---*/
                    return;
                }
                catch (System.Exception exp)
                {
                    /*--- その他のエラー ---*/

                    // システムエラーイベント発行
                    if (null != onError) onError( new ReceiveSoketDataEventArgs( exp.Message, mServer.Client.RemoteEndPoint, mSoketInfo));
                }
            }
        }

        #endregion

        #region<Dispose>

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region<関数 ソケット 接続処理>

        /// <summary>
        /// 接続処理
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                if (mListener == null)
                {
                    // リスナー作成
                    mListener = new TcpListener(IPAddress.Parse(mSoketInfo.IP),  mSoketInfo.Port);
                }

                // クライアントからの接続受付
                mListener.Start();

                // スレッドの作成と開始
                mThreadServer = new Thread(new ThreadStart(ThreadRecive));
                mThreadServer.Start();

                return true;
            }
            catch (System.Exception)
            {
                // リスナー停止
                mListener.Stop();
                return false;
            }
        }

        #endregion

        #region<関数 ソケット 終了処理>

        /// <summary>
        /// 本クラス破棄メソッド
        /// </summary>
        public void Close()
        {

            // 既にリソースが破棄されている場合は何もしない
            //if (!IsConnect) return;

            // リスナー破棄
            if (null != mListener)
            {
                mListener.Stop();
                mListener = null;
            }

            // サーバー破棄
            if (null != mServer)
            {
                if (true == mServer.Connected)
                {
                    mServer.Close();
                }

                if (null != mThreadServer)
                {
                    mThreadServer.Abort();
                    mThreadServer = null;
                }
            }

        }

        #endregion

        #region<関数 ソケット データ送信>

        /// <summary>
		/// データ送信メソッド
		/// </summary>
		/// <param name="Message">送信メッセージ</param>
		public void Send( string Message )
		{
			// Sift-jisに変換して送る
            Byte[] SendByte = SJisCode.GetBytes( Message + "\r" );
            NetworkStream nStream = null;

            try
            {
				nStream = mServer.GetStream();
                nStream.Write( SendByte, 0, SendByte.Length );
            }
            catch( System.Exception exp ){

                throw (new EXP(exp.Message, MY_CLASS, "002"));  
            }
		}

        #endregion

        #region<関数 ソケット 接続情報を文字で取得>

        /// <summary>
        /// 自身（サーバー）の情報を文字で取得
        /// </summary>
        /// <returns></returns>
        public string GetInfo_This()
        {
            return mServer.Client.LocalEndPoint.ToString();
        }

        /// <summary>
        /// スレーブの情報を文字で取得
        /// </summary>
        /// <returns></returns>
        public string GetInfo_Slave()
        {
            return mServer.Client.RemoteEndPoint.ToString();
        }

        #endregion

    }

    /// <summary>
    /// ソケット 接続イベントハンドラ
    /// </summary>
    public class SoketEventArgs : EventArgs
    {

        /// <summary>
        /// ターゲット情報
        /// </summary>
        public System.Net.EndPoint target
        {
            get; private set;
        }

        /// <summary>
        /// 自身の情報
        /// </summary>
        public SoketInfo info
        {
            get; private set;
        }

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SoketEventArgs(System.Net.EndPoint point, SoketInfo info)
        {
            target = point;
            this.info = info;
        }

        #endregion

    }


    /// <summary>
    /// ソケットデータ受信イベントハンドラ
    /// </summary>
    public class ReceiveSoketDataEventArgs : SoketEventArgs
    {

        /// <summary>
        /// 受信データ　プロパティ
        /// </summary>
        public string ReciveData
        {
            get; private set;
        }

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReceiveSoketDataEventArgs(string sData, System.Net.EndPoint point , SoketInfo info)
            : base(point,info)
        {
            ReciveData = sData;
        }

        #endregion

    }


}
