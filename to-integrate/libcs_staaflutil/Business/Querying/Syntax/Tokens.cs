using System;
using System.Diagnostics;

namespace Fairweather.Service.Syntax
{
    public interface IToken
    {
        Token_Type Type { get; }
        Object Data { get; }
        int Precedence { get; }

    }
    [DebuggerDisplay("{m_type.ToString()}, {m_precedence}, {m_data == null ? \"null\" : m_data.ToString()} ")]
    public abstract class Token_Base : IToken
    {
        protected Token_Base(int precedence, Token_Type type, object data) {

            m_precedence = precedence;
            m_type = type;
            m_data = data;

        }

        readonly int m_precedence;
        protected readonly Token_Type m_type;
        readonly object m_data;

        public int Precedence {
            get {
                var ret = m_precedence;
                return ret;
            }
        }

        public object Data {
            get {
                var ret = m_data;
                return ret;
            }
        }

        public Token_Type Type {
            get {
                var ret = m_type;
                return ret;
            }
        }

        public override string ToString() {

            return "{0}, {1}, {2}".spf(m_type, m_precedence, m_data == null ? "null" : m_data.ToString());

        }
    }
    public class Argument_Token : Token_Base
    {

        public Argument_Token(object data)
            : base(int.MinValue, Token_Type.Argument, data) {
        }

    }

    public class Binary_Token : Token_Base
    {

        // You cannot assign to readonly fields of the derived class from the
        // deriving class' constructor
        public Binary_Token(int precedence, object data)
            : base(precedence, Token_Type.Binary_Operator, data) {
        }

    }
    public enum Token_Type
    {
        Binary_Operator = 0x1,
        Argument = 0x2,
    }

}