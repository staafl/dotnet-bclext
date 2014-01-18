using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
enum FlushPolicy { }
enum ConflictResolution { }

class Program
{
    static void Main() {
        using (var fd = new FileDictionary<string, string>("test.txt")) {
            fd["Bulgaria"] = "Sofia";
            fd["Spain"] = "Madrid";
            fd["Switzerland"] = "Bern";
            Console.WriteLine(fd["Bulgaria"]);
        }
    }
}


// FlushPolicy can be a struct
// containing number of writes between flushes
// and etc information


// todo: duplicates?
// todo: key ordering?
// todo: escapable =
// todo: multiline data - use encoding
class FileDictionary<TK, TV> : IDisposable // , IDictionary<TK, TV>
{
    // use fixed key length?
    public FileDictionary(string file, bool new_file = false)
        : this(file, new_file, 100, @"^(\w+) = (.*)", "{0} = {1}", 0) {
    }
    public FileDictionary(string file, bool new_file, int capacity, string get_pattern, string set_format, FlushPolicy flush_policy) {

        this.dict = new Dictionary<TK, TV>(capacity);
        this.file = file;
        this.get = MakeGetter(get_pattern);
        this.set = MakeSetter(set_format);
        this.flush_policy = flush_policy;

        if (!new_file && File.Exists(file)) {
            Reload();
        }
        else {
            File.WriteAllText(file, "");
        }


    }


    static Func<TK, TV, string> MakeSetter(string format) {
        return (_k, _v) => { return string.Format(format, _k, _v); };
    }

    static Func<string, Tuple<TK, TV>> MakeGetter(string pattern) {
        var rx = new Regex(pattern);
        // todo: check regex
        return _str => {
            var _match = rx.Match(_str);
            if (!_match.Success) {
                return null;
            }
            return Tuple.Create(GetKey(_match.Groups[1].Value), GetValue(_match.Groups[2].Value));
        };
    }

    static TK GetKey(string str) {
        return (TK)Convert.ChangeType(str, typeof(TK));
    }
    static TV GetValue(string str) {
        return (TV)Convert.ChangeType(str, typeof(TV));
    }

    public void Dispose() {
        MaybeFlush();
    }


    readonly Dictionary<TK, TV> dict;
    readonly string file;
    readonly Func<string, Tuple<TK, TV>> get;
    readonly Func<TK, TV, string> set;
    readonly FlushPolicy flush_policy;

    public void Reload() {
        dict.Clear();
        // todo: is this a good approach?
        if (File.Exists(file)) {
            foreach (var line in File.ReadAllLines(file)) {
                var tuple = get(line);
                if (tuple == null) {
                    continue;
                }
                dict[tuple.Item1] = tuple.Item2;
            }
        }

    }

    public void Flush() {
        using (var sw = new StreamWriter(file, false)) {
            foreach (var kvp in dict) {
                sw.WriteLine(set(kvp.Key, kvp.Value));
            }
        }
    }

    public bool Dirty { get; private set; }

    public bool MaybeFlush() {
        if (!Dirty) {
            return false;
        }
        Flush();
        return true;
    }

    // Count
    // some mixins would be nice, C# team!


    public TV this[TK key] {
        get { return Get(key); }
        set { Set(key, value); }
    }

    public TV Get(TK key) {
        return dict[key];
    }

    public void Set(TK key, TV value) {
        dict[key] = value;
        Dirty = true;
    }

    public bool TryGetValue(TK key, out TV value) {
        return dict.TryGetValue(key, out value);
    }

}