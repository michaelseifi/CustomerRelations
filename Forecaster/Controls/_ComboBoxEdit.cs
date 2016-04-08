using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Editors;
using System.ComponentModel;
namespace daisybrand.forecaster.Controls
{
    public class _ComboBoxEdit:ComboBoxEdit
    {

        [Description("Filters all text input as one word within the display value")]
        public bool FilterAll
        {
            get
            {
                return true;
            }
            set
            {
                if (value == true)
                {
                    base.ValidateOnTextInput = false;
                    base.FilterCondition = DevExpress.Data.Filtering.FilterCondition.Contains;
                    base.IncrementalFiltering = true;
                    base.ImmediatePopup = true;
                    base.AutoComplete = false;
                }
            }
        }


        protected override bool ProcessPopupKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                if (IsPopupOpen)
                    ClosePopup(PopupCloseMode.Normal);
            }
            return base.ProcessPopupKeyDown(e);
        }
                
    }
}
