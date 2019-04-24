using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FT.C
{
    /// <summary>
    /// 品種ファイル操作クラス
    /// </summary>
    public class FTRecipeFile
    {

        /// <summary>
        /// 品種名
        /// </summary>
        public string RecipeName { get; set; }


        /// <summary>
        /// フォルダー名
        /// </summary>
        public string FolderPass { get; set; }

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="recipeName"></param>
        /// <param name="folderPass"></param>
        public FTRecipeFile(string recipeName, string folderPass)
        {
            this.RecipeName = recipeName;
            this.FolderPass = folderPass;
        }

        /// <summary>
        /// コンストラクタ(最終を品種名とする)
        /// </summary>
        /// <param name="folderPass">品種名を含んだフォルダーパス</param>
        public FTRecipeFile( string folderPass)
        {
            this.RecipeName = FT.C.FTString.GetFileNameToFullPass(folderPass);
            this.FolderPass = folderPass;
        }

        #endregion

        #region<関数　保存されている品種名一覧を取得>

        /// <summary>
        /// 保存されている品種名一覧を取得
        /// </summary>
        /// <param name="fourTechnosPass">FourTechnosのPass</param>
        /// <returns></returns>
        string[] GetRecileNameList(string fourTechnosPass)
        {
            var mcPass = fourTechnosPass + FT.C.FTSystem.PassMachineData;
            return DIR.GetSubDirectory(mcPass);
        }

        #endregion

        #region<関数　現在の品種名取得(Machine.iniより)>

        /// <summary>
        /// 現在の品種名取得(Machine.iniより)
        /// </summary>
        public static string GetRecipieNameToMachineini(string Pass)
        {
            FT.C.INI.MakeFileWhenNoFile(Pass);      // ファイルがなければ作る

            // 装置設定ファイル名
            //using (var MachineInfo = new FT.C.INI(C.GetFourTechnos_Folder() + C.FOLDER_MACHINE + C.FILE_MACHINE))
            using (var MachineInfo = new FT.C.INI(Pass))
            {
                return MachineInfo.GetStringMake("Machine", "RecipieName","FT_Test");
            }
        }

        #endregion

        #region<関数　文字表現>

        /// <summary>
        /// 文字表現
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var st = new System.Text.StringBuilder();
            st.AppendLine("RecipeName：" + RecipeName);
            return st.ToString();
        }

        #endregion

        #region<関数 品種ルートフォルダー>

        /// <summary>
        /// 品種のルートフォルダーを作成する
        /// </summary>
        /// <param name="FourTechnosFolderPass">FourTechnosフォルダーを含んだファイルパス</param>
        public static void MekeRecipeRootFolder(string FourTechnosFolderPass)
        {
            string s = FourTechnosFolderPass + FTSystem.PassPecipeData;
            FT.C.DIR.MakeFolder(s);

        }


        /// <summary>
        /// 品種ルートフォルダーを取得
        /// </summary>
        /// <param name="FourTechnosFolderPass">FourTechnosフォルダーを含んだファイルパス</param>
        /// <returns></returns>
        public static string GetRecipeRootFolder(string FourTechnosFolderPass)
        {
            string s = FourTechnosFolderPass + FTSystem.PassPecipeData;
            return s;
        }

        #endregion

        #region<関数 品種名フォルダーを含んだファイルパスを取得>

        /// <summary>
        /// 品種名フォルダーを含んだファイルパスを取得
        /// </summary>
        /// <param name="FourTechnosFolderPass">Fourtechnosフォルダーを含んだファイルパス</param>
        /// <param name="RecipeName">品種名</param>
        /// <returns></returns>
        public static string GetRecipeNamePass(string RecipeName, string FourTechnosFolderPass)
        {
            return FourTechnosFolderPass + FT.C.FTSystem.PassPecipeData + @"\" + RecipeName;
        }

        #endregion

    }

}
