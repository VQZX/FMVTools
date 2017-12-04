using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class Jump : State
    {
        public Jump()
        {
            Debug.Log("This is the Jump State");
        }
        
        public override void Enter(IState previousState)
        {
            Debug.Log("[Jump] Enter");
        }
        
        public override void Exit(IState previousState)
        {
            Debug.Log("[Jump] Exit");
        }
    }
}