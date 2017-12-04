using StateBasedController.Example.States;
using UnityEngine;
using Idle = StateBasedController.Example.States.Idle;
using Jump = StateBasedController.Example.States.Jump;
using Climb = StateBasedController.Example.States.Climb;

namespace StateBasedController.Example
{
    /// <summary>
    /// An example of a state based character controller with some common example states
    /// </summary>
    public class CharacterControllerExample : Controller<CharacterControllerStateMachine, State >
    {
        /// <summary>
        /// The keycode to change the states by list for testing purposes
        /// </summary>
        [SerializeField]
        protected KeyCode changeStateKeyCode = KeyCode.Space;
        
        // A series of common action a character controller might have
#region States
        private Idle idle;
        private Run run;
        private Jump jump;
        private Climb climb;
#endregion
        
        /// <summary>
        /// Initialise the states, and assign them to the state machine
        /// </summary>
        protected virtual void Awake()
        {
            // initialise states
            idle = new Idle();
            run = new Run();
            jump = new Jump();
            climb = new Climb();
            AssignStates<State>(idle, run, jump, climb);                
        }

        /// <summary>
        /// Base functionality and listen for key press to loop through states
        /// </summary>
        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(changeStateKeyCode))
            {
                StateMachine.NextState();
            }
        }
    }
}