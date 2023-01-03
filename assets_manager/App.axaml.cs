using assets_manager.ViewModels;
using assets_manager.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NLog;

namespace assets_manager
{
    public partial class App : Application
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override void Initialize()
        {
            Config.AppPathConst.AppInitValue();
            Config.NLogConfig.TouchAndApplyNLogConfig();

            AvaloniaXamlLoader.Load(this);
            logger.Info("assets manager start app");
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
