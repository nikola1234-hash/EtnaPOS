﻿using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Services;
using EtnaPOS.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using EtnaPOS.DAL.Models;
using EtnaPOS.SplashScreens;
using EtnaPOS.SplashScreens.Events;
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
            services.AddSingleton<EtnaDbContext>();
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddSingleton<ITreeViewNodeGenerator, TreeViewNodeGenerator>();
            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddSingleton<IEventAggregator, EventAggregator>();

            services.AddSingleton<ISplashScreenEvent, SplashScreenEvent>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            //services.AddTransient<PosViewModel>();


            Configuration = builder.Build();
            ServiceProvider = services.BuildServiceProvider();
        }

        public static T? GetService<T>()
        {
            return (T?)ServiceProvider.GetService(typeof(T));
        }

        public void FireSplashScreen()
        {
            SplashScreenManager.Create(() => new Startup(), new DXSplashScreenViewModel
            {
                IsIndeterminate = true,
                Title = "POS",
                Subtitle = "Ucitavanje...",
                Logo = new Uri("../../Images/Logo.png", UriKind.Relative),
                Status = "Loading the application"
            }).ShowOnStartup();

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FireSplashScreen();

            ConfigureServices();
            CreateInitialCategory();


            ThemedWindow.RoundCorners = true;

            var viewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            MainWindow window = new MainWindow
            {
                DataContext = viewModel
            };

            window.ShowDialog();
        }

        private void CreateInitialCategory()
        {
            
            var _db = App.GetService<EtnaDbContext>();
            var artikli = _db.Kategorije.FirstOrDefault(s => s.Kategorija == "Artikli");
            if (artikli == null)
            {
                KategorijaArtikla kategorija = new KategorijaArtikla
                {
                    Kategorija = "Artikli",
                    DateCreated = DateTime.Now,
                    CreatedBy = "System"
                };
                _db.Kategorije.Add(kategorija);
                _db.SaveChanges();
            }
        }
    }
}