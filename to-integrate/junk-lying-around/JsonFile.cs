using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

enum FlushPolicy
{
    FlushAlways,
    ExplicitOnly,
    SpecifiedInterval
}
enum ConflictResolution
{
    OursFull,
    TheirsFull,
    CustomHandler,
    Exception
}
enum LoadingPolicy
{
    LoadAll,
    LoadOnDemand,
    NoLoad
}
enum DeserializationPolicy
{
    Eager,
    Lazy
}



// FlushPolicy can be a struct
// containing number of writes between flushes
// and etc information

// todo:
// jf.Getter.Prop1.Prop2

namespace staafl.libcs
{
    class JsonFile : IDisposable // , IDictionary<TK, TV>
    {
        public JsonFile(string file,
                        bool newFile = true,
                        int capacity = 100,
                        FlushPolicy flushPolicy = 0)
        {

            this.dict = new Dictionary<string, string>(capacity);
            this.file = file;
            this.flushPolicy = flushPolicy;

            if (!newFile && File.Exists(file))
            {
                Reload();
            }
            else
            {
                File.Delete(file);
            }
        }

        public void Dispose()
        {
            MaybeFlush();
        }

        // todo: string buffer

        readonly Dictionary<string, string> dict;
        readonly string file;
        readonly FlushPolicy flushPolicy;

        public void Reload()
        {

            dict.Clear();

            // todo: is there a better approach?

            if (!File.Exists(file))
                return;

            foreach (var line in File.ReadLines(file))
            {
                var split = line.Split(new[] { ':' }, 2);

                dict[split[0]] = split[1];
            }

        }

        public void Flush()
        {
            File.WriteAllLines(file, this.dict.Select(kvp =>
                kvp.Key + ":" + kvp.Value));
        }

        public bool Dirty { get; private set; }

        public bool MaybeFlush()
        {
            if (!this.Dirty)
            {
                return false;
            }
            this.Flush();
            return true;
        }

        // Count
        // some mixins would be nice, C# team!


        public dynamic this[object key]
        {
            get { return Get<object>(key); }
            set { Set<object>(key, value); }
        }

        public TV Get<TV>(object key)
        {
            return JsonConvert.DeserializeObject<TV>(this.dict[key.ToString()]);
        }

        public void Set<TV>(object key, TV value)
        {
            this.dict[key.ToString()] = JsonConvert.SerializeObject(value);
            this.Dirty = true;
        }

        public bool TryGetValue<TV>(object key, out TV value)
        {
            value = default(TV);
            string temp;
            if (!dict.TryGetValue(key.ToString(), out temp))
                return false;
            value = JsonConvert.DeserializeObject<TV>(temp);
            return true;

        }

    }
}