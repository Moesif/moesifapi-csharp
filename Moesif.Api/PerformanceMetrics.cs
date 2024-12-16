using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moesif.Api
{
    public class PerformanceMetrics
    {
        public delegate void LogAction(string message);

        private readonly Dictionary<string, long> metrics = new Dictionary<string, long>();
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly bool logStage;
        private readonly string functionName;
        private string stageName = "";


        // logStg flag is useful when we're printing the metrics at the end of invoke method.
        // if set to true, it will keep logging stage name when start is called, thereby
        // making it easy to determine if call timeout and what stage even if metrics are not printed
        // because invoke function never completes due to timeout.
        // It should be set to false mostly if goal is to collect metrics only.
        public PerformanceMetrics(string name, bool logStg=false)
        {
            this.functionName = name;
            this.logStage = logStg;
        }

        public void Start(string stgName)
        {
            this.stageName = stgName;

            // This is needed only if lambda or function is timing out before final metrics is printed.
            if (this.logStage)
            {
                var dtStr = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Console.WriteLine($"[{dtStr}] CheckIn: {functionName} / {stageName}");
            }

            stopwatch.Restart();
        }

        public void Stop()
        {
            stopwatch.Stop();
            metrics[stageName] = stopwatch.ElapsedMilliseconds;
        }

        public void StopPreviousStartNew(string newStageName)
        {
            // Stop the previous stage's metrics
            Stop();
            
            // Start new stage
            Start(newStageName);
        }

        private Tuple<string, string> GetMetrics(string prefix="")
        {
            // Stop previous stopwatch if it's not been stopped.
            stopwatch.Stop();

            var stages = string.Join(",", metrics.Keys);
            var values = string.Join(",", metrics.Values);
            var total = metrics.Values.Sum();
            var header = $"{prefix} {functionName},{stages}";
            var result = $"{prefix} {total},{values}";

            return Tuple.Create(header, result);
        }

        public void PrintMetrics(LogAction logAction, string prefix="")
        {
            // Short circuit, return early if no metrics logged.
            if (metrics.Count == 0 || logAction == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                logAction("Performance Metrics:");
            }

            var result = GetMetrics(prefix);
             logAction($@"
                 {result.Item1}
                 {result.Item2}
             ");
        }

//        public void PrintMetrics(ILogger logger, string prefix = "")
//        {
//            // Short circuit, return early if no metrics logged.
//            if (metrics.Count == 0 || logger == null)
//            {
//                return;
//            }
//
//            if (!string.IsNullOrEmpty(prefix))
//            {
//                logger.LogError("Performance Metrics:"); // Use appropriate logging level
//            }
//
//            var result = GetMetrics(prefix);
//            logger.LogError(result.Item1);
//            logger.LogError(result.Item2);
//        }
    }
}
