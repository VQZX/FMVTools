using System.Collections.Generic;

namespace Flusk.Utility
{
    public class StateMachine<TState> where TState : IState
    {
        protected List<TState> states;
        public TState CurrentState { get; protected set; }
        public int CurrentStateIndex { get; protected set; }

        public StateMachine()
        {
            states = new List<TState>();
        }

        public virtual void AddState(TState state)
        {
            states.Add(state);
        }

        public void Tick()
        {
            if (CurrentState == null)
            {
                return;
            }
            CurrentState.Tick();
        }

        public virtual void ChangeState(IState state)
        {
            var previousState = CurrentState;
            if (CurrentState != null)
            {
                CurrentState.Exit(state);
            }
            CurrentState = (TState) state;
            CurrentState.Enter(previousState);

            // find state index
            CurrentStateIndex = states.FindIndex(EqualState);
        }

        private bool EqualState(TState a)
        {
            return a.GetType() == CurrentState.GetType();
        }
    }
}
