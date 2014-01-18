  public interface IDisposableX : IDisposable
  {
    public event EventHandler Disposing;
    public event EventHandler Disposed;
    public bool IsDisposed { get; }
  }

namespace bclx
{
  class NotNull<T> where T : class
  {
    public T Value { get; private set; }
    
    public static NotNull Named
    
    public NotNull(T value) {
      if (value == null) 
        throw new ArgumentNullException(typeof(T).Name + " null argument");
      this.Value = value;
    }
    
    public NotNull(T value, string msg) {
      if (value == null) 
        throw new ArgumentNullException(msg);
      this.Value = value;
    }
    
    public static T implicit operator T(NotNull<T> notNull) {
      return notNull.Value;
    }
    
    public static T explicit operator NotNull<T>(T value) {
      return new NotNull<T>(value);
    }
  }
}