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
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Promotions.xaml
    /// </summary>
    public partial class Promotions : UserControl
    {
        ITab TAB { get; set; }
        public Promotions(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            Update();
            

            //if (MainWindow.myGraphViewModel != null)
            //    MainWindow.myGraphViewModel.PROMOTION_USERCONTROLS.Add(this);

            //this.Unloaded += (s, e) =>
            //{
            //    if (MainWindow.myGraphViewModel != null)
            //        MainWindow.myGraphViewModel.PROMOTION_USERCONTROLS.Remove(this);
            //};
        }

        public void Update()
        {
            if (!Controlers.Collections.DataCollection.IsCollectionNull(TAB.myWeeklyData))
                DataContext = TAB.myWeeklyData;
        }
    }
}
