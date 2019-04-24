using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
//using System.Windows;
using System.Runtime.InteropServices;

namespace FT.C
{
    /// <summary>
    /// イメージクラス
    /// </summary>
    public class FTImage
    {
        #region<関数 単色イメージを作成>

        /// <summary>
        ///　単色イメージを作成
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Width"></param>
        /// <param name="Heigth"></param>
        /// <returns></returns>
        public static Bitmap MakeBitmap(Brush color, int Width, int Heigth)
        {
            Bitmap bImage = new Bitmap(Width, Heigth);
            using (Graphics g = Graphics.FromImage(bImage))
            {
                g.FillRectangle(color, g.VisibleClipBounds);     // 内側を塗りつぶす
            }
            return bImage;
        }

        #endregion

        #region<関数 背景色を設定する>

        /// <summary>
        /// 背景色を設定する
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap SetBackgroundColor(Bitmap bmp, System.Drawing.Color color)
        {
            Bitmap backBmp;

            //背景の画像と合成する
            //背景となる画像を読み込む
            using (SolidBrush myBrush = new System.Drawing.SolidBrush(color))
            {
                backBmp = MakeBitmap(myBrush, bmp.Width, bmp.Height);
                //背景の画像に描画する
                using (Graphics g = Graphics.FromImage(backBmp))
                {
                    bmp.MakeTransparent(System.Drawing.Color.White);
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                }
            }
            return backBmp;
        }

        #endregion

        #region<関数 テスト画像の作成>

        /// <summary>
        /// テスト画像の作成
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap MakeTestImage(int whi, int he)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(whi, he);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    bitmap.SetPixel(i, j, (Math.Abs(i - j) < 2) ? System.Drawing.Color.Blue : System.Drawing.Color.Yellow);
                }
            }

            return bitmap;
        }

        #endregion

        #region<関数 16×16の単色アイコン作成>

        /// <summary>
        /// 単色アイコン作成
        /// </summary>
        /// <param name="color">アイコンの色</param>
        /// <returns>アイコン</returns>
        public static System.Drawing.Image CreateImageOfColor(System.Drawing.Color color)
        {
            System.Drawing.Image image = new System.Drawing.Bitmap(16, 16);
            System.Drawing.GraphicsUnit graphicsUnit = System.Drawing.GraphicsUnit.Pixel;
            System.Drawing.RectangleF bounds = image.GetBounds(ref graphicsUnit);
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image))
            {
                graphics.FillRectangle(new System.Drawing.SolidBrush(color), bounds);
            }
            return image;
        }

        #endregion

        #region<関数 Image画像を取得>

        /// <summary>
        /// Image画像を取得
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image</returns>
        /// <remarks>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </remarks>
        public static System.Drawing.Image GetImage(string filename)
        {
            if (!System.IO.File.Exists(filename))        // イメージファイルがあるかを確認する  True:ファイル有り
                return null;

            using (System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                //fs.Close();
                return img;
            }

        }

        #endregion

        #region<関数 センターラインの描画>

        /// <summary>
        /// センターラインの描画
        /// </summary>
        /// <param name="OriginalImage">オリジナルイメージ</param>
        /// <param name="c">描画色</param>
        /// <returns></returns>
        public static Bitmap AddCenterLine(Bitmap OriginalImage, Color c)
        {
            if (OriginalImage == null) return null;

            Bitmap b = new Bitmap(OriginalImage);
            using (Pen p = new Pen(c))
            using (Graphics g = Graphics.FromImage(b))
            {
                // 横ライン
                g.DrawLine(p, new Point(0, b.Height / 2), new Point(b.Width, b.Height / 2));
                // 縦ライン
                g.DrawLine(p, new Point(b.Width / 2, 0), new Point(b.Width / 2, b.Height));
            }
            return b;
        }

        /// <summary>
        /// センターラインの描画
        /// </summary>
        /// <param name="OriginalImage">オリジナルイメージ</param>
        /// <param name="c">描画色</param>
        /// <param name="WidthHorizontalLine">横線の幅</param>
        /// <param name="WidthVerticalLine">縦線の幅</param>
        /// <returns></returns>
        public static Bitmap AddCenterLine(Bitmap OriginalImage, Color c, int WidthHorizontalLine, int WidthVerticalLine)
        {
            if (OriginalImage == null) return null;

            Bitmap b = new Bitmap(OriginalImage);
            using (Pen p = new Pen(c))
            using (Graphics g = Graphics.FromImage(b))
            {
                // 横ライン
                for (int i = -(WidthHorizontalLine / 2); i < (WidthHorizontalLine / 2); i++)
                {
                    g.DrawLine(p, new Point(0, b.Height / 2 + i), new Point(b.Width, b.Height / 2 + i));
                }
                // 縦ライン
                for (int i = -(WidthVerticalLine / 2); i < (WidthVerticalLine / 2); i++)
                {
                    g.DrawLine(p, new Point(b.Width / 2 + i, 0), new Point(b.Width / 2 + i, b.Height));
                }
            }
            return b;
        }

        #endregion

        #region<関数 Imageをバイト配列変換>

        /// <summary>
        /// Imageをバイト配列に変換する
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }

        /// <summary>
        /// バイト配列をImageに変換する
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] b)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(b);
            return img;
        }

        #endregion

        #region<関数 ファイルに保存>

        /// <summary>
        /// ファイルに保存する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoder"></param>
        /// <param name="renderBitmap"></param>
        private static void SaveFile(string path, System.Windows.Media.Imaging.RenderTargetBitmap renderBitmap, System.Windows.Media.Imaging.BitmapEncoder encoder = null)
        {
            // 出力用の FileStream を作成する
            using (var os = new FileStream(path, FileMode.Create))
            {
                // 変換したBitmapをエンコードしてFileStreamに保存する。
                // BitmapEncoder が指定されなかった場合は、PNG形式とする。
                encoder = encoder ?? new System.Windows.Media.Imaging.PngBitmapEncoder();
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(renderBitmap));
                encoder.Save(os);
            }
        }

        #endregion

        #region<関数 トリミング>

        /// <summary>
        /// トリミング
        /// </summary>
        /// <param name="inImage"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Bitmap Trimming(Bitmap inImage, Rectangle range)
        {
            Bitmap outImage = new Bitmap(range.Width, range.Height);

            Graphics g = Graphics.FromImage(outImage);
            Rectangle desRect = new Rectangle(0, 0, range.Width, range.Height);

            // トリミング
            g.DrawImage(inImage, range, desRect, GraphicsUnit.Pixel);
            g.Dispose();

            return outImage;
        }

        #endregion

        #region<関数 RenderTargetBitmap・BitmapImage変換>

        /// <summary>
        /// BitmapImageに変換
        /// </summary>
        /// <param name="renderBitmap"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(System.Windows.Media.Imaging.RenderTargetBitmap renderBitmap)
        {
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.Default;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }

        #endregion

        #region<関数 RenderTargetBitmap・Bitmap変換>

        /// <summary>
        /// Bitmapに変換
        /// </summary>
        /// <param name="renderBitmap"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ToBitmap(System.Windows.Media.Imaging.RenderTargetBitmap renderBitmap)
        {
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new BmpBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.Default;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();

                using (var tempBitmap = new System.Drawing.Bitmap(stream))
                {
                    return new System.Drawing.Bitmap(tempBitmap);
                }
            }
        }

        #endregion

        #region<関数 RenderTargetBitmap・byte[]変換>

        /// <summary>
        /// byte[]に変換
        /// </summary>
        /// <param name="renderBitmap"></param>
        /// <returns></returns>
        public static byte[] ToBytes(System.Windows.Media.Imaging.RenderTargetBitmap renderBitmap)
        {
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            Byte[] imageByte = null;

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.Default;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();

                Stream streamdd = bitmapImage.StreamSource;

                using (BinaryReader reader = new BinaryReader(streamdd))
                {
                    imageByte = reader.ReadBytes((Int32)streamdd.Length);

                }
            }
            return imageByte;
        }

        #endregion

        #region<関数 BitmapImage・byte[]変換>

        /// <summary>
        /// byte[]に変換
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ToBytes(System.Windows.Media.Imaging.BitmapImage image)
        {
            Stream streamdd = image.StreamSource;
            Byte[] imageByte = null;
            using (BinaryReader reader = new BinaryReader(streamdd))
            {
                imageByte = reader.ReadBytes((Int32)streamdd.Length);
            }
            return imageByte;
        }

        #endregion

        #region<関数 Bitmap・BitmapSource変換>

        /// <summary>
        /// BitmapSourceに変換
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            return ToBitmapImage(bitmap);
        }

        /// <summary>
        /// BitmapImageに変換
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        #endregion

        #region<関数 画像のリサイズ>

        ///// <summary>
        ///// 画像のリサイズ
        ///// </summary>
        ///// <param name="image"></param>
        ///// <param name="changeSize"></param>
        ///// <returns></returns>
        //public static Bitmap Resize1(Bitmap image, Size changeSize)
        //{
        //    if (image == null) return null;

        //    Bitmap resizeBmp = new Bitmap(changeSize.Width, changeSize.Height);
        //    Graphics g = Graphics.FromImage(resizeBmp);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        //    g.DrawImage(image, 0, 0, changeSize.Width, changeSize.Height);
        //    //g.Dispose();

        //    return resizeBmp;
        //}

        enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000
        }


        /// <summary>
        /// 画像を拡大・縮小させる
        /// </summary>
        /// <param name="hdcDest">コピー先のデバイスコンテキストのハンドル</param>
        /// <param name="nXDest">コピー先の左上隅のx座標（ピクセル単位）</param>
        /// <param name="nYDest">コピー先の左上隅のy座標（ピクセル単位）</param>
        /// <param name="nWidthDest">転送先の幅（ピクセル単位）</param>
        /// <param name="nHeightDest">転送先の高さ（ピクセル単位）</param>
        /// <param name="hdcSrc">コピー元のデバイスコンテキストのハンドル</param>
        /// <param name="nXSrc">コピー元の左上隅のx座標（ピクセル単位）</param>
        /// <param name="nYSrc">コピー元の左上隅のy座標（ピクセル単位）</param>
        /// <param name="nWidthSrc">転送元の幅（ピクセル数）</param>
        /// <param name="nHeightSrc">転送元の高さ（ピクセル数）</param>
        /// <param name="dwRop">ラスタオペレーション（定数参照）</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidthDest,
        int nHeightDest,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int nWidthSrc,
        int nHeightSrc,
        TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// 指定されたウィンドウのクライアント領域またはスクリーン全体に対応するディスプレイデバイスコンテキストのハンドルを取得
        /// </summary>
        /// <param name="hwnd">0 (NULL) を指定すると、スクリーン全体のデバイスコンテキストを取得</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);


        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        /// <summary>
        /// 画像のリサイズ
        /// </summary>
        /// <param name="image">入力画像</param>
        /// <param name="changeSize">欲しいサイズ</param>
        /// <returns></returns>
        public static Bitmap Resize(Bitmap image, Size changeSize)
        {
            //// デバイスコンテキストのハンドルを与える
            //IntPtr hdcd = image.GetHbitmap();
            //IntPtr hsrc = CreateCompatibleDC(hdcd);
            //Bitmap b = new Bitmap(changeSize.Width, changeSize.Height);

            //Graphics g_bmp = Graphics.FromImage(b);
            //IntPtr dc_bmp = g_bmp.GetHdc();

            //IntPtr dc_scr = GetDC(IntPtr.Zero);

            //var a = b.GetHbitmap();
            //// Win32API関数StretchBltの呼出
            //StretchBlt(a, 0, 0, image.Width, image.Height, hsrc, 0, 0, changeSize.Width, changeSize.Height, TernaryRasterOperations.SRCCOPY);
            ////DeleteDC(hsrc);

            //return b;


            // サイズ変更した画像を作成する
            Bitmap bitmap = new Bitmap(changeSize.Width, changeSize.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // ラップモードを設定する
                using (System.Drawing.Imaging.ImageAttributes wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, new Rectangle(0, 0, changeSize.Width, changeSize.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    //graphics.DrawImage(image, new Rectangle(0, 0,image.Width, image.Height), 0, 0, changeSize.Width, changeSize.Height, GraphicsUnit.Pixel, wrapMode);

                    return bitmap;
                }
            }
        }

        /// <summary>
        /// 画像のリサイズ
        /// </summary>
        /// <param name="org">入力画像</param>
        /// <param name="ScaleX">欲しいサイズ</param>
        /// <param name="ScaleY">欲しいサイズ</param>
        /// <returns></returns>
        public static System.Windows.Media.ImageSource Resize(System.Windows.Media.ImageSource org, double ScaleX, double ScaleY)
        {
            // 拡大・縮小されたビットマップを作成する
            var tr = new System.Windows.Media.ScaleTransform(ScaleX, ScaleY);
            return new TransformedBitmap((System.Windows.Media.Imaging.BitmapSource)org, tr);
        }

        /// <summary>
        /// 画像のリサイズ
        /// </summary>
        /// <param name="org">入力画像</param>
        /// <param name="changeSize">欲しいサイズ</param>
        /// <returns></returns>
        public static Bitmap Resize2(Bitmap org, Size changeSize)
        {


            int screen_width = changeSize.Width;
            int screen_height = changeSize.Height;

            //textBox_Output.Text += string.Format("スクリーンの幅:{0:d}\r\n", screen_width);
            //textBox_Output.Text += string.Format("スクリーンの高さ:{0:d}\r\n", screen_height);

            Bitmap bmp = new Bitmap(screen_width, screen_height);
            Graphics g_bmp = Graphics.FromImage(bmp);
            IntPtr dc_bmp = g_bmp.GetHdc();

            Graphics g_org = Graphics.FromImage(org);
            IntPtr dc_scr = g_org.GetHdc();
            //IntPtr dc_scr = GetDC(IntPtr.Zero);

            //StretchBlt(dc_bmp, 0, 0, bmp.Width, bmp.Height,
            //            dc_scr, 0, 0, org.Width, org.Height,
            //            TernaryRasterOperations.SRCCOPY);

            StretchBlt(dc_bmp, 200, 200, 200, 200, dc_scr, 50, 50, 50, 50, TernaryRasterOperations.SRCCOPY);

            g_bmp.ReleaseHdc(dc_bmp);
            g_bmp.Dispose();
            ReleaseDC(IntPtr.Zero, dc_scr);

            //bmp.Save(@"C:\Work\61 4T_MonitoringCamera\53 4T_MonitoringCamera_VideoCaptureDevice\capture.bmp");

            return bmp;
            


            //Bitmap b = new Bitmap(changeSize.Width, changeSize.Height);

            //if (bmp != null)
            //{
            //    //this.AutoScrollMinSize = bmp.Size;
            //    //hbmp = bmp.GetHbitmap();
            //    IntPtr pTarget = b.GetHbitmap();
            //    IntPtr pSource = CreateCompatibleDC(pTarget);
            //    IntPtr pOrig = bmp.GetHbitmap();

            //    StretchBlt( pTarget, 0, 0, changeSize.Width, changeSize.Height,
            //                pOrig, 0, 0, bmp.Width, bmp.Height, 
            //                TernaryRasterOperations.SRCCOPY);

            //    IntPtr pNew = SelectObject(pSource, pOrig);
            //    DeleteObject(pNew);
            //    DeleteDC(pSource);
            //    //e.Graphics.ReleaseHdc(pTarget);
            //}

            //return b;
        }

        #endregion


        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// イメージ変換
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static BitmapSource ToBetterBitmapSource(Bitmap source)
        {
            var handle = source.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(

                    handle,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }

            finally
            {
                DeleteObject(handle);

            }

        }

    }

}