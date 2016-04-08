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
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Volatility.xaml
    /// </summary>
    public partial class Volatility : UserControl
    {
        ITab TAB { get; set; }
        /// <summary>
        /// DECLARE THIS IF THIS USERCONTROL IS THE SOLE CONTENT OF THE WINDOW
        /// </summary>
        public Window WIN { get; set; }
        bool IsLoading { get; set; }
        public Volatility(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            TAB.myGraphViewModel.VOLATILITY_NUMBER_OF_WEEKS = 17;
            //if (MainWindow.myGraphViewModel != null)
            //    MainWindow.myGraphViewModel.VOLATILITY_USERCONTROLS.Add(this);
            
            //this.Unloaded += (s, e) =>
            //{
            //    if (MainWindow.myGraphViewModel != null)
            //        MainWindow.myGraphViewModel.VOLATILITY_USERCONTROLS.Remove(this);
            //};
        }

        //private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    MainWindow.SetBusyState(true);
        //}

        private void NUMBER_OF_WEEKS_BTN_Click(object sender, RoutedEventArgs e)
        {
            var text = NUMBER_OF_WEEKS_TXB.Text;
            int value = 0;
            if (text.Length == 0 || !int.TryParse(text, out value) || value < 2 || value > 78)
            {
                LogManger.RaiseErrorMessage(new daisybrand.forecaster.Controlers.Objects.Message { MESSAGE = "Enter a valid number greater between 2 and 78." });
                NUMBER_OF_WEEKS_TXB.Text = TAB.myGraphViewModel.VOLATILITY_NUMBER_OF_WEEKS.ToString();
                NUMBER_OF_WEEKS_TXB.Focus();
                return;
            }
            TAB.myGraphViewModel.VOLATILITY_NUMBER_OF_WEEKS = int.Parse(NUMBER_OF_WEEKS_TXB.Text);
        }

        private void NUMBER_OF_WEEKS_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            //var tbx = sender as TextBox;
            //int value = 0;
            //if (tbx.Text.Length == 0) { }
            //else if (!int.TryParse(tbx.Text, out value) || value < 2 || value > 78)
            //{
            //    Log.RaiseErrorMessage(new daisybrand.forecaster.Controlers.Objects.Message { MESSAGE = "Enter a valid number greater between 2 and 78." });
            //    NUMBER_OF_WEEKS_TXB.Text = MainWindow.myGraphViewModel.VOLATILITY_NUMBER_OF_WEEKS.ToString();
            //}
            
        }

        private void NUMBER_OF_WEEKS_TXB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var focused = Keyboard.FocusedElement;
            if (focused != null && focused.GetType().Equals(typeof(Button)))
            {
                var btn = (Button)focused;
                if (btn.Content.ToString() == "GO")
                {   
                    return;
                }
            }
            (sender as TextBox).Text = TAB.myGraphViewModel.VOLATILITY_NUMBER_OF_WEEKS.ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
                this.DataContext = TAB.myGraphViewModel;
            IsLoading = true;
            if (WIN != null)
            {
                WIN.LocationChanged += WIN_LocationChanged;
                if (MainWindow.mySettings != null && MainWindow.mySettings.VOLATILITY != null)
                {
                    WIN.Top = MainWindow.mySettings.VOLATILITY.TOP;
                    WIN.Left = MainWindow.mySettings.VOLATILITY.LEFT;
                }
            }
        }

        void WIN_LocationChanged(object sender, EventArgs e)
        {
            if (IsLoading) { IsLoading = false; return; }
            var win = sender as Window;
            if (MainWindow.mySettings != null)
            {
                if (MainWindow.mySettings.VOLATILITY == null) MainWindow.mySettings.VOLATILITY = new Controlers.Objects.Settings.Volatility();
                MainWindow.mySettings.VOLATILITY.TOP = win.Top;
                MainWindow.mySettings.VOLATILITY.LEFT = win.Left;
            }
        }
    }
}
