using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmitePB.Manager.Services;
using SmitePB.Manager.Windows;
using System.IO;
using System.Windows;

namespace SmitePB.Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IConfiguration _configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            //build configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //build service provider
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //initialize display
            serviceProvider.GetService<Display>().Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddSingleton<ApiService>();
            services.AddTransient<Display>();
        }
    }
}
