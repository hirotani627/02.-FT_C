/************************************************************/
/*															*/
/*     ＭＤＢ制御クラス										*/
/*     ＭＤＢ制御処理群										*/
/*                                                          */
/*     Ver1.00  2012-04-05  リリース    H.Sato				*/
/*                                                          */
/************************************************************/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using DAO;

namespace FT.C
{

    /// <summary>
    /// ＭＤＢ制御クラス
    /// ＭＤＢ制御処理群
    /// </summary>
    /// <remarks>
    /// 
    /// Ver1.00  2012-04-05  リリース    H.Sato
    /// Ver1.01  2014-04-08  改造       Y.Hirotani
    /// 
    ///　[ 例外リスト ]
    ///	001.ＭＤＢファイルの作成に失敗
    ///	002.
    ///	003.
    ///	004.
    ///	005.
    ///	006.ＭＤＢファイルへの接続に失敗
    ///	007.ＭＤＢファイルの最適化に失敗
    ///	008.
    ///	009.
    ///	010.
    ///	011.ＭＤＢファイルに接続していないのにgetRecordsetをコールした
    ///	012.getRecordsetをコールした際にシステムエラーが発生した
    ///	013.
    ///	014.
    ///	015.
    ///	016.ＭＤＢファイルに接続していないのにExecuteをコールした
    ///	017.Executeをコールした際にシステムエラーが発生した
    ///	018.
    ///	019.
    ///	020.
    ///	021.ＭＤＢファイルに接続していないのにgetRecordCountをコールした
    ///	022.getRecordCountをコールした際にシステムエラーが発生した
    ///	023.
    ///	024.
    ///	025.
    /// </remarks>
    public class MDB :IDisposable
    {

        /// <summary>自クラス名</summary>
		private string MY_CLASS;

        /// <summary>データベースエンジンオブジェクト</summary>
        private DAO.DBEngine mDBE = null;

        /// <summary>ワークスペースオブジェクト</summary>
        private DAO.Workspace mWS = null;

        /// <summary>データベースオブジェクト</summary>
        private DAO.Database mDB = null;

        /// <summary>クラス内変数</summary>
        private object oMissing = System.Reflection.Missing.Value;

        #region<コンストラクタ・Dispose>

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public MDB()
		{
			MY_CLASS = typeof(MDB).Name;
		}

        /// <summary>
        /// リソースを破棄する
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region<データベース作成>

        /// <summary>
        /// データベース作成
        /// </summary>
        /// <param name="PathStr">ＭＤＢファイルパス</param>
        public void Create(string PathStr)
        {
            try
            {
                mDBE = new DAO.DBEngine();
				//mWS = mDBE.Workspaces[0];
				mWS = mDBE.CreateWorkspace("WorkSpace", "Admin", "", oMissing);
                mDB = mWS.CreateDatabase(PathStr, "" , DAO.DatabaseTypeEnum.dbVersion40);
            }
            catch( System.Exception exp )
            {
            /*--- ＤＢ作成エラー ---*/
                throw (new EXP(exp.Message, MY_CLASS, "001"));
            }
            finally
            {
            /*--- 最終処理 ---*/
                Close();
            }
        }

        #endregion

        #region<データベースを開く>
        
        /// <summary>
        /// データベースを開く
        /// </summary>
        /// <param name="PathStr">ＭＤＢファイルパス</param>
        public void Open(string PathStr)
        {

            try
            {
                mDBE = new DAO.DBEngine();
                //mWS = mDBE.Workspaces[0];
                mWS = mDBE.CreateWorkspace("WorkSpace", "Admin", "", oMissing);
                mDB = mWS.OpenDatabase(PathStr, oMissing, oMissing, oMissing);	// ＤＢを開く
            }
            catch (System.Exception exp)
            {
                /*--- ＤＢオープンエラー ---*/
                Close();
                throw (new EXP(exp.Message, MY_CLASS, "006"));
            }

        }

        #endregion

        #region<データベースを閉じる>

        /// <summary>
        /// データベースを閉じる
        /// </summary>
        public void Close()
        {
            if (mDB != null) mDB.Close();
            if (mWS != null) mWS.Close();

            mDB = null;
            mWS = null;
            mDBE = null;
        }

        #endregion

        #region<データベースを最適化>

        /// <summary>
        /// データベースを最適化
		/// </summary>
        /// <param name="PathStr">ＭＤＢファイルパス</param>
        public void Compact(string PathStr)
		{

            using (DIR oDir = new DIR())
            {
                string DirPath = oDir.DirName(PathStr);		// ファイルパスからディレクトリパスを取得
                string FileName = oDir.FileName(PathStr);	// ファイルパスからファイル名を取得

                string TempFile = DirPath + @"\T" + FileName;

                try
                {

                    TXT oFile = new TXT(TempFile);

                    if (true == oFile.FileUN())
                    {
                        /*--- テンポラリが残っているので削除 ---*/
                        oFile.Delete();
                    }
                    oFile.Dispose();											// リソース開放

                    DAO.DBEngine dbE = new DAO.DBEngine();
                    dbE.CompactDatabase(PathStr, TempFile, null, null, null);	// 最適化

                    oFile = new TXT(PathStr);
                    oFile.Delete();												// マスターファイル削除
                    oFile.Dispose();											// リソース開放

                    oFile = new TXT(TempFile);
                    oFile.Move(PathStr);										// テンポラリファイル名をマスターファイル名にリネーム
                    oFile.Dispose();											// リソース開放

                }
                catch (System.Exception exp)
                {
                    /*--- ＤＢ最適化エラー ---*/
                    throw (new EXP(exp.Message, MY_CLASS, "007"));
                }
            }

		}

        #endregion

        #region<ＳＱＬ文直接実行>

        /// <summary>
        /// ＳＱＬ文直接実行
        /// </summary>
        /// <param name="SQLStr">SQL文</param>
        public void Execute(string SQLStr)
        {

            if (mDB == null)
            {
                /*--- ＤＢが開かれてない ---*/
                throw (new EXP("", MY_CLASS, "016"));
            }

            try
            {
                mDB.Execute(SQLStr, oMissing);
            }
            catch (System.Exception exp)
            {
                throw (new EXP(exp.Message, MY_CLASS, "017"));
            }

        }

        #endregion

        #region<レコードを取得>

        /// <summary>
        /// レコードセットオブジェクト取得
        /// </summary>
        /// <param name="SQLStr">SQL文</param>
        /// <returns>レコードセットオブジェクト</returns>
        /// <remarks>
        /// （例）
        /// rsMDB = mMDB.getRecordset("SELECT * FROM UnitMaster ORDER BY UnitID , AlarmNo ");    //すべて読み込み
        /// </remarks>
        public DAO.Recordset getRecordset(string SQLStr)
        {

            if( mDB == null ){
            /*--- ＤＢが開かれてない ---*/
                throw (new EXP("", MY_CLASS, "011"));
            }

            try
            {
                // 指定された結果のレコードセットオブジェクト生成
                return mDB.OpenRecordset(SQLStr, DAO.RecordsetTypeEnum.dbOpenDynaset, oMissing, oMissing );
            }
            catch( System.Exception exp )
            {
                throw (new EXP(exp.Message, MY_CLASS, "012"));
            }

        }

        #endregion

        #region<レコード数を返す>

        /// <summary>
        /// テーブルのレコード数を返す
        /// </summary>
        /// <param name="TblNameStr">テーブル名</param>
        /// <returns>レコード件数</returns>
        public int GetRecordCount(string TblNameStr)
        {
            string SQLStr = "";
            DAO.Recordset rs = null;

            if( mDB == null ){
            /*--- ＤＢが開かれてない ---*/
                throw (new EXP("", MY_CLASS, "021"));
            }

            SQLStr = "SELECT COUNT(*) FROM " + TblNameStr;

            try
            {
                // レコードセットオブジェクト取得
                rs = getRecordset(SQLStr);

                //if( !rs.EOF ){
                    //return Fnc.CInt(rs.Fields[0].Value);
                //}else{
                    return (int)0;
                //}
            }
            catch( System.Exception exp )
            {
                throw (new EXP(exp.Message, MY_CLASS, "022"));
            }

        }

        #endregion

        #region<MDBデータを一件削除>

        /// <summary>
        /// MDBデータを一件削除
        /// </summary>
        /// <param name="RecordNo">削除する行</param>
        /// <param name="DataName">コラム名</param>
        /// <param name="FilePass">ファイルフルパス</param>
        /// <remarks>
        /// 「サンプル」
        /// MDBListDelete(int RecordNo,"INFO ORDER BY SEQ ,UnitNo", mCST_MDB.PathM_Unit ) 
        /// </remarks>
        public static void Delete(int RecordNo, string DataName, string FilePass)
        {
            using (MDB mMDB = new MDB())
            {
                mMDB.Open(FilePass);        //MDB接続
                Recordset rsMDB = mMDB.getRecordset("SELECT * FROM " + DataName);    //すべて読み込み
                rsMDB.MoveFirst();
                rsMDB.Move(RecordNo);
                rsMDB.Delete();
                mMDB.Close();
            }
        }

        #endregion

        #region<MDBの項目をすべて削除>

        /// <summary>
        /// MDBの項目をすべて削除
        /// </summary>
        /// <param name="FilePath">ファイルフルパス</param>
        /// <param name="FileNamePath">カラム名</param>
        public void Delete(string FilePath, string FileNamePath)
        {
            using (MDB mMDB = new MDB())
            {
                mMDB.Open(FilePath);        //MDB接続
                Recordset rsMDB = mMDB.getRecordset("SELECT * FROM " + FileNamePath);    //すべて読み込み
                int i;
                for (i = 0; !rsMDB.EOF; i++)
                {
                    rsMDB.Delete();
                    rsMDB.MoveNext();
                }
                mMDB.Close();
            }
        }

        #endregion

        #region<ファイル確認>

        /// <summary>
        /// ファイル確認
        /// </summary>
        /// <param name="MDB_FilePass"></param>
        /// <param name="mMDB"></param>
        public static void ChekFile(string MDB_FilePass, MDB mMDB)
        {
            // フォルダーある？
            if (!DIR.DirUM_static_CutFipePass(MDB_FilePass))
                DIR.MakeFolder_CutFipePass(MDB_FilePass);

            // mdbファイルある？
            if (!DIR.FileUM_static(MDB_FilePass))
                mMDB.Create(MDB_FilePass);
        }

        #endregion

    }
 
}
