using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.C
{
    /// <summary>
    /// メールクラス（ネットからのコピペ）
    /// </summary>
    class FTMail
    {

            // try
            //{
            //    // 送信先を,や;区切りで複数指定されていれば分割し、mailTo配列に入れる
            //    string[] separator = { ",;" };
            //    string[] mailTo = textBoxSendTo.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
 
            //    // 送信先が１件でもあれば
            //    if (mailTo.Length >= 1)
            //    {
            //        MailMessage msg = new MailMessage(
            //            "from@from.com",        // 差出人Fromアドレス
            //            mailTo[0],              // 送信先アドレス
            //            textBoxTitle.Text,      // タイトル名
            //            textBoxMessage.Text);   // メッセージ
 
            //        // ヘッダを追加する時はこんな感じで（任意）
            //        msg.Headers.Add("X-Mailer", "hogehoge");
 
            //        // 文字コードを設定
            //        Encoding enc = Encoding.GetEncoding("iso-2022-jp");
            //        msg.SubjectEncoding = enc;
            //        msg.BodyEncoding = enc;
 
            //        // 送信先が２件以上あればCCに追加
            //        if (mailTo.Length >= 2)
            //        {
            //            foreach (string to in mailTo)
            //                msg.CC.Add(to);
 
            //            // 最初のアドレスはToと重複なので削除
            //            msg.CC.RemoveAt(0);
            //        }
 
            //        // SMTPサーバーのアドレスとポート
            //        SmtpClient sc = new SmtpClient("pop.hogehoge.jp", 25);
 
            //        // IDとPassword
            //        sc.Credentials = new NetworkCredential("id", "password");
 
            //        // 暗号化のためのSSLを利用するか？
            //        sc.EnableSsl = false;
 
            //        // 呼び出しがタイムアウトになるまでの時間（ミリ秒）
            //        sc.Timeout = 10000;
 
            //        // メール送信
            //        sc.Send(msg);
 
            //        // リソース解放
            //        msg.Dispose();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}

    }
}
