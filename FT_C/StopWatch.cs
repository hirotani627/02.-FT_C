﻿using System;
using System.Diagnostics;                   // トレース機能

namespace FT.C
{
    /// <summary>
    /// 時間計測クラス（ガベージコレクション計測付き）
    /// </summary>
    /// <remarks>
    /// タイマー開始：このクラスのコンストラクタが働いた時
    /// タイマー停止：リソースの解放時
    /// </remarks>
    /// <example>
    /// using (FT.C.TimeMeasurement OT = new FT.C.TimeMeasurement("識別文字"))
    ///　{
    ///     // 計測する処理
    ///  }
    /// </example>
    public class TimeMeasurement : IDisposable
    {
        #region<内部変数>

        /// <summary>スタート時のタイマー刻み数保存用</summary>
        private Int64   mStartionTime;                  

        /// <summary>識別文字</summary>
        private string  mText;

        /// <summary>ガベージコレクションの保存用</summary>  
        private Int32   mCollectionCount;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TimeMeasurement(string text)
        {
            PrepareForOperation();

            mText = text;
            mCollectionCount = GC.CollectionCount(0);   // ガベージコレクションの発生回数
            mStartionTime = Stopwatch.GetTimestamp();   // 現在のタイマー刻み数を保存
        }

        #endregion

        #region<初期化関数>

        /// <summary>
        /// 初期化関数
        /// </summary>
        private static void PrepareForOperation()
        {
            GC.Collect();                               // ガベージコレクトを強制的に行う
            GC.WaitForPendingFinalizers();              // キューが空になるまでスレッドを中断する
            GC.Collect();                               //
        }

        #endregion

        #region<廃棄処理>

        /// <summary>
        /// 廃棄処理
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("{0:###.000} seconds (GCs={1}) {2}",
                (Stopwatch.GetTimestamp() - mStartionTime) / (Double)Stopwatch.Frequency,       // 経過時間の計算
                 GC.CollectionCount(0) - mCollectionCount,                                      // ガベージコレクションの発生回数
                 mText
                );
        }

        #endregion

    }

    /// <summary>
    /// 時間計測クラス(長時間の計測)
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DateTimeMeasurement :IDisposable
    {

        #region<内部変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        /// <summary>日付・日時データ（開始時間保存用）</summary>
        DateTime startDt;

        /// <summary>日付・日時データ（経過時間用）</summary>
        DateTime endDt;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DateTimeMeasurement()
        {
            startDt = DateTime.Now;
            endDt = startDt;
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
        protected  void Dispose(bool disposing)
        {
            // 既にリソースが破棄されている場合は何もしない
            if (disposed) return;

            try
            {
                // 破棄されていないアンマネージリソースの解放処理

            }
            finally
            {
            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<関数 タイマースタート・タイマーストップ>

        /// <summary>
        /// タイマースタート（前回の時間はリセット）
        /// </summary>
        public void Start()
        {
            startDt = DateTime.Now;
            endDt = startDt;
        }

        /// <summary>
        /// タイマーストップ
        /// </summary>
        public void Stop()
        {
            endDt = DateTime.Now;
        }

        #endregion

        #region<関数 経過時間の取得>

        /// <summary>
        /// 経過時間を取得（x時間y分）
        /// </summary>
        /// <returns></returns>
        public string GetTime()
        {
            double minut = TotalMinutes();

            int h = (int)(minut / 60);
            int m = (int)(minut % 60);

            return h + "時間" + m + "分";
        }


        /// <summary>
        /// 現在の経過時間を取得
        /// </summary>
        /// <param name="itm">表示時間</param>
        /// <param name="lan">言語</param>
        /// <returns></returns>
        public string GetTime(ENM.DateTimeContens itm, FT.C.ENM.Lang lan)
        {
            
            string sb = "";
            if ((itm & ENM.DateTimeContens.Days) == ENM.DateTimeContens.Days)
                sb += Days().ToString() + ENM.DateTimeContens.Days.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Hours) == ENM.DateTimeContens.Hours)
                sb += Hours().ToString() + ENM.DateTimeContens.Hours.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Minutes) == ENM.DateTimeContens.Minutes)
                sb += Minutes().ToString() + ENM.DateTimeContens.Minutes.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Seconds) == ENM.DateTimeContens.Seconds)
                sb += Seconds().ToString() + ENM.DateTimeContens.Seconds.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Milliseconds) == ENM.DateTimeContens.Milliseconds)
                sb += Milliseconds().ToString() + ENM.DateTimeContens.Milliseconds.ToLang(lan) + " ";

            return sb;
        }

        /// <summary>
        /// 現在の経過時間を取得
        /// </summary>
        /// <param name="itm">表示時間</param>
        /// <param name="lan">言語</param>
        /// <returns></returns>
        public string GetTimeNow(ENM.DateTimeContens itm , FT.C.ENM.Lang lan)
        {
            DateTime ts = DateTime.Now;

            string sb = "";
            if ((itm & ENM.DateTimeContens.Days) == ENM.DateTimeContens.Days)
                sb += Days(ts).ToString() + ENM.DateTimeContens.Days.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Hours) == ENM.DateTimeContens.Hours)
                sb += Hours(ts).ToString() + ENM.DateTimeContens.Hours.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Minutes) == ENM.DateTimeContens.Minutes)
                sb += Minutes(ts).ToString() + ENM.DateTimeContens.Minutes.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Seconds) == ENM.DateTimeContens.Seconds)
                sb += Seconds(ts).ToString() + ENM.DateTimeContens.Seconds.ToLang(lan) + " ";

            if ((itm & ENM.DateTimeContens.Milliseconds) == ENM.DateTimeContens.Milliseconds)
                sb += Milliseconds(ts).ToString() + ENM.DateTimeContens.Milliseconds.ToLang(lan) + " ";
            
            return sb;
        }

        /// <summary>
        /// 経過時間（日にち）
        /// </summary>
        /// <returns></returns>
        public int Days()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.Days;
        }

        /// <summary>
        /// 経過時間（日にち）
        /// </summary>
        /// <returns></returns>
        public int Days(DateTime nowTime)
        {
            TimeSpan ts = nowTime - startDt; // 時間の差分を取得
            return ts.Days;
        }

        /// <summary>
        /// 経過時間（トータル時間）
        /// </summary>
        /// <returns></returns>
        public double TotalHours()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.TotalHours;
        }
        
        /// <summary>
        /// 経過時間（時間）
        /// </summary>
        /// <returns></returns>
        public int Hours()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.Hours;
        }

        /// <summary>
        /// 経過時間（時間）
        /// </summary>
        /// <returns></returns>
        public int Hours(DateTime nowTime)
        {
            TimeSpan ts = nowTime - startDt; // 時間の差分を取得
            return ts.Hours;
        }

        /// <summary>
        /// 経過時間（トータル分）
        /// </summary>
        /// <returns></returns>
        public double TotalMinutes()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.TotalMinutes;
        }

        /// <summary>
        /// 経過時間（分）
        /// </summary>
        /// <returns></returns>
        public int Minutes()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.Minutes;
        }

        /// <summary>
        /// 経過時間（分）
        /// </summary>
        /// <returns></returns>
        public int Minutes(DateTime nowTime)
        {
            TimeSpan ts = nowTime - startDt; // 時間の差分を取得
            return ts.Minutes;
        }

        /// <summary>
        /// 経過時間（トータル秒）
        /// </summary>
        /// <returns></returns>
        public double TotalSeconds()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.TotalSeconds;
        }

        /// <summary>
        /// 経過時間（秒）
        /// </summary>
        /// <returns></returns>
        public int Seconds()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.Seconds;
        }

        /// <summary>
        /// 経過時間（秒）
        /// </summary>
        /// <returns></returns>
        public int Seconds(DateTime nowTime)
        {
            TimeSpan ts = nowTime - startDt; // 時間の差分を取得
            return ts.Seconds;
        }

        /// <summary>
        /// 経過時間（トータルミリ秒）
        /// </summary>
        /// <returns></returns>
        public double TotalMilliseconds()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.TotalMilliseconds;
        }

        /// <summary>
        /// 経過時間（ミリ秒）
        /// </summary>
        /// <returns></returns>
        public int Milliseconds()
        {
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            return ts.Milliseconds;
        }

        /// <summary>
        /// 経過時間（ミリ秒）
        /// </summary>
        /// <returns></returns>
        public int Milliseconds(DateTime nowTime)
        {
            TimeSpan ts = nowTime - startDt; // 時間の差分を取得
            return ts.Milliseconds;
        }

        #endregion

        /// <summary>
        /// 時間項目
        /// </summary>
        [Flags]
        public enum Item
        {

            /// <summary>日にち</summary>
            [LANG("日", "d", "d")]
            Days = 0x01,

            /// <summary>時間</summary>
            [LANG("時間", "h", "h")]
            Hours = 0x02,

            /// <summary>分</summary>
            [LANG("分", "m", "m")]
            Minutes = 0x04,

            /// <summary>秒</summary>
            [LANG("秒", "s", "s")]
            Seconds = 0x08,

            /// <summary>ミリ秒</summary>
            [LANG("ミリ", "ms", "ms")]
            Milliseconds = 0x10,

        }
    }

}
