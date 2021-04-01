using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using DeribitClient.Messages.MarketData;
using DeribitClient.Validator;
using Divergic.Logging.Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace DeribitClient.Tests
{
    public class ConnectionTests
    {
        private ITestOutputHelper _output;
        private readonly ILogger _logger;
        public ConnectionTests(ITestOutputHelper output)
        {
            this._output = output;
        }
        
        private ServiceProvider ServiceSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection()
                .Configure<ConnectionSettings>(config.GetSection(ConnectionSettings.ConfigSectionName))
                .AddOptions()
                .AddLogging(builder => builder.AddXunit(this._output))
                .AddSingleton<IServerErrorHandler, ServerErrorHandler>()
                .AddSingleton<ConnectionManager>()
                .AddSingleton<ChanneledConnection>();

            return serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public async void GetOrderbookTest()
        {
            var serviceProvider = this.ServiceSetup();  
            var connection = serviceProvider.GetService<ConnectionManager>();
            
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
                    var response = await connection.Incoming.Read();
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

        [Fact]
        public async void HeartbeatTest()
        {
            var service = this.ServiceSetup();
            var connection = service.GetService<ConnectionManager>();

            await connection.Connect();
            Assert.True(connection.Connected);
            
            connection.SetHeartbeat(10);

            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 30)); 
            Assert.True(connection.Connected);
            Assert.True(connection.Incoming.Count == 0);
        }

        [Fact]
        public async void ConnectionStateEventTest()
        {
            var service = this.ServiceSetup();
            var connection = service.GetService<ConnectionManager>();
            int stateChangeCount = 0;

            connection.OnConnectionChange += (sender, b) =>
            {
                stateChangeCount++;
                Assert.True(b == connection.Connected);
            };
            
            Assert.True(stateChangeCount == 0);
            await connection.Connect();
            Assert.True(stateChangeCount == 1);
            await connection.Disconnect();
            Assert.True(stateChangeCount == 2);
            await connection.Connect();
            Assert.True(stateChangeCount == 3);
            await connection.Disconnect();
            Assert.True(stateChangeCount == 4);
        }
    }
}