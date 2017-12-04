using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class State : IState
    {
        public State()
        {
            Debug.Log("This is the Example State");
        }
        
        public virtual void Enter(IState previousState)
        {
            
        }

        public virtual void Tick()
        {
            
        }

        public virtual void Exit(IState nextState)
        {
            
        }
    }
}