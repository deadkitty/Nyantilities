using Nyantilities.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Nyantilities.Controls
{
    public sealed partial class LoadingControl : NyaControl
    {
        public String LoadingText
        {
            get { return (String)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(String), typeof(LoadingControl), new PropertyMetadata("Lädt ...", LoadingTextChanged));

        private static void LoadingTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            LoadingControl control = sender as LoadingControl;
            control.LoadingContent.Text = control.LoadingText;
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingControl), new PropertyMetadata(true, IsLoadingChanged));

        private static void IsLoadingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            LoadingControl control = sender as LoadingControl;
            control.LoadingCtrl.IsLoading = control.IsLoading;
        }

        public LoadingControl()
        {
            InitializeComponent();
        }
    }
}
