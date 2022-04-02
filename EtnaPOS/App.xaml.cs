using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Services;
using EtnaPOS.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EtnaPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        protected void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddTransient<EtnaDbContext>();
            ServiceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureServices();
            var viewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            MainWindow window = new MainWindow
            {
                DataContext = viewModel
            };
            window.Show();
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }
    }
}
