using System.Runtime.InteropServices;

namespace assets_manager.Config;

public class CustomFontManagerImpl
{

    public static string GetPlatformFontFamily()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "Microsoft YaHei";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "Menlo"; //换成OSX下的中文字体
        }
        else
        {
            return "Noto Sans CJK SC";
        }
    }

}
