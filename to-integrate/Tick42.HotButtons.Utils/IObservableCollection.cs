using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Represents the functionality of an ObservableCollection.
    /// </summary>
    /// <typeparam name="TElem"></typeparam>
    public interface IObservableCollection<TElem> : INotifyPropertyChanged, INotifyCollectionChanged,
        IEnumerable<TElem>, IEnumerable, ICollection<TElem>
    {
    }
}