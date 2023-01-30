using assets_manager.Views.loadPages;
using Avalonia.Controls;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using System;

namespace assets_manager.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            var nv = this.FindControl<NavigationView>("nvSample1");
            nv.SelectionChanged += OnNVUserInfoSelectionChanged;
        }

        private void OnNVUserInfoSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.IsSettingsSelected)
            {
                (sender as NavigationView).Content = new AppPageSetting();
            }
            else if (e.SelectedItem is NavigationViewItem nvi)
            {
                var smpPage = $"assets_manager.Views.loadPages.{nvi.Tag}";
                var pg = Activator.CreateInstance(Type.GetType(smpPage));
                (sender as NavigationView).Content = pg;
            }
        }

    }
}
