using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace FT.C
{
    /// /// <summary>
    /// コントロール・コンポーネントグラフィック共通クラス
    /// </summary>
    /// 
    /// <remarks>
    /// Graphicsに対しての処理群
    /// 
    /// Ver1.00  2013-12-07  リリース    Y.Hirotani
    /// 
    /// 
    /// Yuuji Taniguchiさんのコードを元に改造しています。
    /// </remarks>
    ///
    public class Graphic : IDisposable
    {

        #region<内部定数・変数>

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        /// <summary>描画ターゲット</summary>
        private Graphics mTarget;

        #endregion

        #region<内部列挙対>

        /// <summary>
        /// 描画する線スタイル
        /// </summary>
        /// <remarks>System.Drawing.Drawing2D.DashStyleの拡張</remarks>
        public enum EdgeMode
        {
            /// <summary>実線</summary>
            Solid,

            /// <summary>ダッシュで構成される直線</summary>
            Dash,

            /// <summary>ドットで構成される直線</summary>
            Dot,

            /// <summary>ダッシュとドットの繰り返しパターン</summary>
            DashDot,

            /// <summary>ダッシュと 2 つのドットの繰り返し</summary>
            DashDotDot,

            /// <summary>縁なし</summary>
            Non,
        }

        /// <summary>
        /// 塗りつぶしモード
        /// </summary>
        public enum FillMode
        {
            /// <summary>塗りつぶし</summary>
            Fill,

            /// <summary>線</summary>
            Line
        }

        #endregion

        #region<プロパティ>

        /// <summary>
        /// ターゲットプロパティ
        /// </summary>
        public Graphics Target
        {
            get { return mTarget; }
        }
   
        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">描画ターゲット</param>
        public Graphic(Graphics target)
        {
            mTarget = target;
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
        protected void Dispose(bool disposing)
        {
            // 既にリソースが破棄されている場合は何もしない
            if (disposed) return;

            try
            {
                // 破棄されていないアンマネージリソースの解放処理
                mTarget = null;
            }
            finally
            {

                // リソースは破棄されている
                disposed = true;
            }
        }

        #endregion

        #region<関数　ターゲットのグラフィックパスを取得>

        /// <summary>
        /// ターゲットのグラフィックパスを取得
        /// </summary>
        /// <returns></returns>
        private GraphicsPath GetTargetGPath()
        {
            GraphicsPath gp = new GraphicsPath();
            RectangleF lr = mTarget.VisibleClipBounds;
            gp.StartFigure();
            gp.AddLine(lr.Right, lr.Top, lr.Right, lr.Top);
            gp.AddLine(lr.Right, lr.Bottom, lr.Right, lr.Bottom);
            gp.AddLine(lr.Left, lr.Bottom, lr.Left, lr.Bottom);
            gp.AddLine(lr.Left, lr.Top, lr.Left, lr.Top);
            gp.CloseFigure();
            return gp;
        }

        #endregion

        #region<関数　文字列・イメージの描画処理>

        /// <summary>
        /// 文字列の描画処理
        /// </summary>
        /// <param name="str">描画する文字列</param>
        /// <param name="Font">フォント</param>
        /// <param name="StringColor">文字カラー</param>
        /// <param name="Position">表示位置（左と上点を使用する）</param>
        public void DrawString(String str, Font Font, Color StringColor, Rectangle Position)
        {
            using (SolidBrush br = new SolidBrush(StringColor))
            {
                mTarget.DrawString(str, Font, br, Position.Left, Position.Top);
            }
        }

        /// <summary>
        /// 画像の描画処理
        /// </summary>
        /// <param name="imgSource">描画イメージ</param>
        /// <param name="r">イメージの位置と大きさ</param>
        public void DrawImage(Image imgSource, Rectangle r)
        {
            mTarget.DrawImage(imgSource, r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// 文字列を縦に描画する
        /// </summary>
        /// <param name="msg">文字列</param>
        /// <param name="font">フォント</param>
        public void DrowStringVertical(string msg ,Font font)
        {
            // 縦書き用フォントを利用
            //using (Font font = new Font("@MS ゴシック", 24))
            {
                // 縦方向にフォーマット
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
                mTarget.DrawString("てすと", font, Brushes.Black, 10, 10, sf);
            }
        }

        /// <summary>
        /// 文字列を矩形内で折り返して描画する
        /// </summary>
        /// <param name="msg">文字列</param>
        /// <param name="size">サイズ</param>
        /// <param name="font">フォント</param>
        public void DrawStringRectangle(string msg ,Size size ,Font font )
        {
            //using (Font font = new Font("MS ゴシック", 24))
            {
                // PictureBoxのクライアント領域いっぱいを矩形とします
                RectangleF rect = new RectangleF(0, 0, size.Width, size.Height);
                mTarget.DrawString(msg, font, Brushes.Black, rect);
            }

        }


        #endregion

        #region<関数　部品の配置位置の算出>

        /// <summary>
        /// マージンを追加した四角形を取得
        /// </summary>
        /// <param name="rec">元の四角形</param>
        /// <param name="offset">マージン 正:縮小　負:拡大</param>
        /// <returns></returns>
        public static RectangleF GetMargin(RectangleF rec, float offset)
        {
            RectangleF recf = new RectangleF();
            recf = rec;
            recf.X += offset;
            recf.Y += offset;
            recf.Width -= (offset * 2);
            recf.Height -= (offset * 2);
            return recf;
        }

        /// <summary>
        /// 外枠基準で内部部品の配置情報を取得する
        /// </summary>
        /// <param name="contentAlignment">内部部品の配置</param>
        /// <param name="rectPart">内部部品の大きさ（位置は無視）</param>
        /// <param name="rectCanvas">外枠の大きさ</param>
        /// <param name="pddCanvas">外枠内部のパッディング値</param>
        /// <returns>内部部品の配置情報（パッティングも含む)</returns>
        /// <remarks>部品を配置して、その後にパッディング分オフセットする</remarks>
        public Rectangle GetAlignment(ContentAlignment contentAlignment, Rectangle rectPart, Rectangle rectCanvas, Padding pddCanvas)
        {
            Rectangle rectValue = rectCanvas;           // 外枠の大きさをコピーする
            Int32 tmpX = 0;                             // X軸計算用関数
            Int32 tnpY = 0;                             // Y軸計算用関数

            // 内部部品の配置の切り分け
            switch (contentAlignment)
            {
                case ContentAlignment.TopLeft:                          
                    break;
                case ContentAlignment.TopCenter:
                    tmpX = (rectValue.Width - rectPart.Width) / 2;       
                    rectValue.X += tmpX;                                    // コントロールを中央に配置
                    rectValue.Width -= tmpX;                                // 右にずらして左がはみ出た部分を消す
                    break;
                case ContentAlignment.TopRight:
                    tmpX = (rectValue.Width - rectPart.Width);            // 右端に寄せる
                    rectValue.X += tmpX;                                    // 右にずらして左がはみ出た部分を消す
                    rectValue.Width -= tmpX;
                    break;
                case ContentAlignment.MiddleLeft:
                    tnpY = (rectValue.Height - rectPart.Height) / 2;
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
                case ContentAlignment.MiddleCenter:
                    tmpX = (rectValue.Width - rectPart.Width) / 2;
                    rectValue.X += tmpX;
                    rectValue.Width -= tmpX;
                    tnpY = (rectValue.Height - rectPart.Height) / 2;
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
                case ContentAlignment.MiddleRight:
                    tmpX = (rectValue.Width - rectPart.Width);
                    rectValue.X += tmpX;
                    rectValue.Width -= tmpX;
                    tnpY = (rectValue.Height - rectPart.Height) / 2;
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
                case ContentAlignment.BottomLeft:
                    tnpY = (rectValue.Height - rectPart.Height);
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
                case ContentAlignment.BottomCenter:
                    tmpX = (rectValue.Width - rectPart.Width) / 2;
                    rectValue.X += tmpX;
                    rectValue.Width -= tmpX;
                    tnpY = (rectValue.Height - rectPart.Height);
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
                case ContentAlignment.BottomRight:
                    tmpX = (rectValue.Width - rectPart.Width);
                    rectValue.X += tmpX;
                    rectValue.Width -= tmpX;
                    tnpY = (rectValue.Height - rectPart.Height);
                    rectValue.Y += tnpY;
                    rectValue.Height -= tnpY;
                    break;
            }
            rectValue.Width = rectPart.Width;
            rectValue.Height = rectPart.Height;

            // パッディング処理
            rectValue.X += pddCanvas.Left;
            rectValue.Y += pddCanvas.Top;
            rectValue.Width -= (pddCanvas.Left + pddCanvas.Right);
            rectValue.Height -= (pddCanvas.Top + pddCanvas.Bottom);

            return rectValue;

        }

        #endregion

        #region<関数　描画 単色>

        /// <summary>
        /// 単色でグラフィックの描画
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="color">色</param>
        /// <param name="Alpha">透明色</param>
     
        public void DrowFill(GraphicsPath gp, Color color,int Alpha = 255 )
        {
            //グラデーションブラシの作成
            try
            {
                Brush b = new SolidBrush(Color.FromArgb(Alpha, color));
                mTarget.FillPath(b, gp);                           // グラデーションで塗りつぶす
            }
            catch
            {
            }
        }

        /// <summary>
        /// 単色で線の描画
        /// </summary>
        /// <param name="gp">直線グラフィックパス</param>
        /// <param name="edgeMode">線のスタイル</param>
        /// <param name="color">色</param>
        /// <param name="edgeSize">エッジの半径</param>
        public void DrowLine(GraphicsPath gp, EdgeMode edgeMode, Color color, float edgeSize)
        {
            //縁の描画処理
            if (edgeMode != EdgeMode.Non)
            {
                using (Pen p = new Pen(color))
                {
                    p.Width = edgeSize;
                    switch (edgeMode)
                    {
                        case EdgeMode.Solid:
                            p.DashStyle = DashStyle.Solid;               // 実線で縁を塗る
                            break;
                        case EdgeMode.Dash:
                            p.DashStyle = DashStyle.Dash;                // ダッシュで縁を塗る
                            break;
                        case EdgeMode.Dot:
                            p.DashStyle = DashStyle.Dot;                 // ドットで縁を塗る
                            break;
                        case EdgeMode.DashDot:
                            p.DashStyle = DashStyle.DashDot;             // ダッシュとドットで縁を塗る
                            break;
                        case EdgeMode.DashDotDot:
                            p.DashStyle = DashStyle.DashDotDot;          // ダッシュとドット二つで縁を塗る
                            break;
                    }
                    mTarget.DrawPath(p, gp);
                }
            }

            //// 既存のPenを使って黒の直線
            //gra.DrawLine(Pens.Black, 0, 0, pictureBox1.Width, 0);

            //// 青色、太さ３のペンで直線
            //Pen pen = new Pen(Color.Blue, 3);
            //gra.DrawLine(pen, 0, 10, pictureBox1.Width, 10);

            //// ペンを破線にして直線
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //gra.DrawLine(pen, 0, 20, pictureBox1.Width, 20);


        }

        /// <summary>
        /// 四角形の描画
        /// </summary>
        /// <param name="rc">四角形のデータ</param>
        /// <param name="color">色</param>
        public  void DrowRectangle(Rectangle rc , Color color)
        {
            using (Pen p = new Pen(color))
            {
                mTarget.DrawRectangle(p,rc);
            }
        }

        /// <summary>
        /// 四角形（塗りつぶし）の描画
        /// </summary>
        /// <param name="rc">四角形のデータ</param>
        /// <param name="color">色</param>
        public  void DrowFillRectangle(Rectangle rc , Color color)
        {
            using (Brush p = new SolidBrush(color))
            {
                mTarget.FillRectangle(p, rc);
            }
        }

        #endregion

        #region<関数　描画　グラデーション>

        /// <summary>
        /// グラデーションで線を描画
        /// </summary>
        /// <param name="gradationData">グラデーションデータ</param>
        /// <param name="gp"></param>
        /// <param name="edgeMode"></param>
        /// <param name="edgeSize"></param>
        public void LineGradient(string gradationData , GraphicsPath gp, EdgeMode edgeMode, float edgeSize)
        {
            DrowGradient(gradationData, gp, edgeMode, FillMode.Line, edgeSize);
        }

        /// <summary>
        /// グラデーションで内部を塗りつぶす
        /// </summary>
        /// <param name="gradationData">グラデーション情報</param>
        /// <param name="target">塗りつぶす対象</param>
        public void FillGradient(string gradationData, GraphicsPath target)
        {
            DrowGradient(gradationData, target, EdgeMode.Non, FillMode.Fill); 
        }

        /// <summary>
        /// グラデーションでグラフィックの描画
        /// </summary>
        /// <param name="gradationData">グラデーション情報</param>
        /// <param name="gp">塗りつぶす領域</param>
        /// <param name="fillMode">内部塗りつぶし・縁線の切り替え</param>
        /// <param name="edgeMode">エッジモード</param>
        /// <param name="edgeSize">エッジサイズ</param>
        private void DrowGradient(string gradationData, GraphicsPath gp, EdgeMode edgeMode, FillMode fillMode, float edgeSize = 1)
        {
            // this.BackGroundGradient に設定されている内容を解析してグラデーションパターンをセットする
            string[] b = gradationData.Split(',');
            Int32 v = Convert.ToInt32(b[0]);                    // インデックス0番目はグラデーションの方向を示す
            List<Color> ARGB = new List<Color>();               // カラーデータ　（0:透明 1:赤 2:緑 3:青）
            List<float> pos = new List<float>();                // 描画開始位置データ

            for (Int32 i = 1; i < b.Count(); i++)
            {
                string[] v2 = b[i].Split(':');                  // :で区切る
                ARGB.Add(ColorTranslator.FromHtml(v2[0]));      // カラーデータ
                pos.Add(float.Parse(v2[1]));                    // 描画開始位置データ
            }

            if (ARGB.Count() > 0)                               // カラーデータが一つ以上
            {
                // 最後のデータが1.0以外の場合は強制的に1.0とする
                if (pos[pos.Count() - 1] != 1.0f)
                {
                    pos[pos.Count() - 1] = 1.0f;
                }

                // グラデーションブラシの作成
                using (LinearGradientBrush b2 = new LinearGradientBrush(mTarget.VisibleClipBounds,       // 描画対象
                                                                        Color.FromArgb(0, 0, 0, 0),     // 開始色
                                                                        Color.FromArgb(0, 0, 0, 0),     // 終了色
                                                                        v))                             // 方向
                {
                    ColorBlend cb = new ColorBlend();                   // 色ブレンドの補間
                    cb.Colors = ARGB.ToArray();                         // ブレンドする色配列
                    cb.Positions = pos.ToArray();                       // 塗りつぶし形状
                    b2.InterpolationColors = cb;                        // グラデーションブラシの設定
                    mTarget.SmoothingMode = SmoothingMode.AntiAlias;     // アンチエーリアス処理を行う

                    if (fillMode == FillMode.Line)                      // 描画処理の切り替え
                    {
                        using (Pen p = new Pen(b2))
                        {
                            p.Width = edgeSize;
                            switch (edgeMode)
                            {
                                case EdgeMode.Solid:
                                    p.DashStyle = DashStyle.Solid;               // 実線で縁を塗る
                                    break;
                                case EdgeMode.Dash:
                                    p.DashStyle = DashStyle.Dash;                // ダッシュで縁を塗る
                                    break;
                                case EdgeMode.Dot:
                                    p.DashStyle = DashStyle.Dot;                 // ドットで縁を塗る
                                    break;
                                case EdgeMode.DashDot:
                                    p.DashStyle = DashStyle.DashDot;             // ダッシュとドットで縁を塗る
                                    break;
                                case EdgeMode.DashDotDot:
                                    p.DashStyle = DashStyle.DashDotDot;          // ダッシュとドット二つで縁を塗る
                                    break;
                            }
                            mTarget.DrawPath(p, gp);                        // グラデーション線の描画
                        }
                    }
                    else
                    {
                        mTarget.FillPath(b2, gp);                           // グラデーションで塗りつぶす
                    }
                }
            }
            ARGB.Clear();
            pos.Clear();
        }

        #endregion

        #region<関数　ボタンクリック状態の描画処理>

        /// <summary>
        /// ボタンクリック状態の描画を行う
        /// </summary>
        public void DrowPushState()
        {
            GraphicsPath gp = GetTargetGPath();             // ターゲットのグラフィックパスを取得
            DrowFill(gp, Color.Gray, 180);                  // 塗りつぶす
        }

         #endregion

        #region<関数　角丸パスの生成>

        /// <summary>
        /// 角丸パスの生成
        /// </summary>
        /// <param name="rect">コントロールのサイズ</param>
        /// <param name="radius">角のアール</param>
        /// <returns></returns>
        public　static GraphicsPath GetRoundRect(RectangleF rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();     // 直線
            rect.X += 1;
            rect.Y += 1;
            rect.Width -= 2;
            rect.Height -= 2;
            path.StartFigure();                         // 新しい図形を開く

            // 左上の角丸
            path.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
            // 上の線
            path.AddLine(rect.Left + radius, rect.Top, rect.Right - radius, rect.Top);
            // 右上の角丸
            path.AddArc(rect.Right - radius * 2, rect.Top, radius * 2, radius * 2, 270, 90);
            // 右の線
            path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius);
            // 右下の角丸
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            // 下の線
            path.AddLine(rect.Right - radius, rect.Bottom, rect.Left + radius, rect.Bottom);
            // 左下の角丸
            path.AddArc(rect.Left, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            // 左の線
            path.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Top + radius);

            path.CloseFigure();

            return path;
        }

        #endregion

    }

    /// <summary>
    /// グラフィック雑用
    /// </summary>
    public static class GraphicMisce
    {
        #region<関数　テキストサイズを取得>

        /// <summary>
        /// テキストサイズを取得する
        /// </summary>
        /// <returns></returns>
        public static SizeF GetTextSize(string txt, Font font, Point margin)
        {
            SizeF size = TextRenderer.MeasureText(txt, font);        // テキストサイズを取得
            size.Width += (margin.X * 2);
            size.Height += (margin.Y * 2);
            return size;
        }

        /// <summary>
        /// テキストサイズを取得する
        /// </summary>
        /// <returns></returns>
        public static SizeF GetTextSize(string txt, Font font)
        {
            return TextRenderer.MeasureText(txt, font);
        }

        #endregion
    }

}
