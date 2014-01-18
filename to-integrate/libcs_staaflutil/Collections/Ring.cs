using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
      public class Ring<TValue>
            : IReadWrite<int, TValue>
            , IEnumerable<TValue>
      {

            // ****************************

            public Ring(int length)
                  : this(length, false) {

            }

            public Ring(int length, bool reflected) {

                  m_reflected = reflected;
                  m_length = length;
                  m_array = new TValue[m_length];

            }

            public Ring(bool reflected, params TValue[] elements) {

                  m_reflected = reflected;
                  m_length = elements.Length;
                  m_array = elements.ToArray();

            }

            public Ring(List<TValue> list, bool reflected) {

                  m_reflected = reflected;
                  m_length = list.Count;
                  m_array = list.ToArray();

            }

            // ****************************

            const int sign_bit = (1 << 31);

            static readonly EqualityComparer<TValue>
            comparer = EqualityComparer<TValue>.Default;

            readonly bool m_reflected;
            readonly int m_length;
            readonly TValue[] m_array;

            int m_current_index = 0;

            // ****************************


            public bool Reflected {
                  get { return m_reflected; }
            }

            public int Length {
                  get { return m_length; }
            }



            // ****************************

            public bool Contains(int index) {
                  return 0 <= index && index < m_length;
            }

            public TValue this[int index] {
                  get {
                        index = Get_Index(index);

                        var ret = m_array[index];

                        return ret;

                  }
                  set {
                        index = Get_Index(index);

                        if (comparer.Equals(value, m_array[index]))
                              return;

                        m_array[index] = value;
                  }

            }


            // ****************************


            int Get_Index(int index) {

                  bool sign = (index & sign_bit) == 0x1;

                  index &= ~sign_bit;

                  index %= m_length;

                  if (m_reflected != sign) // xor
                        index = m_length - index;

                  return index;

            }


            public IEnumerable<TValue> Loop() {

                  return Loop(true, -1);

            }

            public IEnumerable<TValue> Loop(bool forward, int total_loops) {

                  bool temp = !m_reflected;

                  if (!forward) {
                        goto MIDDLE;
                  }

            START:

                  do {
                        if (total_loops-- == 0)
                              goto END;

                        for (int ii = m_current_index; ii < m_length; ++ii) {

                              yield return m_array[ii];

                        }

                        for (int ii = 0; ii < m_current_index; ++ii) {

                              yield return m_array[ii];

                        }

                  } while (temp);


            MIDDLE:

                  do {
                        if (total_loops-- == 0)
                              goto END;

                        for (int ii = m_current_index; ii >= 0; --ii) {

                              yield return m_array[ii];

                        }

                        for (int ii = m_length - 1; ii > m_current_index; --ii) {

                              yield return m_array[ii];

                        }
                  } while (temp);

                  goto START;
            END: yield break;
            }

            public IEnumerator<TValue> GetEnumerator() {


                  foreach (var item in this.Loop(true, 1)) {
                        yield return item;
                  }

            }

            IEnumerator IEnumerable.GetEnumerator() {

                  return this.GetEnumerator();
            }

            // ****************************

            public TValue Current {
                  get {
                        return m_array[m_current_index];
                  }
                  set {
                        m_array[m_current_index] = value;
                  }
            }


            public bool MoveNext() {
                  m_current_index = Get_Index(m_current_index + 1);
                  return true;

            }

            public bool MovePrev() {
                  m_current_index = Get_Index(m_current_index - 1);
                  return true;

            }

            // ****************************

            /*       Old IEnumerator<TValue> impl        */

           // , IEnumerator<TValue>

           //object IEnumerator.Current {
           //       get {
           //             return m_array[m_current_index];
           //       }
           // }

           // public void Dispose() {

           //       m_current_index = 0;

           // }

           // public void Reset() {

           //       m_current_index = 0;


           // }

      }
}