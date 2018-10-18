using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace App1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            connection = new HubConnectionBuilder()
                 .WithUrl("https://localhost:44350/myhub")
                  .ConfigureLogging(logging => //Microsoft.Extensions.Logging.Console
                  {
                      logging.AddConsole();
                      logging.SetMinimumLevel(LogLevel.Debug);
                  })
                .Build();

         connection.Closed += async (error) =>
         {
             await Task.Delay(new Random().Next(0, 5) * 1000);
             await connection.StartAsync();
         };


            // var hc = new HttpClient();
            //var xx= hc.GetStringAsync("https://localhost:44350");
            // //https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap4-loopbackaccessrules'

        }
        HubConnection connection;
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {

            connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                  {
                    
                      Liste1.Items.Add($"{user}: {message}");
                  });
            });



            try
            {
                await connection.StartAsync();
                Liste1.Items.Add("Connection started");
                btnConnect.IsEnabled = false;
                btnSend.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Liste1.Items.Add(ex.Message);
            }


        }

        private async void Send_ClickAsync(object sender, RoutedEventArgs e)
        {
            await connection.InvokeAsync("BroadcastMessage",
        "demo", "demo");
        }
    }
}
