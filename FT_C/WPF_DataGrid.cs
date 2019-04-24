using System.Windows.Controls;
using System.Windows.Data;

namespace FT.C
{
    /// <summary>
    /// 
    /// </summary>
    public class WPF_DataGrid
    {
        /// <summary>
        /// テーブルの初期化
        /// </summary>
        /// <remarks>
        ///  DataGrid x:Name="dataGrid" AutoGenerateColumns="False" DataContext="{Binding}" ItemsSource="{Binding}" PreviewDragOver="dataGrid_PreviewDragOver" Drop="dataGrid_Drop"   
        ///  </remarks>
        public static void SetTables(System.Data.DataTable dt, ref System.Windows.Controls.DataGrid dataGrid)
        {

            if (null == dt) return;

            // 項目クリア
            dataGrid.Columns.Clear();

            // 水平スクロールバー
            dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            // 垂直スクロールバー
            dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            // 行の追加＆バインド定義
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string sel = "F" + (i + 1).ToString();
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = sel, IsReadOnly = false, FontSize = 12, Binding = new Binding(sel) });
            }

            // グリッドにバインド
            dataGrid.DataContext = dt;

        }
    }
}
