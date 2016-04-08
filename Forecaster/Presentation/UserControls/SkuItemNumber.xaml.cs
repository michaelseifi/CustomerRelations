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
    /// Interaction logic for SkuItemNumber.xaml
    /// </summary>
    public partial class SkuItemNumber : UserControl
    {
        public SkuItemNumber()
        {
            InitializeComponent();
        }

        private void ITEM_NUM_BTN_Click(object sender, RoutedEventArgs e)
        {
            var sku = (sender as Button).CommandParameter as Controlers.Interfaces.ISku;
            if (sku.SETTING == null) sku.SETTING = new Controlers.Objects.Sku.Setting(int.Parse(sku.SHIPTO), sku.SKUID);
            sku.SETTING.ITEM_NUM = ITEM_NUM.Text;
            sku.SETTING.Save();
        }
    }
}
