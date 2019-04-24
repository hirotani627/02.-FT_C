using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// 計算クラス
    /// </summary>
    public class FTMath
    {
        /// <summary>
        /// 度をラジアンに変換
        /// </summary>
        /// <param name="degrees">角度°</param>
        /// <returns></returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return ((System.Math.PI / 180.0) * degrees);
        }

        /// <summary>
        /// ラジアンを度に変換
        /// </summary>
        /// <param name="radians">ラジアン</param>
        /// <returns></returns>
        public static double ConvertRadiansToDegrees(double radians)
        {
            return ((180.0 / System.Math.PI) * radians);
        }

        /// <summary>
        /// θ 0～360deg → ±180degに変換する
        /// </summary>
        /// <param name="dig">変換元角度</param>
        /// <returns></returns>
        public static double Convert360To180(double dig)
        {
            dig += 180.0;
            dig %= 360.0;
            if (dig < 0)
                dig += 180.0;
            else
                dig -= 180.0;

            return dig;
        }

        #region<関数　絶対値>

        /// <summary>
        /// int値の絶対値を返す
        /// </summary>
        /// <param name="Value">値</param>
        /// <returns>絶対値（int値）</returns>
        public static int ABS(int Value)
        {

            return System.Math.Abs(Value);

        }

        /// <summary>
        /// double値の絶対値を返す
        /// </summary>
        /// <param name="Value">値</param>
        /// <returns>絶対値（double値）</returns>
        public static double ABS(double Value)
        {

            return System.Math.Abs(Value);

        }

        #endregion

        #region<関数　数値確認>

        /// <summary>
        /// 文字列が数値であるかどうかを返す
        /// </summary>
        /// <param name="stTarget">検査対象となる文字列</param>
        /// <returns>数値：true 数値でない：false</returns>
        public static bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }

        /// <summary>
        /// オブジェクトが数値であるかどうかを返す
        /// </summary>
        /// <param name="oTarget">検査対象となるオブジェクト</param>
        /// <returns>数値：true 数値でない：false</returns>
        public static bool IsNumeric(object oTarget)
        {

            return IsNumeric(oTarget.ToString());
        }

        #endregion

    }
}
