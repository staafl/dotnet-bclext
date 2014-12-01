using System;
using System.Linq;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Wraps an array. Is considered equal to another ArrayEqualityWrapper iff the two instances
    ///     wrap arrays of equal length and with equal values.
    ///     <para />
    ///     NB: doesn't accept null.<para />
    ///     NB: Consistency depends on array not being modified.<para />
    /// </summary>
    public class ArrayEqualityWrapper
    {
        private readonly object[] array_;
        private int hash_;

        public ArrayEqualityWrapper(object[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            array_ = array;
        }

        public override bool Equals(object other)
        {
            var asArrayComparer = other as ArrayEqualityWrapper;
            if (asArrayComparer == null)
            {
                return false;
            }
            return array_.Length == asArrayComparer.array_.Length &&
                   (array_ == asArrayComparer.array_ ||
                    array_.SequenceEqual(asArrayComparer.array_));
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            if (hash_ != 0)
            {
                return hash_;
            }
            if (array_.Length == 0)
            {
                return 0;
            }
            var tempHash = int.MinValue;
            foreach (object elem in array_)
            {
                if (elem != null)
                {
                    tempHash = elem.GetHashCode();
                    break;
                }
            }
            tempHash ^= array_.Length;
            return (hash_ = tempHash);
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }
    }
}