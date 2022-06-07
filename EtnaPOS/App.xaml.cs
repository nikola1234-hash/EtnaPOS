using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Services;
using EtnaPOS.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.IO;
using System.Windows;
using EtnaPOS.Windows;


namespace EtnaPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IConfiguration Configuration { get; set; }
        protected void ConfigureServices()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(path: AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsetting.json", true, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddSingleton<ITreeViewNodeGenerator, TreeViewNodeGenerator>();
            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddSingleton<IEventAggregator, EventAggregator>();

            services.AddSingleton<EtnaDbContext>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<PosViewModel>();


            Configuration = builder.Build();
            ServiceProvider = services.BuildServiceProvider();
        }
        public static T? GetService<T>()
        {
            return (T?)ServiceProvider.GetService(typeof(T));
        }
     
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigureServices();
            var viewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            MainWindow window = new MainWindow
            {
                DataContext = viewModel
            };
            var selectDateDialog = new SelectWorkDay();
            var result = selectDateDialog.ShowDialog();

            if (result == true)
            {
                var r = window.ShowDialog();
                if (r == null || r == false)
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }
            else
            {
                MessageBox.Show("Program ce se zatvoriti.");
            }

            
            
        }
    }
}
