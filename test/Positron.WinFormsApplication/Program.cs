﻿using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Positron.Application;
using Positron.Server;
using Positron.UI;
using Positron.UI.Builder;

namespace Positron.WinFormsApplication
{
    static class Program
    {
        public static IWindowHandler WindowHandler { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            var environmentName = "Development";
#else
            var environmentName = "Production";
#endif

            var builder = new WebHostBuilder()
                .UseEnvironment(environmentName)
                .UseUsePositronServer()
                .UseStartup<Startup>();

            var webHost = builder.Build();
            try
            {
                webHost.Start();
                LoadApp(webHost);
            }
            finally
            {
                webHost.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadApp(IWebHost webHost)
        {
            var app = new App();
            app.InitializeComponent();

            var uiBuilder = new PositronUiBuilder()
                .SetWebHost(webHost);

            WindowHandler = uiBuilder.Build();

            System.Windows.Forms.Application.Run(new MainForm());
        }
    }
}