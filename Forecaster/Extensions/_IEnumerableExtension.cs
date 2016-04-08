using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using daisybrand.forecaster.Controlers.Interfaces;

namespace daisybrand.forecaster.Extensions
{
    public static class _IEnumerableExtension
    {
   
        
        //public static IEnumerable<T> AddIfEmpty<T>(this IEnumerable<T> list, Func<T, bool> predicate, object objToAdd)
        //{
        //    T obj = (T)objToAdd;
            
        //    if (!predicate(obj))
        //        return list.Add(obj);      
        //    return list;
        //}

        //public static double GetQSStdDev(this IEnumerable<IDailyData> list)
        //{
        //    var average = list.Average(x => x.QS);
        //    var sum = list.Sum(x => Math.Pow(x.QS - average, 2));            
        //    return Math.Sqrt((sum) / (MainWindow.myFirst120Data.Count() - 1));
        //}

        //public static void ExcludeCurrentWeek(this Controlers.Collections.DataCollection data)
        //{
        //    foreach (var item in data)
        //        if (item.REPORT_AS_OF_DATE < data.LASTDAYOFLASTWEEKWITHREALDATA)
        //            data.Remove(item);
        //}

        //public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T item)
        //{
        //    var list = enumerable.ToList();
        //    list.Add(item);
        //    return list;
        //}

        public static void Update<T>(this IEnumerable<T> enumerable, Action<T> updator)
        {
            foreach (var item in enumerable)
                updator(item);
        }

        public static Dictionary<int, double> ZScore(this IEnumerable<int> list)
        {
            var temp = new Dictionary<int, double>();
            var average = list.Average();
            foreach (var item in list.Distinct())
            {
                var cal = (1 / Math.Sqrt(2 * Math.PI)) * Math.Exp(-(Math.Pow(item - average, 2)) / (2 * average.GetStdDev()));
                //var cal = (item - average) / average.GetStdDev();
                temp.Add(item, cal);
            }
            return temp;
        }
        public static decimal Median(this IEnumerable<int> list)
        {
            var temp = list.ToArray();
            Array.Sort(temp);

            var count = temp.Length;
            if (count == 0)
                throw new InvalidOperationException("Empty collection");
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                int a = temp[count / 2 - 1];
                int b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }

        public static double GetVarianceP(this IEnumerable<int> list)
        {
            var average = list.Average();
            var sum = list.Sum(x => Math.Pow(x - average, 2));
            return sum / list.Count();//Math.Sqrt((sum) / (list.Count()));
        }

        public static double GetVarianceS(this IEnumerable<int> list)
        {
            var average = list.Average();
            var sum = list.Sum(x => Math.Pow(x - average, 2));
            return sum / (list.Count() - 1); //Math.Sqrt((sum) / (list.Count()));
        }

        public static double GetStdDev(this int variance)
        {
            return Math.Sqrt(variance);
        }

        public static double GetVolatility(this double stdDev, double average)
        {
            return stdDev / average; 
            // 100 * (stdDev / average);
            // this number will turn to percentage once passed through toPercentageConverter
        }

        //public static double GetVarianceP(this IEnumerable<double> list)
        //{
        //    var average = list.Average();
        //    var sum = list.Sum(x => Math.Pow(x - average, 2));
        //    return sum / list.Count();//Math.Sqrt((sum) / (list.Count()));
        //}

        //public static double GetVarianceS(this IEnumerable<double> list)
        //{
        //    var average = list.Average();
        //    var sum = list.Sum(x => Math.Pow(x - average, 2));
        //    return sum / (list.Count() - 1); //Math.Sqrt((sum) / (list.Count()));
        //}

        public static string ConvertStringArrayToStringWithAPipe(this IEnumerable<string> array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append('|');
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static double GetStdDev(this double variance)
        {
            return Math.Sqrt(variance);
        }
    }
}
