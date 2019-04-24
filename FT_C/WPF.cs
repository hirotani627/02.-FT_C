using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace FT.C
{

    /// <summary>
    /// 数値判定ルール
    /// </summary>
    public class NumberValidationRule : ValidationRule
    {
        /// <summary>最小値</summary>
        public double MinValue { get; set; }

        /// <summary>最大値</summary>
        public double MaxValue { get; set; }

        /// <summary>空データを許可しない</summary>
        public bool NotEmpty { get; set; }

        /// <summary>メッセージ</summary>
        public string MessageHeader { get; set; }

        /// <summary>言語 プロパティ</summary>
        public FT.C.ENM.Lang Lang { get; set; } = ENM.Lang.Jpn;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumberValidationRule()
        {
            MinValue = double.MinValue;
            MaxValue = double.MaxValue;
            NotEmpty = false;
            MessageHeader = null;
        }

        /// <summary>
        /// ルール
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="notEmpty"></param>
        /// <param name="msgHeader"></param>
        public NumberValidationRule(double min, double max, bool notEmpty = false, string msgHeader = null)
        {
            MinValue = min;
            MaxValue = max;
            NotEmpty = notEmpty;
            MessageHeader = msgHeader;
        }

        /// <summary>
        /// 判定
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (null == value)
            {
                if (NotEmpty)
                {
                    var msg = "Fill in the value.";
                    if (!string.IsNullOrWhiteSpace(MessageHeader))
                        msg = MessageHeader + ":" + msg;
                    return new ValidationResult(false, msg);
                }
                else
                    return ValidationResult.ValidResult;
            }
            string str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                if (NotEmpty)
                {
                    var msg = "Fill in the value.";
                    if (!string.IsNullOrWhiteSpace(MessageHeader))
                        msg = MessageHeader + ":" + msg;
                    return new ValidationResult(false, msg);
                }
                else
                    return ValidationResult.ValidResult;
            }

            double ret;
            if (!double.TryParse(str, out ret))
            {
                var msg = "Fill in the value.";
                if (!string.IsNullOrWhiteSpace(MessageHeader))
                    msg = MessageHeader + ":" + msg;
                return new ValidationResult(false, msg);
            }
            if ((MinValue > ret) || (MaxValue < ret))
            {
                var msg = "Value out of range.(" + MinValue + "～" + MaxValue + ")";
                if (!string.IsNullOrWhiteSpace(MessageHeader))
                    msg = MessageHeader + ":" + msg;
                return new ValidationResult(false, msg);
            }

            return ValidationResult.ValidResult;
        }
    }



    /// <summary>
    /// コンバータクラス(Enum - bool)
    /// </summary>
    public class EnumRadioConverter : System.Windows.Data.IValueConverter
    {
        /// <summary>
        /// Enum → bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // バインドする列挙値の文字列表記がパラメータに渡されているか
            var paramString = parameter as string;
            if (paramString == null)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }

            // パラメータが Enum の値として正しいか
            if (!Enum.IsDefined(value.GetType(), paramString))
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }

            // パラメータを Enum に変換
            var paramParsed = Enum.Parse(value.GetType(), paramString);

            // 値が一致すれば true を返す
            var result = (value.Equals(paramParsed));
            return result;
        }

        /// <summary>
        /// bool → Enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // バインドする列挙値の文字列表記がパラメータに渡されているか
            var paramString = parameter as string;
            if (paramString == null)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }

            // チェックが入っているか?
            if (value == null)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }


            var isChecked = (bool)value;
            if (!isChecked)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }

            // 列挙型にパースして返す
            var e = Enum.Parse(targetType, paramString);
            return e;
        }
    }



    /// <summary>
    /// 表示変換コンバータ
    /// </summary>
    [System.Windows.Data.ValueConversion(typeof(bool), typeof(Visibility))]
    public class DispValueConverter : System.Windows.Data.IValueConverter
    {

        /// <summary>
        /// モデル → UIコントロール
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">IsCollapsed(描画領域も消す)</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var a = (bool)value;

            if (a)
            {
                return Visibility.Visible;
            }
            else
            {
                var b = parameter as bool?;
                if(b != null)
                    return Visibility.Collapsed;

                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// モデル ← UIコントロール
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="paramete"></param>
        /// <param name="culturer"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object paramete, System.Globalization.CultureInfo culturer)
        {
            var a = (Visibility)value;
            switch (a)
            {
                case Visibility.Visible:
                    return true;

                default:
                case Visibility.Hidden:
                    return false;
            }
        }
    }

}
