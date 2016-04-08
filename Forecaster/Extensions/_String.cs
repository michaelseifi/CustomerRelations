using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace daisybrand.forecaster.Extensions
{
    public static class _String
    {
        public static string NullIfEmptyOrNull(this string text)
        {
            return string.IsNullOrEmpty(text) ? null : text;
        }

        public static string TrimLeadingZeros(this string text)
        {
            int i = 0;
            if (int.TryParse(text, out i))
                return i.ToString();
            return string.Empty;
        }

        public static string ToZeroIfEmpty(this object text)
        {
            if (text.ToString() == string.Empty) { return "0"; }
            return text.ToString();
        }

        public static DateTime? ToDate(this object text)
        {
            try
            {
                var date = Convert.ToDateTime(text);
                return date;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static Controlers.Objects.CellComment.Field ToField(this string s)
        {
            return (Controlers.Objects.CellComment.Field)Enum.Parse(typeof(Controlers.Objects.CellComment.Field), s);
        }

        public static bool IsInt(this string s)
        {
            try
            {
                var i = int.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ContainsAny(this string s, string[] array)
        {
            foreach (var str in array)
            {
                if (s.Contains(str)) return true;
            }
            return false;
        }

        public static string ToNullIfEmpty(this string s)
        {
            if (s == "") return null;
            return s;
        }

        public static string OperatorParse(this string s)
        {
            if (s == "") return s;
            if (s.Substring(0, 1) == "=")
            {
                
                var a = s.Substring(1);
                try
                {
                    return Evaluate(a).ToString();
                }
                catch
                {
                    var mult = a.IndexOf('*');
                    var sub = a.IndexOf('-');
                    var div = a.IndexOf('/');
                    var add = a.IndexOf('+');
                    if (mult > 0)
                    {
                        var value1 = a.Substring(0, mult);
                        var value2 = a.Substring(mult + 1);
                        if (value1.IsDecimal() && value2.IsDecimal())
                            return (Convert.ToDecimal(value1) * Convert.ToDecimal(value2)).ToString();
                    }
                    else if (div > 0)
                    {

                        var value1 = a.Substring(0, div);
                        var value2 = a.Substring(div + 1);
                        if (value1.IsDecimal() && value2.IsDecimal())
                            return (Convert.ToDecimal(value1) / Convert.ToDecimal(value2)).ToString();
                    }
                    else if (sub > 0)
                    {
                        var value1 = a.Substring(0, sub);
                        var value2 = a.Substring(sub + 1);
                        if (value1.IsDecimal() && value2.IsDecimal())
                            return (Convert.ToDecimal(value1) - Convert.ToDecimal(value2)).ToString();
                    }
                    else if (add > 0)
                    {
                        var value1 = a.Substring(0, add);
                        var value2 = a.Substring(add + 1);
                        if (value1.IsDecimal() && value2.IsDecimal())
                            return (Convert.ToDecimal(value1) + Convert.ToDecimal(value2)).ToString();
                    }
                    else if (a.IsDecimal())
                    {
                        return a;
                    }

                    throw new FormatException("Cannot parse the text to a mathematical operation.");
                }
            }            
            else
            {
                return s;
            }
        }

        public static double Evaluate(string expression)
        {
            using (DataTable table = new DataTable())
            {
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }
        }

        public static bool IsDecimal(this string s)
        {
            try
            {
                var value = Convert.ToDecimal(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int ToInt(this string s)
        {
            return decimal.ToInt32(Convert.ToDecimal(s));
        }

        public static string FilterWeekId(this string s)
        {
            var index = s.IndexOf(":");
            if (index > -1)
                return s.Substring(index + 1);
            return s;
        }

        public static string FilterComma(this string s)
        {
            return s.Replace(",", "");
        }
    }
}
