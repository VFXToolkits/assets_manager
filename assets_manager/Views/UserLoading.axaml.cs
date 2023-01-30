using assets_manager.Services;
using Avalonia.Controls;
using assets_manager.ViewModels.Base;

namespace assets_manager.Views
{
    public partial class UserLoading : UserControl
    {
        public UserLoading()
        {
            InitializeComponent();

            UserViewModelBase? user_view_model = DIServer.GetService<UserViewModelBase>();
            DataContext = user_view_model;
        }
    }
}
