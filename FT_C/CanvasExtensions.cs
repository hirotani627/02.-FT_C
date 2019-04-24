using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;                   // トレース機能

namespace FT.C
{
    /// <summary>
    /// WPFCanvasコントロール拡張クラス
    /// </summary>
    public static class CanvasExtensions
    {

        /// <summary>
        /// キャンパスからRenderTargetBitmapに変換
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.RenderTargetBitmap ToRenderTargetBitmap(System.Windows.Controls.Canvas canvas)
        {
            var size = new Size(canvas.Width, canvas.Height);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            //Trace.WriteLine("size.Width:" + size.Width + "size.Height:" + size.Height);

            // VisualObjectをBitmapに変換する
            var renderBitmap = new RenderTargetBitmap((int)size.Width,       // 画像の幅
                                                      (int)size.Height,      // 画像の高さ
                                                      96.0d,                 // 横96.0DPI
                                                      96.0d,                 // 縦96.0DPI
                                                      PixelFormats.Pbgra32); // 32bit(RGBA各8bit)
            renderBitmap.Render(canvas);
            return renderBitmap;
        }

    }
}
