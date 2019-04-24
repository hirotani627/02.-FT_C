using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// 時間クラス
    /// </summary>
    public class TIM
    {
        /// <summary>
        /// 出力形式
        /// </summary>
        public enum OutMode
        {
            /// <summary></summary>
            _,

            /// <summary></summary>
            non,
        }

        /// <summary>
        /// 現在の時刻を文字列で取得する
        /// </summary>
        /// <returns>（書式）2002_05_12 20_30_15</returns>
        public static string GetNowTimeToString(OutMode mode)
        {
            string s = "";
            DateTime dt = DateTime.Now;
            s = dt.ToString("G");           // 2002/05/12 20:30:15
            s = s.Replace('/', '_');
            s = s.Replace(':', '_');
            return s;
        }

        /// <summary>
        /// 現在の時刻を文字列で取得する
        /// </summary>
        /// <returns>2002/05/12 20:30:15</returns>
        public static string GetNowTime(string mode = "yyyy/MM/dd HH:mm:ss")
        {
            string s = "";
            DateTime dt = DateTime.Now;
            s = dt.ToString(mode);           // 2002/05/12 20:30:15
            return s;
        }

        /// <summary>
        /// 時刻を文字列で取得する
        /// </summary>
        /// <returns>2002/05/12 20:30:15</returns>
        public static string GetTime(DateTime  dt ,string mode = "yyyy/MM/dd HH:mm:ss")
        {
            string s = "";
            s = dt.ToString(mode);           // 2002/05/12 20:30:15
            return s;
        }


        /// <summary>
        /// 有効期限の確認
        /// </summary>
        /// <param name="time1">比較する元の時間</param>
        /// <param name="time2">閾時間</param>
        /// <returns>true:期限内　false:期限外</returns>
        public static bool ChekTimeLimit(DateTime time1, DateTime time2)
        {
            if (time1 < time2)
            {
                //Console.WriteLine("time1 は time2 より古い");
                return false;
            }
            if (time1 == time2)
            {
                //Console.WriteLine("time1 と time2 は等しい");
                return true;
            }
            //Console.WriteLine("time1 は time2 より新しい");
            return true;
        }


        /// <summary>
        /// 有効期限の確認（現在時刻から）
        /// </summary>
        /// <param name="time1">比較する時間</param>
        /// <returns>true:期限内　false:期限外</returns>
        public static bool ChekTimeLimitNow(DateTime time1)
        {
            return ChekTimeLimit(time1, DateTime.Now);
        }

        /// <summary>
        /// 日時クラスの取得
        /// </summary>
        /// <param name="s">文字列"yyyy/MM/dd HH:mm:ss"</param>
        /// <returns></returns>
        public static DateTime Get(string s)
        {
            // カンマ区切りで分割して配列に格納する
            string[] sDete = FTString.Cut(s, " ", true).Split('/');
            int[] iDete = CNV.StringsToInt(sDete);

            string[] sTime = FTString.Cut(s, " ", false).Split(':');
            int[] iTime = CNV.StringsToInt(sTime);

            if(iTime.Length < 3)
                return new DateTime(iDete[0], iDete[1], iDete[2], iTime[0], iTime[1], 0);
            else
                return new DateTime(iDete[0], iDete[1], iDete[2], iTime[0], iTime[1], iTime[2]);
        }
    }
}
