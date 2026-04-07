using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PCStoreApp.Data;
using PCStoreApp.Services.Interfaces;
using PCStoreApp.Views;
using System.IO;
using System.Windows;

namespace PCStoreApp
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;
        private IConfiguration? _configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настройка конфигурации
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            // Настройка DI контейнера
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Глобальная обработка исключений
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Запуск окна авторизации
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Конфигурация
            services.AddSingleton(_configuration!);

            // Логирование
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            // База данных
            services.AddDbContext<PCStoreDbContext>(options =>
            {
                var connectionString = _configuration!.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Сервисы (будут добавлены в следующих задачах)
            // services.AddScoped<IAuthenticationService, AuthenticationService>();
            // services.AddScoped<IAuditService, AuditService>();
            // services.AddScoped<INavigationService, NavigationService>();

            // Views (будут добавлены в следующих задачах)
            services.AddTransient<LoginWindow>();
            // services.AddTransient<MainWindow>();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = _serviceProvider?.GetService<ILogger<App>>();
            logger?.LogError(e.Exception, "Необработанное исключение в приложении");

            MessageBox.Show(
                "Произошла непредвиденная ошибка. Приложение будет закрыто.\n\n" +
                $"Ошибка: {e.Exception.Message}",
                "Ошибка приложения",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;
            Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}