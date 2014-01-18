
using System;
using System.Collections.Generic;
using System.Threading;

namespace Fairweather.Service
{
    sealed public class HashTable<TKey, TValue>
    {
        static readonly bool is_int = typeof(TKey) == typeof(int);
        static readonly bool is_string = typeof(TKey) == typeof(string);
        
        static readonly EqualityComparer<TKey> comparer = EqualityComparer<TKey>.Default;

        public static HashTable<TKey, TValue> Default { get; set; }

        Node[] spine;

        readonly int m_spine_size;

        Node to_modify;

        readonly object sync_root = new object();

        public object Sync_Root {
            get { return sync_root; }
        }


        // yield foreach for IEnumerable<KVP>?
        // check wes dyer's blog
        public TValue this[TKey index] {
            get {
                TValue ret = default(TValue);
                this.Index(index, ref ret, Indexing_Reason.Get);

                return ret;

            }
            set {
                this.Index(index, ref value, Indexing_Reason.Write);

            }
        }

        public HashTable() : this(hash_size) { }

        private HashTable(int spine_size) {

            m_spine_size = spine_size;

            Clear();

        }

        public void Clear() {

            spine = new Node[m_spine_size];

        }

        long m_version;
        int m_count;

        public int Count {
            get { return m_count; }
        }

        int cnt;

        public int Cnt {
            get { return cnt; }
            set { cnt = value; }
        }

        /*       Interface        */

        public void Add(TKey key, TValue value) {

            Index(key, ref value, Indexing_Reason.Add);

        }

        // TryInsertValue?
        public bool TryGetValue(TKey key, out TValue value) {

            value = default(TValue);

            return Index(key, ref value, Indexing_Reason.Try_Get);

        }

        /// <summary> This method is not threadsafe </summary>
        public bool TryModify(TKey key, Modifier modifier) {


            TValue temp = default(TValue);

            if (!Index(key, ref temp, Indexing_Reason.Modify))
                return false;

            to_modify.m_value = modifier(to_modify.m_value);

            return true;
        }

        /// <summary> This method is not threadsafe
        /// On success "value" is the value of the modified key
        /// Otherwise "value" is undefined </summary>
        public bool TryModify(TKey key, Modifier modifier, out TValue value) {

            value = default(TValue);
            TValue temp = default(TValue);

            if (!Index(key, ref temp, Indexing_Reason.Modify))
                return false;

            to_modify.m_value = modifier(to_modify.m_value);
            value = to_modify.m_value;

            return true;
        }

        public bool Remove(TKey key) {

            var temp = default(TValue);

            var ret = Index(key, ref temp, Indexing_Reason.Remove);

            return ret;

        }


        /*       Implementation        */

        // Indexing_Reason.Get -> "value" will contain the indexing result,
        //                        if "key" is not present, a KeyNotFoundException is thrown
        //                        return is always true
        //                        
        // Indexing_Reason.Add -> "value" is passed with the value to add
        //                        if "key" is already present, an InvalidOperationException
        //                             is thrown
        //                        return is always true
        //                        
        // Indexing_Reason.Write -> "value" is passed with the value to add   
        //                          if "key" is already present, its value will be overwritten
        //                          return is always true
        //                          
        // Indexing_Reason.TryGet -> "value" will contain the indexing result;
        //                           if "key" is not present, return is false,
        //                             otherwise return is true          
        //                           
        // Indexing_Reason.Modify -> "value" is ignored
        //                           if "key" is found, "to_modify" is set to the triple containing the key
        //                             and return is true
        //                             otherwise return is false  
        //                             
        // Indexing_Reason.Modify -> "value" is ignored
        //                           if "key" is found, its value is removed from the hashtable
        //                             and return is true
        //                             otherwise return is false                           
        //                             
        bool Index(TKey key, ref TValue value, Indexing_Reason reason) {

            key.Throw_If_Null();

            int hash = (key.GetHashCode()) % m_spine_size;
            var main_table = this;
            
            // used for recalculating the hash-code
            // on deep indexing
            int scratch1 = 0;
            //int scratch2 = 0;

            if (is_int) {
                scratch1 = (int)(IConvertible)key;
            }
            else if (is_string) {
                scratch1 = (key.ToString()).Length;
            }

            Node node;
            Node[] spine;

            do {
                spine = main_table.spine;
                node = spine[hash];

                /*       Key is not found        */

                if (node == null) {

                    if (reason == Indexing_Reason.Get)
                        throw new KeyNotFoundException();

                    if (((int)reason & returns_false) == returns_false)
                        return false;

                    //Interlocked.Increment(ref m_count);
                    
                    unchecked {

                        /*       Is this threadsafe?        */

                        ++m_version;
                    }

                    spine[hash] = new Node(null, key, value);

                    return true;

                }

                /*       Key is found        */

                if (comparer.Equals(node.m_key, key)) {

                    switch (reason) {
                    case Indexing_Reason.Write: {

                            node.m_value = value;

                            return true;
                        }

                    case Indexing_Reason.Add:
                        throw new InvalidOperationException("key_present");

                    case Indexing_Reason.Get:
                    case Indexing_Reason.Try_Get: {
                            value = node.m_value;
                            return true;
                        }

                    case Indexing_Reason.Modify:
                        to_modify = node;
                        return true;

                    case Indexing_Reason.Remove: {
                            Remove(ref node);
                            spine[hash] = node;
                            return true;
                        }
                    default:
                        throw new Logic_Exception();
                    }

                }

                // A different key with the same hash has been found
                // Examine the key's bucket and if needed, instantiate it

                var bucket = node.m_bucket;

                if (bucket == null) {    // Bucket is not instantiated

                    if (reason == Indexing_Reason.Get)
                        throw new KeyNotFoundException();

                    if (((int)reason & returns_false) == returns_false)
                        return false;

                    bucket = new HashTable<TKey, TValue>(bucket_size);

                    node.m_bucket = bucket;

                }

                // Prepare to search the bucket
                cnt++;
                main_table = bucket;
                

                //++scratch;
                if(is_int){
                    hash = B.RotateLeft(scratch1, hash % 31);
                   hash &= ~(1 << 31); // abs
                }
                else if(is_string){
                    hash = scratch1;
                }
                else{
                    continue;
                }
                    
                hash %= bucket_size;
                /*                // Is this safe?
                if(is_int){
                   hash = key.RotateLeft(scratch);
                   hash &= ~(1 << 31); // abs
                }
                else if(is_string){
                    hash = (scratch1 % (scratch2 + 1))
                                    hash = (key.ToString().GetHashCode()).GetHashCode();
                hash &= ~(1 << 31);
                */
                



               // hash = hash.GetHashCode() % bucket_size;

            } while (true);

        }

        /*       Does not work        */

        void Remove(ref Node node) {

            var main_table = this;
            Node sub_triple;
            HashTable<TKey, TValue> bucket;

            do {
                Interlocked.Decrement(ref main_table.m_count);

                bucket = node.m_bucket;

                if (bucket == null) {

                    node = null;
                    return;

                }

                sub_triple = bucket.Any_Bucket();

                node.m_key = sub_triple.m_key;
                node.m_value = sub_triple.m_value;


                node = sub_triple;
                main_table = bucket;

            } while (true);

        }

        Node Any_Bucket() {

            (m_count == 0).Throw_If_True();

            for (int ii = 0; ii < spine.Length; ++ii) {

                var item = spine[ii];
                if (item == null)
                    continue;

                return item;

            }

            throw new Logic_Exception();

        }

        /*       Definitions        */

        // http://www.cs.arizona.edu/icon/oddsends/primes.htm

        const int hash_size = 7919;//80021;//7919;
        const int bucket_size = 73;
        const string key_present = "Key already present in HashTable";

        public delegate TValue Modifier(TValue arg);


        const int throws_on_found = 0x100;
        const int throws_on_not_found = 0x1000;
        /// <summary> on not found </summary>
        const int returns_false = 0x10000;

        enum Indexing_Reason
        {
            Write,

            Add = throws_on_found | 0x1,
            Get = throws_on_not_found | 0x1,

            Try_Get = returns_false | 0x2,
            Modify = returns_false | 0x4,
            Remove = returns_false | 0x8,
        }
        
        sealed class Node
        {

            public HashTable<TKey, TValue> m_bucket;
            public TKey m_key;
            public TValue m_value;

            public Node(HashTable<TKey, TValue> bucket,
                        TKey  key,
                        TValue value){

                this.m_bucket = bucket;
                this.m_key = key;
                this.m_value = value;
            }

        }

    }
}