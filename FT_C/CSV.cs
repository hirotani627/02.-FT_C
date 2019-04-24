using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace FT.C
{
    /// <summary>
    /// CSVファイル制御クラス
    /// </summary>
    public class CSV
    {

        #region<定数>

        /// <summary>ファイルパス</summary>
        public const string _csv = @".csv";

        #endregion

        #region<関数 読み込み>

        /// <summary>
        /// 一行目がヘッダとして処理
        /// </summary>
        /// <param name="csvPass">CSVファイルの参照先(ファイル名を含む)</param>
        /// <param name="data"></param>
        /// <returns>True:読み込みOK　False:失敗</returns>
        public static bool Read(string csvPass, out DataTable data)
        {
            return Read(FTString.GetString_Front(csvPass, @"\"), FTString.GetString_End(csvPass, @"\"), out data);
        }

        /// <summary>
        /// 一行目がヘッダとして処理
        /// </summary>
        /// <param name="csvDir">CSVファイルの参照先</param>
        /// <param name="csvFileName">ファイル名</param>
        /// <param name="data"></param>
        /// <returns>True:読み込みOK　False:失敗</returns>
        public static bool Read(string csvDir, string csvFileName, out DataTable data)
        {

            //接続文字列
            string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + csvDir + ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
            using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(conString))
            {

                string commText = "SELECT * FROM [" + csvFileName + "]";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(commText, con);

                //DataTableに格納する
                data = new DataTable();
                try
                {
                    da.Fill(data);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// 一行目もデータとして処理
        /// </summary>
        /// <param name="csvPass">CSVファイルの参照先(ファイル名を含む)</param>
        /// <param name="data"></param>
        /// <returns>True:読み込みOK　False:失敗</returns>
        public static bool ReadNonHeder(string csvPass, out DataTable data)
        {
            return ReadNonHeder(FTString.GetString_Front(csvPass, @"\"), FTString.GetString_End(csvPass, @"\"), out data);
        }

        /// <summary>
        /// 一行目もデータとして処理
        /// </summary>
        /// <param name="csvDir">CSVファイルの参照先</param>
        /// <param name="csvFileName">ファイル名</param>
        /// <param name="data"></param>
        /// <returns>True:読み込みOK　False:失敗(ファイルがひらけない)</returns>
        public static bool ReadNonHeder(string csvDir, string csvFileName, out DataTable data)
        {

            //接続文字列
            string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + csvDir + ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
            using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(conString))
            {

                string commText = "SELECT * FROM [" + csvFileName + "]";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(commText, con);

                //DataTableに格納する
                data = new DataTable();
                try
                {
                    da.Fill(data);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 一行目もデータとして処理
        /// </summary>
        /// <param name="FullPass">フルパス</param>
        /// <param name="data">データテーブル</param>
        /// <param name="cut">区切り文字</param>
        /// <returns>True:読み込みOK　False:失敗</returns>
        public static bool Read(string FullPass  , out DataTable data , char cut)
        {

            string csvDir = FTString.GetString_Front(FullPass, @"\");
            string csvFileName = FTString.GetString_End(FullPass, @"\");

            data = new DataTable();
            try
            {
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(csvDir))
                {
                    var dt = new DataTable();

                    // カラム作成  
                    for (int i = 0; i < 3; i++)
                    {
                        dt.Columns.Add("COL_" + i, typeof(string));
                    }

                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行を指定値に分けて配列に格納する
                        var values = line.Split(cut);
                        //DataTableに格納する

                        try
                        {
                            // 行データ追加  
                            var row = dt.NewRow();
                            for (int i = 0; i < values.Length; i++)
                            {
                                row[i] = values[i];
                            }
                            dt.Rows.Add(row);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
            }
            return true;

        }

        #endregion

        #region<関数 読み込み（改行＆カンマ区切りがないファイル）>

        /// <summary>
        /// 読み込み（改行＆カンマ区切りがないファイル）
        /// </summary>
        /// <param name="FullPass">ファイルパス</param>
        /// <param name="data">出力データ</param>
        /// <returns>True:読み込みOK False:失敗</returns>
        public static bool ReadCsv(string FullPass , out DataTable data )
        {
            data = new DataTable();
            try
            {
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(FullPass))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');

                        
                        while(data.Columns.Count < values.Length )
                        {
                            data.Columns.Add();
                        }
                        
                        var row = data.NewRow();
                        // 出力する
                        for (int i=0; i< values.Length; i++)
                        {
                            row[i] = values[i];
                        }
                        data.Rows.Add(row);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region<関数　 書き込み>

        /// <summary>
        /// 書き込む
        /// </summary>
        /// <param name="csvDir">CSVファイルの参照先</param>
        /// <param name="csvFileName">ファイル名</param>
        /// <param name="data">書き込むデータ</param>
        /// <returns></returns>
        public static bool Wire(string csvDir, string csvFileName, DataTable data)
        {

            //接続文字列
            string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + csvDir + ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
            using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(conString))
            {

                string commText = "SELECT * FROM [" + csvFileName + "]";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(commText, con);

                //DataTableに格納する
                data = new DataTable();
                try
                {
                    da.Update(data);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// DataTableの内容をCSVファイルに保存する(データ追加動作)
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="dc">データ配置方向</param>
        public static void ConvertDataTableToCsv(DataTable dt, string csvPath, FT.C.DirectionContens dc = DirectionContens.Horizontal)
        {
            if (dt == null) return;                     // データなし

            if (FT.C.DIR.FileUM_static(csvPath))
                ConvertDataTableToCsv(dt, csvPath, false, dc, true);         // CSVファイルがすでにある場合
            else
                ConvertDataTableToCsv(dt, csvPath, true, dc, true);         // ない場合
        }

        /// <summary>
        /// DataTableの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        /// <param name="append">
        /// データをファイルの末尾に追加するかどうかを判断します。
        /// ファイルが存在し、append が false の場合は、ファイルが上書きされます。
        /// ファイルが存在し、appendが true の場合は、データがファイルの末尾に追加されます。それ以外の場合は、新しいファイルが作成されます。</param>
        /// <param name="rowSize">行番号</param>
        /// <param name="dc">データ配置方向</param>
        public static void ConvertDataTableToCsv(DataTable dt, string csvPath, bool writeHeader, FT.C.DirectionContens dc, bool append = false, int rowSize = 0)
        {

            // ディレクトリを作成
            if (!FT.C.DIR.DirUM_static_CutFipePass(csvPath))
                FT.C.DIR.MakeFolder_CutFipePass(csvPath);

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(csvPath, append, enc))
            {
                switch (dc)
                {
                    case DirectionContens.Vertical:
                        throw new MissingMethodException();     // Todo未実装

                    case DirectionContens.Horizontal:
                    default:
                        Wire横(dt, writeHeader, rowSize, sr);
                        break;
                }

                //閉じる
                sr.Close();
            }
        }

        /// <summary>
        /// DataTableの内容をCSVファイルに書き込み（横方向）
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        /// <param name="rowSize">行番号</param>
        /// <param name="sr">ファイルストリーム</param>
        private static void Wire横(DataTable dt, bool writeHeader, int rowSize, System.IO.StreamWriter sr)
        {
            int colCount = dt.Columns.Count;
            int lastColIndex = colCount - 1;

            //ヘッダを書き込む
            if (writeHeader)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //レコードを書き込む
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row[i].ToString();
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            // 足りない個数を埋める
            int addrow = rowSize - dt.Rows.Count;
            if (addrow > 0)
            {
                for (int j = 0; j < addrow; j++)
                {

                    for (int i = 0; i < colCount; i++)
                    {
                        //フィールドの取得
                        string field = "-";
                        //"で囲む
                        field = EncloseDoubleQuotesIfNeed(field);
                        //フィールドを書き込む
                        sr.Write(field);
                        //カンマを書き込む
                        if (lastColIndex > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する
                    sr.Write("\r\n");
                }
            }
        }

        #endregion

        #region<関数　 ダブルクォート処理>

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private static bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

        #endregion

    }


    /// <summary>
    /// CSV使用インターフェース
    /// </summary>
    public interface ICSV
    {

        /// <summary>
        /// プロパティ　CSVファイル名(ファイルパスは含まない)
        /// </summary>
        string CsvFileName { get; set; }

        /// <summary>
        /// CSVに保存する
        /// </summary>
        /// <param name="FilePass">ファイル名</param>
        void SavaCSV(string FilePass);

    }

}
