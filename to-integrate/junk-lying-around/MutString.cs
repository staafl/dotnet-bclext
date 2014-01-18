public class MutString : IEnumerable<char>
{
    public MutString(string str) {
        sb.Append(str);
    }
    
    public override string ToString() {
        return sb.ToString();
    }
    
    public IEnumerator<char> GetEnumerator() {
        return sb.GetEnumerator();
    }
    
    public bool Equals(MutString other) {
        return sb.ToString().Equals(other.sb.ToString());
    }
    
    readonly StringBuilder sb = new StringBuilder();
}