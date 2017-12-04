using Flusk.Utility;
using UnityEngine;

namespace StateBasedController
{
    /// <summary>
    /// A generalise state based controller
    /// </summary>
    /// <typeparam name="TStateMachine">
    /// The state machine type, inheriting from StateMachine
    /// </typeparam>
    /// <typeparam name="TState">
    /// The state type, inheriting from IState, and have a constructor for initialization
    /// </typeparam>
    public class Controller<TStateMachine, TState> : MonoBehaviour 
        where TState : IState, new ()
        where TStateMachine : StateMachine<TState>, new ()
    {
        public TStateMachine StateMachine { get; protected set; }

        /// <summary>
        /// Assign as many states as neccesary,
        /// the first state is the default state
        /// States must be initialised before adding
        /// </summary>
        public virtual void AssignStates<T>(params T [] states) where T : TState, new ()
        {
            StateMachine = new TStateMachine();
            int length = states.Length;
            if (length == 0)
            {
                Debug.LogError("[Controller] No states added!");
                return;
            }
            for (int i = 0; i < length; i++)
            {
                StateMachine.AddState(states[i]);
            }
            StateMachine.ChangeState(states[0]);
        }

        protected virtual void Update()
        {
            StateMachine.Tick();
        }
    }
}