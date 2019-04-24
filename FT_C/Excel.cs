using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace FT.C
{
    /// <summary>
    /// エクセルデータ接続クラス
    /// </summary>
    public class Excel
    {
        /// <summary>
        /// エクセルデータ(未完成)
        /// </summary>
        public void Test04()
        {
            OleDbConnection conn = new OleDbConnection();

            conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\test.xls;Extended Properties=Excel 8.0;";

            // 接続します。
            conn.Open();

            // 接続を解除します。
            conn.Close();
        }

    }
}
