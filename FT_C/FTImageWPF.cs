using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;


namespace FT.C
{
    /// <summary>
    /// WPF用イメージ変換クラス
    /// </summary>
    public static class FTImageWPF
    {

        #region<関数 BitmapImageの取得>

        /// <summary>
        /// BitmapImageの取得
        /// </summary>
        /// <param name="filePass">イメージファイルパス</param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage ReadBitmap(string filePass)
        {
            if (!FT.C.DIR.FileUM_static(filePass)) return null;    // ファイルなし

            return new System.Windows.Media.Imaging.BitmapImage(new Uri(filePass, UriKind.RelativeOrAbsolute));
        }

        #endregion

        #region<関数 トリミング>

        /// <summary>
        /// トリミング
        /// </summary>
        /// <param name="bitmapSource">変更元</param>
        /// <param name="range">トリミング範囲</param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage Triminng(System.Windows.Media.Imaging.BitmapImage bitmapSource, Int32Rect range)
        {
            System.Drawing.Bitmap b = FTImageWPF.ToBitmap(bitmapSource);
            var b2 = FTImage.Trimming(b, new System.Drawing.Rectangle(range.X, range.Y, range.Width, range.Height));
            b.Dispose();
            var isImage = FTImageWPF.ToBitmapImage(b2);
            b2.Dispose();
            return (System.Windows.Media.Imaging.BitmapImage)isImage;
        }

        #endregion

        #region<関数 イメージ変換>

        /// <summary>Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.</summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///   <para>If the function succeeds, the return value is nonzero.</para>
        ///   <para>If the specified handle is not valid or is currently selected into a DC, the return value is zero.</para>
        /// </returns>
        /// <remarks>
        ///   <para>Do not delete a drawing object (pen or brush) while it is still selected into a DC.</para>
        ///   <para>When a pattern brush is deleted, the bitmap associated with the brush is not deleted. The bitmap must be deleted independently.</para>
        /// </remarks>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// イメージ変換
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static System.Windows.Media.ImageSource ToImageSource(this System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        /// <summary>
        /// イメージ変換
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(this System.Drawing.Bitmap bmp)
        {
            System.Windows.Media.Imaging.BitmapSource bitmapSource;
            // MemoryStreamを利用した変換処理
            using (var ms = new System.IO.MemoryStream())
            {
                // MemoryStreamに書き出す
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                // MemoryStreamをシーク
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                // MemoryStreamからBitmapFrameを作成
                // (BitmapFrameはBitmapSourceを継承しているのでそのまま渡せばOK)
                bitmapSource =
                    System.Windows.Media.Imaging.BitmapFrame.Create(
                        ms,
                        System.Windows.Media.Imaging.BitmapCreateOptions.None,
                        System.Windows.Media.Imaging.BitmapCacheOption.OnLoad
                    );
            }

            return bitmapSource.Clone();
        }

        /// <summary>
        /// イメージ変換
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(this System.Drawing.Bitmap bmp)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapSource = new BitmapImage();
            // MemoryStreamを利用した変換処理
            using (var ms = new System.IO.MemoryStream())
            {
                // MemoryStreamに書き出す

                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                // MemoryStreamをシーク
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                    BitmapEncoder enc = new BmpBitmapEncoder();

                    enc.Frames.Add(BitmapFrame.Create(bitmapSource));

                    enc.Save(ms);

                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(ms);

             }

            return bitmapSource.Clone();
        }


        /// <summary>
        /// イメージ変換
        /// </summary>
        /// <param name="bitmapSource">BitmapImage</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ToBitmap(System.Windows.Media.Imaging.BitmapImage bitmapSource)
        {
            // 処理
            var bitmap = new System.Drawing.Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb
            );
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb
            );
            bitmapSource.CopyPixels(
                System.Windows.Int32Rect.Empty,
                bitmapData.Scan0,
                bitmapData.Height * bitmapData.Stride,
                bitmapData.Stride
            );
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        #endregion

    }
}
