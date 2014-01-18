using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fairweather.Service
{
      // Preorder == Depth-First Traversal
      // In-Order == Symmetric Traversal
      // Post-Order ==
      // Level-Order == Breadth-First Traversal
      public static class BinaryTree
      {
            [DebuggerStepThrough]
            public struct Tree_Node<T>
            {
                  #region Tree_Node<T>

                  readonly T m_value;

                  readonly Node_Type m_type;

                  readonly int m_depth;


                  public T Value {
                        get {
                              return this.m_value;
                        }
                  }

                  public Node_Type Type {
                        get {
                              return this.m_type;
                        }
                  }

                  public int Depth {
                        get {
                              return this.m_depth;
                        }
                  }


                  public Tree_Node(T value,
                              Node_Type type,
                              int depth) {
                        this.m_value = value;
                        this.m_type = type;
                        this.m_depth = depth;
                  }


                  /* Boilerplate */

                  public override string ToString() {

                        string ret = "";

                        ret += "value = " + this.m_value;
                        ret += ", ";
                        ret += "type = " + this.m_type;
                        ret += ", ";
                        ret += "depth = " + this.m_depth;

                        ret = "{Tree_Node<T>: " + ret + "}";
                        return ret;

                  }

                  public bool Equals(Tree_Node<T> obj2) {

#pragma warning disable
                        if ((this.m_value == null) !=
                        (obj2.m_value == null))
                              return false;

                        if (this.m_value != null &&
                       !this.m_value.Equals(obj2.m_value))
                              return false;


                        if ((this.m_type == null) !=
                        (obj2.m_type == null))
                              return false;

                        if (this.m_type != null &&
                       !this.m_type.Equals(obj2.m_type))
                              return false;


                        if ((this.m_depth == null) !=
                        (obj2.m_depth == null))
                              return false;

                        if (this.m_depth != null &&
                       !this.m_depth.Equals(obj2.m_depth))
                              return false;
#pragma warning restore


                        return true;
                  }

                  public override bool Equals(object obj2) {

                        var ret = (obj2 != null && obj2 is Tree_Node<T>);

                        if (ret)
                              ret = this.Equals((Tree_Node<T>)obj2);


                        return ret;

                  }

                  public static bool operator ==(Tree_Node<T> left, Tree_Node<T> right) {

                        var ret = left.Equals(right);
                        return ret;

                  }

                  public static bool operator !=(Tree_Node<T> left, Tree_Node<T> right) {

                        var ret = !left.Equals(right);
                        return ret;

                  }

                  public override int GetHashCode() {

                        unchecked {
                              int ret = 23;
                              int temp;

#pragma warning disable
                              if (this.m_value != null) {
                                    ret *= 31;
                                    temp = this.m_value.GetHashCode();
                                    ret += temp;

                              }

                              if (this.m_type != null) {
                                    ret *= 31;
                                    temp = this.m_type.GetHashCode();
                                    ret += temp;

                              }

                              if (this.m_depth != null) {
                                    ret *= 31;
                                    temp = this.m_depth.GetHashCode();
                                    ret += temp;

                              }

#pragma warning restore
                              return ret;
                        }
                  }

                  public void Mutate(T value) {
                        this = new Tree_Node<T>(value, m_type, m_depth);
                  }

                  #endregion
            }

            public enum Node_Type
            {
                  Root = 0x1,
                  Left_Child,
                  Right_Child,
            }



            /*       Helper methods        */

            public static void Print<T>(this IBinaryTree<T> tree) {

                  foreach (var node in tree.In_Order()) {

                        var tabs = new string('\t', node.Depth);

                        Console.WriteLine(tabs + node.Value.ToString());

                  }

            }

            public static int Depth<T>(this IBinaryTree<T> tree) {

                  var nodes = tree.Pre_Order();

                  if (nodes.Is_Empty())
                        return 0;

                  return nodes.Max(node => node.Depth) + 1;

            }

            public static int Weight<T>(this IBinaryTree<T> tree) {

                  var ret = tree.Level_Order(false).Count();

                  return ret;

            }


            /*       Tier 1        */

            public static
            MutableBinaryTree<T> Walk_From_Bottom<T>(this IBinaryTree<T> tree,
                                                          Func<T, T?, T?, T> func)
            where T : struct {


                  return Walk_Helper_Struct(tree, func, false);

            }

            public static
            MutableBinaryTree<T> Walk_From_Bottom<T>(this IBinaryTree<T> tree,
                                                          Func<T, T, T, T> func)
            where T : class {


                  return Walk_Helper_Class(tree, func, false);

            }

            public static
            MutableBinaryTree<T> Walk_From_Top<T>(this IBinaryTree<T> tree,
                                                       Func<T, T?, T?, T> func)
            where T : struct {

                  return Walk_Helper_Struct(tree, func, true);

            }

            public static
            MutableBinaryTree<T> Walk_From_Top<T>(this IBinaryTree<T> tree,
                                                       Func<T, T, T, T> func)
            where T : class {

                  return Walk_Helper_Class(tree, func, true);

            }

            /*       Tier 2        */

            static
            MutableBinaryTree<T> Walk_Helper_Struct<T>(this IBinaryTree<T> tree,
                                                           Func<T, T?, T?, T> func,
                                                           bool from_top)
            where T : struct {

                  Long_Walker<T> func1 = (n1, n2, n3) =>
                  {
                        var arg1 = n1.Value;
                        var arg2 = n2.HasValue ? (T?)n2.Value.Value : null;
                        var arg3 = n3.HasValue ? (T?)n3.Value.Value : null;

                        var ret = func(arg1, arg2, arg3);

                        return ret;
                  };

                  return Walk(tree,
                                       func1,
                                       (parent, left) => func1(parent, left, null),
                                       (parent) => func1(parent, null, null),
                                       from_top);

            }

            static
            MutableBinaryTree<T> Walk_Helper_Class<T>(this IBinaryTree<T> tree,
                                                          Func<T, T, T, T> func,
                                                          bool from_top)
            where T : class {

                  Long_Walker<T> func1 = (n1, n2, n3) =>
                  {
                        var arg1 = n1.Value;
                        var arg2 = n2.HasValue ? n2.Value.Value : null;
                        var arg3 = n3.HasValue ? n3.Value.Value : null;

                        var ret = func(arg1, arg2, arg3);

                        return ret;
                  };

                  return Walk(tree,
                                       func1,
                                       (parent, left) => func1(parent, left, null),
                                       (parent) => func1(parent, null, null),
                                       from_top);

            }

            /*       Tier 3        */

            delegate T Long_Walker<T>(Tree_Node<T> parent, Tree_Node<T>? left_child, Tree_Node<T>? right_child);
            delegate T Middle_Walker<T>(Tree_Node<T> parent, Tree_Node<T>? left_child);
            delegate T Short_Walker<T>(Tree_Node<T> parent);

            static
            MutableBinaryTree<T> Walk<T>(this IBinaryTree<T> tree,
                                               Long_Walker<T> @long,
                                               Middle_Walker<T> @middle,
                                               Short_Walker<T> @short,
                                               bool from_top) {

                  tree.tifn();
                  @long.tifn();
                  @middle.tifn();
                  @short.tifn();

                  var array = tree.Level_Order(true).ToArray();
                  var tree2 = array.From_Level_Order();
                  var array2 = tree2.Level_Order(true).ToArray();
                  array.Piecewise_Equal(array2, true).tiff();
                  int cnt = array.Length;

                  if (cnt == 0)
                        return new MutableBinaryTree<T>();

                  int ii = from_top ? 0 : cnt - 1;

                  // Todo: examine the difference in compiled code
                  // between a lambda which uses the local variable
                  // and one which only uses parameters as bound variables
                  Func<int, int, bool> go_on = from_top ? (Func<int, int, bool>)((_int, _cnt) => _int < _cnt) :
                                                          (Func<int, int, bool>)((_int, _) => _int >= 0);

                  Func<int, int>
                      step = from_top ? (Func<int, int>)(_int => _int + 1) :
                                        (Func<int, int>)(_int => _int - 1);

                  for (; go_on(ii, cnt); ii = step(ii)) {

                        var null_parent = array[ii];

                        if (!null_parent.HasValue)
                              continue;

                        var parent = null_parent.Value;

                        int l_child_ind = ii * 2 + 1;
                        int r_child_ind = l_child_ind + 1;

                        T new_value;

                        var left_is_out = (l_child_ind >= cnt);
                        var right_is_out = (r_child_ind >= cnt);

                        if (right_is_out && left_is_out) {

                              if (from_top)
                                    break;

                              continue;

                        }

                        var left_child = array[l_child_ind];

                        if (left_is_out) {

                              new_value = @middle(parent, left_child);

                        }
                        else {

                              var right_child = array[r_child_ind];
                              new_value = @long(parent, left_child, right_child);

                        }

                        parent.Mutate(new_value);
                        array[ii] = parent;


                  }

                  var ret = array.From_Level_Order();

                  return ret;

            }


            /*       Traversals        */

            public static
            IEnumerable<Tree_Node<T>> In_Order<T>(this IBinaryTree<T> tree) {

                  // http://blogs.msdn.com/ericlippert/archive/2007/12/19/immutability-in-c-part-seven-more-on-binary-trees.aspx

                  var stack = new Stack<Triple<Node_Type, int, IBinaryTree<T>>>();


                  Triple<Node_Type, int, IBinaryTree<T>>? null_triple;

                  null_triple = Triple.Make(Node_Type.Root, 0, tree);

                  while (null_triple != null || !stack.Is_Empty()) {

                        while (null_triple != null) {

                              var triple = null_triple.Value;

                              var depth = triple.Second;
                              var current = triple.Third;

                              stack.Push(triple);

                              null_triple = null;

                              if (current.Left != null)
                                    null_triple = Triple.Make(Node_Type.Left_Child,
                                                             depth + 1,
                                                             current.Left);

                        }

                        {
                              var triple = stack.Pop();

                              var type = triple.First;
                              var depth = triple.Second;
                              var current = triple.Third;


                              yield return new Tree_Node<T>(current.Value, type, depth);

                              null_triple = null;

                              if (current.Right != null)
                                    null_triple = Triple.Make(Node_Type.Right_Child,
                                                              depth + 1,
                                                              triple.Third.Right);


                        }
                  }

            }

            public static
            IEnumerable<Tree_Node<T>?> Level_Order<T>(this IBinaryTree<T> tree, bool keep_nulls) {

                  keep_nulls.tiff();

                  // See http://en.wikipedia.org/wiki/Iterative_deepening_depth-first_search
                  // for a possibly better approach

                  if (tree == null)
                        yield break;

                  var queue = new Queue<Pair<int, IBinaryTree<T>>?>();


                  yield return new Tree_Node<T>(tree.Value, Node_Type.Root, 0);

                  queue.Enqueue(Pair.Make(0, tree));

                  Pair<int, IBinaryTree<T>> parent_node;
                  Pair<int, IBinaryTree<T>>? null_parent_node;

                  int last_seen = 0;

                  while (!queue.Is_Empty()) {

                        IBinaryTree<T> parent;

                        do {
                              null_parent_node = queue.Dequeue();

                        } while (!queue.Is_Empty() && !(keep_nulls || null_parent_node != null));

                        if (null_parent_node == null) {

                              if (!keep_nulls)
                                    yield break;

                              yield return null;
                              queue.Enqueue(null);
                              queue.Enqueue(null);
                              continue;
                        }
                        parent_node = null_parent_node.Value;
                        parent = parent_node.Second;

                        int parent_depth = parent_node.First;
                        int current_depth = parent_depth + 1;

                        var type = Node_Type.Left_Child;

                        foreach (var child in new IBinaryTree<T>[] { parent.Left, parent.Right }) {

                              if (child == null) {

                                    // quick cop-out
                                    if (last_seen < current_depth)
                                          yield break;

                                    if (keep_nulls) {
                                          yield return null;
                                          queue.Enqueue(null);
                                          queue.Enqueue(null);

                                    }

                              }
                              else {
                                    if (child.Left != null || child.Right != null)
                                          last_seen = current_depth + 1;

                                    var node = new Tree_Node<T>(child.Value, type, current_depth);
                                    yield return node;

                                    var pair = Pair.Make(current_depth, child);
                                    queue.Enqueue(pair);
                              }
                              type = Node_Type.Right_Child;
                        }

                  }
            }

            public static
            IEnumerable<Tree_Node<T>> Pre_Order<T>(this IBinaryTree<T> tree) {

                  if (tree == null)
                        yield break;

                  var stack = new Stack<Triple<Node_Type, int, IBinaryTree<T>>>();

                  stack.Push(Triple.Make(Node_Type.Root, 0, tree));

                  do {
                        var triple = stack.Pop();

                        var type = triple.First;
                        var depth = triple.Second;
                        var current = triple.Third;

                        yield return new Tree_Node<T>(current.Value, type, depth);

                        var left = current.Left;
                        var right = current.Right;

                        if (right != null)
                              stack.Push(Triple.Make(Node_Type.Right_Child, depth + 1, right));

                        if (left != null)
                              stack.Push(Triple.Make(Node_Type.Left_Child, depth + 1, left));

                  }
                  while (!stack.Is_Empty());
            }



            /*       Construction        */

            public static
            MutableBinaryTree<T> From_Level_Order<T>(this IEnumerable<Tree_Node<T>?> source) {

                  // stack based implementation removed in revision 89

                  var nodes = source.Force();

                  var ret = new MutableBinaryTree<T>();

                  /*       Prelude        */

                  if (nodes.Count == 0)
                        return ret;

                  var root_null = nodes.First();
                  nodes.RemoveAt(0);

                  if (!root_null.HasValue)
                        return ret;

                  ret.Value = root_null.Value.Value;

                  var current = ret;

                  var queue = new Queue<MutableBinaryTree<T>>();

                  bool left = false;

                  /*       Main loop        */


                  foreach (var node_null in nodes) {

                        left = !left;

                        bool has_value = node_null.HasValue;

                        MutableBinaryTree<T> temp = null;

                        if (!has_value) {
                              queue.Enqueue(null);
                              if (!left)
                                    current = queue.Dequeue();

                              continue;
                        }

                        var node = node_null.Value;

                        temp = new MutableBinaryTree<T>(node.Value);

                        queue.Enqueue(temp);

                        if (left) {
                              if (current != null)
                                    current.Left = temp;
                        }
                        else {

                              if (current != null)
                                    current.Right = temp;

                              current = queue.Dequeue();

                        }


                  }

                  return ret;
            }

            public static
            MutableBinaryTree<T> From_Preorder<T>(this IEnumerable<Tree_Node<T>> source) {

                  var forced = source.Force();

                  forced.Reverse();

                  var nodes = new Stack<Tree_Node<T>>(forced);

                  // visited nodes
                  var nodes1 = new Stack<Tree_Node<T>>(forced.Count);

                  // to return
                  var root = new MutableBinaryTree<T>();

                  if (nodes.Is_Empty())
                        return root;

                  var first_node = nodes.Pop();
                  (first_node.Type == Node_Type.Root).tiff();

                  root.Value = first_node.Value;
                  nodes1.Push(first_node);

                  var parents = new Stack<MutableBinaryTree<T>>();
                  parents.Push(root);

                  using (var enumer = nodes.GetEnumerator())
                        while (enumer.MoveNext()) {

                              var node = enumer.Current;
                              bool left = node.Type == Node_Type.Left_Child;
                              var temp = new MutableBinaryTree<T>(node.Value);

                              int difference = node.Depth - nodes1.Peek().Depth;

                              if (difference != 1) {

                                    left.tift();

                                    // ascending

                                    int to_pop = -difference + 1;

                                    for (int ii = 0; ii < to_pop; ++ii) {
                                          nodes1.Pop();
                                          parents.Pop();
                                    }

                              }

                              // descending

                              if (left)
                                    parents.Peek().Left = temp;
                              else
                                    parents.Peek().Right = temp;

                              parents.Push(temp);
                              nodes1.Push(node);


                        }

                  return root;
            }

            /*       Transformation        */

            public static
            MutableBinaryTree<TResult> Transform<TResult, TSource>(this IBinaryTree<TSource> source,
                                                                        Func<TSource, TResult> func) {

                  var temp = source.Pre_Order().Force();

                  Func<Tree_Node<TSource>, Tree_Node<TResult>> λ =
                  (node) =>
                  {
                        var new_val = func(node.Value);
                        var _ret = new Tree_Node<TResult>(new_val, node.Type, node.Depth);
                        return _ret;
                  };

                  var temp2 = temp.Select(λ);

                  var ret = temp2.From_Preorder();

                  return ret;

            }


            public static
            MutableBinaryTree<TResult> Transform_Level_Order<TResult, TSource>(this IBinaryTree<TSource> source,
                                                                                    Func<TSource, TResult> func) {

                  var temp = source.Level_Order(true).Force();

                  Func<Tree_Node<TSource>?, Tree_Node<TResult>?> λ =
                  (null_node) =>
                  {
                        if (!null_node.HasValue)
                              return null;

                        var node = null_node.Value;

                        var new_val = func(node.Value);
                        var _ret = new Tree_Node<TResult>(new_val, node.Type, node.Depth);
                        return _ret;
                  };

                  var temp2 = temp.Select(λ);

                  var ret = temp2.From_Level_Order();

                  return ret;

            }


            /*       Empty Tree        */
            /*       Commented-out - 60 lines    */

            //public static
            //IBinaryTree<T> Empty<T, TTree>(this TTree tree)
            //where TTree : IBinaryTree<T> {
            //    return Empty_Tree<T>.Instance;
            //}

            //public static
            //bool Is_Empty<T, TTree>(this TTree tree)
            //where TTree : IBinaryTree<T> {

            //    return (tree.Equals(Empty_Tree<T>.Instance)) || tree == null;

            //}


            //class Empty_Tree<T> : IBinaryTree<T>
            //{
            //    static readonly Empty_Tree<T> rd_instance;

            //    public static Empty_Tree<T> Instance {
            //        get { return Empty_Tree<T>.rd_instance; }
            //    }


            //    static Empty_Tree() {
            //        rd_instance = new Empty_Tree<T>();
            //    }

            //    Empty_Tree() {
            //    }

            //    void Throw() {
            //        throw new InvalidOperationException("Empty Tree");
            //    }

            //    public T Value {
            //        get {
            //            Throw();
            //            return default(T);
            //        }
            //    }
            //    public IBinaryTree<T> Left {
            //        get {
            //            Throw();
            //            return null;
            //        }
            //    }

            //    public IBinaryTree<T> Right {
            //        get {
            //            Throw();
            //            return null;
            //        }
            //    }

            //}

      }



}