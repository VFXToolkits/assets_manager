using assets_manager.Services.Storage;
using assets_manager.ViewModels;
using assets_manager.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace assets_manager.Services;

public static class DIServer
{
    private static IServiceProvider? _serviceProvider;

    private static bool _isInitialized;


    public static void AddBaseServer(ServiceCollection sc)
    {
        sc.AddSingleton<KeyValueStorage>();
        sc.AddSingleton<UserViewModelBase>();

        sc.AddSingleton<ProjectAllControlViewModel>();
    }

    private static void Initialize()
    {
        var Server_Collection = new ServiceCollection();

        AddBaseServer(Server_Collection);

        _serviceProvider = Server_Collection.BuildServiceProvider();
        _isInitialized = true;
    }
    public static T? GetService<T>()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
        return ActivatorUtilities.GetServiceOrCreateInstance<T>(_serviceProvider);
    }

}

