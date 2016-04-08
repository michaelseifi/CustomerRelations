using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
namespace daisybrand.forecaster.Controls.SubControls
{
    public class _ComboBoxEdit_Customers:_ComboBoxEdit
    {
        public delegate void SearchHandler( EventArgs args);
        public event EventHandler OnSearch;
        
        //public static readonly DependencyProperty SourceProperty =
        //        DependencyProperty.Register("FilterAll", typeof(bool), null, null);

        public _ComboBoxEdit_Customers()
            : base()
        {
            MinWidth = 250;
        }
       


        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            
        }

        protected override void OnDisplayTextChanged(string displayText)
        {
            base.OnDisplayTextChanged(displayText);
        }

        protected override void OnSelectedIndexChanged(int oldSelectedIndex, int selectedIndex)
        {
            base.OnSelectedIndexChanged(oldSelectedIndex, selectedIndex);            
        }

    }
}
