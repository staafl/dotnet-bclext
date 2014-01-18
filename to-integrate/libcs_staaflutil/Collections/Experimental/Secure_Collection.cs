using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service
{
    /* Incomplete */
    /* Usage: Pass this collection as IRead<TIndex, TValue> to code which
     * should not modify its contents.
     * 
     * Access to the collection is controlled through one or more key 
     * objects.
     * */
    class Secure_Collection<TIndex, TValue> : IReadWrite<TIndex, TValue>
    {
        public class Key
        {
            bool m_taken_out;
            object m_locker;

            readonly Secure_Collection<TIndex, TValue> m_collection;

            public Secure_Collection<TIndex, TValue> Collection {
                get { return m_collection; }
            }

            public bool Taken_Out {
                get { return m_taken_out; }
            }

            // Talking with the collection

            internal void Take_Out(object id) {

                lock (m_locker) {

                    if (m_taken_out)
                        throw new Invalid_Key_Operation_Exception("Key is already taken.");

                    if (id != m_collection.id)
                        throw new Invalid_Key_Operation_Exception("Access error.");

                    m_taken_out = true;

                }

            }

            internal void Return(object id) {

                lock (m_locker) {

                    if (!m_taken_out)
                        throw new Invalid_Key_Operation_Exception("Key is already returned.");

                    if (id != m_collection.id)
                        throw new Invalid_Key_Operation_Exception("Access error.");

                    m_taken_out = false;

                }

            }

            public Key(Secure_Collection<TIndex, TValue> collection) {

                this.m_locker = new object();
                this.m_collection = collection;

            }
        }

        int keys_available;
        bool locked;

        readonly int max_keys;

        /* Used to identify the collection to the keys.
         
           Should never be exposed to outside code.
         * */
        readonly object id;

        /* Used to synchronize the locking and key-management
           operations.
         
           Should never be exposed to outside code.
         * */
        readonly object locker;

        readonly Key[] keys;
        readonly Dictionary<TIndex, TValue> m_inner_collection;

        public Secure_Collection(int max_keys, bool locked) {

            this.max_keys = max_keys;
            keys_available = max_keys;

            this.locked = locked;

            locker = new object();
            id = new object();

            keys = new Key[max_keys];

            for (int ii = 0; ii < max_keys; ++ii) {

                keys[ii] = new Key(this);

            }

            m_inner_collection = new Dictionary<TIndex, TValue>();
        }

        /* IMPLEMENTATION */

        void Lock() {
            locked = true;
        }

        void Unlock() {
            locked = false;
        }

        Key Get_Free_Key() {

            Key ret = null;

            for (int ii = 0; ii < max_keys; ++ii) {

                if (!keys[ii].Taken_Out) {
                    ret = keys[ii];
                    break;
                }

            }

            return ret;
        }

        /* INTERFACE */

        public IReadWrite<TIndex, TValue> As_Writable {
            get {
                return this as IReadWrite<TIndex, TValue>;
            }
        }

        public bool Return_Key(Key key) {

            lock (locker) {
                bool ret = Is_My_Key(key);

                if (ret) {
                    // Mark the key as available
                    key.Return(id);

                    ++keys_available;
                }

                return ret;
            }
        }

        public Key Get_Key(out bool result) {

            Key key = null;
            bool temp = false;

            lock (locker) {

                temp = keys_available > 0;

                if (temp) {
                    key = Get_Free_Key();

                    if (key.IsNullOrEmpty())
                        throw new InvalidOperationException();

                    // Mark the key as in use
                    key.Take_Out(id);

                    --keys_available;
                }

            }

            result = temp;
            return key;
        }

        public void Lock(Key key) {

            lock (locker) { // Make sure no thread returns the key while we're using it

                if (!key.Taken_Out)
                    throw new Invalid_Key_Operation_Exception("Key is not valid.");

                bool ret = Is_My_Key(key);

                if (!ret)
                    throw new Invalid_Key_Operation_Exception("Key is not valid.");

                Lock();
            }
        }

        public void Unlock(Key key) {

            lock (locker) {

                if (!key.Taken_Out)
                    throw new Invalid_Key_Operation_Exception("Key is not valid.");

                bool ret = Is_My_Key(key);

                if (!ret)
                    throw new Invalid_Key_Operation_Exception("Key is not valid.");

                Unlock();
            }
        }

        public bool Is_My_Key(Key key) {

            bool ret = keys.Contains(key);

            return ret;
        }

        public int Keys_Available {
            get { return keys_available; }
        }

        public int Max_Keys {
            get { return max_keys; }
        }

        public bool Locked {
            get { return locked; }
        }

        /* Temporary Lock */
        /* Experimental */
        public IDisposable Temporary_Lock() {

            IDisposable ret;

            lock (locker)
                ret = new Temporary_Key(this);

            return ret;
        }

        /* Usage: wrap collection.Temporary_Lock() in a using() block to 
         * ensure temporary write access to the collection.
         * 
         * This is not thread safe. Failing to receive an access key results
         * in an exception.
         * */
        class Temporary_Key : IDisposable
        {
            bool disposed;
            readonly Key m_key;

            public Temporary_Key(Secure_Collection<TIndex, TValue> collection) {

                bool result;

                m_key = collection.Get_Key(out result);

                // This could be made to act asynchronously using a WaitHandle
                // Additionally, the main class could use events to signal listeners
                // when keys become available.
                if (!result)
                    throw new Invalid_Key_Operation_Exception("No keys available.");

                collection.Unlock(m_key);
            }

            public void Dispose() {

                lock (m_key) {

                    if (disposed)
                        return;

                    disposed = true;

                    m_key.Collection.Lock(m_key);
                    m_key.Collection.Return_Key(m_key).Throw_If_False();
                }
            }
        }

        // COLLECTION
        TValue IReadWrite<TIndex, TValue>.this[TIndex index] {
            set {
                if (locked)
                    throw new Locked_Collection_Exception();

                m_inner_collection[index] = value;
            }
            get {
                return m_inner_collection[index];
            }
        }

        public virtual TValue this[TIndex index] { get { return m_inner_collection[index]; } }
    }
}
