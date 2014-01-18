using System;
using System.Collections;
using System.Collections.Generic;
namespace Fairweather.Service
{
    public class Sorted_List<T> : ICollection<T>
    {
        readonly SortedList<T, bool> inner;

        public Sorted_List() {
            inner = new SortedList<T, bool>();
        }

        public Sorted_List(int capacity) {
            inner = new SortedList<T, bool>(capacity);
        }

        public Sorted_List(IEnumerable<T> elems) {
            inner = new SortedList<T, bool>(elems.Pairs(_ => false).dict());

        }
        public Sorted_List(params T[] elems)
            : this(elems as IEnumerable<T>) {

        }


        public IEnumerator<T> GetEnumerator() {

            return inner.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {

            return inner.Keys.GetEnumerator();
        }
        public T this[int index] {
            get {
                return inner.Keys[index];
            }
        }



        public void CopyTo(T[] array, int index) {
            throw new NotImplementedException();
        }

        public void Add(T elem) {
            inner.Add(elem, false);
        }
        public bool Remove(T elem) {
            return inner.Remove(elem);
        }
        public void Clear() {
            inner.Clear();
        }
        public bool Contains(T elem) {
            return inner.ContainsKey(elem);
        }
        public int Count {
            get {
                return inner.Count;
            }
        }

        public bool IsReadOnly {
            get { return false; }
        }


    }
}
