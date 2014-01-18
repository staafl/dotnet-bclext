using System.Diagnostics;

namespace Fairweather.Service.Syntax
{
    public partial class BinaryAST
    {
        // Todo: Replace this with popsicle immutability
        [DebuggerDisplay("{m_middle.ToString()}")]
        class Intermediate_AST : MutableBinaryTree<IToken>
        {
            IToken m_middle;
            IToken[] m_left_half;
            IToken[] m_right_half;

            readonly IToken[] m_tokens;
            public Intermediate_AST(IToken[] tokens)
                : base(null, null, null) {

                tokens.IsNullOrEmpty().tift();

                m_tokens = tokens;

                int min = 0;
                int min_prec = m_tokens[min].Precedence;

                for (int ii = 1; ii < m_tokens.Length; ++ii) {

                    var prec = m_tokens[ii].Precedence;

                    if (min_prec == int.MinValue) {
                        min_prec = prec;
                        min = ii;
                    }
                    else if (prec < min_prec && prec != int.MinValue) {
                        min = ii;
                        min_prec = prec;
                    }
                }

                (min_prec == int.MinValue && tokens.Length > 1).tift();

                m_middle = tokens[min];
                m_left_half = min > 0 ? tokens.Slice(0, min - 1) : new IToken[0];
                m_right_half = min < tokens.Length - 1 ? tokens.Slice(min + 1) : new IToken[0];
            }

            public IToken Middle {
                get {
                    return m_middle;
                }
            }

            public IToken[] Left_Half {
                get {
                    return m_left_half.Slice();
                }
            }

            public IToken[] Right_Half {
                get {
                    return m_right_half.Slice();
                }
            }

            public void Set_Left(IToken[] tokens) {
                this.m_left = new Intermediate_AST(tokens);
            }
            public void Set_Right(IToken[] tokens) {
                this.m_right = new Intermediate_AST(tokens);
            }
            public void Set_Root(IToken token) {
                this.m_value = token;
            }


            public BinaryAST To_AST() {

                var ret = this.Pre_Order().From_Preorder();

                return new BinaryAST(ret);

            }
        }

    }
}