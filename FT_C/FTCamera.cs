using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{

    class cbItems<Type>
    {
        public Type Code;
        public string Name { get; set; }
    }


    /// <summary>
    /// カメラ共通クラス
    /// </summary>
    public static class FTCamera
    {
        /// <summary>
        /// 画面表示モード
        /// </summary>
        public enum DispModeContens
        {
            /// <summary>同じ大きさで上下に連結</summary>
            UpDown = 0,

            /// <summary>同じ大きさで左右に連結</summary>
            LeftRight,

            /// <summary>下画面を小で連結する</summary>
            UoLarge_DoenSmall,

            /// <summary>上画面を小で連結する</summary>
            UoSmal_DoenlLarge,

            /// <summary>四角形</summary>
            Grid,

        }

        private static IEnumerable<cbItems<Types>> MakeComboItem<Types>()
        {
            foreach (Types dow in Enum.GetValues(typeof(Types)))
            {
                yield return new cbItems<Types>
                {
                    Code = dow,
                    Name = dow.ToString(),
                };
            }
        }



        /// <summary>
        /// 解像度モード
        /// </summary>
        public enum ResolutionContens
        {
            /// <summary></summary>
            QVGA_320x240 = 0,

            /// <summary></summary>
            VGA_640x480,

            /// <summary></summary>
            XGA_1024x768,

            /// <summary></summary>
            SXGA_1280x1024,

            /// <summary></summary>
            FullHD_1920x1080
        }

        /// <summary>
        /// 解像度の取得
        /// </summary>
        /// <param name="select">指定解像度</param>
        /// <param name="W">幅</param>
        /// <param name="H">高さ</param>
        public static void GetResolution(ResolutionContens select, out int W, out int H)
        {
            switch (select)
            {
                default:
                case ResolutionContens.VGA_640x480:
                    W = 640;
                    H = 480;
                    break;

                case ResolutionContens.QVGA_320x240:
                    W = 320;
                    H = 240;
                    break;

                case ResolutionContens.XGA_1024x768:
                    W = 1024;
                    H = 768;
                    break;

                case ResolutionContens.SXGA_1280x1024:
                    W = 1280;
                    H = 1024;
                    break;

                case ResolutionContens.FullHD_1920x1080:
                    W = 1920;
                    H = 1080;
                    break;
            }
        }
    }
}
