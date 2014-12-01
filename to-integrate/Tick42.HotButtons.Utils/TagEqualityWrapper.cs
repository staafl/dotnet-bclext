using System;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Augments an object's equality and hash logic with an extra object.
    /// </summary>
    public class TagEqualityWrapper
    {
        private readonly object inner_;
        private readonly object tag_;

        public TagEqualityWrapper(object tag, object inner)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            tag_ = tag;
            inner_ = inner;
        }

        protected bool Equals(TagEqualityWrapper other)
        {
            return Equals(tag_, other.tag_) && Equals(inner_, other.inner_);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((TagEqualityWrapper) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((tag_ != null ? tag_.GetHashCode() : 0)*397) ^ (inner_ != null ? inner_.GetHashCode() : 0);
            }
        }
    }
}