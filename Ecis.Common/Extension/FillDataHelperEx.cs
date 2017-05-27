using System;
using System.Windows.Forms;

namespace ZMH.Common.CommonHelper
{
    /// <summary>
    /// Fill Data Helper
    /// Author: Minghua
    /// </summary>
    public static class FillDataHelperEx
    {
        public static string GetTxtIfNotEmpty<T>(this T txtBox, Predicate<T> input) where T : TextBoxBase
        {
            if (input.Invoke(txtBox))
            {
                return txtBox.Text.TrimEnd();
            }
            else
            {
                return null;
            }
        }

        public static string GetTxtIfNotEmpty<T>(this T txtBox) where T : TextBoxBase
        {
            Predicate<T> input = p => !string.IsNullOrWhiteSpace(p.Text);
            return GetTxtIfNotEmpty(txtBox, input);
        }

        public static string GetControlTxtIfNotEmpty<T>(this T txtBox, Predicate<T> input) where T : Control
        {
            if (input.Invoke(txtBox))
            {
                return txtBox.Text.TrimEnd();
            }
            else
            {
                return null;
            }
        }

        public static string GetControlTxtIfNotEmpty<T>(this T txtBox) where T : Control
        {
            Predicate<T> input = p => !string.IsNullOrWhiteSpace(p.Text);
            return GetControlTxtIfNotEmpty(txtBox, input);
        }

        public static DateTime? GetDateTimeIfNotEmpty<T>(this T dtPicker, Predicate<T> input) where T : DateTimePicker
        {
            if (input.Invoke(dtPicker))
            {
                return dtPicker.Value;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? GetDateTimeIfNotEmpty<T>(this T dtPicker) where T : DateTimePicker
        {
            Predicate<T> input = p => !string.IsNullOrWhiteSpace(p.Text);
            return GetDateTimeIfNotEmpty(dtPicker, input);
        }
    }
}