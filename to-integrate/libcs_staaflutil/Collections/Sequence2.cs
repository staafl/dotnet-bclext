using System.Collections.Generic;
using System.Linq;


namespace Fairweather.Service
{
    /// <summary>
    /// A sequence of values allowing for insertion and deletion. 
    /// This class can be useful when tracking, for example, the rows on a grid (in that case, I would
    /// however recommend using the actual row objects as indices to a dictionary).
    /// It's closer to a List[T] than to a SortedList[int,T]
    /// </summary>
    public class Sequence<T> : SortedList<int, T>
    //, IEnumerable<T>, IEnumerable<KeyValuePair<int, T>>, System.Collections.IEnumerable
    {

        public Sequence() : base() { }

        public Sequence(int capacity)
            : base(capacity) {

        }

        public Sequence(IEnumerable<T> elems) : this(elems, 0) { }

        public Sequence(IEnumerable<T> elems, int start)
            : base(elems.Ixed(start).dict()) {
        }

        Sequence(IEnumerable<Pair<int, T>> elems)
            : base(elems.dict()) {
        }

        // ****************************


        ///  Inserts an element at the specified location.
        ///  All elements to the right of the new element 
        ///  are shifted one position to the right, i.e. are
        ///  incremented
        public void Insert(int ix, T value) {

            Insert_Range(ix, new T[] { value });

        }

        ///  Inserts count elements starting at the specified 
        ///  location. All elements to the right of the inserted
        ///  range are shifted to the right
        public void Insert_Range(int start, IEnumerable<T> values) {

            (start < 0).tift();

            values = values.Force();

            var count = values.Count();

            if (start < this.Count)
                this.Rekey(ind =>
                {
                    return Pair.Make(ind >= start, ind + count);
                });

            this.rwi().Fill(values.Ixed(start));
        }

        /// Erases the specified element of values. All values to the 
        /// right of the deleted element are shifted left, i.e. decremented.
        public new void RemoveAt(int ix) {

            (ix < 0).tift("Only non-negative values allowed.");

            if (!Remove(ix))
                return;

            this.Rekey(key => Pair.Make(key > ix, key - 1));

            //            Remove_Range(index, 1);

        }

        /// Erases the specified range of values. All values to the 
        /// right of the deleted range are shifted left.
        public void Remove_Range(int start, int count) {

            (start < 0).tift();
            (count < 1).tift();

            int last = start + count - 1;

            this.Drop(Magic.range(start, last));

            this.Rekey(ind => Pair.Make(ind > last, ind - count));
        }

        // ****************************

        /// Either adds or removes the selected element
        /// from the sequence without shifting any of the
        /// other elements.
        public void Set(int ix, T value) {

            (ix < 0).tift("Only non-negative values allowed.");

            this[ix] = value;

        }

        ///
        public void Set_Range(int start, int count, T value) {

            (count < 1).tift();
            (start < 0).tift();

            for (int ii = 0; ii < count; ++ii)
                this[start + ii] = value;

        }

        // ****************************


        public IEnumerable<int> Indices {
            get {
                foreach (var key in Keys)
                    yield return key;
            }
        }

        public Sequence<T> Invert(T def_element) {

            var first_key = this.Keys.First();
            var last_key = this.Keys.Last();

            var keys = this.Keys;

            var list = new List<int>();

            for (int ii = 0; ii < last_key; ++ii) {

                if (!keys.Contains(ii))
                    list.Add(ii);

            }

            return new Sequence<T>(list.Zip(def_element.Repeat()));

        }

        // ****************************

        /// <summary>
        /// Whether the sequence contains a value at index 'ix'
        /// </summary>
        /// <param name="ix"></param>
        /// <returns></returns>
        public bool Contains(int ix) {

            (ix < 0).tift("Only non-negative values allowed.");

            return this.ContainsKey(ix);

        }


        //public IEnumerable<Pair<int, T>> Elems() {

        //    return this.Cast<Pair<int, T>>();

        //}

        //IEnumerator<KeyValuePair<int, T>>
        //IEnumerable<KeyValuePair<int, T>>.GetEnumerator() {
        //    return base.GetEnumerator();
        //}


        //public new IEnumerator<T> GetEnumerator() {
        //    foreach (var kvp in this as IEnumerable<KeyValuePair<int, T>>)
        //        yield return kvp.Value;
        //}

        //System.Collections.IEnumerator
        //System.Collections.IEnumerable.GetEnumerator() {
        //    return this.GetEnumerator();
        //}
        //// ****************************




    }
}
