using assets_manager.Config;
using DynamicData.Kernel;
using NLog;
using Python.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace assets_manager.GPythonNet.MainSiteLib;

public class UserLogin
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public bool IsLogin { get; set; }
    public UserLogin(string username, string password)
    {
        logger.Info("UserLogin");

        if (!PythonEngine.IsInitialized) {
            PythonEngine.Initialize();
        }

        logger.Info($"load python path {PythonEngine.PythonPath}");
        using (Py.GIL())
        {
            dynamic ManagerBase = Py.Import("AMBase");
            ManagerBase.Base.set_user_base_api(Config.AppPathConst.AppSetConfigField.BaseUrl, Config.AppPathConst.AppSetConfigField.LoginUrlName);

            dynamic UserInfo = Py.Import("UserLogin");

            dynamic User_Base_object = UserInfo.UserLogin(username, password);

            logger.Info($"connect url: {User_Base_object.server_url}");

            List<string> UserInfoDict = ((string[])User_Base_object.get_user_info()).ToList<string>();
            logger.Info($"current name: {UserInfoDict[0]}");
            IsLogin = false;

            RunFieldConfig.UserName = username;
            RunFieldConfig.Password = password;
            RunFieldConfig.Name = UserInfoDict[0];
        }
        PythonEngine.Shutdown();

    }
}
