using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fairweather.Service;
using BinaryOperation = System.Func<decimal, decimal, decimal>;
using Dict = System.Collections.Generic.Dictionary<string, System.Action<Common.Calculator_Engine>>;
using UnaryOperation = System.Func<decimal, decimal>;

namespace Common
{
    [DebuggerStepThrough]
    public class Calculator_Engine : IState_Machine_User<Calculator_Engine>
    {
        static void Eval(Calculator_Engine obj) {

            decimal result = obj.producer(obj.reg1, obj.reg2);

            obj.Output(result);


        }
        static void Load1(Calculator_Engine obj) {

            decimal temp = obj.Input();
            obj.reg1 = temp;


        }
        static void Load2(Calculator_Engine obj) {

            decimal temp = obj.Input();
            obj.reg2 = temp;


        }
        static void SetOp(Calculator_Engine obj) {

            obj.producer = obj.buffer;


        }
        static void UnsetOp(Calculator_Engine obj) {

            obj.producer = FirstParam;
            obj.buffer = FirstParam;

        }

        State_Machine<Calculator_Engine> machine;
        void Move(CalculatorStep step) {

            int temp = (int)step;
            char ch = st_steps[temp];
            machine.Move(ch);
        }
        void Move(char step) {

            machine.Move(step);
        }



        #region STATIC DEFINITIONS
        static readonly Action<decimal>
            Void_Output = (dec) => { ;};

        static readonly Func<decimal>
            Void_Input = () => default(decimal);

        static readonly UnaryOperation
            Clr = (dd) => 0.0M;

        static readonly BinaryOperation
            Sub = (shend, mnend) =>
                {
                    decimal ret = shend - mnend;
                    if (ret >= 0 || AllowNegativeValues)
                        return ret;
                    return 0.0M/*%*/;
                };
        static readonly BinaryOperation
            Div = (ddend, dsor) =>
                {
                    if (dsor != 0.0M/*%*/)
                        return ddend / dsor;

                    switch (DivideByZeroHandling) {
                    case DivideByZeroHandling.ThrowException:
                        throw new DivideByZeroException();
                    case DivideByZeroHandling.ReturnZero:
                        return 0.0M/*%*/;
                    default:
                        throw new DivideByZeroException();
                    }
                };

        static List<CalculatorActions>
            bin_ops = new List<CalculatorActions> { CalculatorActions.Add, CalculatorActions.Sub, CalculatorActions.Mul, CalculatorActions.Div };
        static List<CalculatorActions>
            un_ops = new List<CalculatorActions> { CalculatorActions.Sqrt, CalculatorActions.Clr };

        static BinaryOperation[] b_ops = { A.Add, Sub, A.Mul, A.Div };
        static UnaryOperation[] u_ops = { A.SqRt, Clr };

        #endregion

        #region DATA
#pragma warning disable
        static Action<Calculator_Engine> Void = (obj) => { };

        static readonly char[] st_steps = new char[] { '0', '1', '2' };
        char[] mod_steps;
        bool bm_steps;
        public char[] Steps {
            [DebuggerStepThrough]
            get {
                if (!bm_steps)
                    return st_steps;
                return mod_steps;
            }
        }

        public string InitialState {
            [DebuggerStepThrough]
            get {
                return "0";
            }
        }
        static readonly string[]
            st_nodes = {"0", 
                             "00", "01", "02",
                             "010", "011", "012",
                             "0101", "0102", 
                             "01020", "01021", "01022",};

        string[] mod_nodes;
        bool bm_nodes;
        public string[] Nodes {
            [DebuggerStepThrough]
            get {
                if (!bm_nodes)
                    return st_nodes;

                return mod_nodes;
            }
        }

        bool bm_jumps;
        public Dictionary<string, string> Jumps {
            [DebuggerStepThrough]
            get {
                if (!bm_jumps)
                    return st_jumps;
                return mod_jumps;
            }
        }

        static readonly Dictionary<string, string>
            st_jumps = new Dictionary<string, string>() {
                {"00", "0"}, {"02", "0"},{"012", "0"}, 
                {"011", "01"},
                {"0101", "01"}, 
                {"01020", "0"}, {"01021", "01"}, {"01022", "0102"}};

        Dictionary<string, string> mod_jumps;

        public Dictionary<string, Dictionary<string, Action<Calculator_Engine>>>
            Transitions {
            [DebuggerStepThrough]
            get { return st_transitions; }
        }
        static readonly Dictionary<string, Dict>
            st_transitions = new Dictionary<string, Dict>() {

                {"0", new Dict(){{"00", (obj) => {  }},
                                 {"01", (obj) => { Load1(obj); SetOp(obj); }},
                                 {"02", (obj) => { Load1(obj); UnsetOp(obj); Eval(obj); }},
                                }
                },

                {"01", new Dict(){{"010", (obj) => {  }},
                                  {"011", (obj) => { SetOp(obj); }},
                                  {"012", (obj) => { UnsetOp(obj); }},
                                 }
                },

                {"010", new Dict(){{"0101", (obj) => { Load2(obj); Eval(obj); Load1(obj); SetOp(obj); }},
                                   {"0102", (obj) => { Load2(obj); Eval(obj); }},
                                  }
                },

                {"0102", new Dict(){{"01020", (obj) => { }},
                                    {"01021", (obj) => {Load1(obj); SetOp(obj); }},
                                    {"01022", (obj) => {Load1(obj); Eval(obj); }},
                                   }
                }
            };
#pragma warning restore
        #endregion

        public Calculator_Engine()
            : base() {

            machine = new State_Machine<Calculator_Engine>(this);
            Disconnect();
        }


        Action<decimal> m_output;
        Func<decimal> m_input;
        internal Action<decimal> Output {
            [DebuggerStepperBoundary]
            get { return m_output; }
            [DebuggerStepperBoundary]
            set { m_output = value; Reset(); }
        }
        internal Func<decimal> Input {
            [DebuggerStepperBoundary]
            get { return m_input; }
            [DebuggerStepperBoundary]
            set { m_input = value; Reset(); }
        }

        bool m_fresh = true;
        internal bool FreshInput {
            [DebuggerStepperBoundary]
            get { return m_fresh; }
            [DebuggerStepperBoundary]
            set { m_fresh = value; }
        }

        internal void Disconnect() {
            m_output = Void_Output;
            m_input = Void_Input;
            Reset();
        }

        /// <summary>  Brings the engine back to its initial
        /// state
        /// </summary>
        internal void Reset() {

            machine.Reset();
            UnsetOp(this);
            reg1 = reg2 = 0.0M/*%*/;
            m_fresh = true;

        }

        decimal reg1;
        decimal reg2;

        static BinaryOperation FirstParam = (dec1, dec2) => dec1;
        BinaryOperation producer = FirstParam;
        BinaryOperation buffer = FirstParam;

        void HandleBinaryOperation(BinaryOperation op) {

            if (FreshInput) {
                Move(CalculatorStep.Type);
                FreshInput = false;
            }

            buffer = op;
            Move(CalculatorStep.Operator);
        }

        void HandleEq() {

            if (FreshInput) {
                Move(CalculatorStep.Type);
                FreshInput = false;
            }

            Move(CalculatorStep.Eq);
        }

        public void Operate(CalculatorActions act) {

            if (m_input == null)
                throw new InvalidOperationException("Input channel not specified");

            if (m_output == null)
                throw new InvalidOperationException("Output channel not specified");

            if (act == CalculatorActions.Eq) {
                HandleEq();
                return;
            }

            int ind;

            ind = bin_ops.IndexOf(act);
            if (ind != -1) {
                HandleBinaryOperation(b_ops[ind]);
                return;
            }

            //ind = un_ops.IndexOf(act);
            //if (ind != -1) {
            //    HandleUnaryOperation(u_ops[ind]);
            //    return;
            //}

            throw new ArgumentException("Unknown operation: " + act.Get_String());
        }

        static DivideByZeroHandling div_by_0 = DivideByZeroHandling.ReturnZero;
        static public DivideByZeroHandling DivideByZeroHandling {
            get { return div_by_0; }
            set { div_by_0 = value; }
        }

        static bool allow_neg = false;
        static public bool AllowNegativeValues {
            get { return allow_neg; }
            set { allow_neg = value; }
        }
    }

    public enum DivideByZeroHandling
    {
        ReturnZero,
        ThrowException
    }
    public enum CalculatorActions
    {
        Add = 0x1,
        Sub,
        Mul,
        Div,
        Eq,
        Clr,
        Sqrt
    }
    public enum CalculatorStep
    {
        Type,
        Operator,
        Eq
    }
}
