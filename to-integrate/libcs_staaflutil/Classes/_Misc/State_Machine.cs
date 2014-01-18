using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fairweather.Service
{
    public class State_Machine<TUser>
    where TUser : IState_Machine_User<TUser>
    {
        public State_Machine(TUser user) {

            m_location = m_initial_state = user.InitialState;

            allowed_nodes = new Set<string>(user.Nodes);
            allowed_steps = user.Steps;

            this.Transitions = user.Transitions;
            this.Jumps = user.Jumps;
            this.user = user;

        }


        readonly TUser user;
        readonly string m_initial_state;

        public void Reset() {
            m_location = m_initial_state;
        }

        protected readonly char[] allowed_steps;
        protected readonly Set<string> allowed_nodes;

        protected readonly Dictionary<string, Dictionary<string, Action<TUser>>> Transitions;
        protected readonly Dictionary<string, string> Jumps;

        protected string m_location;
        public string Location {
            [DebuggerStepThrough]
            get { return m_location; }
            protected set { m_location = value; }
        }

        [DebuggerStepThrough]
        protected bool Is_Allowed_Step(Char ch) {

            bool ret = allowed_steps.Contains(ch);

            return ret;
        }
        [DebuggerStepThrough]
        protected bool Is_Allowed_Node(string node) {

            bool ret = allowed_nodes.Contains(node);

            return ret;
        }
        [DebuggerStepThrough]
        protected bool Is_Leaf(string node, out string destination) {

            bool ret = Jumps.TryGetValue(node, out destination);

            return ret;
        }
        [DebuggerStepThrough]
        protected bool Is_Allowed_Transition(string from, string to, out Action<TUser> transition) {

            bool ret;
            if (!Transitions.ContainsKey(from)) {
                ret = false;
                transition = null;
            }
            else
                ret = Transitions[from].TryGetValue(to, out transition);

            return ret;
        }

        public void Move(char step) {
            string destination;
            Action<TUser> transition;

            if (!Is_Allowed_Step(step))
                throw new ArgumentException();

            string proposed = Location + step;

            //these two can be coalesced
            if (!Is_Allowed_Node(proposed))
                throw new ArgumentException();

            if (!Is_Allowed_Transition(Location, proposed, out transition))
                throw new ArgumentException();
            //

            //Console.WriteLine("{0} -> {1}", Location, proposed);
            Location = proposed;

            transition(user);

            if (Is_Leaf(Location, out destination))
                Jump(destination);
        }

        protected void Jump(string destination) {

            //Console.WriteLine("{0} -> -> {1}", Location, destination);
            Location = destination;
        }
    }
}
