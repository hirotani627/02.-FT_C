﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{

    /// <summary>
    /// ユーザー権限　インターフェースa
    /// </summary>
    public interface IAuthority
    {
        /// <summary>権限設定　プロパティ</summary>
        /// <remarks>指定の権限以上でなければコントロールを無効にする</remarks>
        FT.C.ENM.Authority Authority { get; set; }

        /// <summary>権限あり プロパティ</summary>
        bool Authoritative { get; set; }
    }


    /// <summary>
    /// 編集権限　インターフェイス
    /// </summary>
    public interface IEditLevel
    {
        /// <summary>権限あり プロパティ</summary>
        bool EditLevel { get; set; }
    }

    /// <summary>
    /// 権限クラス
    /// </summary>
    public static class Authority
    {

        #region<関数　変換　コンバート>

        /// <summary>
        /// ユーザ権限値からint値に変換
        /// </summary>
        /// <param name="Value">ユーザ権限値</param>
        /// <returns>int値</returns>
        public static int CInt(ENM.Authority Value)
        {

            int iValue = 0;

            switch (Value)
            {
                case ENM.Authority.Operator: iValue = 0; break;
                case ENM.Authority.Engineer: iValue = 1; break;
                case ENM.Authority.Admin: iValue = 2; break;
                case ENM.Authority.FT: iValue = 3; break;
            }

            return iValue;

        }


        /// <summary>
        /// int値からユーザ権限値に変換
        /// </summary>
        /// <param name="Value">int値</param>
        /// <returns>ユーザ権限値</returns>
        public static ENM.Authority CAuthority(int Value)
        {

            ENM.Authority eAuth = ENM.Authority.Operator;

            switch (Value)
            {
                case (int)0: eAuth = ENM.Authority.Operator; break;
                case (int)1: eAuth = ENM.Authority.Engineer; break;
                case (int)2: eAuth = ENM.Authority.Admin; break;
                case (int)3: eAuth = ENM.Authority.FT; break;
            }

            return eAuth;
        }


        /// <summary>
        /// string値からユーザ権限値に変換
        /// </summary>
        /// <param name="Value">string値	</param>
        /// <returns>ユーザ権限値</returns>
        public static ENM.Authority CAuthority(string Value)
        {
            ENM.Authority eAuth = ENM.Authority.Operator;

            switch (Value)
            {
                case "0": eAuth = ENM.Authority.Operator; break;
                case "1": eAuth = ENM.Authority.Engineer; break;
                case "2": eAuth = ENM.Authority.Admin; break;
                case "3": eAuth = ENM.Authority.FT; break;
            }

            return eAuth;
        }

        #endregion

    }

}
