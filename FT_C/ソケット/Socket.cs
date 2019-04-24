using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// ソケットクラス
    /// </summary>
    public class Socket
    {
        /* ソケット通信関連定義 */

        /// <summary>ソケット接続要求回数</summary>
        ///	<remarks>
        /// 本回数要求を続けても返事がない場合はエラー
        /// </remarks>
        public const int SCK_CONT_MAX = 3;

        /// <summary>ソケットポート番号オフセット</summary>
        public const int SCK_PORT_OFST = 50000;

        /// <summary>ソケット受信バッファサイズ</summary>
        public const int SCK_REV_BUFSIZE = 1024;

        /// <summary>ソケット自ノードループバック用ＩＰアドレス</summary>
        public const string SCK_IP_MYSELF = "127.0.0.1";

        /// <summary>ソケットパラメータ数</summary>
        public const int SCK_PARAM_CNT = 30;

    }

    /// <summary>
    /// ソケット情報
    /// </summary>
    public class SoketInfo
    {

        /// <summary>名前</summary>
        public string Name;

        /// <summary>IPアドレス</summary>
        public string IP;

        /// <summary>ポートNo</summary>
        public int Port;
    }

    /// <summary>ソケットデータ構造</summary>
    public class SocketData
    {
        /// <summary>コマンド</summary>
        public string Command;

        /// <summary>パラメータ</summary>
        public string[] Param;

        /// <summary>コンストラクタ</summary>
        /// <remarks></remarks>
        public SocketData()
        {
            int iLp1;

            Command = "";
            Param = new string[Socket.SCK_PARAM_CNT];
            for (iLp1 = 0; iLp1 < Socket.SCK_PARAM_CNT; iLp1++) { Param[iLp1] = ""; }
        }
    }

}
