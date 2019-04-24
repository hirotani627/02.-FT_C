using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;           // トレース機能

namespace FT.C
{
    /// <summary>
    /// ZG圧縮クラス
    /// </summary>
    /// <remarks>
    /// ファイル単独で圧縮解凍する
    /// フォルダー単位でできない
    /// </remarks>
    public class GZip
    {

        #region<内部変数>

        /// <summary>ファイルパス</summary>
        string dirpath = @"c:\test1";

        /// <summary></summary>
        DirectoryInfo di;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GZip()
        {
            di = new DirectoryInfo(dirpath);
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

                di = null;
            }
            finally
            {

            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<関数　ファイルの圧縮>

        /// <summary>
        /// ファイルの圧縮
        /// </summary>
        public void Compress()
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                Compress(fi);       // 圧縮
            }
        }

        #endregion

        #region<関数　ファイルの解凍>

        /// <summary>
        /// ファイルの解凍
        /// </summary>
        public void Decompress()
        {
            foreach (FileInfo fi in di.GetFiles("*.gz"))
            {
                Decompress(fi);     // 解凍
            }
        }

        #endregion

        #region<関数　ファイルの圧縮>

        /// <summary>
        /// ファイルの圧縮
        /// </summary>
        /// <param name="fi"></param>
        /// <remarks>
        /// 拡張子が.gzでない場合は圧縮
        /// </remarks>
        public static void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and already compressed files.
                if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile = File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress))     // 圧縮
                        {
                            // Copy the source file into  the compression stream.
                            inFile.CopyTo(Compress);

                            Trace.WriteLine("Compressed;"+ fi.Name +  " from " + fi.Length + " to " + outFile.Length + " bytes.");
                        }
                    }
                }
            }
        }
        
        #endregion

        #region<関数　ファイルの解凍>

        /// <summary>
        /// ファイルの解凍
        /// </summary>
        /// <param name="fi"></param>
        public static void Decompress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example
                // "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                //Create the decompressed file.
                using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))      //解凍
                    {
                        // Copy the decompression stream into the output file.
                        Decompress.CopyTo(outFile);

                        Trace.WriteLine("Decompressed: {0}", fi.Name);
                    }
                }
            }
        }

        #endregion

    }
}
