using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
// ThreadExceptionEventHandler

using Fairweather.Service;
namespace Fairweather.Service
{
    // Metadata?
    public class Logging
    {
        const bool hidden_log = false;

        static Logging() {
            Auto_Logging = true;
        }

        static readonly Dictionary<string, Logging> instances = new Dictionary<string, Logging>();

        static public Logging Get_Logger(string file_name, string backup_name, int max_log_size,
                       bool def_instance) {

            Logging ret;
            file_name = Path.GetFullPath(file_name.ToUpper());
            if (!instances.TryGetValue(file_name, out ret))
                instances[file_name] = ret = new Logging(file_name, backup_name, max_log_size, def_instance); ;

            return ret;

        }
        public Logging(string file_name, string backup_name, int max_log_size,
                       bool def_instance) {

            // validate stuff
            m_FILE_NAME = file_name;
            m_FILE_BACK = backup_name;

            m_MAX_LOG_SIZE = Math.Max(max_log_size, 0x1000);

            (m_FILE_NAME.IsNullOrEmpty()).tift();
            (m_FILE_BACK.IsNullOrEmpty()).tift();


            if (def_instance)
                default_instance = this;

        }

        public static bool Auto_Logging {
            get;
            set;
        }

        internal const string stack_trace = "STACK_TRACE";
        internal const string stack_frame = "STACK_FRAME";
        const int KB = 0x400;
        const int MB = 0x10000;
        const int buffer_size = 5;
        const string cond_trace = "COND_TRACE";

        static readonly Set<Exception>
        hs_notified = new Set<Exception>();

        static Logging default_instance;

        static object s_locker = new object();



        readonly Ring<Triple<DateTime, string, string>?>
        buffer = new Ring<Triple<DateTime, string, string>?>(buffer_size);

        readonly string m_FILE_NAME;

        readonly string m_FILE_BACK;


        readonly int m_MAX_LOG_SIZE;


        /*       Interface        */


        /// <summary>
        /// Sends the exception to the default Logging instance,
        /// if (one is present) ^ (auto_logging)
        /// </summary>
        public static void Notify(Exception ex, params string[] data_keys) {

            Notify(ex, null, data_keys);

        }

        public static void Notify(Exception ex, Assembly calling_assembly, params string[] data_keys) {

            // don't forget that a params array can be null
            data_keys = data_keys ?? new string[0];

            calling_assembly = calling_assembly ?? Assembly.GetCallingAssembly();

            if (!Auto_Logging)
                return;

            var def = default_instance;

            if (def == null)
                return;

            def.Log(ex, true, calling_assembly);

        }

        public void Log(Exception ex) {

            Log(ex, false, null);

        }


        /*       Implementatoin        */



        /// "notified": if the method is called from Notify
        void Log(Exception ex,
                 bool notified,
                 Assembly calling_assembly) {

            calling_assembly = calling_assembly ?? Assembly.GetCallingAssembly();

            bool seen_before = hs_notified.Contains(ex);

            if (notified && seen_before)
                return;

            // adding/removing null to/from a hashset is safe, but makes no sense

            if (ex != null) {
                if (notified)
                    hs_notified.Add(ex);
                else
                    hs_notified.Remove(ex);
            }

            if (File.Exists(m_FILE_NAME)) {

                var file = new FileInfo(m_FILE_NAME);

                if (file.Length > m_MAX_LOG_SIZE * KB) {

                    File.Delete(m_FILE_BACK);
                    File.Move(m_FILE_NAME, m_FILE_BACK);

                }
            }

            else {
#pragma warning disable
                using (File.CreateText(m_FILE_NAME)) { }
                if (hidden_log)
                    File.SetAttributes(m_FILE_NAME, FileAttributes.Hidden);
#pragma warning restore
            }

            try {
                using (var sw = new StreamWriter(m_FILE_NAME, true)) {

                    Log_Inner(ex, sw, notified, calling_assembly);

                }
            }
            catch (ArgumentException) { }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            catch (SecurityException) { }



        }


        static readonly int id = Process.GetCurrentProcess().Id;

        void Log_Inner(Exception ex,
                      StreamWriter sw,
                      bool notified,
                      Assembly calling_assembly) {

            sw.WriteLine(Line_Break);
            sw.WriteLine(Datestamp);

            sw.WriteLine(Get_Assembly_Data());

            if (calling_assembly != null)
                sw.WriteLine(Get_Assembly_Data(calling_assembly, false));

            sw.WriteLine("PID: " + id);
            sw.WriteLine("TID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            if (notified)
                sw.WriteLine("NOTIFIED");

            sw.WriteLine();


            if (ex == null)
                sw.WriteLine("Null exception.");

            while (ex != null) {

                sw.WriteLine("Hash: " + ex.GetHashCode());
                sw.WriteLine(ex.ToString());

                bool include = notified || String.IsNullOrEmpty(ex.StackTrace);

                Log_Data_Collection(ex.Data, sw, include);

                sw.WriteLine(Line_Break);

                ex = ex.InnerException;

                if (ex != null) {

                    sw.WriteLine();
                    sw.WriteLine("Inner Exception follows:");
                    sw.WriteLine();

                }
            }

            Dump_Trace_Buffer(sw);

        }



        void Log_Data_Collection(IDictionary data, StreamWriter sw, bool include_frame_trace) {

            if (data == null || data.Count == 0)
                return; // Nothing to log

            if (!include_frame_trace) {

                bool ok = false;

                foreach (var key in data.Keys) {

                    if (key.ToString() != stack_trace &&
                        key.ToString() != stack_frame) {
                        ok = true;
                        break;
                    }

                }

                if (!ok)
                    return; // Nothing to log

            }


            lock (data.SyncRoot) {

                sw.WriteLine();
                sw.WriteLine("Data collection follows:");
                sw.WriteLine();

                foreach (DictionaryEntry kvp in data) {
                    var key = kvp.Key;

                    if (include_frame_trace ||
                        (key.ToString() != stack_trace &&
                         key.ToString() != stack_frame)) {

                        sw.WriteLine("{0}:{1}", kvp.Key, kvp.Value);

                    }

                }
            }

        }


        // Helpers

        string Get_Assembly_Data(Assembly assembly, bool entry) {

            var name = assembly.GetName();
            var ver = name.Version;

            var ret =
            (entry ? "Entry" : "Calling") + " asm: " + name.FullName;
            // Entry asm: ses, Version=1.0.1060.222, Culture=neutral, PublicKeyToken=null

            return ret;

        }

        string Get_Assembly_Data() {
            var entry_assembly = Assembly.GetEntryAssembly();
            return Get_Assembly_Data(entry_assembly, true);
        }

        string Line_Break {
            get {
                return new String('=', 100);
            }
        }

        string Datestamp {
            get {
                return DateTime.Now.ToString("[HH:mm:ss dd/MM/yyyy]");
            }
        }



        // Tracing

        [Conditional(cond_trace)]
        public static void Trace_Default(string str) {

            if (default_instance == null)
                return;

            default_instance.Trace(str);
        }

        [Conditional(cond_trace)]
        public void Trace(string str) {

            buffer.Current = Triple.Make(DateTime.Now, str, D.Get_Caller_Name());
            buffer.MoveNext();

        }

        [Conditional(cond_trace)]
        public void Dump_Trace_Buffer(StreamWriter sw) {

            sw.WriteLine("Last logged messages (most recent first):");

            buffer.MovePrev();

            foreach (var null_triple in buffer.Loop(false, 1)) {

                if (!null_triple.HasValue)
                    continue;

                var triple = null_triple.Value;

                sw.WriteLine("{0}: {1} at {2}".spf(triple.First, triple.Second, triple.Third));

            }

        }
    }
}
