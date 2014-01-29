using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace bclx
{
    public static class Utils
    {
        public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> seq)
        {
            using (var enm = seq.GetEnumerator())
            {
                if (!enm.MoveNext()) yield break;
                while (true)
                {
                    var toYield = enm.Current;
                    if (!enm.MoveNext()) yield break;
                    yield return toYield;
                }
            }
        }
        
        public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> seq, int howManyToDropFromEnd)
        {
            if (howManyToDropFromEnd == 0)
            {
                foreach (var elem in seq)
                {
                    yield return elem;
                }
                yield break;
            }

            var list = new List<T>(howManyToDropFromEnd);

            using (var enm = seq.GetEnumerator())
            {
                for (int ii = 0; ii < howManyToDropFromEnd; ii++)
                {
                    if (!enm.MoveNext()) yield break;
                    list.Add(enm.Current);
                }

                for (int ii = 0; ; ii++)
                {
                    if (!enm.MoveNext()) yield break;
                    yield return list[ii];
                    list.Add(enm.Current);
                }
            }
        }

        public static void NotNull(this object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException("name");
        }
        /// <summary>
        /// Checks an event for non-nullness and invokes it safely.
        /// </summary>
        public static void SafeRaise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler != null) handler(sender, e);
        }

        /// <summary>
        /// Checks an event for non-nullness and invokes it safely.
        /// </summary>
        public static void Raise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null) handler(sender, e);
        }

        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            return ((MemberExpression) expression.Body).Member.Name;
        }

        public static string BackupFile(string path, bool move, bool copy)
        {
            var timestamp = DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss-fff");
            var name = Path.GetFileNameWithoutExtension(path) + timestamp;
            if (path != null)
            {
                var targetPath = Path.Combine(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Path.GetDirectoryName(path),
                    name + Path.GetExtension(path));
                if (TryFileOperation(path, targetPath, move: move, copy: copy))
                    return targetPath;
                else
                    return null;
            }
            return null;
        }

        public static bool TryFileOperation(string filePath1, string filePath2 = null, bool move = false, bool copy = false, bool delete = true)
        {
            if (filePath1 == null)
                return false;
            if (!File.Exists(filePath1))
                return false;
            try
            {
                if (move)
                    File.Move(filePath1, filePath2);
                if (copy)
                    File.Copy(filePath1, filePath2);
                if (delete)
                    File.Delete(filePath1);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool HasNonNull(IEnumerable seq)
        {
            if (seq == null)
                return false;
            foreach (var val in seq)
            {
                if (val != null)
                    return true;
                var asEnum = val as IEnumerable;
                if (HasNonNull(asEnum))
                    return true;
            }
            return false;
        }
    }
}
