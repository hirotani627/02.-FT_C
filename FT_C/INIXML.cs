using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;                   // トレース機能
using System.Xml;
using System.Xml.Serialization;

namespace FT.C
{
    /// <summary>
    /// XML保存クラス
    /// </summary>
    public  class INIXML :IDisposable
    {

        #region<内部変数>

        /// <summary>データ変数</summary>
        private string mFilePass;

        /// <summary>リソースが破棄(解放)されていることを表すフラグ</summary>
        private bool disposed = false;

        #endregion

        #region<プロパティ>

        /// <summary>
        /// ファイルパスプロパティ
        /// </summary>
        public string FilePass
        {
            get { return mFilePass; }
            set { mFilePass = value; }
        }

        #endregion

        #region<コンストラクタ>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public INIXML()
        {
        }

        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filePath">Iniファイルパス</param>
        public INIXML(string filePath)
        {
            mFilePass = filePath;
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

        #region<関数　オブジェクトをXMLファイルに保存する>

        /// <summary>
        /// オブジェクトをXMLファイルに保存する
        /// </summary>
        public void SaveObject<T>( object ob)
        {
            SaveObject<T>(mFilePass, ob);
        }

        /// <summary>
        /// オブジェクトをXMLファイルに保存する
        /// </summary>
        /// <param name="ob">保存データ</param>
        /// <param name="fullPass">保存ファイルパス（フルパス）</param>
        public static void SaveObject<T>(string fullPass , object ob)
        {
            if (fullPass == null)
                return;

            if (!FileUM(fullPass))
            {
                if (!FT.C.DIR.DirUM_static_CutFipePass(fullPass))
                    FT.C.DIR.MakeFolder_CutFipePass(fullPass);      // ディレクトリを作成

            }

            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            
            //書き込むファイルを開く（UTF-8 BOM無し）
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fullPass, false, new System.Text.UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, (T)ob);

                //ファイルを閉じる
                sw.Close();
            }
        }

        #endregion

        #region<関数　XMLファイルをオブジェクトに復元する>

        /// <summary>
        /// XMLファイルをオブジェクトに復元する(ファイルがなければ作る)
        /// </summary>
        /// <param name="ob">ファイルがない時のデータ</param>
        public object LoadObjectMake<T>( object ob)
        {
            return LoadObjectMake<T>(mFilePass, ob);
        }

        /// <summary>
        /// XMLファイルをオブジェクトに復元する(ファイルがなければ作る)
        /// </summary>
        /// <param name="ob">ファイルがない時のデータ</param>
        /// <param name="fullPass">保存ファイルパス（フルパス）</param>
        public static object LoadObjectMake<T>(string fullPass, object ob)
        //where  T : object
        {
            if (fullPass == null)
                return null;

            if (!FileUM(fullPass))
            {
                if (!FT.C.DIR.DirUM_static_CutFipePass(fullPass))
                    FT.C.DIR.MakeFolder_CutFipePass(fullPass);      // ディレクトリを作成

                T t = (T)ob;
                SaveObject<T>(fullPass, t);                         //　ファイルを作成
            }

            return LoadObject<T>(fullPass);
        }

        /// <summary>
        /// XMLファイルをオブジェクトに復元する
        /// </summary>
        /// <param name="fullPass">保存ファイルパス（フルパス）</param>
        public static object LoadObject<T>(string fullPass)
        {
            if (fullPass == null)
                return null;

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            //読み込むファイルを開く
            using (System.IO.StreamReader sr = new System.IO.StreamReader(fullPass, new System.Text.UTF8Encoding(false)))
            {

                object obj;
                if (sr.BaseStream.Length > 0)
                {
                    //XMLファイルから読み込み、逆シリアル化する
                    obj = serializer.Deserialize(sr);
                }
                else
                    obj = null;
                //ファイルを閉じる
                //sr.Close();

                //T t = (T)obj;
                return obj;
            }
        }

        #endregion

        #region<関数　ファイル有無を返す>

        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// <returns>true : 有</returns>
        public bool FileUM()
        {
            return FT.C.DIR.FileUM_static(mFilePass);
        }

        /// <summary>
        /// ファイル有無を返す
        /// </summary>
        /// <param name="fullPass">保存ファイルパス（フルパス）</param>
        /// <returns>true : 有</returns>
        public static bool FileUM(string fullPass)
        {
            return FT.C.DIR.FileUM_static(fullPass);
        }
       
        #endregion

        #region<関数　ディレクトリ（フォルダー）有無を返す>

        /// <summary>
        /// ディレクトリ（フォルダー）有無を返す
        /// </summary>
        /// <returns>true : 有</returns>
        public static bool DirUM(string dirName)
        {
            return FT.C.DIR.DirUM_static(dirName);
        }

        #endregion

       }

}
