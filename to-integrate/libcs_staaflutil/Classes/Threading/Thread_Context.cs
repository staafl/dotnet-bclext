static class Thread_Context
{
    // FIXME: Use reader/writer locking?
    // FIXME: Remove empty dictionaries?

    static readonly Dictionary<int, Dictionary<string, object>>
    dicts = new Dictionary<int, Dictionary<string, object>>();

    public static Dictionary<string, object>
    Get_Dict() {

        Dictionary<string, object> ret;

        lock (dicts)
            if (!dicts.TryGetValue(id, out ret))
                dicts[id] = ret = new Dictionary<string, object>();

        return ret;
    }

    public static IDisposable
    Set(string key, object value) {

        object old;
        var dict = Get_Dict();
        var was = dict.TryGetValue(key, out old);

        dict[key] = value;

        return new On_Dispose(was ? () => dict.Remove(key)
                                  : () => dict[key] = old);

    }

    public static bool
    Try_Get(string key, out object value) {
        return Get_Dict().TryGetValue(key, out value);
    }

}