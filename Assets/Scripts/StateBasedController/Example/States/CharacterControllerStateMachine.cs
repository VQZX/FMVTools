using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class CharacterControllerStateMachine : StateMachine<State>
    {
        /// <summary>
        /// A simple method to change states quickly
        /// </summary>
        public void NextState()
        {
            int length = states.Count;
            int nextIndex = (CurrentStateIndex + 1) % length;
            ChangeState(states[nextIndex]);
        }
    }
}