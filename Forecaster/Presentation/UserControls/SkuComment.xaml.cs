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
    /// Interaction logic for SkuComment.xaml
    /// </summary>
    public partial class SkuComment : UserControl
    {

        public Controlers.Interfaces.ITab TAB { get; set; }
        public SkuComment()
        {
            InitializeComponent();
        }

        private void SKU_COMMENT_OK_Click(object sender, RoutedEventArgs e)
        {
            var skuid = (sender as Button).CommandParameter as Controlers.Interfaces.ISku;
            if (skuid.SETTING == null) skuid.SETTING = new Controlers.Objects.Sku.Setting(int.Parse(TAB.CUSTOMER_NUMBER), TAB.SKU.SKUID);
            skuid.SETTING.COMMENT = COMMENT.Text;
            skuid.SETTING.Save();
            //var tab = MainWindow.myMainWindowViewModel.TABS.Where(t => t.CAPTION == skuid);
            //if (tab != null && tab.Count() > 0)
            //{
            //    var firstTab = tab.First();
            //    if (firstTab.SKU.SETTING != null)
            //    {   
            //        firstTab.SKU.SETTING.COMMENT = COMMENT.Text;
            //        firstTab.SKU.SETTING.Save();
            //    }
            //}
        }
    }
}
