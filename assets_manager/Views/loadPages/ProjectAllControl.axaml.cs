using assets_manager.Services;
using assets_manager.ViewModels;
using Avalonia.Controls;

namespace assets_manager.Views.loadPages
{
    public partial class ProjectAllControl : UserControl
    {
        public ProjectAllControl()
        {
            InitializeComponent();

            ProjectAllControlViewModel? user_view_model = DIServer.GetService<ProjectAllControlViewModel>();
            if (user_view_model != null) { 
                DataContext = user_view_model;
            }
        }
    }
}
