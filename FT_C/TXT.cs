using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;


namespace FT.C
{

	/// <summary>
	/// テキストファイル制御クラス
	/// </summary>
	/// 
	/// <remarks>
	/// テキストファイル制御処理群
	/// 
	/// Ver1.00  2012-04-05  リリース    H.Sato
    /// Ver1.01  2013-08-20  リリース    Y.Hirotani
    /// 
	/// [ 例外リスト ]
	/// 001.読込もうとしたファイルが存在しない
	/// 002.Readをコールした際にシステムエラーが発生した
	/// 
	/// 006.全てのデータをバイナリ形式で読込んだ際にシステムエラーが発生した
	/// 007.読込んだデータより(オフセット+データ長)が大きかった
	/// 008.ファイルを上書き中にシステムエラーが発生した
	/// 009.ファイルを書込み中にシステムエラーが発生した
    /// 010.ファイルを書込み中にシステムエラーが発生した(データクラス定義に  [Serializable]  が記述されていない可能性があります）
	/// 
	/// 015.ファイルの削除中にシステムエラーが発生した
	/// 016.ファイルの移動中にシステムエラーが発生した
	/// 017.ファイルの複写中にシステムエラーが発生した
	/// 018.ファイル名の変更中にシステムエラーが発生した
	/// 
	/// 020.読込もうとしたファイルが存在しない
	/// 021.ReadAllをコールした際にシステムエラーが発生した
	/// 
	/// </remarks>
	/// 
	public class TXT : IDisposable
	{

	/* クラス内変数 */

        /// <summary>自クラス名</summary>
		private string MY_CLASS;

        /// <summary>ファイルパス</summary>
        private string mstrFilePath = "";

        #region<コンストラクタ・Dispose>
        
        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <param name="filePath">テキストファイルパス</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public TXT(string filePath)
        {
			MY_CLASS = typeof(TXT).Name;
            mstrFilePath = filePath;
        }

        /// <summary>
        /// リソースを破棄する
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public void Dispose()
        {
        }

        #endregion

        #region<関数　ファイルの作成>

        /// <summary>
        /// ファイルの作成
        /// </summary>
        public void MakeFile()
        {
            MakeFile_static(mstrFilePath);
        }

        /// <summary>
        /// ファイルの作成
        /// </summary>
        public static void MakeFile_static(string pass)
        {
            // hStream が破棄されることを保証するために using を使用する
            // 指定したパスのファイルを作成する
            //Console.WriteLine("MakeFile:{0}",mstrFilePath);

            using (System.IO.FileStream hStream = System.IO.File.Create(pass))
            {
                // 作成時に返される FileStream を利用して閉じる
                if (hStream != null)
                {
                    hStream.Close();
                }
            }
        }

        #endregion

        #region<関数　ファイルの削除>

        /// <summary>
		/// ファイル削除
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Delete()
		{

			try
			{
				System.IO.File.Delete(mstrFilePath);

			}catch(System.Exception exp){

				throw (new EXP(exp.Message, MY_CLASS, "015"));

			}

		}

        /// <summary>
        /// ファイル削除
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public static void Delete( string pass)
        {

            try
            {
                System.IO.File.Delete(pass);

            }
            catch (System.Exception exp)
            {
                throw (new ApplicationException("ファイル削除失敗:" + exp.Message));
            }

        }


        #endregion

        #region<関数　ファイルの移動>

        /// <summary>
		/// ファイル移動
		/// </summary>
		/// 
		/// <param name="MovePath">移動先パス</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Move(string MovePath)
		{
			
			try
			{
				System.IO.File.Move(mstrFilePath, MovePath);

			}catch(System.Exception exp){

				throw (new EXP(exp.Message, MY_CLASS, "016"));

			}

		}

        #endregion

        #region<関数　ファイルの複写>

        /// <summary>
		/// ファイル複写
		/// </summary>
		/// 
		/// <param name="CopyPath">複写先パス</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void Copy(string CopyPath)
		{

			try
			{
				System.IO.File.Copy(mstrFilePath, CopyPath, true);

			}catch(System.Exception exp){

				throw (new EXP(exp.Message, MY_CLASS, "017"));
			}

		}

        /// <summary>
        /// ファイル複写
        /// </summary>
        /// <param name="OriginalPath">コピー元パス</param>
        /// <param name="CopyPath">複写先パス</param>
        /// <returns>True:コピー成功</returns>
        /// <remarks>
        /// 例
        ///  TXT.Copy(@"C:\Work\a.mdb",@"C:\Work\b.mdb");
        /// </remarks>
        public static bool Copy(string OriginalPath, string CopyPath)
        {
            if (!FileUN(OriginalPath))
                return false;

            // コピー先フォルダーの作成
            if (!DIR.DirUM_static_CutFipePass(CopyPath))
                DIR.MakeFolder_CutFipePass(CopyPath);

            // ファイルのコピー
            System.IO.File.Copy(OriginalPath, CopyPath, true);
            
            return true;
        }

        #endregion

        #region<関数　ファイルの名変更>

        /// <summary>
		/// ファイル名変更
		/// </summary>
		/// 
		/// <param name="NewFileName">変更後のファイル名</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void ReName(string NewFileName)
		{

			try
			{

				string strNewPath = DirName(mstrFilePath) + @"\" + NewFileName;

				Move(strNewPath);

			}catch(System.Exception exp){

				throw (new EXP(exp.Message, MY_CLASS, "018"));

			}

		}

        #endregion

        #region<関数 ファイルの有無>

        /// <summary>
		/// ファイル有無を返す
		/// </summary>
		/// 
		/// <returns>true : 有</returns>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public bool FileUN()
        {
            return System.IO.File.Exists(mstrFilePath);
        }

        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// 
        /// <returns>true : 有</returns>
        public static bool FileUN(string pass)
        {
            return System.IO.File.Exists(pass);
        }


        #endregion

        #region<関数 ファイルパス>

        /// <summary>
		/// ファイルパスからディレクトリパスだけを返す
		/// </summary>
		/// 
		/// <param name="FilePath">ファイルパス</param>
		/// <returns>ディレクトリパス</returns>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public string DirName(string FilePath)
		{
			return Path.GetDirectoryName(FilePath);
		}

        #endregion

        #region<関数 フォルダからファイル名を取得>

        /// <summary>
        /// フォルダからファイル名を取得
        /// </summary>
        /// <param name="filepass">ファイルパス</param>
        /// <param name="passNotNumber">数値以外のフォルダーを"-1"としてあつかう</param>
        /// <param name="searchPattern"></param>
        /// <returns>ファイル名が不正なら-1を返す</returns>
        public static int[] GetNo(string filepass, bool passNotNumber, string searchPattern = "*")
        {
            //"C:\My Documents"以下のファイルをすべて取得
            //ワイルドカード"*"は、すべてのファイルを意味する
            string[] files = System.IO.Directory.GetFiles(filepass, searchPattern, 
                                                            System.IO.SearchOption.AllDirectories);

            return FTString.NumberToString(files, passNotNumber);   // 文字から数字に置き換える

        }

        #endregion

        #region<関数　データ読込み>

        /// <summary>
        /// データ読込み
        /// </summary>
        /// 
        /// <param name="Offset">オフセット値 ※何バイト目かを指定 0～</param>
        /// <param name="Length">データ長     ※バイト数で指定</param>
        /// <returns>読込んだデータ</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public byte[] Read(int Offset, int Length)
        {

            int iLp1;
            byte[] Value;

            if (false == System.IO.File.Exists(mstrFilePath))
            {
                /*--- ファイルが存在しない ---*/
                throw (new EXP("", MY_CLASS, "001"));
            }

            try
            {
                byte[] RByte = System.IO.File.ReadAllBytes(mstrFilePath);	// 全てのデータをバイナリ形式で読込み
                Value = new byte[Length];

                for (iLp1 = 0; iLp1 < Length; iLp1++)
                {
                    Value[iLp1] = RByte[Offset + iLp1];                     // 指定された分だけ抜粋
                }

                return Value;
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "002"));
            }

        }

        #endregion

        #region<関数　データ書込み>

        /// <summary>
        /// データ書込み
        /// </summary>
        /// <param name="Offset">オフセット値 ※何バイト目かを指定 0～</param>
        /// <param name="Length">データ長     ※バイト数で指定</param>
        /// <param name="Value">書込むデータ</param>
        public void Write(int Offset, int Length, byte[] Value)
        {

            byte[] RByte;
            int iLp1;

            if (true == System.IO.File.Exists(mstrFilePath))
            {
                /*--- ファイルが存在する場合 ---*/

                try
                {
                    RByte = System.IO.File.ReadAllBytes(mstrFilePath);		// 全てのデータをバイナリ形式で読込み
                }
                catch (System.Exception exp)
                {
                    throw (new EXP(exp.Message, MY_CLASS, "006"));
                }
                if (RByte.Length < Offset + Length)
                {
                    /*--- 読込んだデータより(オフセット+データ長)が大きければ書替え不可 ---*/
                    throw (new EXP("", MY_CLASS, "007"));
                }

                for (iLp1 = 0; iLp1 < Length; iLp1++)
                {
                    RByte[Offset + iLp1] = Value[iLp1];                     // 指定された分だけデータ差替え
                }

                try
                {
                    System.IO.File.WriteAllBytes(mstrFilePath, RByte);		// ファイルを上書き
                }
                catch (System.Exception exp)
                {
                    throw (new EXP(exp.Message, MY_CLASS, "008"));
                }
            }
            else
            {
                /*--- ファイルが存在しない場合 ---*/

                try
                {
                    System.IO.File.WriteAllBytes(mstrFilePath, Value);
                }
                catch (System.Exception exp)
                {
                    throw (new EXP(exp.Message, MY_CLASS, "009"));
                }
            }
        }

        #endregion

        #region<関数　データ書込み(string)>

        /// <summary>
        /// 新しいファイルを作成し、指定した文字列をそのファイルに書き込んだ後、ファイルを閉じます。既存のターゲット ファイルは上書きされます。
        /// </summary>
        /// <param name="contents">文字列</param>
        /// <returns></returns>
        public bool WriteText(string contents)
        {
            return WriteText(mstrFilePath, contents);
        }

        /// <summary>
        /// 新しいファイルを作成し、指定した文字列をそのファイルに書き込んだ後、ファイルを閉じます。既存のターゲット ファイルは上書きされます。
        /// </summary>
        /// <param name="fullPass">ファイルパス(拡張子付き)</param>
        /// <param name="contents">文字列</param>
        /// <returns></returns>
        public static bool WriteText(string fullPass, string contents)
        {
            System.IO.File.WriteAllText(fullPass, contents);
            return true;
        }

        #endregion

        #region<関数　ファイルデータを全て一括で返す(string)>

        /// <summary>
        /// ファイルデータを全て一括で返す
        /// </summary>
        /// 
        /// <returns>ファイルデータ</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public string ReadAll()
        {

            if (false == System.IO.File.Exists(mstrFilePath))
            {
                /*--- ファイルが存在しない ---*/
                throw (new EXP("", MY_CLASS, "020"));
            }

            try
            {

                System.IO.StreamReader sr = new System.IO.StreamReader(mstrFilePath, System.Text.Encoding.GetEncoding("shift_jis"));

                return sr.ReadToEnd();

            }
            catch (System.Exception exp)
            {

                throw (new EXP(exp.Message, MY_CLASS, "021"));

            }

        }

        #endregion

        #region<関数 シリアルデータ読込み>

        /// <summary>
        /// シリアルデータ読込み
        /// </summary>
        /// 
        /// <returns>読込んだクラスデータ</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public object Read()
        {

            if (false == System.IO.File.Exists(mstrFilePath))
            {
                /*--- ファイルが存在しない ---*/
                throw (new EXP("", MY_CLASS, "001"));
            }

            try
            {
                FileStream fs = new FileStream(mstrFilePath, FileMode.Open, FileAccess.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();

                //読み込んで逆シリアル化する
                object obj = bf.Deserialize(fs);
                fs.Close(); 
                fs.Dispose();
                bf = null;
                return obj;
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "002"));
            }
        }

        #endregion

        #region<関数 シリアルデータ書込み>

        /// <summary>
        /// シリアルデータ書込み
        /// </summary>
        /// 
        /// <param name="ClassValue">保存するクラス</param>
        /// 
        /// <remarks>
        /// ファイルがない場合は新規作成して書き込む
        /// ファイルがある場合は上書き保存する
        /// </remarks>
        /// 
        public void Write(object ClassValue)
        {

            try
            {
                Stream fs = new FileStream(mstrFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                BinaryFormatter bf = new BinaryFormatter();

                //シリアル化して書き込む
                bf.Serialize(fs, ClassValue);
                fs.Close();
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "010"));
            }
        }

        #endregion

        #region<関数 フォルダ操作>

        /// <summary>
        /// フォルダー一覧を作成する
        /// </summary>
        /// <returns></returns>
        public string[] ScanFolders()
        {
            return Directory.GetFiles(mstrFilePath,"*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// フォルダーの有無を返す
        /// </summary>
        public bool FolderUN()
        {
            return System.IO.Directory.Exists(mstrFilePath + @"\");
        }

        /// <summary>
        /// フォルダーの作成
        /// </summary>
        public void MakeFolder()
        {
            // フォルダ (ディレクトリ) を作成する
            System.IO.Directory.CreateDirectory(mstrFilePath + @"\");
        }

        #endregion

    }

}
