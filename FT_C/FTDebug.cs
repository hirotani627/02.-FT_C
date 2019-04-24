using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// デバッククラス
    /// </summary>
    public class FTDebug
    {
        /// <summary>ファイルネーム</summary>
        public string mFileName = @"debug.txt";

        /// <summary>ファイルパス</summary>
        private string mFilePass = "";

        /// <summary>出力タイプ</summary>
        private FTDebugType mDebugType;

        /// <summary>
        /// ファイル名　プロパティ
        /// </summary>
        public string FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }

        /// <summary>
        /// ファイルパス　プロパティ
        /// </summary>
        public string FilePass
        {
            get { return mFilePass; }
            set { mFilePass = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FTDebug(FTDebugType type)
        {
            mDebugType = type;

            switch (mDebugType)
            {
                case FTDebugType.Text:
                    // デバッグ情報を出力するファイル名（実行ファイルと同じフォルダに置きます）
                    string filename = AppDomain.CurrentDomain.BaseDirectory + mFileName;

                    // 出力先としてテキストファイルを追加
                    System.Diagnostics.TextWriterTraceListener texttrace = new System.Diagnostics.TextWriterTraceListener(filename);
                    System.Diagnostics.Debug.Listeners.Add(texttrace);
                    break;
            }
        }

        /// <summary>
        /// メッセージの追加
        /// </summary>
        public void AddMesserg(string messege)
        {
            switch(mDebugType)
            {
                case FTDebugType.Text:
                    System.Diagnostics.Debug.WriteLine(messege);
                    System.Diagnostics.Debug.Flush();               // 出力バッファをフラッシュし、キャッシュをちゃんと書き込む
                    break;

                case FTDebugType.console:
                    System.Diagnostics.Debug.Write(messege);
                    break;

            }
        }

        /// <summary>
        /// インデント
        /// </summary>
        public void Indent()
        {
            switch (mDebugType)
            {
                case FTDebugType.Text:
                    System.Diagnostics.Debug.Indent();
                    break;
                case FTDebugType.console:
                    System.Diagnostics.Debug.Indent();
                    break;
            }
        }

    }


    /// <summary>
    /// デバック出力タイプ
    /// </summary>
    public enum FTDebugType
    {
        /// <summary>コンソール</summary>
        console,

        /// <summary>テキスト出力</summary>
        Text,
    }
}
