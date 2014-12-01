using System;
using System.Collections.Generic;
using System.Linq;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Data describing an Exception as an event, with additional context. See ExceptionStreamHelpers.
    /// </summary>
    public class ExceptionStreamData : EventArgs
    {
        readonly Queue<Tuple<string, object>> contextQueue_;
        private bool handled_;

        public ExceptionStreamData(Exception exception, IEnumerable<Tuple<string, object>> context, bool canHandle, IEnumerable<string> owners)
        {
            Owners = owners.OrEmpty();
            CanHandle = canHandle;
            Exception = exception;
            Context = contextQueue_ = new Queue<Tuple<string, object>>();
            foreach (var contextItem in context.OrEmpty())
            {
                contextQueue_.Enqueue(contextItem);
            }
        }

        public bool CanHandle { get; private set; }
        public bool Logged { get; set; }
        public bool Handled
        {
            get
            {
                if (CanHandle)
                {
                    return handled_;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (CanHandle)
                {
                    handled_ = value;
                }
                else
                {
                    handled_ = false;
                }
            }
        }

        public Exception Exception { get; private set; }
        public IEnumerable<Tuple<string, object>> Context { get; private set; }
        public IEnumerable<string> Owners { get; private set; }

        public object this[string key]
        {
            get
            {
                return Context.FirstOrDefault(t => t.Item1 == key);
            }
        }

        public ExceptionStreamData AddContext(IEnumerable<Tuple<string, object>> context)
        {
            var newData = new ExceptionStreamData(Exception, contextQueue_, CanHandle, Owners);
            foreach (var contextItem in context.OrEmpty())
            {
                newData.contextQueue_.Enqueue(contextItem);
            }
            return newData;
        }

        public override string ToString()
        {
            return string.Format(
                "Handled: {0}, Exception: {1},  CanHandle: {2}, Context:[{3}]",
                Handled,
                Exception,
                CanHandle,
                string.Join(",", Context));
        }
    }
}