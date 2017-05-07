﻿using System;
using System.Threading;
using System.Diagnostics;
using Guru.DependencyInjection;
using System.Collections.Concurrent;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Thread";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //new DependencyInjection.TestRunner().Run();

            new Formatter.TestRunner().Run();

            // new Network.TestRunner().Run();

            // new Network.UnsplashCrawler().Run();

            // new EntityFramework.TestRunner().Run();

            // new Cache.TestRunner().Run();

            // new Mq.TestRunner().Run();

            //new Jobs.TestRunner().Run();

            //new Markdown.TestRunner().Run();

            stopwatch.Stop();
            Console.WriteLine($"test done. cost: {stopwatch.Elapsed.TotalMilliseconds}ms.");
        }

        private static ConcurrentDictionary<object, string> _ImplementationResolvers = new ConcurrentDictionary<object, string>();
    }
}