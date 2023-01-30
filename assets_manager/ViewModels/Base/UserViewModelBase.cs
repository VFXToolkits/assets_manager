using ReactiveUI;
using System;

namespace assets_manager.ViewModels.Base
{
    public class UserViewModelBase : ViewModelBase
    {
        private bool _user_loading_show;

        public bool UserLoadingShow
        {
            get => _user_loading_show;
            set
            {
                _user_loading_show = value;
                this.RaisePropertyChanged();
            }
        }


    }
}
