using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ConsoleApp.Middleware;

using Guru.Network;
using Guru.DependencyInjection;
using Guru.Formatter.Abstractions;

namespace ConsoleApp.Network
{
    public class TestRunner
    {
        private readonly IHttpClientBroker _Broker;

        public TestRunner()
        {
            _Broker = ContainerEntry.Resolve<IHttpClientBroker>();
        }

        public void Run()
        {
            HttpBrokerTest().GetAwaiter().GetResult();
        }

        private async Task HttpBrokerTest()
        {
            var request = _Broker.Get();

            var host = "http://localhost:5000";

            using (var response = await request.GetAsync($"{host}/test/hi1"))
            {
                if (response.StatusCode == 200)
                {
                    var text = await response.GetBodyAsync<string, ITextFormatter>();

                    Console.WriteLine(text);
                }
            }

            using (var response = await request.GetAsync(
                $"{host}/test/hi2", 
                new Dictionary<string, string>()
                {
                    { "welcome", "abc" }
                }))
            {
                if (response.StatusCode == 200)
                {
                    var text = await response.GetBodyAsync<string, ITextFormatter>();

                    Console.WriteLine(text);
                }
            }

            using (var response = await request.PostAsync<IJsonFormatter>(
                $"{host}/test/hi3", 
                new Request() 
                { 
                    Data = "hello, world!" 
                }))
            {
                if (response.StatusCode == 200)
                {
                    var text = await response.GetBodyAsync<string, ITextFormatter>();

                    Console.WriteLine(text);
                }
            }

            using (var response = await request.PostAsync<IJsonFormatter>(
                $"{host}/test/hi4", 
                new Dictionary<string, string>()
                {
                    { "word", "abc" },
                    { "welcome", "def" }
                },
                new Request() 
                { 
                    Data = "hello, world!" 
                }))
            {
                if (response.StatusCode == 200)
                {
                    var text = await response.GetBodyAsync<string, ITextFormatter>();

                    Console.WriteLine(text);
                }
            }

            using (var response = await request.PostAsync<IJsonFormatter>(
                $"{host}/test/hi5", 
                new Dictionary<string, string>()
                {
                    { "word", "abc" },
                    { "welcome", "def" },
                    { "number", "123" }
                },
                new Request() 
                { 
                    Data = "hello, world!" 
                }))
            {
                if (response.StatusCode == 200)
                {
                    var data = await response.GetBodyAsync<Response, IJsonFormatter>();

                    Console.WriteLine(data.Result);
                }
            }

            using (var response = await request.PostAsync<IJsonFormatter>(
                $"{host}/test/hi6", 
                new Dictionary<string, string>()
                {
                    { "word", "abc" },
                    { "welcome", "def" },
                    { "number", "123" },
                    { "price", "12.3" }
                },
                new Request() 
                { 
                    Data = "hello, world!" 
                }))
            {
                if (response.StatusCode == 200)
                {
                    var data = await response.GetBodyAsync<Response, IJsonFormatter>();

                    Console.WriteLine(data.Result);
                }
            }
        }
    }
}