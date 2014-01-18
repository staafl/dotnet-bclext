using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Token_Node = Fairweather.Service.BinaryTree.Tree_Node<Fairweather.Service.Syntax.IToken>;

namespace Fairweather.Service.Syntax
{
      [DebuggerDisplay("{m_tree.ToString()}")]
      public partial class BinaryAST
      {

            IBinaryTree<IToken> m_tree;

            public IBinaryTree<IToken> Tree {
                  get { return m_tree; }
                  set { m_tree = value; }
            }

            BinaryAST(IBinaryTree<IToken> tree) {
                  this.m_tree = tree;
            }

            public static
            BinaryAST Make(IToken[] tokens) {

                  var result_root = new Intermediate_AST(tokens); ;

                  var queue = new Queue<Triple<bool, int, Intermediate_AST>?>();

                  var result = new List<Token_Node?>();

                  queue.Enqueue(Triple.Make(false, 0, result_root));

                  result.Add(new Token_Node(result_root.Middle, BinaryTree.Node_Type.Root, 0));

                  while (!queue.Is_Empty()) {

                        var null_triple = queue.Dequeue();

                        if (!null_triple.HasValue) {

                              if (!queue.Any(_null_triple => _null_triple.HasValue))
                                    break;

                              queue.Enqueue(null);
                              queue.Enqueue(null);
                              result.Add(null);
                              result.Add(null);

                              continue;
                        }

                        var triple = null_triple.Value;
                        var current = triple.Third;

                        var middle = current.Middle; // the element with highest precedence
                        var left = current.Left_Half;
                        var right = current.Right_Half;

                        //(middle.Type == Token_Type.Binary_Operator).tiff();

                        /// Todo: Generalize this to more than two branches
                        /// use array indexing instead of explicit properties and variables

                        bool last_left = (left.Length == 1);
                        bool last_right = (right.Length == 1);

                        if (last_left) {
                              var type = left.Single().Type;
                              (type == Token_Type.Argument).tiff();
                        }

                        if (last_right) {
                              var type = right.Single().Type;
                              (type == Token_Type.Argument).tiff();
                        }

                        current.Set_Root(middle);

                        if (left.Length != 0)
                              current.Set_Left(left);

                        if (right.Length != 0)
                              current.Set_Right(right);

                        int depth = triple.Second;

                        bool is_right = false;

                        foreach (var child in new MutableBinaryTree<IToken>[] { current.Left, current.Right }) {

                              if (child == null) {

                                    queue.Enqueue(null);
                                    result.Add(null);

                              }
                              else {

                                    var type = is_right ? BinaryTree.Node_Type.Right_Child
                                                        : BinaryTree.Node_Type.Left_Child;

                                    var ast = child as Intermediate_AST;
                                    var node = new Token_Node(ast.Middle, type, depth);
                                    result.Add(node);

                                    var new_triple = Triple.Make(is_right, depth + 1, ast);
                                    queue.Enqueue(new_triple);

                              }
                              is_right = true;

                        }


                  }

                  var ok = result.From_Level_Order();

                  return new BinaryAST(ok);

                  //return result_root.To_AST();

            }


      }
}
