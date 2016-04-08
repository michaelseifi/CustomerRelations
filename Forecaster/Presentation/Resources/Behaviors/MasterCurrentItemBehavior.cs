using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Grid;
using System.Windows.Interactivity;

namespace daisybrand.forecaster.Presentation.Resources.Behaviors
{
    public class MasterCurrentItemBehavior: Behavior<GridControl>
    {
        public static readonly DependencyProperty CurrentItemProperty = 
            DependencyProperty.Register("CurrentItem", typeof(object), typeof(MasterCurrentItemBehavior), new UIPropertyMetadata(null, OnCurrentItemChanged));

        private static void OnCurrentItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MasterCurrentItemBehavior masterCurrentItemBehavior = o as MasterCurrentItemBehavior;
            if (masterCurrentItemBehavior != null)
                masterCurrentItemBehavior.OnCurrentItemChanged((object)e.OldValue, (object)e.NewValue);
        }

        protected virtual void OnCurrentItemChanged(object oldValue, object newValue)
        {
            if (isChangeInternal)
            {
                isChangeInternal = false;
                return;
            }
            AssociatedObject.View.MoveFocusedRow(AssociatedObject.DataController.FindRowByRowValue(newValue));
        }

        public object CurrentItem
        {
            get
            {
                return (object)GetValue(CurrentItemProperty);
            }
            set
            {
                SetValue(CurrentItemProperty, value);
            }
        }

        object CurrentItemInternal
        {
            get
            {
                return CurrentItem;
            }
            set
            {
                if (CurrentItem == value)
                    return;
                isChangeInternal = true;
                CurrentItem = value;
            }
        }

        bool isChangeInternal = false;

        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    AssociatedObject.c += AssociatedObject_CurrentItemChanged;
        //    AssociatedObject.View.FocusedViewChanged += AssociatedObject_FocusedViewChanged;
        //}

        //private void AssociatedObject_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        //{
        //    CurrentItemInternal = e.NewItem;
        //}

        //void AssociatedObject_FocusedViewChanged(object sender, FocusedViewChangedEventArgs e)
        //{
        //    AssociatedObject.View.FocusedRowHandle = (e.NewView.DataControl as GridControl).GetMasterRowHandle();
        //}

        //protected override void OnDetaching()
        //{
        //    AssociatedObject.CurrentItemChanged -= AssociatedObject_CurrentItemChanged;
        //    base.OnDetaching();
        //}
    }
}
