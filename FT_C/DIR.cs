using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Scripting;
using System.Diagnostics;           // トレース機能

namespace FT.C
{

    /// <summary>
    /// ディレクトリ制御クラス
    /// </summary>
    /// 
    /// <remarks>
    /// ディレクトリ制御処理群
    /// 
    /// Ver1.00  2012-04-05  リリース    H.Sato
    /// 
    /// 
    /// [ 例外リスト ]
    /// 001.Createをコールした際にシステムエラーが発生した
    /// 
    /// 006.Deleteをコールした際にシステムエラーが発生した
    /// 007.削除しようとしたフォルダが存在しない
    /// 
    /// 011.コピー元のディレクトリが存在しない
    /// 012.Copyをコールした際にシステムエラーが発生した
    /// 
    /// 016.移動元のディレクトリが存在しない
    /// 017.Moveをコールした際にシステムエラーが発生した
    /// 018.ReNameをコールした際にシステムエラーが発生した
    /// 
    /// 021.ルートディレクトリが存在しない
    /// 022.ディレクトリが存在しない
    /// 
    /// </remarks>
    /// 
    public class DIR : IDisposable
    {

        /* クラス内変数 */

        /// <summary>自クラス名</summary>
        private string MY_CLASS;

        #region<コンストラクタ・Dispose>

        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public DIR()
        {
            MY_CLASS = typeof(DIR).Name;
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

        #region<関数　ディレクトリ作成>

        /// <summary>
        /// ディレクトリ作成
		/// </summary>
		/// <param name="DirPath">作成するディレクトリパス</param>
        public bool Create(string DirPath)
        {
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);      // フォルダ作成
                return true;
            }
            return false;
        }

        /// <summary>
        /// フォルダーを作成する(すでに存在していればスキップ)
        /// </summary>
        public static bool MakeFolder(string filePass)
        {
            if (!Directory.Exists(filePass))
            {
                Directory.CreateDirectory(filePass);      // フォルダ作成
                return true;
            }
            return false;
        }

        /// <summary>
        /// フォルダーを作成する(パスの最後から最初に見つけた\までを無視して実行)
        /// </summary>
        /// <param name="filePass">フォルダーパス</param>
        /// <remarks>.exeを含んだパスのままフォルダーを作成できます</remarks>
        public static void MakeFolder_CutFipePass(string filePass)
        {
            string s;
            s = FTString.GetString_Front(filePass, @"\");

            FT.C.DIR dir = new DIR();
            if (!dir.DirUM(s))
            {
                dir.Create(s);
            }
            dir = null;
        }

        #endregion

        #region<関数　ディレクトリ削除>

        /// <summary>
		/// ディレクトリ削除(ファイルは削除できない)
		/// </summary>
		/// <param name="DirPath">削除するディレクトリパス</param>
		/// <remarks>
		/// ※指定ディレクトリ内にあるサブディレクトリ、ファイル全てを削除します
		/// </remarks>
        public void Delete(string DirPath)
        {

            if (Directory.Exists(DirPath))
            {
                /*--- フォルダが有れば削除 ---*/
                try
                {
                    Directory.Delete(DirPath, true);    // フォルダ削除
                }
                catch (System.Exception exp)
                {
                    throw (new EXP(exp.Message, MY_CLASS, "006"));
                }
            }
            else
            {
                /*--- 削除するフォルダが存在しない ---*/
                throw (new EXP("", MY_CLASS, "007"));
            }
        }

        /// <summary>
        /// フォルダー削除(ファイルは削除できない)
        /// </summary>
        /// <returns></returns>
        public static bool DeleteFolder(string filePass)
        {
            if (!Directory.Exists(filePass)) return false;    // 対象フォルダー無し

            Directory.Delete(filePass, false);    // フォルダ削除
            return true;
        }

        /// <summary>
        /// ディレクトリ削除(パスの最後から最初に見つけた\までを無視して実行)
        /// </summary>
        /// <param name="DirPath">ディレクトリパス</param>
        public static void DeleteFolder_CutFipePass(string DirPath)
        {
            string pas = FTString.GetString_Front(DirPath, @"\");
            DeleteFolder(pas);

        }

        #endregion

        #region<関数　ディレクトリコピー>

        /// <summary>
		/// ディレクトリコピー
		/// </summary>
		/// <param name="Source">コピー元のパス</param>
		/// <param name="Destination">コピー先のパス</param>
        public static bool Copy_static(string Source, string Destination)
        {
            if (!Directory.Exists(Source))
            {
                /*--- コピー元のパスが存在しない ---*/
                return false;
            }

            // コピー元フォルダーの作成
            MakeFolder(Destination);

            // データコピー
            try
            {
                FileSystemObject fObj = new FileSystemObject();
                fObj.CopyFolder(Source, Destination, true);
            }
            catch 
            {
                return false;
            }

            return true;
        }

        /// <summary>
		/// ディレクトリコピー
		/// </summary>
		/// <param name="Source">コピー元のパス</param>
		/// <param name="Destination">コピー先のパス</param>
        public void Copy(string Source, string Destination)
        {

            if (!Directory.Exists(Source))
            {
                /*--- コピー元のパスが存在しない ---*/
                throw (new EXP("", MY_CLASS, "011"));
            }

            FileSystemObject fObj = new FileSystemObject();

            try
            {
                fObj.CopyFolder(Source, Destination, true);
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "012"));
            }

        }

        #endregion

        #region<関数　ディレクトリ移動>

        /// <summary>
        /// ディレクトリ移動
        /// </summary>
        /// 
        /// <param name="Source">移動元のパス</param>
        /// <param name="Destination">移動先のパス</param>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public void Move(string Source, string Destination)
        {

            if (!Directory.Exists(Source))
            {
                /*--- 移動元のパスが存在しない ---*/
                throw (new EXP("", MY_CLASS, "016"));
            }

            FileSystemObject fObj = new FileSystemObject();

            try
            {
                fObj.MoveFolder(Source, Destination);
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "017"));
            }

        }

        /// <summary>
		/// ディレクトリ移動
		/// </summary>
		/// <param name="Source">移動元のパス</param>
		/// <param name="Destination">移動先のパス</param>
        public static bool Move_static(string Source, string Destination)
        {

            // 移動元のパスが存在？
            if (!Directory.Exists(Source))
                return false;

            // 移動先のフォルダーが存在？
            if (!Directory.Exists(Destination))
            {
                MakeFolder(Destination);        // フォルダー作成
            }

            FileSystemObject fObj = new FileSystemObject();
            try
            {
                fObj.MoveFolder(Source, Destination);
                fObj = null;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp.Message);
                throw (exp);
            }
            return true;

        }

        #endregion

        #region<関数　ディレクトリ名変更>

        /// <summary>
		/// ディレクトリ名変更
		/// </summary>
		/// 
		/// <param name="SourcePath">変更元のパス</param>
		/// <param name="NewDirName">変更先のディレクトリ名</param>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
		public void ReName(string SourcePath, string NewDirName)
        {

            try
            {

                string strNewPath = DirName(SourcePath) + @"\" + NewDirName;

                Move(SourcePath, strNewPath);

            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "018"));
            }

        }

        #endregion

        #region<関数　ディレクトリパス取得>

        /// <summary>
        /// サブディレクトリパスを返す
        /// </summary>
        /// <param name="rootDirPath">ルートディレクトリパス</param>
        /// <returns>サブディレクトリパス</returns>
        public static string[] GetSubDirectory(string rootDirPath)
        {

            if (!Directory.Exists(rootDirPath))
            {
                return new string[0];
            }

            return Directory.GetDirectories(rootDirPath);

        }

        /// <summary>
        /// サブディレクトリパスを返す
        /// </summary>
        /// <param name="rootDirPath">ルートディレクトリパス</param>
        /// <returns>サブディレクトリパス</returns>
        public string[] SubDirectory(string rootDirPath)
        {

            if (!Directory.Exists(rootDirPath))
            {
                /*--- ルートディレクトリが存在しない ---*/
                throw (new EXP("", MY_CLASS, "021"));
            }

            return Directory.GetDirectories(rootDirPath);

        }

        #endregion

        #region<関数　拡張子の取得>

        /// <summary>
        /// ディレクトリ内の指定した拡張子のファイルパスを返す
        /// </summary>
        /// 
        /// <param name="DirPath">検索ディレクトリパス</param>
        /// <param name="Extension">拡張子文字列</param>
        /// <returns>ファイルパス</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public string[] FileSearch(string DirPath, string Extension)
        {

            if (!Directory.Exists(DirPath))
            {
                /*--- ディレクトリが存在しない ---*/
                throw (new EXP("", MY_CLASS, "022"));
            }

            return Directory.GetFiles(DirPath, "*." + Extension);

        }

        #endregion

        #region<関数　ファイルパスの加工>

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

        /// <summary>
        /// ファイルパスからファイル名だけを返す
        /// </summary>
        /// 
        /// <param name="FilePath">ファイルパス</param>
        /// <returns>ファイル名</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public string FileName(string FilePath)
        {

            return Path.GetFileName(FilePath);

        }

        #endregion

        #region<関数　フォルダディレクトリ・ファイル有無確認>

        /// <summary>
		/// フォルダディレクトリ有無を返す
		/// </summary>
		/// 
		/// <param name="DirPath">ディレクトリパス</param>
		/// <returns>true : 有</returns>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		/// 
        public bool DirUM(string DirPath)
        {
            return Directory.Exists(DirPath);
        }

        /// <summary>
        /// フォルダディレクトリ有無を返す
        /// </summary>
        /// 
        /// <param name="DirPath">ディレクトリパス</param>
        /// <returns>true : 有</returns>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// 
        public static bool DirUM_static(string DirPath)
        {
            return Directory.Exists(DirPath);
        }

        /// <summary>
        /// フォルダディレクトリ有無を返す(パスの最後から最初に見つけた\までを無視して実行)
        /// </summary>
        /// 
        /// <param name="DirPath">フォルダパス</param>
        /// <returns>true : 有</returns>
        public static bool DirUM_static_CutFipePass(string DirPath)
        {
            string pas = FTString.GetString_Front(DirPath, @"\");
            return Directory.Exists(pas);
        }

        #endregion

        #region<関数 フォルダ名一覧を取得>

        /// <summary>
        /// フォルダ名一覧を取得
        /// </summary>
        public string[] GetFolderNames(string FilePath)
        {
            if (!Directory.Exists(FilePath))
            //if(!System.IO.File.Exists(FilePath))
            {
                /*--- ルートディレクトリが存在しない ---*/
                throw (new EXP("ルートディレクトリが存在しない", MY_CLASS, "021"));
            }

            try
            {
                string[] FilesPass = GetFoldersPass(FilePath);      // フォルダーパス
                return GetFolderName(FilesPass);                      // ファイルパスからファイル名のみを抽出
            }
            catch (System.Exception exp)
            {
                throw (new EXP("ルートディレクトリが存在しない\n" + exp.Message, MY_CLASS, "021"));
            }

        }

        /// <summary>
        /// フォルダ名一覧を取得
        /// </summary>
        public static bool GetFolderNames(string FilePath, out string[] FileNames)
        {
            if (!Directory.Exists(FilePath))
            {
                FileNames = null;
                return false;
            }

            try
            {
                string[] fp;
                if (!GetFoldersPass(FilePath, out fp))
                { // フォルダーパス
                    FileNames = null;
                    return false;
                }
                FileNames = GetFolderName(fp);   // ファイルパスからファイル名のみを抽出
                return true;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp.Message);
                FileNames = null;
                return false;
            }
        }

        #endregion

        #region<関数 フォルダーパス一覧を取得>

        /// <summary>
        /// フォルダーパス一覧を取得
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public string[] GetFoldersPass(string FilePath)
        {
            string[] FilesPass;                         // フォルダーパス

            //フォルダーパスを取得
            try
            {
                FilesPass = Directory.GetDirectories(FilePath, "*", SearchOption.TopDirectoryOnly);
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "006"));
            }

            return FilesPass;
        }

        /// <summary>
        /// フォルダーパス一覧を取得
        /// </summary>
        /// <param name="FilePath">検索するパス</param>
        /// <param name="FilesPass">パス一覧</param>
        /// <returns></returns>
        public static bool GetFoldersPass(string FilePath, out string[] FilesPass)
        {
            //フォルダーパスを取得
            try
            {
                FilesPass = Directory.GetDirectories(FilePath, "*", SearchOption.TopDirectoryOnly);
                return true;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp.Message);
                FilesPass = null;
                return false;
            }
        }

        #endregion

        #region<関数 フォルダーパスからファイル名のみを抽出>

        /// <summary>
        /// フォルダーパスからフォルダー名のみを抽出
        /// </summary>
        /// <param name="filesPass"></param>
        /// <returns></returns>
        public static string GetFolderName(string filesPass)
        {
            return Path.GetFileName(filesPass);          //フォルダーパスからフォルダー名のみを抽出
        }

        /// <summary>
        /// フォルダーパスからフォルダー名のみを抽出
        /// </summary>
        /// <param name="FilesPass"></param>
        /// <returns></returns>
        public static string[] GetFolderName(string[] FilesPass)
        {
            string[] FileNames = new string[FilesPass.Length];          // フォルダー名

            for (int i = 0; i < FilesPass.Length; i++)
            {
                FileNames[i] = GetFolderName(FilesPass[i]);          //フォルダーパスからフォルダー名のみを抽出
            }
            return FileNames;
        }

        #endregion

        #region<関数 フォルダからフォルダ番号を取得>

        /// <summary>
        /// フォルダからフォルダ番号を取得(数値以外のフォルダーを無視)
        /// </summary>
        /// <param name="filepass"></param>
        /// <returns></returns>
        public int[] GetFolderNo(string filepass)
        {
            return GetFolderNo(filepass, false);
        }

        /// <summary>
        /// フォルダからフォルダ番号を取得
        /// </summary>
        /// <param name="filepass">ファイルパス</param>
        /// <param name="passNotNumber">数値以外のフォルダーを"-1"としてあつかう</param>
        /// <returns>ファイル名が不正なら-1を返す</returns>
        public int[] GetFolderNo(string filepass, bool passNotNumber)
        {
            string[] FileNames;                                         // ファイル番号
            FT.C.DIR dir = new DIR();                                   // ディレクトリ編集クラス
            try
            {
                FileNames = dir.GetFolderNames(filepass);                   // フォルダーを取得する
                return FTString.NumberToString(FileNames, passNotNumber);   // 文字から数字に置き換える
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "106"));
            }
        }

        #endregion

        #region<関数 マイドキュメントパスを取得>

        /// <summary>
        /// マイドキュメントパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetDocmentPass()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        #endregion

        #region<関数 filePass 以下のファイルの中で str の文字列を含むファイル名を取得>

        /// <summary>
        /// filePass 以下のファイルの中で str の文字列を含むファイル名を取得
        /// </summary>
        /// <param name="filePass">検索するファイルパス</param>
        /// <param name="str">ファイルに含まれる文字（拡張子など）</param>
        /// <param name="serch">検索モード</param>
        /// <returns></returns>
        public static IEnumerable<string> SerchDir(string filePass, string str, SearchOption serch)
        {
            DirectoryInfo di = new DirectoryInfo(filePass);
            IEnumerable<System.IO.FileInfo> fiList = di.GetFiles("*", serch);
            return fiList.Where(fi => (0 <= fi.Name.IndexOf(str, StringComparison.CurrentCultureIgnoreCase))).Select(fi => fi.FullName);
        }

        #endregion

        #region<関数　ファイル移動>

        /// <summary>
		/// ファイル移動
		/// </summary>
		/// <param name="Source">移動元のファイルパス</param>
		/// <param name="Destination">移動先のフォルダーパス</param>
        public static bool MoveFile(string Source, string Destination)
        {
            string pass = Destination + @"\" + Path.GetFileName(Source);

            // パスが同じ？
            if (pass == Source) return true;

            // 移動先のフォルダーが存在？
            if (!Directory.Exists(Destination))
            {
                MakeFolder(Destination);        // フォルダー作成
            }
            else
            {
                System.IO.FileInfo ft = new System.IO.FileInfo(pass);
                if (ft.Exists)
                {
                    // 同一名のファイルが存在する場合は削除する
                    ft.Delete();
                }
                ft = null;
            }


            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(Source);
                if (fi.Exists)
                {

                    fi.MoveTo(pass);
                    fi = null;
                }
                else
                {
                    // コピー元のファイルがない！
                    fi = null;
                    return false;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }

            return true;
        }


        /// <summary>
        /// フォルダー内のファイルを移動する(複数のファイルに対応)
        /// 拡張子がわかればOK。ファイル名は自動検索
        /// </summary>
        /// <param name="tmpFile">移動元フォルダー名</param>
        /// <param name="outPass">移動先</param>
        /// <param name="type">拡張子（フィルター）</param>
        public static bool MoveFile_Folder(string tmpFile, string outPass, string type)
        {
            if (!FT.C.DIR.DirUM_static(tmpFile))
            {
                Trace.WriteLine(tmpFile + "のディレクトリなし");
                return false;
            }

            var d = FT.C.DIR.GetFilesPass(tmpFile, type);
            foreach (var dd in d)
            {
                FT.C.DIR.MoveFile(dd, outPass);
            }
            return true;
        }

        #endregion

        #region<関数ファイルパス一覧を取得>

        /// <summary>
        /// ファイルパス一覧を取得(C:から始まる)
        /// ※フォルダーは取得できない
        /// </summary>
        /// <param name="FilePath">ファイルパス</param>
        /// <param name="type">拡張子
        /// <param name="sp">タイプ</param>
        /// ファイルタイプ 
        /// </param>
        /// <returns></returns>
        public static IEnumerable<string> GetFilesPass(string FilePath, string type = "", SearchOption sp = SearchOption.TopDirectoryOnly)
        {
            //ファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    FilePath, "*" + type, sp);
            return files;
        }

        #endregion

        #region<関数 EXEを実行しているファイルパスを取得>

        /// <summary>
        /// EXEを実行しているファイルパスを取得
        /// </summary>
        /// <returns></returns>
        public static string GetExeFilePass()
        {
            string pass = System.Windows.Forms.Application.ExecutablePath;
            return FT.C.DIR.GetDirectoryName(pass);
        }

        #endregion

        #region<関数 フルパスから一番深いフォルダーパスを取得>

        /// <summary>
        /// フルパスから一番深いフォルダーパスを取得
        /// </summary>
        /// <param name="orignal">フルパス</param>
        /// <remarks>
        /// </remarks>
        public static string GetDirectoryName(string orignal)
        {

            return System.IO.Path.GetDirectoryName(orignal);
        }

        #endregion

        #region<関数　ファイル有無確認>

        /// <summary>
        /// ファイル有無を返す(フォルダは確認できない)
        /// </summary>
        /// 
        /// <param name="FilePath">ファイルパス</param>
        /// <returns>true : 有</returns>
        public bool FileUM(string FilePath)
        {
            return System.IO.File.Exists(FilePath);
        }

        /// <summary>
        /// ファイル有無を返す(フォルダは確認できない)
        /// </summary>
        /// <param name="FilePath">ファイルパス</param>
        /// <returns>true : 有</returns>
        public static bool FileUM_static(string FilePath)
        {
            return System.IO.File.Exists(FilePath);
        }

        #endregion

        #region<関数　ファイル削除>

        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="fullPass">ファイルパス(拡張子付き)</param>
        /// <returns></returns>
        public static bool DeleteFile(string fullPass)
        {
            System.IO.File.Delete(fullPass);
            return true;
        }

        #endregion

    }
}
