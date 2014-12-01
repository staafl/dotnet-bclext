using System;
using System.Windows;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Used to bind Routed Events to callbacks via attached behaviors.
    /// </summary>
    public static class RoutedEventBehaviorFactory
    {
        public static DependencyProperty CreateEventToCallbackBehavior<T>(
            RoutedEvent routedEvent,
            string propertyName,
            Type ownerType,
            Action<DependencyObject, EventArgs, T> behaviorCallback,
            bool handledEventsToo = false)
        {
            // attached behavior
            var behavior = new RoutedEventCallbackBehaviour<T>(routedEvent, behaviorCallback, handledEventsToo);

            // register the attached behavior as an attached property and subscribe its PropertyChangedHook
            // callback to property changes
            DependencyProperty property =
                DependencyProperty.RegisterAttached(
                    propertyName,
                    typeof(T),
                    ownerType,
                    new PropertyMetadata(default(T), behavior.PropertyChangedHook));

            return property;
        }

        /// <summary>
        ///     An attached behavior that uses an attached property to invoke a callback when a routed
        ///     event is raised.
        /// </summary>
        /// <typeparam name="T">The type of the attached property used for obtaining a reference to the element we're attaching to.</typeparam>
        private class RoutedEventCallbackBehaviour<T>
        {
            private readonly Action<DependencyObject, EventArgs, T> behaviorCallback_;
            private readonly bool handledEventsToo_;
            private readonly RoutedEvent routedEvent_;
            private DependencyProperty property_;

            public RoutedEventCallbackBehaviour(
                RoutedEvent routedEvent,
                Action<DependencyObject, EventArgs, T> behaviorCallback,
                bool handledEventsToo)
            {
                routedEvent_ = routedEvent;
                behaviorCallback_ = behaviorCallback;
                handledEventsToo_ = handledEventsToo;
            }

            /// <summary>
            /// Attached property hook. Attaches/detaches EventHandler on setting/unsetting.
            /// </summary>
            public void PropertyChangedHook(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                // the first time the property changes, save it for reference
                property_ = property_ ?? e.Property;

                object oldValue = e.OldValue;
                object newValue = e.NewValue;

                var element = sender as UIElement;
                if (element == null)
                {
                    return;
                }
                if (oldValue != null)
                {
                    element.RemoveHandler(routedEvent_, (RoutedEventHandler) EventHandler);
                }
                if (newValue != null)
                {
                    element.AddHandler(routedEvent_, (RoutedEventHandler) EventHandler, handledEventsToo_);
                }
            }

            private void EventHandler(object sender, EventArgs e)
            {
                var dp = sender as DependencyObject;
                if (dp == null || property_ == null)
                {
                    return;
                }

                var value = (T) dp.GetValue(property_);
                behaviorCallback_(dp, e, value);
            }
        }
    }
}