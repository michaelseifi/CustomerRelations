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

namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : UserControl
    {
        /// <summary>
        /// DECLARE THIS IF THIS USERCONTROL IS THE SOLE CONTENT OF THE WINDOW
        /// </summary>
        public Window WIN { get; set; }
        bool IsLoading { get; set; }
        public Calculator()
        {

            InitializeComponent();
 
        }

        void WIN_LocationChanged(object sender, EventArgs e)
        {
            if (IsLoading) { IsLoading = false; return; }
            var win = sender as Window;
            if (MainWindow.mySettings != null)
            {
                if (MainWindow.mySettings.CALCULATOR == null) MainWindow.mySettings.CALCULATOR = new Controlers.Objects.Settings.Calculator();
                MainWindow.mySettings.CALCULATOR.TOP = win.Top;
                MainWindow.mySettings.CALCULATOR.LEFT = win.Left;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            if (WIN != null)
            {
                WIN.LocationChanged += WIN_LocationChanged;
                if (MainWindow.mySettings != null && MainWindow.mySettings.CALCULATOR != null)
                {
                    WIN.Top = MainWindow.mySettings.CALCULATOR.TOP;
                    WIN.Left = MainWindow.mySettings.CALCULATOR.LEFT;
                }
            }

        }
    }
}
