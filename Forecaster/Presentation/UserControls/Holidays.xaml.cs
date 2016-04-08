using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Collections;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Holidays.xaml
    /// </summary>
    public partial class Holidays : UserControl
    {

        public Presentation.Views.Exceptions OWNER { get; set; }
        public Holidays()
        {
            InitializeComponent();
            this.DataContext = MainWindow.myHolidays;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var tag = new Guid(btn.Tag.ToString());
            var coll = MainWindow.myHolidays;
            var hol = coll.Where(x => x.HOLIDAYID == tag).FirstOrDefault();
            hol.Delete();
            coll.Remove(hol);
        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            if (DESCRIPTION.Text != string.Empty && STARTDATE.SelectedDate != null && ENDDATE.SelectedDate != null)
            {
                var hol = new Holiday
                {
                    DESCRIPTION = DESCRIPTION.Text,
                    START = (DateTime)STARTDATE.SelectedDate,
                    END = (DateTime)ENDDATE.SelectedDate,
                    HOLIDAYID = Guid.NewGuid()
                };
                hol.Insert();                
                MainWindow.myHolidays.Add(hol);
            }
        }

        private void CLOSE_Click(object sender, RoutedEventArgs e)
        {
            if(OWNER != null) OWNER.Close();
        }
    }
}
