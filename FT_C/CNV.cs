using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;                   // トレース機能

namespace FT.C
{
    /// /// <summary>
    /// データコンバートクラス
    /// </summary>
    /// 
    /// <remarks>
    /// データコンバート処理群
    /// 
    /// Ver1.00  2012-09-24  リリース    Y.Hirotani
    /// 
    /// 
    /// </remarks>
    public class CNV
    {

        #region<関数 intとして有効かを調べる>

        /// <summary>
        /// intとして有効かを調べる
        /// </summary>
        /// <param name="str">元の文字</param>
        /// <param name="dValue">変換値</param>
        /// <returns></returns>
        public static bool Checkint(string str, out int dValue)
        {
            bool breslt = false;
            breslt = int.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out dValue);
            return breslt;
        }

        #endregion

        #region<関数　doubleとして有効かを調べる>

        /// <summary>
        /// doubleとして有効かを調べる
        /// </summary>
        /// <param name="str">変換元の文字列</param>
        /// <returns>True:有効</returns>
        public static bool CheckDouble(string str)
        {
            bool breslt = false;
            double dValue = 0;
            breslt = double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out dValue);
            return breslt;
        }

        /// <summary>
        /// doubleとして有効かを調べる
        /// </summary>
        /// <param name="str">変換元の文字列</param>
        /// <param name="dValue">有効なdouble値</param>
        /// <returns>True:有効</returns>
        public static bool CheckDouble(string str, out double dValue)
        {
            bool breslt = false;
            breslt = double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out dValue);
            return breslt;
        }

        #endregion

        #region<関数　byte配列の連結>

        /// <summary>
        /// byte配列の連結を行う
        /// </summary>
        /// <param name="Front">先頭のbyte配列</param>
        /// <param name="Back">結合するbyte配列</param>
        /// <returns>連結したbyte配列</returns>
        public static byte[] ConnectByte(byte[] Front, byte[] Back)
        {
            byte[] value = new byte[Front.Length + Back.Length];
            Front.CopyTo(value, 0);
            Back.CopyTo(value, Front.Length);
            return value;
        }


        #endregion

        #region<関数　byte配列を抜き取る>

        /// <summary>
        /// byte配列を抜き取る
        /// </summary>
        /// <param name="bValue"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetByte(byte[] bValue, int offset, int length)
        {
            //Skipでoffsetの要素を飛ばして、Takeでlengthの要素を取得する
            return bValue.Skip(offset).Take(length).ToArray();
        }

        #endregion

        #region<関数　byte配列から文字を取得>

        /// <summary>
        /// 文字を取得
        /// </summary>
        /// <param name="bValue">文字データ配列</param>
        /// <returns></returns>
        public static string GetString(byte[] bValue)
        {
            //char[] cs = new char[bValue.Length];
            //return Convert.ToString(cs);

            return Encoding.ASCII.GetString(bValue);

        }

        /// <summary>
        /// 文字を取得
        /// </summary>
        /// <param name="bValue"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetString(byte[] bValue, int offset, int length)
        {
            byte[] b = GetByte(bValue, offset, length);

            string s = GetString(b);
            return s.TrimEnd('\0');

        }

        #endregion

        #region<関数　DataColumnの作成>

        /// <summary>
        /// 列の取得
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public static DataColumn GetColumn(string DataType)
        {
            DataColumn Colum = new DataColumn();
            Colum.ColumnName = ColumValue;                              // 値列
            Colum.DataType = System.Type.GetType(DataType);       // 列のデータタイプ設定
            return Colum;
        }

        /// <summary>
        /// 列の取得
        /// </summary>
        /// <param name="DataType">データのタイプ</param>
        /// <param name="ColumnName">列名</param>
        /// <returns></returns>
        /// <remarks>
        /// 型
        /// System.Int32
        /// System.Double
        /// 
        /// </remarks>
        public static DataColumn GetColumn(string DataType, string ColumnName)
        {
            DataColumn Colum = new DataColumn();
            Colum.ColumnName = ColumnName;                              // 値列
            Colum.DataType = System.Type.GetType(DataType);       // 列のデータタイプ設定
            return Colum;
        }

        #endregion

        #region<関数　ArrayからDataTebleに変換>

        /// <summary>値列名</summary>
        public const string ColumValue = "Value";

        /// <summary>表示列名</summary>
        public const string ColumDisp = "Disp";


        /// <summary>
        /// ArrayからDataTebleデータに変換する
        /// </summary>
        /// <param name="ar">入力Arrayデータ</param>
        /// <returns>
        /// EndMarkが含まれて入れば自動で削除
        /// 
        /// 【サンプルlistBox1】  ※ENMを直接書き込みたいとき
        /// Array ar = Enum.GetValues(typeof(FT.C.ENM.Lang));       
        /// DataTable dt = FT.C.CNV.ArrayToDataTeble(ar);
        /// this.listBox1.DataSource = dt;
        /// this.listBox1.DisplayMember = FT.C.CNV.ColumDisp;
        /// this.listBox1.ValueMember = FT.C.CNV.ColumValue;
        /// 
        /// </returns>
        public static DataTable ArrayToDataTeble(Array ar)
        {
            DataTable dt = new DataTable();
            DataColumn Colum = new DataColumn();
            Colum.ColumnName = ColumValue;                              // 値列
            Colum.DataType = System.Type.GetType("System.Int32");       // 列のデータタイプ設定
            dt.Columns.Add(Colum);                                      // 列の作成

            DataColumn ColumD = new DataColumn();
            ColumD.ColumnName = ColumDisp;                              // 表示列
            ColumD.DataType = System.Type.GetType("System.String");     // 列のデータタイプ設定
            dt.Columns.Add(ColumD);                                     // 列の作成

            for (int i = 0; i < ar.Length; i++)
            {
                if ("EndMark" != ar.GetValue(i).ToString())
                {
                    DataRow row = dt.NewRow();                              // 行の作成
                    row[ColumValue] = i;
                    row[ColumDisp] = ar.GetValue(i);
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }


        /// <summary>
        /// ArrayからDataTebleデータに変換する
        /// </summary>
        /// <param name="ar">入力Arrayデータ</param>
        /// <param name="exclusion">除外</param>
        /// <returns>
        /// EndMarkが含まれて入れば自動で削除
        /// </returns>
        public static DataTable ArrayToDataTeble(Array ar, string[] exclusion)
        {
            DataTable dt = new DataTable();
            DataColumn Colum = new DataColumn();
            Colum.ColumnName = ColumValue;                              // 値列
            Colum.DataType = System.Type.GetType("System.Int32");       // 列のデータタイプ設定
            dt.Columns.Add(Colum);                                      // 列の作成

            DataColumn ColumD = new DataColumn();
            ColumD.ColumnName = ColumDisp;                              // 表示列
            ColumD.DataType = System.Type.GetType("System.String");     // 列のデータタイプ設定
            dt.Columns.Add(ColumD);                                     // 列の作成

            string s;
            for (int i = 0; i < ar.Length; i++)
            {
                s = ar.GetValue(i).ToString();
                if ("EndMark" != s)
                {
                    int j;
                    for (j = 0; j < exclusion.Length; j++)
                    {
                        if (s == exclusion[j])
                            break;
                    }

                    if (j >= exclusion.Length)
                    {
                        DataRow row = dt.NewRow();                              // 行の作成
                        row[ColumValue] = i;
                        row[ColumDisp] = ar.GetValue(i);
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }

        #endregion

        #region<関数　string[]からIntデータに変換>

        /// <summary>
        /// string[]からIntデータに変換
        /// </summary>
        /// <param name="sDete"></param>
        /// <returns></returns>
        internal static int[] StringsToInt(string[] sDete)
        {
            int[] j = new int[sDete.Length];
            for (int i = 0; i < sDete.Length; i++)
            {
                if (!Checkint(sDete[i], out j[i]))
                    return null;
            }
            return j;
        }

        #endregion

        #region<関数　string[]からDataTebleに変換>

        /// <summary>
        /// string[]からDataTebleデータに変換する
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public static DataTable StringsToDataTeble(string[] ar)
        {

            DataTable dt = new DataTable();
            DataColumn Colum = new DataColumn();
            Colum.ColumnName = ColumValue;                              // 値列
            Colum.DataType = System.Type.GetType("System.Int32");       // 列のデータタイプ設定
            dt.Columns.Add(Colum);                                      // 列の作成

            DataColumn ColumD = new DataColumn();
            ColumD.ColumnName = ColumDisp;                              // 表示列
            ColumD.DataType = System.Type.GetType("System.String");     // 列のデータタイプ設定
            dt.Columns.Add(ColumD);                                     // 列の作成

            for (int i = 0; i < ar.Length; i++)
            {
                DataRow row = dt.NewRow();                              // 行の作成
                row[ColumValue] = i;
                row[ColumDisp] = ar[i];
                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion

        #region<関数　FT.C.ENM.Langから言語を取得>

        /// <summary>
        /// FT.C.ENM.Langから言語を取得する
        /// </summary>
        /// <returns></returns>
        public static List<FT.C.LangString> GetLangs()
        {
            int couunt = System.Enum.GetNames(typeof(FT.C.ENM.Lang)).Length;
            List<FT.C.LangString> st = new List<FT.C.LangString>();

            for (int i = 0; i < couunt; i++)
            {
                FT.C.ENM.Lang lang = (FT.C.ENM.Lang)i;
                if (lang.ToString() != "EndMark")
                {
                    FT.C.LangString item = new FT.C.LangString(lang.ToJpn(), lang.ToEng(), lang.ToOth());
                    st.Add(item);
                }
            }
            return st;
        }

        #endregion

        #region<関数　文字から日にちクラスの作成>

        /// <summary>
        /// 文字から日にちクラスの作成
        /// </summary>
        /// <param name="yar"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool MakeDate(string yar, string month, string day, out DateTime date)
        {

            int iyar;
            if (FT.C.CNV.Checkint(yar, out iyar))
            {
                int imonth;
                if (FT.C.CNV.Checkint(month, out imonth))
                {
                    int iday;
                    if (FT.C.CNV.Checkint(day, out iday))
                    {
                        date = new DateTime(iyar, imonth, iday);
                        return true;
                    }
                }
            }

            date = new DateTime();
            return false;

        }

        #endregion

        #region<関数　型変換→bool>

        /// <summary>
        /// byte値からbool値に変換
        /// </summary>
        /// <param name="Value">byte値</param>
        /// <returns>0：false 0!=：true</returns>
        public static bool CBool(byte Value)
        {
            bool bValue;

            if (Value > 0) bValue = true;
            else bValue = false;
            return bValue;
        }


        /// <summary>
        /// int値からbool値に変換
        /// </summary>
        /// <param name="Value">int値</param>
        /// <returns>0：false 0!=：true	</returns>
        public static bool CBool(int Value)
        {
            bool bValue;

            if (Value > 0) bValue = true;
            else bValue = false;
            return bValue;
        }


        /// <summary>
        /// long値からbool値に変換
        /// </summary>
        /// <param name="Value">long値</param>
        /// <returns>0：false 0!=：true	</returns>
        public static bool CBool(long Value)
        {
            bool bValue;

            if (Value > 0) bValue = true;
            else bValue = false;
            return bValue;
        }

        /// <summary>
        /// double値からbool値に変換
        /// </summary>
        /// <param name="Value">double値</param>
        /// <returns>0：false 0!=：true</returns>
        public static bool CBool(double Value)
        {
            bool bValue;

            if (Value > 0) bValue = true;
            else bValue = false;
            return bValue;
        }


        /// <summary>
        /// string値からbool値に変換
        /// </summary>
        /// <param name="Value">string値</param>
        /// <returns>0：false 0!=：true	</returns>
        public static bool CBool(string Value)
        {

            bool bValue;

            if ("0" == Value) bValue = false;
            else bValue = true;
            return bValue;
        }

        #endregion

        #region<関数　型変換→byte>

        /// <summary>
        /// bool値からbyte値に変換
        /// </summary>
        /// <param name="Value">bool値</param>
        /// <returns>byte値</returns>
        public static byte CByte(bool Value)
        {

            byte btValue;
            if (false == Value) btValue = 0;
            else btValue = 1;

            return btValue;
        }


        /// <summary>
        /// int値からbyte値に変換
        /// </summary>
        /// <param name="Value">int値</param>
        /// <returns>byte値</returns>
        public static byte CByte(int Value)
        {

            return Convert.ToByte(Value);

        }


        /// <summary>
        /// long値からbyte値に変換
        /// </summary>
        /// <param name="Value">long値</param>
        /// <returns>byte値	</returns>
        public static byte CByte(long Value)
        {
            return Convert.ToByte(Value);
        }


        /// <summary>
        /// double値からbyte値に変換
        /// </summary>
        /// <param name="Value">double値</param>
        /// <returns>byte値	</returns>
        public static byte CByte(double Value)
        {
            return Convert.ToByte(Value);
        }


        /// <summary>
        /// string値からbyte値に変換
        /// </summary>
        /// <param name="Value">string値	</param>
        /// <returns>byte値	</returns>
        public static byte CByte(string Value)
        {
            return Convert.ToByte(Value);
        }

        #endregion

        #region<関数　型変換→byte[]>

        /// <summary>
        /// byte[]として取り出す
        /// </summary>
        /// <param name="orignal"></param>
        public static byte[] GetByte(int orignal)
        {
            byte[] value = new byte[4];
            for (int i = 0; i < value.Length; i++)
            {
                value[value.Length - 1 - i] = (byte)(orignal >> (i * 8));
            }
            return value;
        }

        /// <summary>
        /// byte[]として取り出す（空文字判定あり）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetByte(string value)
        {

            if (string.IsNullOrEmpty(value))
                return new byte[0];                 // 空文字

            var c = value.ToCharArray();
            byte[] bout = new byte[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                bout[i] = (byte)c[i];
            }
            return bout;
        }

        /// <summary>
        /// byte[]として取り出す
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetByteASSII(string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        /// <summary>
        /// byte[]として取り出す
        /// </summary>
        /// <param name="value">入力文字</param>
        /// <param name="Length">出力配列の長さ</param>
        /// <returns></returns>
        public static byte[] GetByteASSII(string value, int Length)
        {
            byte[] b = Encoding.ASCII.GetBytes(value);
            return ChangeLength(b,Length);
        }

        /// <summary>
        /// 配列を指定の長さに変換する
        /// </summary>
        /// <param name="b">入力文字</param>
        /// <param name="Length">出力配列の長さ</param>
        /// <returns></returns>
        public static byte[] ChangeLength(byte[] b, int Length)
        {
            byte[] a = new byte[Length];
            for (int i = 0; i < b.Length; i++)
            {
                a[i] = b[i];
            }

            return a;
        }

        /// <summary>
        /// byte[]として取り出す
        /// </summary>
        /// <param name="IsChek">型チェックする？</param>
        /// <param name="value">変更する値</param>
        /// <returns></returns>
        public static byte[] GetByteASSII(string value, bool IsChek = true)
        {
            try
            {
                if (IsChek)
                {
                    if (FTString.IsAlphanumeric(value))
                        return Encoding.ASCII.GetBytes(value);
                    else
                        return new byte[0];
                }
                else
                {
                    return Encoding.ASCII.GetBytes(value);
                }

            }
            catch
            {
                return new byte[0];
            }
        }

        #endregion

        #region<関数　型変換→int>

        /// <summary>
        /// bool値からint値に変換
        /// </summary>
        /// <param name="Value">bool値</param>
        /// <returns>int値</returns>
        public static int CInt(bool Value)
        {
            int iValue;

            if (Value == true) iValue = (int)1;
            else iValue = (int)0;
            return iValue;
        }

        /// <summary>
        /// byte値からint値に変換
        /// (ラッパーなので、この中身を直接呼べばよい)
        /// </summary>
        /// <param name="Value">byte値</param>
        /// <returns>int値</returns>
        public static int CInt(byte Value)
        {
            return Convert.ToInt32(Value);
        }


        /// <summary>
        /// long値からint値に変換
        /// (ラッパーなので、この中身を直接呼べばよい)
        /// </summary>
        /// <param name="Value">long値</param>
        /// <returns>int値</returns>
        public static int CInt(long Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// double値からint値に変換
        /// (ラッパーなので、この中身を直接呼べばよい)
        /// </summary>
        /// <param name="Value">double値</param>
        /// <returns>int値</returns>
        public static int CInt(double Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// string値からint値に変換
        /// (ラッパーなので、この中身を直接呼べばよい)
        /// </summary>
        /// <param name="Value">string値</param>
        /// <returns>int値</returns>
        public static int CInt(string Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// object値からint値に変換
        /// (ラッパーなので、この中身を直接呼べばよい)
        /// </summary>
        /// <param name="Value">object値</param>
        /// <returns>int値</returns>
        public static int CInt(object Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// byte配列（unsigned short）からIntに変換
        /// </summary>
        /// <param name="high">上位Bit</param>
        /// <param name="low">下位Bit</param>
        /// <returns></returns>
        public static int CInt(byte high, byte low)
        {
            int ss = ((int)(high)) << 8;
            return (((int)(high)) << 8) + low;
        }

        /// <summary>
        /// stringからintとして取り出す
        /// </summary>
        /// <param name="orignal"></param>
        public static int GetInt(string orignal)
        {
            if (null == orignal) return 0;
            int i;
            if (!Checkint(orignal, out i))
            {

            }

            return i;
        }


        /// <summary>
        /// INtimeのLongを取り出す
        /// </summary>
        /// <param name="orignal">オリジナルデータ</param>
        /// <param name="offsett">開始位置</param>
        /// <param name="length"></param>
        public static int GetInt(byte[] orignal, int offsett, int length)
        {
            byte[] bb = GetByte(orignal, offsett, length);
            return GetInt(bb);
        }


        /// <summary>
        /// byte配列からintとして取り出す
        /// </summary>
        /// <param name="orignal"></param>
        /// <returns></returns>
        public static int GetInt(byte[] orignal)
        {
            int value = 0;
            for (int i = 0; i < orignal.Length; i++)
            {
                value = value + (orignal[i] << (i * 8));
            }
            //for (int i = 0; i < orignal.Length; i++)
            //{
            //    value = value + (orignal[i] << (i * 8));
            //}
            //for (int i = 0; i < orignal.Length; i++)
            //{
            //    value = value + (orignal[orignal.Length - 1 - i] << (i * 8));
            //}
            return value;
        }

        #endregion

        #region<関数　byte配列にデータを書き込むt>

        /// <summary>
        /// byte配列にデータを書き込む
        /// </summary>
        /// <param name="orignal"></param>
        /// <param name="offset"></param>
        /// <param name="seValue"></param>
        public static void SetByte(byte[] orignal, int offset, byte[] seValue)
        {
            for (int i = 0; i < seValue.Length; i++)
            {
                orignal[offset + i] = seValue[seValue.Length - 1 - i];
            }
        }

        #endregion

        #region<関数　char配列にデータを書き込む>

        /// <summary>
        /// byte配列にデータを書き込む
        /// </summary>
        /// <param name="orignal"></param>
        /// <param name="offset"></param>
        /// <param name="seValue"></param>
        public static void SetChar(byte[] orignal, int offset, byte[] seValue)
        {
            if (orignal.Length < offset + seValue.Length)
                return;

            for (int i = 0; i < seValue.Length; i++)
            {
                orignal[offset + i] = seValue[i];
            }
        }

        #endregion

        #region<関数　16進文字列を数値型に変換>

        /// <summary>
        /// 16進文字列を数値型に変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int hex2Int(string s)
        {
            return Convert.ToInt32(s, 16);
        }

        #endregion

        #region<関数　文字列内の指定範囲を数値として取得>

        /// <summary>
        /// 文字列内の指定範囲を数値として取得
        /// </summary>
        /// <param name="orignal">オリジナル文字</param>
        /// <param name="i開始">開始位置０～</param>
        /// <param name="iLeng">長さ</param>
        /// <returns></returns>
        public static double GetDouble(string orignal, int i開始, int iLeng)
        {
            int len = i開始 + iLeng;

            char[] c;
            if (len >= orignal.Length)
                c = orignal.ToCharArray(i開始, orignal.Length - i開始);    // 終了が範囲外
            else if (i開始 >= orignal.Length)
            {
                c = new char[1];                                       // 開始が範囲外
                c[0] = '0';
            }
            else
                c = orignal.ToCharArray(i開始, iLeng);

            double num;
            FT.C.FTString.NumberToString(new string (c), out num);
            return num;
        }

        #endregion



        ///// <summary>
        ///// INtimeのLongを取り出す
        ///// </summary>
        ///// <param name="orignal">入力</param>
        ///// <param name="offsett"></param>
        ///// <param name="length"></param>
        //public static int GetLong(byte[] orignal, int offsett, int length)
        //{
        //    byte[] bb = GetByte(orignal, offsett, length);
        //    return GetInt(bb);
        //}

        ///// <summary>
        ///// intとして取り出す
        ///// </summary>
        ///// <param name="orignal"></param>
        //public static int GetInt(byte[] orignal)
        //{
        //    return GetValue(orignal);
        //}


        ///// <summary>
        ///// INtimeのLongとして取り出す
        ///// </summary>
        ///// <param name="orignal"></param>
        //public static int GetLong(byte[] orignal)
        //{
        //    return GetValue(orignal);
        //}
    }
}
