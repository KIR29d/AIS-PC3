using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using CustomPcStoreApp.Data;
using CustomPcStoreApp.Services;
using CustomPcStoreApp.Services.Interfaces;
using CustomPcStoreApp.ViewModels;

namespace CustomPcStoreApp;

public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = CreateHostBuilder().Build();
        
        // Запуск хоста
        _host.Start();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }

    private IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Конфигурация базы данных
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                // Регистрация сервисов
                services.AddScoped<IAuthenticationService, AuthenticationService>();
                services.AddScoped<IComponentService, ComponentService>();
                services.AddScoped<IClientService, ClientService>();
                services.AddScoped<IOrderService, OrderService>();
                services.AddScoped<IWarehouseService, WarehouseService>();
                services.AddScoped<IAssemblyService, AssemblyService>();
                services.AddScoped<IUserManagementService, UserManagementService>();
                services.AddScoped<IAuditService, AuditService>();

                // Регистрация ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<ComponentsViewModel>();
                services.AddTransient<ClientsViewModel>();
                services.AddTransient<OrdersViewModel>();
                services.AddTransient<WarehouseViewModel>();
                services.AddTransient<AssemblyTasksViewModel>();
                services.AddTransient<UsersViewModel>();
                services.AddTransient<AuditViewModel>();

                // Singleton для пользовательской сессии
                services.AddSingleton<UserSession>();
            });
    }

    public static T GetService<T>() where T : class
    {
        return ((App)Current)._host?.Services.GetRequiredService<T>() 
            ?? throw new InvalidOperationException("Service not available");
    }
}