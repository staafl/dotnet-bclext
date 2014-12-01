using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using DOT.Logging;
using DOT.Metrics;
using DOT.Metrics.Publishing;

namespace Tick42.HotButtons.Utils
{
    public class MetricsAndLogging
    {
        private readonly IPublishingMetricSystem metrics_;
        private readonly ISmartLogger logger_;

        public MetricsAndLogging(IPublishingMetricSystem metrics, ISmartLogger logger)
        {
            metrics_ = metrics;
            logger_ = logger;
        }

        public MetricsAndLogging Subnode(object obj)
        {
            new { obj }.NotNull();
            return Subnode(obj.GetTypeName());
        }

        public MetricsAndLogging Subnode(string subnode)
        {
            if (metrics_ == null)
            {
                return this;
            }
            return new MetricsAndLogging(metrics_.GetOrCreateChild(subnode), this.logger_);
        }

        public void Set(
            string metric,
            object value,
            StringMetricOptions options = null,
            MetricConflationMode conflation = 0)
        {
            if (metrics_ != null)
            {
                CreateStringMetric(metric, options, conflation)
                    .SafeSetValue(value.ToFriendlyString());
            }
        }

        public void Inc(
            string metric,
            CountMetricOptions options = null,
            MetricConflationMode conflation = 0,
            int incrementBy = 1)
        {
            if (metrics_ != null)
            {
                CreateCountMetric(metric, options, conflation)
                    .IncrementBy(incrementBy);
            }
        }

        public bool Log(
            string message,
            LogLevel logLevel = LogLevel.Debug)
        {
            if (this.logger_ != null)
            {
                this.logger_.Log(logLevel, message);
                return true;
            }
            return false;
        }

        public IDisposable LogBracket(
            string message,
            LogLevel logLevel = LogLevel.Debug)
        {
            if (this.logger_ != null)
            {
                return CreateLoggingBracket(message, logLevel);
            }
            return Disposable.Empty;
        }

        public void LogAndSet(
            string metric,
            object value,
            string message,
            LogLevel logLevel = LogLevel.Debug,
            StringMetricOptions options = null,
            MetricConflationMode conflation = 0)
        {
            if (logger_ != null)
            {
                logger_.Log(logLevel, message);
            }
            if (metrics_ != null)
            {
                var stringMetric = CreateStringMetric(metric, options, conflation);
                stringMetric.SafeSetValue(value.ToFriendlyString());
            }
        }

        public IDisposable LogThenSet(string metric,
            Func<object> value,
            string message,
            LogLevel logLevel = LogLevel.Debug,
            StringMetricOptions options = null,
            MetricConflationMode conflation = 0)
        {
            var disposable = Disposable.Empty;

            if (logger_ != null)
            {
                disposable = CreateLoggingBracket(message, logLevel);
            }

            if (metrics_ != null)
            {
                var stringMetric = CreateStringMetric(metric, options, conflation);
                disposable = new CompositeDisposable(
                    disposable,
                    Disposable.Create(() => stringMetric.SafeSetValue(value().ToFriendlyString())));
            }

            return disposable;
        }

        private IDisposable CreateLoggingBracket(string message, LogLevel logLevel)
        {
            logger_.Log(logLevel, message + " (before)");
            return Disposable.Create(
                () => logger_.Log(logLevel, message + " (after)"));
        }


        private IStringMetric CreateStringMetric(
            string metric,
            StringMetricOptions options,
            MetricConflationMode conflation)
        {
            var countMetric =
                SimpleCache.GetOrCreate(
                    () =>
                        metrics_.GetOrCreateStringMetric(
                            metric,
                            options ?? new StringMetricOptions().WithConflation(conflation)),
                    "MetricsAndLogging.GetOrCreateCountMetric",
                    metric);
            return countMetric;
        }

        public void LogAndInc(string metric, string message, LogLevel logLevel = LogLevel.Debug, CountMetricOptions options = null, MetricConflationMode conflation = 0, int incrementBy = 1)
        {
            if (logger_ != null)
            {
                logger_.Log(logLevel, message);
            }
            if (metrics_ != null)
            {
                var countMetric = CreateCountMetric(metric, options, conflation);
                countMetric.IncrementBy(incrementBy);
            }
        }

        public IDisposable LogThenInc(string metric, string message, LogLevel logLevel = LogLevel.Debug, CountMetricOptions options = null, MetricConflationMode conflation = 0, int incrementBy = 1)
        {
            var disposable = Disposable.Empty;

            if (logger_ != null)
            {
                disposable = CreateLoggingBracket(message, logLevel);
            }

            if (metrics_ != null)
            {
                var countMetric = CreateCountMetric(metric, options, conflation);
                disposable = new CompositeDisposable(
                    disposable,
                    Disposable.Create(() => countMetric.IncrementBy(incrementBy)));
            }

            return disposable;
        }

        private ICountMetric CreateCountMetric(
            string metric,
            CountMetricOptions options,
            MetricConflationMode conflation)
        {
            var countMetric =
                SimpleCache.GetOrCreate(
                    () =>
                        metrics_.GetOrCreateCountMetric(
                            metric,
                            options ?? new CountMetricOptions().WithConflation(conflation)),
                    "MetricsAndLogging.GetOrCreateCountMetric",
                    metric);
            return countMetric;
        }
    }
}
