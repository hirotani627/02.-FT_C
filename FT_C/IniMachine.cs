using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FT.C;
using System.Diagnostics;                   // トレース機能

namespace FT.NICHIA
{
    /// <summary>
    /// Machine.iniファイル
    /// </summary>
    public class IniMachine
    {

        #region<定数　ini項目>

        /// <summary>iniセクション</summary>           
        const string csSecsion = "Machine";

        /// <summary>ini項目　装置名</summary>
        const string csPass_MachineName = "MachineName";

        /// <summary>ini項目　装置番号</summary>
        const string csPass_MachineNo = "MachineNo";

        /// <summary>ini項目　言語</summary>
        const string csPass_Language = "Language";

        /// <summary>ini項目　品種名</summary>
        const string csPass_RecipieName = "RecipieName";

        #endregion

        #region<定数　フォルダーパス>

        /// <summary>データ名</summary>            
        private const string F_Name = @"\Machine.ini";

        /// <summary>パス</summary>            
        private const string F_Pass = @"\Fourtechnos\DATA_Machine";
        
        #endregion

        #region<変数>

        /// <summary>アプリケーションデータiniファイル</summary>            
        private INI iniAppData;

        #endregion

        #region<プロパティ>

        /// <summary>
        /// プロパティ 装置名
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// プロパティ 装置番号
        /// </summary>
        public int MachineNo { get; set; }

        /// <summary>
        /// プロパティ 言語
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// プロパティ 品種名
        /// </summary>
        public string RecipieName { get; set; }

        #endregion

        #region<コンストラクタ>

        /// <summary>コンストラクタ</summary>
        /// <param name="MyDocmentPass">マイドキュメントのパス</param>
        public IniMachine(string MyDocmentPass)
        {

            iniAppData = new INI(MyDocmentPass + F_Pass + F_Name);
            if (!iniAppData.FileUN())
                Trace.WriteLine(MyDocmentPass + F_Pass + "に" + F_Name +　"がありません");
                //iniAppData.MakeFile();
                
        }

        #endregion

        #region<関数　データロード>

        /// <summary>
        /// INIファイルからデータのロード
        /// </summary>
        /// <remarks>項目がなければ作る</remarks>
        public void Load()
        {

            // 装置名
            MachineName = iniAppData.GetString(csSecsion, csPass_MachineName);

            // 装置番号
            MachineNo = iniAppData.Getint(csSecsion, csPass_MachineNo,-1);

            // 言語
            Language = iniAppData.Getint(csSecsion, csPass_Language, 0);

            // 品種名
            Load_RecipieName();

        }

        /// <summary>
        /// INIファイルからデータのロード(品種名)
        /// </summary>
        public void Load_RecipieName()
        {
            // 品種名
            RecipieName = iniAppData.GetString(csSecsion, csPass_RecipieName);
        }

        #endregion

        #region<関数　ファイル有無>

        /// <summary>
        /// ファイル有無
        /// </summary>
        /// <returns></returns>
        public bool FileUN()
        {
            if (iniAppData != null)
                return iniAppData.FileUN();

            return false;
        }

        #endregion

    }
}
