using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// ディスプレイクラス
    /// </summary>
    public class Display
    {
        /// <summary>
        /// ディスプレイの幅を取得
        /// </summary>
        /// <returns></returns>
        public static int GetDispSizeWidth()
        {
            int w;
            //ディスプレイの幅
            w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            return w;
        }


        /// <summary>
        /// ディスプレイの高さ取得
        /// </summary>
        /// <returns></returns>
        public static int GetDispSizeHigth()
        {
            int h;
            //ディスプレイの幅
            h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            return h;
        }

    }
}
