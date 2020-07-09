using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// State machine based on actions
    /// </summary>
    /// <typeparam name="S">State</typeparam>
    public class AStateMachine<S>
    {
        private Dictionary<S, Action> _states = new Dictionary<S, Action>();

        private S _state;
        private Action _action;

        public AStateMachine<S> Push(
            S state)
        {
            _state = state;
            _action = _states[state];
            return this;
        }

        public AStateMachine<S> Publish(
            S state,
            Action action)
        {
            _states.Add(state, action);
            return this;
        }

        public AStateMachine<S> Update()
        {
            _action();
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCurrentState(
            S state)
        {
            return EqualityComparer<S>.Default.Equals(_state, state);
        }
    }
}
