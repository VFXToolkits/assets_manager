using assets_manager.GPythonNet;
using assets_manager.Services;
using assets_manager.Services.Storage;
using assets_manager.ViewModels.Base;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace assets_manager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private string _user_name;
    private string _password;
    private bool _is_remember;
    private bool _is_loading = true;
    public string UserName
    {
        get => _user_name;
        set
        {
            _user_name = value;
            this.RaisePropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            this.RaisePropertyChanged();
        }
    }

    public bool IsRemember
    {
        get => _is_remember;
        set
        {
            _is_remember = value;
            this.RaisePropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _is_loading;
        set
        {
            _is_loading = value;
            this.RaisePropertyChanged();
        }
    }


    public MainWindowViewModel()
    {
        LoadUserInfo();

        var canLogin = this.WhenAnyValue(
                x => x.UserName,
                x => x.Password,
                (user, pass) => !string.IsNullOrWhiteSpace(user) &&
                                !string.IsNullOrWhiteSpace(pass));
        LoginCommand = ReactiveCommand.CreateFromTask(
            () => LoginServer(),
            canLogin);
    }

    private void LoadUserInfo() {
        KeyValueStorage? LocalValue = DIServer.GetService<KeyValueStorage>();

        if (LocalValue == null) {
            return;
        }

        string is_remember = LocalValue.Get("is_remember");
        if (is_remember == "True") {
            GetSavePreferences();
        } else {
            LocalValue.Set("is_remember", "False");
        }

    }

    private async Task LoginServer()
    {
        IsLoading = true;
        UserViewModelBase? user_view_model = DIServer.GetService<UserViewModelBase>();
        user_view_model.UserLoadingShow = true;

        var python_server = new PythonMainServer();
        python_server.InitPythonServer();

        await Task.Run(() => {
            var currentuser = new GPythonNet.MainSiteLib.UserLogin(UserName, Password);
        });

        if (_is_remember) {
            SetSavePreferences();
        }
        IsLoading = false;
        user_view_model.UserLoadingShow = false;
    }

    private void SetSavePreferences()
    {

    }

    private void GetSavePreferences()
    {

    }


}
