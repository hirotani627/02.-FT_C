using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// 形状クラス
    /// </summary>
    public class Shape 
    {

        #region <関数　線の回転>

        /// <summary>
        /// 線を回転
        /// </summary>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="EndX"></param>
        /// <param name="EndY"></param>
        /// <param name="T">回転角度</param>
        public static void Rotation(ref double StartX, ref double StartY, ref double EndX, ref double EndY, double T)
        {
            double 中心を原点に持ってくるオフセットX;
            double 中心を原点に持ってくるオフセットY;
            GetCenter(StartX, StartY, EndX, EndY, out 中心を原点に持ってくるオフセットX, out 中心を原点に持ってくるオフセットY);

            double SX = StartX - 中心を原点に持ってくるオフセットX;
            double SY = StartY - 中心を原点に持ってくるオフセットY;
            double EX = EndX - 中心を原点に持ってくるオフセットX;
            double EY = EndY - 中心を原点に持ってくるオフセットY;

            // 原点基準で回転
            Rotation(ref SX, ref SY, T);
            Rotation(ref EX, ref EY, T);

            StartX = SX + 中心を原点に持ってくるオフセットX;
            StartY = SY + 中心を原点に持ってくるオフセットY;
            EndX = EX + 中心を原点に持ってくるオフセットX;
            EndY = EY + 中心を原点に持ってくるオフセットY;
        }

        /// <summary>
        /// 線分の中心を求める
        /// </summary>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="EndX"></param>
        /// <param name="EndY"></param>
        /// <param name="CenterX"></param>
        /// <param name="CenterY"></param>
        public static void GetCenter(double StartX, double StartY, double EndX, double EndY, out double CenterX, out double CenterY)
        {
            CenterX = EndX + (StartX - EndX);
            CenterY = EndY + (StartY - EndY);
        }

        /// <summary>
        /// 原点(0,0)基準での回転
        /// </summary>
        /// <param name="pointX">回転する点</param>
        /// <param name="pointY">回転する点</param>
        /// <param name="T">回転する量（原点基準）</param>
        public static void Rotation(ref double pointX, ref double pointY, double T)
        {
            double AD = pointX;
            double BD = pointY;
            double Cx = AD * Math.Cos(T) - BD * Math.Sin(T);
            double Cy = BD * Math.Cos(T) + AD * Math.Sin(T);
            pointX = Cx;
            pointX = Cy;
        }

        #endregion

        #region <関数　指定範囲に含まれているかの判定>

        /// <summary>
        /// 指定ポイントが範囲内に含まれるかを取得
        /// </summary>
        /// <param name="point">判定ポイント</param>
        /// <param name="lin">判定線</param>
        /// <param name="margnX">X方向マージン</param>
        /// <param name="margnY">Y方向マージン</param>
        public static bool IsContains(System.Drawing.Point point, Microsoft.VisualBasic.PowerPacks.LineShape lin, int margnX, int margnY)
        {
            System.Drawing.Rectangle rec = GetRange(lin, margnX, margnY);
            //this.rectangleShape1.SetBounds(rec.Left, rec.Top, rec.Width, rec.Height);     // 画面に範囲を表示するサンプル
            bool ok = rec.Contains(point);
            return ok;
        }

        /// <summary>
        /// 範囲の取得
        /// </summary>
        /// <param name="lin">判定線</param>
        /// <param name="margnX">X方向マージン</param>
        /// <param name="margnY">Y方向マージン</param>
        /// <returns></returns>
        public static System.Drawing.Rectangle GetRange(Microsoft.VisualBasic.PowerPacks.LineShape lin, int margnX, int margnY)
        {
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle();
            if (lin.X1 < lin.X2)
            {
                rec.Width = lin.X2 - lin.X1 + margnX;
                rec.X = lin.X1 - margnX / 2;
            }
            else
            {
                rec.Width = lin.X1 - lin.X2 + margnX;
                rec.X = lin.X2 - margnX / 2;
            }

            if (lin.Y1 < lin.Y2)
            {
                rec.Height = Math.Abs(lin.Y2 - lin.Y1) + margnY;
                rec.Y = lin.Y1 - margnY / 2;
            }
            else
            {
                rec.Height = Math.Abs(lin.Y1 - lin.Y2) + margnY;
                rec.Y = lin.Y2 - margnY / 2;
            }

            return rec;
        }

        #endregion

    }
}
