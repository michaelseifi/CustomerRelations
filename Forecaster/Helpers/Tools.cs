using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster
{
    public static class Tools
    {
        public static Window GetParentWindowOfBorder(DependencyObject child)
        {
            DependencyObject parentObj = VisualTreeHelper.GetParent(child);
            if (child.GetType().Equals(typeof(System.Windows.Controls.Border)))
                parentObj = ((System.Windows.Controls.Border)child).Parent;
            if (parentObj == null)
                return null;
            Window parent = parentObj as Window;
            if (parent != null)
                return parent;
            return GetParentWindowOfBorder(parentObj);
        }

        public static Window GetParentWindow(DependencyObject child)
        {
            if (child.GetType().Equals(typeof(System.Windows.Window))) return child as Window;
            DependencyObject parentObj = VisualTreeHelper.GetParent(child);
            if (parentObj == null)
                if (child.GetType().BaseType.Equals(typeof(System.Windows.Controls.UserControl)))
                    parentObj = ((UserControl)child).Parent;
            if (parentObj == null) return null;
            Window parent = parentObj as Window;
            if (parent != null)
                return parent;
            return GetParentWindow(parentObj);
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static string GerResourceTextFile(string fileName)
        {
            string result = "";
            try
            {
                using (Stream stream = Application.Current.MainWindow.GetType().Assembly.GetManifestResourceStream("daisybrand.forecaster.Config." + fileName))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
            return result;
        }

        /// <summary>
        /// <para>CHECKS TO SEE IF THE SYSTME IS IN EDIT MODE.</para>
        /// <para>RETURNS FALSE IF IN READ ONLY MODE.</para>
        /// </summary>
        /// <returns></returns>
        public static bool IsInEditMode([CallerMemberName] string memberName = "")
        {
            if (MainWindow.myMainWindowViewModel != null)
                return !MainWindow.myMainWindowViewModel.IsReadOnly;
            else
            {
                if (!string.IsNullOrEmpty(memberName))
                    LogManger.Insert("EDITING", String.Format("Calling {0} while in edit mode", memberName));
                return true;
            }
        }

        public static List<DayOfWeek> GetDaysOfWeek(string startDay, string endDay)
        {
            List<DayOfWeek> list = new List<DayOfWeek>();
            var start = startDay.GetDayOfWeek(DayOfWeek.Monday);
            var end = endDay.GetDayOfWeek(DayOfWeek.Friday);

            var startInt = (int)start;
            var endInt = (int)end;

            for (int i = startInt; i <= endInt; i++)
            {
                var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
                foreach (var day in days)
                {
                    if ((int)day == i)
                    {
                        list.Add(day);
                        break;
                    }
                } 
                
            }
            return list;
        }

        public static List<DateTime> GetDates(List<DayOfWeek> days, DateTime lastDate)
        {
            List<DateTime> list = new List<DateTime>();
            var count = days.Count();
            for (int i = count - 1; i >= 0; i--)
            {
                var day = days[i];
                if (day == lastDate.DayOfWeek)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        list.Add(lastDate.AddDays(j - i));
                    }
                }
            }
            return list;
        }

        public static List<DateTime> GetWeekDates(DateTime startDate)
        {
            List<DateTime> list = new List<DateTime>(7);
            
            DateTime sunday = DateTime.Now;
            for (int i = 0; i < 7; i++ )
                if (startDate.AddDays(-i).DayOfWeek == DayOfWeek.Sunday)
                {
                    sunday = startDate.AddDays(-i);
                    break;
                }

            for (int i = 0; i < 7; i++)
            {
                var d = sunday.AddDays(i);
                list.Add(new DateTime(d.Year, d.Month, d.Day));
            }

            return list;
        }

        public static List<DateTime> GetWeekDates(DateTime start, DateTime end)
        {
            List<DateTime> list = new List<DateTime>(7);

            DateTime sunday = DateTime.Now;
            var span = new TimeSpan(end.Ticks - start.Ticks);
            var cnt = span.Days +  1;
            for (int i = 0; i < 7; i++)
                if (start.AddDays(-i).DayOfWeek == DayOfWeek.Sunday)
                {
                    cnt += i;
                    sunday = start.AddDays(-i);
                    break;
                }

            for (int i = 0; i < cnt; i++)
            {
                var d = sunday.AddDays(i);
                list.Add(new DateTime(d.Year, d.Month, d.Day));
                i += 6;
            }

            return list;
        }
        #region async wait
        public static Task<bool> WaitTask(int seconds)
        {
            return Task.Run<bool>(() => _Wait(seconds));
        }

        /// <summary>
        /// PAUSE FOR parameter SECONDS
        /// </summary> 
        public static void Wait(int seconds)
        {
            _Wait(seconds);
        }

        private static bool _Wait(int seconds)
        {
            DateTime later = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < later)
            {

            }
            return true;
        }
        #endregion
    }
}
