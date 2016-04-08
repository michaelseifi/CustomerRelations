using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
namespace daisybrand.forecaster.Controls.SubControls
{
    public class _ComboBoxEdit_View : _ComboBoxEdit
    {
        public _ComboBoxEdit_View()
            : base()
        {
            base.IsTextEditable = false;
            this.Items.Add(new ComboBoxEditItem() { Content = "Full", IsSelected = true });
            this.Items.Add(new ComboBoxEditItem() { Content = "Quick" });
            this.Loaded += _ComboBoxEdit_View_Loaded;
            this.SelectedIndexChanged += _ComboBoxEdit_View_SelectedIndexChanged;
        }

        void _ComboBoxEdit_View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainWindow.myCustomerSearchViewModel != null)
                MainWindow.myCustomerSearchViewModel.SELECTED_VIEW = (SelectedItem as ComboBoxEditItem).Content as string;
        }

        void _ComboBoxEdit_View_SelectedIndexChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainWindow.myCustomerSearchViewModel != null)
                MainWindow.myCustomerSearchViewModel.SELECTED_VIEW = (SelectedItem as ComboBoxEditItem).Content as string;
        }
    }
}
