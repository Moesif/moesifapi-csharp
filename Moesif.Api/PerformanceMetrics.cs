using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moesif.Api
{
    public class PerformanceMetrics
    {
        private static readonly Dictionary<string, List<long>> Metrics = new Dictionary<string, List<long>>();
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        public static void StartMeasurement(string stageName)
        {
            Stopwatch.Restart();
        }

        public static void StopMeasurement(string stageName)
        {
            Stopwatch.Stop();
            if (Metrics.ContainsKey(stageName))
            {
                Metrics[stageName].Add(Stopwatch.ElapsedMilliseconds);
            }
            else
            {
                Metrics[stageName] = new List<long> { Stopwatch.ElapsedMilliseconds };
            }
        }

        public static void PrintMetrics(Action<string> logger)
        {
            logger("Performance Metrics:");
            foreach (var metric in Metrics)
            {
                string result = string.Join(",", metric.Value);
                logger($"{metric.Key}");
                logger(result);
                // logger($"  Count: {metric.Value.Count}");
                // logger($"  Total: {metric.Value.Sum()}ms");
                // logger($"  Average: {metric.Value.Average():F2}ms");
                // logger($"  Min: {metric.Value.Min()}ms");
                // logger($"  Max: {metric.Value.Max()}ms");
            }
        }
    }
}

// Usage example
// class Program
// {
//     static void Main(string[] args)
//     {
//         Moesif.Api.PerformanceMetrics.StartMeasurement("Stage 1");
//         // Stage 1 code
//         System.Threading.Thread.Sleep(100);
//         Moesif.Api.PerformanceMetrics.StopMeasurement("Stage 1");
//
//         Moesif.Api.PerformanceMetrics.StartMeasurement("Stage 2");
//         // Stage 2 code
//         System.Threading.Thread.Sleep(200);
//         Moesif.Api.PerformanceMetrics.StopMeasurement("Stage 2");
//
//         Moesif.Api.PerformanceMetrics.StartMeasurement("Stage 3");
//         // Stage 3 code
//         System.Threading.Thread.Sleep(150);
//         Moesif.Api.PerformanceMetrics.StopMeasurement("Stage 3");
//
//         Moesif.Api.PerformanceMetrics.PrintMetrics();
//     }
// }
