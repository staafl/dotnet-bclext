using System.Diagnostics;
namespace Fairweather.Service
{
    public interface IBinaryTree<T>
    {
        IBinaryTree<T> Left { get; }
        IBinaryTree<T> Right { get; }
        T Value { get; }
    }

  
    //public class PopsicleBinaryTree<T> : IBinaryTree<T>
    //{

    //}

    [DebuggerDisplay("{Value.ToString()}")]
    public class MutableBinaryTree<T> : IBinaryTree<T>
    {
        protected MutableBinaryTree<T> m_left;
        protected MutableBinaryTree<T> m_right;
        protected T m_value;

        public virtual MutableBinaryTree<T> Left { get { return m_left; } set { m_left = value; } }
        public virtual MutableBinaryTree<T> Right { get { return m_right; } set { m_right = value; } }

        IBinaryTree<T> IBinaryTree<T>.Left { get { return m_left; } }
        IBinaryTree<T> IBinaryTree<T>.Right { get { return m_right; } }

        public virtual T Value { get { return m_value; } set { m_value = value; } }

        public MutableBinaryTree() : this(default(T), null, null) { }
        public MutableBinaryTree(T value) : this(value, null, null) { }
        public MutableBinaryTree(T value,
                                   MutableBinaryTree<T> left,
                                   MutableBinaryTree<T> right) {

            m_value = value;
            m_left = left;
            m_right = right;

        }

    }

    public class ImmutableBinaryTree<T> : IBinaryTree<T>
    {
        readonly ImmutableBinaryTree<T> m_left;
        readonly ImmutableBinaryTree<T> m_right;
        readonly T m_value;

        public ImmutableBinaryTree<T> Left { get { return m_left; } }
        public ImmutableBinaryTree<T> Right { get { return m_right; } }
        public T Value { get { return m_value; } }

        IBinaryTree<T> IBinaryTree<T>.Left { get { return m_left; } }
        IBinaryTree<T> IBinaryTree<T>.Right { get { return m_right; } }

        public ImmutableBinaryTree<T> Set_Left(ImmutableBinaryTree<T> left) {

            return new ImmutableBinaryTree<T>(m_value, left, m_right);

        }

        public ImmutableBinaryTree<T> Set_Right(ImmutableBinaryTree<T> right) {

            return new ImmutableBinaryTree<T>(m_value, m_left, right);

        }

        public ImmutableBinaryTree<T> Set_Root(T value) {

            return new ImmutableBinaryTree<T>(value, m_left, m_right);

        }



        public ImmutableBinaryTree(T value,
                                   ImmutableBinaryTree<T> left,
                                   ImmutableBinaryTree<T> right) {

            m_value = value;
            m_left = left;
            m_right = right;

        }

    }
}