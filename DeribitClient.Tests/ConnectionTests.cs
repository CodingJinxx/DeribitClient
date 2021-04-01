using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using DeribitClient.Messages.MarketData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DeribitClient.Tests
{
    public class ConnectionTests
    {
        private ServiceProvider ServiceSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection()
                .Configure<ConnectionSettings>(config.GetSection(ConnectionSettings.ConfigSectionName))
                .AddOptions()
                .AddLogging()
                .AddSingleton<RoutedConnection>()
                .AddSingleton<ChanneledConnection>();

            return serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public async void GetOrderbookTest()
        {
            var serviceProvider = this.ServiceSetup();  
            var connection = serviceProvider.GetService<RoutedConnection>();
            
            await connection.Connect();
            Assert.True(connection.Connected);

            var getOrderBook = new GetOrderBookRequest()
            {
                depth = 5,
                instrument_name = "BTC-PERPETUAL"
            };
            connection.Send(getOrderBook);

            CancellationTokenSource source = new CancellationTokenSource();
            TaskFactory factory = new TaskFactory();
            var wh = new ManualResetEvent(false);
            factory.StartNew(Consumer, source.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Assert.True(wh.WaitOne(20000));
            async void Consumer()
            {
                while (!source.Token.IsCancellationRequested)
                {
                    var response = await connection.MarketDataMessagesOut.Read();
                    wh.Set();
                }
            }
        }

        [Fact]
        public async void ConnectionTest()
        {
            var service = this.ServiceSetup();
            var connection = service.GetService<ChanneledConnection>();

            await connection.Connect();
            Assert.True(connection.Connected);
        }
    }
}