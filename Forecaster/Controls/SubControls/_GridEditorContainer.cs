using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

namespace daisybrand.forecaster.Controls.SubControls
{
    public class _GridEditorContainer : Border
    {
        public _GridEditorContainer()
        {
            Focusable = true;
        }
        protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            ((Control)Child).Focus();
        }
    }
}
