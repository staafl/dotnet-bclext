using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     A wrapper for propagating exceptions through an IObserver of ExceptionStreamData.
    /// </summary>
    // owning Subject<>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class ExceptionStreamHelper
    {
        readonly string owner_;
        readonly Subject<ExceptionStreamData> exceptionSubject_;
        readonly Stack<Tuple<string, object>> tempContexts_;
        readonly Queue<Tuple<string, object>> persistentContexts_;
        private readonly HashSet<string> owners_;

        ExceptionStreamHelper(string owner, Subject<ExceptionStreamData> exceptionSubject, IEnumerable<Tuple<string, object>> persistentContexts, Stack<Tuple<string, object>> tempContexts, HashSet<string> owners)
        {
            owner_ = owner;
            exceptionSubject_ = exceptionSubject;
            persistentContexts_ = new Queue<Tuple<string, object>>();
            foreach (var persistent in persistentContexts)
            {
                persistentContexts_.Enqueue(persistent);
            }
            tempContexts_ = tempContexts;
            owners_ = owners;
        }

        /// <summary>
        /// Creates a new instance with the specified owner.
        /// </summary>
        public ExceptionStreamHelper(string owner)
        {
            owner_ = owner;
            owners_ = new HashSet<string> { owner };
            persistentContexts_ = new Queue<Tuple<string, object>>();
            tempContexts_ = new Stack<Tuple<string, object>>();
            exceptionSubject_ = new Subject<ExceptionStreamData>();
        }

        public ExceptionStreamHelper Clone(object owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            return Clone(owner.GetTypeName());
        }

        /// <summary>
        /// Creates a new stream wrapper with the same state as this one, but with a different owner.
        /// </summary>
        /// <param name="owner">The name of the new owner, used to qualify context when exceptions are
        /// propagated upstream.</param>
        /// <returns>The new wrapper.</returns>
        public ExceptionStreamHelper Clone(string owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            var newHelper = new ExceptionStreamHelper(owner, exceptionSubject_, persistentContexts_, tempContexts_, new HashSet<string>(owners_));
            newHelper.owners_.Add(owner);
            return newHelper;
        }

        /// <summary>
        /// Adds a context object that will be dumped with every subsequent exception from
        /// this stream, until the returned IDisposable is disposed. The context properties are qualified
        /// with the name of this instance's Owner.<para/>
        /// NB: It is important that the IDisposables are properly disposed and in the order they are
        /// created; consider using a 'using()' statement from C#.
        /// </summary>
        /// <param name="context">An object whose properties will be interrogated for context
        /// when pushing exceptions upstream.</param>
        public IDisposable AddTemporaryContext(object context)
        {
            if (context == null)
            {
                throw this.Throw(new ArgumentNullException("context"));
            }
            // Todo: guard against corruption by improper disposing/lack of disposing
            tempContexts_.Push(Tuple.Create(Owner, context));
            return Disposable.Create(() => tempContexts_.Pop());
        }

        /// <summary>
        /// Adds a context object that will be dumped with every subsequent exception from
        /// this stream. The context properties are qualified with the name of this instance's Owner.
        /// </summary>
        /// <param name="context">An object whose properties will be interrogated for context
        /// when pushing exceptions upstream.</param>
        public void AddPersistentContext(object context)
        {
            if (context == null)
            {
                throw this.Throw(new ArgumentNullException("context"));
            }
            persistentContexts_.Enqueue(Tuple.Create(Owner, context));
        }

        /// <summary>
        /// The stream on which this helper publishes the exceptions.
        /// </summary>
        public IObservable<ExceptionStreamData> ExceptionStream
        {
            get
            {
                return this.exceptionSubject_.AsObservable();
            }
        }

        /// <summary>
        /// The name of this instance's owner. Used to qualify the context property names.
        /// </summary>
        public string Owner
        {
            get { return owner_; }
        }

        public IEnumerable<string> Owners
        {
            get { return owners_; }
        }

        /// <summary>
        /// Propagates an exception upstream, together with any currently attached context, 
        /// allowing any observers to log and/or handle it. If the exception is unhandled, it is
        /// rethrown at the end of the method.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        /// <param name="context"></param>
        public void HandleOrThrow(
            Exception ex,
            object context = null)
        {
            NotifyHandleOrThrow(ex, context, canHandle: true, notifyOnly: false);
        }

        /// <summary>
        /// Propagates an exception upstream, together with any currently attached context, 
        /// allowing any observers to log it. The exception is not thrown.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public Exception Notify(
            Exception ex,
            object context = null)
        {
            return NotifyHandleOrThrow(ex, context, canHandle: true, notifyOnly: true);
        }

        /// <summary>
        /// Propagates an exception upstream, together with any currently attached context, 
        /// allowing any observers to log it. The exception cannot be handled, but is rethrown
        /// at the end of the method.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public Exception Throw(
            Exception ex,
            params object[] context)
        {
            return NotifyHandleOrThrow(ex, context, canHandle: false, notifyOnly: false);
        }

        /// <summary>
        /// Returns a sequence of tuples from a context object. 
        /// For each property of 'context', the resulting tuple's Item1 is 'this.Owner' + property.Name, and Item2 is property.Value.
        /// </summary>
        static IEnumerable<Tuple<string, object>> GetContextItems(string owner, object context)
        {
            if (context == null)
            {
                return new Tuple<string, object>[0];
            }

            return context.ReflectDeclaredProperties()
                .Select(p => Tuple.Create(owner + "." + p.Item1, (object) p.Item2.ToFriendlyString()));
        }

        // main overload
        Exception NotifyHandleOrThrow(
            Exception ex,
            object context,
            bool canHandle,
            bool notifyOnly)
        {
            // todo: current stack trace
            var data = new ExceptionStreamData(ex, GetContextItems(Owner, context), canHandle, Owners);
            foreach (var otherContext in tempContexts_.OrEmpty().Concat(persistentContexts_.OrEmpty()))
            {
                data = data.AddContext(GetContextItems(otherContext.Item1, otherContext.Item2));
            }
            exceptionSubject_.OnNext(data);

            if (notifyOnly || (canHandle && data.Handled))
            {
                return data.Exception;
            }
            else
            {
                // data.Exception.Data["context"] = data.ToFriendlyString();
                throw data.Exception;
            }
        }
    }
}