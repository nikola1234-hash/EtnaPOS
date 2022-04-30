using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;
using EtnaPOS.Services;
using EtnaPOS.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
            
            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddSingleton<PosViewModel>();  
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddTransient<EtnaDbContext>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<IProductService, ProductService>();
            


            Configuration = builder.Build();
            ServiceProvider = services.BuildServiceProvider();
        }
        public static T? GetService<T>()
        {
            return (T?)ServiceProvider.GetService(typeof(T));
        }
        private void ConfigureData()
        {
            string folder = "\\Data";
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            var fileExists = File.Exists(Directory.GetCurrentDirectory() + folder + "\\products.json");
            if (!fileExists)
                File.Create(Directory.GetCurrentDirectory() + folder + "\\products.json");
            if (doesExist) return;
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + folder);


        }
    
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigureData();
            ConfigureServices();
            var viewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            MainWindow window = new MainWindow
            {
                DataContext = viewModel
            };
            window.Show();
        }
    }
}
