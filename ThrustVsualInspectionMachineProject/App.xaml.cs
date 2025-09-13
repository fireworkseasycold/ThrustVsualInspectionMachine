using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

using ThrustVsualInspectionMachineProject.Constants;
using ThrustVsualInspectionMachineProject.Models;
using ThrustVsualInspectionMachineProject.ViewModels;
using ThrustVsualInspectionMachineProject.Views;

using NLog;
using SqlSugar;

namespace ThrustVsualInspectionMachineProject;

// For more information about application lifecycle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview
// For docs about using Prism in WPF see https://prismlibrary.com/docs/wpf/introduction.html

// WPF UI elements use language en-US by default.
// If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
// Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
public partial class App : PrismApplication
{
    private string[] _startUpArgs;

    public App()
    {
    }

    protected override Window CreateShell()
        => Container.Resolve<ShellWindow>();

    protected override async void OnInitialized()
    {
        base.OnInitialized();
        await Task.CompletedTask;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _startUpArgs = e.Args;
        // 初始化 NLog 配置（如果存在 NLog.config）
        try
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var nlogConfigPath = Path.Combine(appLocation!, "NLog.config");
            if (File.Exists(nlogConfigPath))
            {
                LogManager.Setup().LoadConfigurationFromFile(nlogConfigPath);
            }
        }
        catch
        {
            // 忽略 NLog 初始化错误，避免启动失败。
        }

        base.OnStartup(e);
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Core Services

        // App Services

        // Views
        containerRegistry.RegisterForNavigation<MainPage, MainViewModel>(PageKeys.Main);
        containerRegistry.RegisterForNavigation<ShellWindow, ShellViewModel>();

        // Configuration
        var configuration = BuildConfiguration();
        var appConfig = configuration
            .GetSection(nameof(AppConfig))
            .Get<AppConfig>();

        // Register configurations to IoC
        containerRegistry.RegisterInstance<IConfiguration>(configuration);
        containerRegistry.RegisterInstance<AppConfig>(appConfig);

        // 注册 NLog 日志实例
        var appLogger = LogManager.GetLogger("App");
        containerRegistry.RegisterInstance<NLog.ILogger>(appLogger);

        // 注册 SqlSugar 客户端（从配置读取）
        try
        {
            var connStr = configuration["Database:ConnectionString"];
            var dbTypeStr = configuration["Database:DbType"]; // 例如: SqlServer, MySql, Sqlite
            if (!string.IsNullOrWhiteSpace(connStr) && Enum.TryParse<DbType>(dbTypeStr, ignoreCase: true, out var dbType))
            {
                var sugarClient = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = connStr,
                    DbType = dbType,
                    IsAutoCloseConnection = true
                });
                containerRegistry.RegisterInstance<SqlSugarClient>(sugarClient);
            }
        }
        catch (Exception ex)
        {
            // 记录初始化数据库客户端的异常（若日志可用）
            try { appLogger?.Error(ex, "Initialize SqlSugarClient failed."); } catch { }
        }

        // 注册仓储
        containerRegistry.RegisterSingleton<Services.ISampleRepository, Services.SampleRepository>();
    }

    private IConfiguration BuildConfiguration()
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        return new ConfigurationBuilder()
            .SetBasePath(appLocation)
            .AddJsonFile("appsettings.json")
            .AddCommandLine(_startUpArgs)
            .Build();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}
