using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FT.C
{
	
	/// <summary>
	/// ＩＮＩファイル制御クラス
	/// </summary>
	/// 
	/// <remarks>
	/// ＩＮＩファイル制御処理群
	/// 
	/// Ver1.00  2012-04-05  リリース    H.Sato
    ///                     追加         Y.Hirotani
    /// 
	/// [ 例外リスト ]
	/// 001.データの書込みに失敗
	/// 
	/// </remarks>
	/// 
	public class INI : IDisposable, FT.C.IINI
	{

	/* API宣言 */

        // Iniファイル読込み
        [DllImport("kernel32.dll", EntryPoint="GetPrivateProfileString")]
        private static extern uint GetPrivateProfileString(string lpApplicationName
                                                         , string lpKeyName
                                                         , string lpDefault
                                                         , StringBuilder StringBuilder
                                                         , uint nSize
                                                         , string lpFileName );

        // Iniファイル書込み
        [DllImport("kernel32.dll", EntryPoint="WritePrivateProfileString")] 
        private static extern uint WritePrivateProfileString(string lpApplicationName
                                                           , string lpEntryName
                                                           , string lpEntryString
                                                           , string lpFileName );

        /// <summary>iniファイル名</summary>
        public static string _Fileini = ".ini";

        /* クラス内定数 */

        private const int BUFF_LEN = 256;                               // 文字列変数サイズ：256文字

        #region<内部変数>

        /// <summary>自クラス名</summary>
		private string MY_CLASS;

        /// <summary>ファイルパス</summary>
        private string mstrFilePath = "";

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<コンストラクタ>

        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <param name="filePath">Iniファイルパス</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public INI(string filePath)
        {
			MY_CLASS = typeof(INI).Name;
            mstrFilePath = filePath;
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
            }
            finally
            {

            }
            // リソースは破棄されている
            disposed = true;

        }

        #endregion

        #region<関数　Iniファイルよりデータ取得>

        /// <summary>
		/// Iniファイルよりデータ取得
		/// </summary>
		/// 
		/// <param name="Section">セクション名</param>
		/// <param name="Key">キー名</param>
		/// <returns>取得データ値</returns>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public string GetIniString(string Section, string Key){

            string Value;
            string Default = null;
            StringBuilder SB = new StringBuilder( BUFF_LEN );

            if( GetPrivateProfileString(Section, Key, Default, SB, Convert.ToUInt32(SB.Capacity), mstrFilePath) <= 0 ){
            /*--- データがない ---*/
                Value = "";
            }else{
                Value = SB.ToString();
            }

            return Value;
        }

        #endregion

        #region<関数　Iniファイルへデータ書込み>

        /// <summary>
		/// Iniファイルへデータ書込み
		/// </summary>
		/// 
		/// <param name="Section">セクション名</param>
		/// <param name="Key">キー名</param>
		/// <param name="Value">書込むデータ</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public void SetIniString(string Section, string Key, string Value){

            if( 0 == WritePrivateProfileString(Section, Key, Value, mstrFilePath ) ){
            /*--- 書込みエラー ---*/
                throw (new EXP("", MY_CLASS, "001"));
            }

        }

        #endregion

        #region<関数　ファイル有無を返す>

        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// 
        /// <returns>true : 有</returns>
        public bool FileUN()
        {
            return System.IO.File.Exists(mstrFilePath);
        }

        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// <returns>true : 有</returns>
        public static bool FileUM(string filePass)
        {
            return FT.C.DIR.FileUM_static(filePass);
        }

        #endregion

        #region<関数　ファイルの作成>

        /// <summary>
        /// ファイルの作成
        /// </summary>
        public void MakeFile()
        {

            // ディレクトリを作成
            if (!FileUM(mstrFilePath))
            {
                if (!FT.C.DIR.DirUM_static_CutFipePass(mstrFilePath))
                    FT.C.DIR.MakeFolder_CutFipePass(mstrFilePath);      
            }

            // hStream が破棄されることを保証するために using を使用する
            // 指定したパスのファイルを作成する
            using (System.IO.FileStream hStream = System.IO.File.Create(mstrFilePath))
            {
                // 作成時に返される FileStream を利用して閉じる
                if (hStream != null)
                {
                    hStream.Close();
                }
            }
        }

        /// <summary>
        /// ファイルの作成
        /// </summary>
        public static void MakeFile(string FilePath)
        {

            // ディレクトリを作成
            if (!FileUM(FilePath))
            {
                if (!FT.C.DIR.DirUM_static_CutFipePass(FilePath))
                    FT.C.DIR.MakeFolder_CutFipePass(FilePath);
            }

            // hStream が破棄されることを保証するために using を使用する
            // 指定したパスのファイルを作成する
            using (System.IO.FileStream hStream = System.IO.File.Create(FilePath))
            {
                // 作成時に返される FileStream を利用して閉じる
                if (hStream != null)
                {
                    hStream.Close();
                }
            }
        }

        /// <summary>
        /// ファイルがないときに作る
        /// </summary>
        /// <returns>True:ファイルが存在した</returns>
        public static bool MakeFileWhenNoFile(string FilePath)
        {
            if (!FileUM(FilePath))
            {
                MakeFile(FilePath);
                return false;
            }
            return true;
        }
        #endregion

        #region<INIファイルBool値 ロード取得>

        /// <summary>
        /// INIファイルからBool値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// /// <param name="iniValue">項目がなかった時の値</param>
        public bool GetBool( string Secsion, string Item, bool iniValue )
        {
            bool value = iniValue;      // iniの設定値
            string sData;               // iniで取得した内容

            sData = GetIniString(Secsion, Item);
            if ("" != sData)
            {
                if ((sData == "true") || (sData == "True"))
                    value = true;
                else
                    value = false;
            }

            return value;
        }

        /// <summary>
        /// INIファイルからBool値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <remarks>項目がなければ作る</remarks>
        public bool GetBoolMake( string Secsion, string Item, bool iniValue )
        {
            bool value = iniValue;      // iniの設定値
            string sData;               // iniで取得した内容

            sData = GetIniString(Secsion, Item);
            if ("" != sData)
            {
                if ((sData == "true") || (sData == "True"))
                    value = true;
                else
                    value = false;
            }
            else
                SetIniString(Secsion, Item, value.ToString());

            return value;
        }

        #endregion

        #region<INIファイルstring値 ロード取得>

        /// <summary>
        /// INIファイルからstring値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        public string GetString( string Secsion, string Item)
        {
            string sData;               // iniで取得した内容
            sData = GetIniString(Secsion, Item);
            return sData;
        }

        /// <summary>
        /// INIファイルからstring値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <remarks>項目がなければ作る</remarks>
        public string GetStringMake( string Secsion, string Item, string iniValue )
        {
            string sData;               // iniで取得した内容

            sData = GetIniString(Secsion, Item);
            if (string.IsNullOrEmpty( sData ))
                SetIniString(Secsion, Item, iniValue);

            return sData;
        }

        #endregion

        #region<INIファイルint値 ロード取得>

        /// <summary>
        /// INIファイルからint値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <remarks></remarks>
        public int Getint( string Secsion, string Item, int iniValue )
        {
            string sData;                   // iniで取得した内容
            int value = iniValue;

            sData = GetIniString(Secsion, Item);
            if ("" != sData)
            {
                double d;
                if (double.TryParse(sData, out d))
                    value = Convert.ToInt32(sData);           // 数字に変換可能の場合
                else
                    value = iniValue;                         // 数字に変換不可の場合
            }

            return value;
        }

        /// <summary>
        /// INIファイルからint値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <remarks>項目がなければ作る</remarks>
        public int GetintMake( string Secsion, string Item, int iniValue )
        {
            string sData;                   // iniで取得した内容
            int value = iniValue;

            sData = GetIniString(Secsion, Item);
            if ("" != sData)
            {
                double d;
                if (double.TryParse(sData, out d))
                    value = Convert.ToInt32(sData);           // 数字に変換可能の場合
                else
                    value = iniValue;                         // 数字に変換不可の場合
            }
            else
                SetIniString(Secsion, Item, iniValue.ToString());

            return value;
        }

        #endregion

        /// <summary>
        /// INIファイルからdouble値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <remarks>項目がなければ作る</remarks>
        public double GedoubleMake(string Secsion, string Item, double iniValue)
        {
            string sData;                   // iniで取得した内容
            double value = iniValue;

            sData = GetIniString(Secsion, Item);
            if ("" != sData)
            {
                double d;
                if (double.TryParse(sData, out d))
                    value = Convert.ToDouble(sData);          // 数字に変換可能の場合
                else
                    value = iniValue;                         // 数字に変換不可の場合
            }
            else
                SetIniString(Secsion, Item, iniValue.ToString());

            return value;
        }
    }

    /// <summary>
    /// Iniファイル接続インターフェース
    /// </summary>
    public interface IIniConnect
    {
        /// <summary>データロード(項目がなければ作る)</summary>
        void Load();

        /// <summary>データセーブ(項目がなければ作る)</summary>
        void Save();
    }

    /// <summary>
    /// Iniファイルインターフェース
    /// </summary>
    public interface IINI
    {
        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// <returns></returns>
        bool FileUN();

        /// <summary>
        /// INIファイルからBool値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <returns></returns>
        bool GetBool(string Secsion, string Item, bool iniValue);

        /// <summary>
        /// INIファイルからBool値のロード
        /// </summary>
        /// <param name="Secsion">セクション名</param>
        /// <param name="Item">項目</param>
        /// <param name="iniValue">項目がなかった時の値</param>
        /// <returns>項目がなければ作る</returns>
        bool GetBoolMake(string Secsion, string Item, bool iniValue);

        /// <summary>
        /// Iniファイルよりデータ取得
        /// </summary>
        /// <param name="Section">セクション名</param>
        /// <param name="Key">キー名</param>
        /// <returns>取得データ値</returns>
        string GetIniString(string Section, string Key);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Secsion"></param>
        /// <param name="Item"></param>
        /// <param name="iniValue"></param>
        /// <returns></returns>
        int Getint(string Secsion, string Item, int iniValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Secsion"></param>
        /// <param name="Item"></param>
        /// <param name="iniValue"></param>
        /// <returns></returns>
        int GetintMake(string Secsion, string Item, int iniValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Secsion"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        string GetString(string Secsion, string Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Secsion"></param>
        /// <param name="Item"></param>
        /// <param name="iniValue"></param>
        /// <returns></returns>
        string GetStringMake(string Secsion, string Item, string iniValue);

        /// <summary>
        /// ファイルの作成
        /// </summary>
        void MakeFile();

        /// <summary>
        /// Iniファイルへデータ書込み
        /// </summary>
        /// <param name="Section">セクション名</param>
        /// <param name="Key">キー名</param>
        /// <param name="Value">書込むデータ</param>
        void SetIniString(string Section, string Key, string Value);
    }

}
