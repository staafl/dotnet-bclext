
using System;
using System.Collections.Generic;

namespace Fairweather.Service
{

    public class Heap<T> where T : struct
    {
        static readonly Comparison<T> def_comparer = (t1, t2) => Comparer<T>.Default.Compare(t1, t2);
        const int def_capacity = 40;
        T[] elems;
        int counter;
        readonly Comparison<T> m_cmp;
        readonly bool m_min_dom;


        public Heap(int initial_capacity, bool min_dominant, Comparer<T> comparer) :
            this(initial_capacity,
                 min_dominant,
                 (t1, t2) => comparer.Compare(t1, t2)) { }

        // would the compiler optimize it away if I had pointed to the second constructor?
        public Heap() : this(def_capacity, true, def_comparer) { }

        public Heap(bool min_dominant) : this(def_capacity, min_dominant, def_comparer) { }

        public Heap(bool min_dominant, Comparison<T> comparer) : this(def_capacity, min_dominant, comparer) { }

        /*       Main Ctor        */

        public Heap(int initial_capacity, bool min_dominant, Comparison<T> comparer) {

            m_cmp = comparer;
            m_min_dom = min_dominant;
            elems = new T[initial_capacity];
            counter = 0;


        }

        public Heap(T[] elements, bool min_dominant, Comparison<T> comparer)
            : this(elements.Length + 10, min_dominant, comparer) {


            int count = elements.Length;
            counter = count;

            if (count == 0)
                return;

            Array.Copy(elements, elems, count);

            int power = A.Next_Power_Of_2(count);

            int parents = power; // number of elements above the last level
            parents >>= 1;
            --parents;

            for (int ii = parents; ii >= 0; --ii) {

                Bubble_Down(ii);

            }

            Verify_Heap();

        }

        bool Dominates(int index1, int index2, bool _) {

            return Dominates(elems[index1], elems[index2]);

        }

        /// <summary> Returns true if the elements are equal
        /// </summary>
        bool Dominates(T elem1, T elem2) {

            int cmp = m_cmp(elem1, elem2);

            if (cmp == 0)
                return true;

            // (cmp == -1) == m_min_down
            if (cmp == -1)
                return m_min_dom;

            if (cmp == 1)
                return !m_min_dom;

            throw new InvalidOperationException();
        }

        static int Left_Child(int index) {

            int ret = index * 2 + 1;

            return ret;
        }

        static int Right_Child(int index) {

            int ret = index * 2 + 2;

            return ret;
        }

        static int Parent(int index) {

            int ret = (index - 1) / 2;

            return ret;

        }

        void Insert(T elem) {

            elems[counter] = elem;
            ++counter;

            Check_Size();

            Bubble_Up(counter);
        }

        void Bubble_Up(int index) {

            do {
                if (index == 0)
                    return;

                var parent_ind = Parent(index);

                var current = elems[index];
                var parent = elems[parent_ind];


                if (Dominates(parent, current))
                    return;

                elems.Swap(parent_ind, index);
                index = parent_ind;

            } while (true);
        }

        void Bubble_Down(int index) {

            do {
                var current = elems[index];

                T new_parent = current;
                var new_index = index;

                var child_l_ind = Left_Child(index);

                for (int ii = 0; ii <= 1; ++ii) {

                    int child_ind = child_l_ind + ii;

                    if (child_ind >= counter)
                        break;

                    var child = elems[child_ind];


                    if (!Dominates(new_parent, child)) {

                        new_parent = child;
                        new_index = child_ind;
                    }
                }

                if (new_parent.Equals(current))
                    return;

                elems.Swap(new_index, index);
                index = new_index;

            } while (true);
        }

        void Verify_Heap() {

            for (int ii = 0; ; ++ii) {

                int child = Left_Child(ii);
                if (child >= counter)
                    break;

                Dominates(ii, child, true).Throw_If_False();

                ++child;
                if (child >= counter)
                    break;

                Dominates(ii, child, true).Throw_If_False();

            }

        }

        public bool Is_Empty {
            get {
                return counter == 0;
            }
        }
        public int Count {
            get {
                return counter;
            }
        }
        public T Top {
            get {
                var ret = elems[0];
                return ret;
            }
        }

        public void Remove_Top() {

            --counter;

            elems[0] = elems[counter];
            Bubble_Down(0);

            Check_Size();
        }

        void Check_Size() {

            int len = elems.Length;
            int free = len - counter;

            if (free <= 1) {

                var new_arr = new T[len * 2];
                Array.Copy(elems, new_arr, counter);
                elems = new_arr;
                return;

            }

            if (len > def_capacity && free > len / 2) {

                var new_arr = new T[len / 2];
                Array.Copy(elems, new_arr, counter);
                elems = new_arr;
                return;
            }
        }
    }

}