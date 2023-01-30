using DynamicData;
using NLog;
using Python.Runtime;
using System;
using System.IO;
using System.Linq;

namespace assets_manager.GPythonNet;

public class PythonMainServer
{
    private string python_dll_path = Config.AppPathConst.AppSetConfigField.PythonPath;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public string PythonScriptPath = $"{Config.AppPathConst.BaseDirectory}GPythonNet/{Config.AppPathConst.AppSetConfigField.LoginModule}";
    public bool IsInitialize;

    public void PythonInitMainDLL() {

        if (string.IsNullOrEmpty(python_dll_path)) {
            logger.Error("python_dll_path is null");
            return;
        }

        if (!File.Exists(python_dll_path)) {
            logger.Error("python_dll_path is not exists");
            return ;
        }

        Runtime.PythonDLL = python_dll_path;

        string PYTHON_HOME = Path.GetDirectoryName(python_dll_path);

        PythonEngine.PythonPath = string.Join(Path.PathSeparator.ToString(),
            GetAllPythonPath(PYTHON_HOME)
            ) ;

        logger.Info("python init done");
        IsInitialize = true;
    }

    private string[] GetAllPythonPath(string python_home)
    {
        string[] python_paths = new string[]
            {
                PythonEngine.PythonPath,
                PythonScriptPath
            };
        string python_sc_lib = Config.AppPathConst.AppSetConfigField.PythonScriptLib;
        if (python_sc_lib != "") {
            string[] lib_python_paths = new string[]
                {
                PythonEngine.PythonPath,
                PythonScriptPath,
                python_sc_lib
                };
            return lib_python_paths;
        }
        return python_paths;
    }

    public void InitPythonServer() {
        if (!IsInitialize) {
            PythonInitMainDLL();
        }
    }
}
