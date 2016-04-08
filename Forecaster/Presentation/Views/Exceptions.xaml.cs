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
using System.Windows.Shapes;
using daisybrand.forecaster.Helpers;

namespace daisybrand.forecaster.Presentation.Views
{
    /// <summary>
    /// Interaction logic for Exceptions.xaml
    /// </summary>
    public partial class Exceptions : Window
    {
        public object CONTENT { get; set; }
        public Exceptions()
        {
            var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            this.MaxHeight = screen.Bounds.Height - 20;
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            //CONTENT = new UserControls.CustomerExceptions() { OWNER = this };
            this.DataContext = MainWindow.myTopMenuViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (CONTENT != null)
                    HOLDER.Content = CONTENT;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to display the object");
                var t = new TextBlock() { Text = "Unable to display the object" };
                HOLDER.Content = t;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CONTENT = null;
            this.DataContext = null;
        }
    }
}
