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
	/// ソケット通信（クライアント）クラス
	/// </summary>
	/// 
	/// <remarks>
	/// ソケット通信処理群
	/// 
	/// Ver1.00  2012-04-05  リリース    H.Sato
	/// 
	/// 
	/// [ 例外リスト ]
	/// 001.通信開始に失敗した
	/// 002.データの送信に失敗した
	/// 
	/// </remarks>
	///
	public class CLT : IDisposable
	{

	/* デリゲート */

		/// <summary>クライアントが接続した</summary>
        public delegate void dlgConnect();

		/// <summary>データを受信した</summary>
		public delegate void dlgRev(string RevStr);

		/// <summary>クライアントが切断した</summary>
		public delegate void dlgDisConnect();

		/// <summary>システムエラーが発生した</summary>
		public delegate void dlgError(string ErrStr);

	/* イベント */

		/// <summary>クライアントが接続した</summary>
		public event dlgConnect		onConnect = null;

		/// <summary>データを受信した</summary>
		public event dlgRev			onRecive = null;

		/// <summary>クライアントが切断した</summary>
		public event dlgDisConnect	onDisConnect = null;

		/// <summary>システムエラーが発生した</summary>
		public event dlgError		onError = null;

	/* クラス内変数 */

		private int mApriNo=0;											// アプリケーション番号
		private string MY_CLASS;										// 自クラス名
		private TcpClient mClient = null;								// クライアントオブジェクト
		private Thread mThreadClient = null;							// クライアントのスレッドオブジェクト
		private Encoding UniCode = Encoding.GetEncoding("utf-16");		// Uniコード
		private Encoding SJisCode = Encoding.GetEncoding("shift-jis");	// Shif-jisコード

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <param name="ApriNo">アプリケーション番号(0～)</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public CLT( int ApriNo )
		{

			MY_CLASS = typeof(CLT).Name;
			mApriNo = ApriNo;

		}

		/// <summary>
		/// 終了処理
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Dispose()
		{
			Close();
		}

		/// <summary>
		/// 通信開始メソッド
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Start()
		{

			try
            {   
                // スレッドの作成と開始
                mThreadClient = new Thread( new ThreadStart( ThreadRecive ) );
                mThreadClient.Start();
            }
            catch( System.Exception exp ){
 
                throw (new EXP(exp.Message, MY_CLASS, "001"));
            }

		}

		/// <summary>
		/// データ送信メソッド
		/// </summary>
		/// 
		/// <param name="Message">送信メッセージ</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Send(string Message)
		{
			// Sift-jisに変換して送る
            Byte[] SendByte = SJisCode.GetBytes( Message + "\r" );
            NetworkStream nStream = null;

            try
            {
				nStream = mClient.GetStream();
                nStream.Write( SendByte, 0, SendByte.Length );
            }
            catch( System.Exception exp ){

                throw (new EXP(exp.Message, MY_CLASS, "002"));  
            }
		}
		
		/// <summary>
		/// 本クラス破棄メソッド
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Close()
		{

			// クライアント破棄
			if( null != mClient ){
				if( true == mClient.Connected ){
					mClient.Close();
				}
				
				if( null != mThreadClient ){
					mThreadClient.Abort();
					mThreadClient = null;
				}
			}

		}

		/// <summary>
		/// 受信スレッド
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		private void ThreadRecive()
		{

			Byte[]	Rev1Byte	= new Byte[1];
			Byte[]	RevByte		= new Byte[CST.SCK_REV_BUFSIZE];
			Byte[]	RevNeed;
			Byte[]	RevUni;
			int		iLp1;
			int		nPt = 0;
			string	RevStr;

			while(true){

				try{
					// クライアントのソケットを用意
                    mClient = new TcpClient(CST.SCK_IP_MYSELF, CST.SCK_PORT_OFST + mApriNo);      // (ソケット自ノードループバック用ＩＰアドレス ,ソケットポート番号オフセット アプリ番号）
					break;
				}
				catch( System.Net.Sockets.SocketException ){
				/*--- サーバーがリッスン状態になっていない ---*/
					Thread.Sleep(500);
				}
				catch( System.Exception exp ){
				/*--- その他のエラー ---*/
					
					// システムエラーイベント発行
					if( null != onError ){
						onError( exp.Message );
					}
					return;
				}
			}

			// サーバーと接続した事がわかる様にイベントを発行
			if( null != onConnect ){
				onConnect();
			}

			NetworkStream nStream = mClient.GetStream();

			// 受信用ループ
			while(true){

				try{

					//受信が無い場合はここで待機する
					//文字受信が有った場合とクライアントが接続を切った場合に
					//次のステップに進む
					int RevByteCount = nStream.Read( Rev1Byte, 0, Rev1Byte.Length );

					if( 0 < RevByteCount ){
					/*--- １バイト受信 ---*/

						if( 0x0D == Rev1Byte[0] ){
						/*--- 改行コード迄、受信した ---*/

							// 必要な分だけ切り出す
							RevNeed = new Byte[nPt];

							for( iLp1 = 0; iLp1 < nPt; iLp1++ ){
								RevNeed[iLp1] = RevByte[iLp1];
							}

							// Shift-jisからUniコードに変換
							RevUni = Encoding.Convert( SJisCode, UniCode, RevNeed );

							// Uniコードのバイト配列から文字列に変換する
							RevStr = UniCode.GetString(RevUni);

							// 受信イベント発行
							if( null != onRecive ){
								onRecive( RevStr );
							}

							nPt = 0;
							
						}else{

							RevByte[nPt] = Rev1Byte[0];
							nPt++;
						}
              
					}else{
					/*--- サーバーが切断 ---*/

						// サーバーが切断した事がわかる様にイベントを発行
						if( null != onDisConnect ){
							onDisConnect();
						}
						return;

					}
				}
				catch (System.Threading.ThreadAbortException){
				/*--- スレッドが破棄された ---*/
					return;
				}
				catch( System.Exception exp ){
				/*--- その他のエラー ---*/
					
					// システムエラーイベント発行
					if( null != onError ){
						onError( exp.Message );
					}

				}
			}

		}

	}

}
