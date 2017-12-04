using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class Climb : State
    {
        public Climb()
        {
            Debug.Log("This is the Run State");
        }
        
        public override void Enter(IState previousState)
        {
            Debug.Log("[Climb] Enter");
        }
        
        public override void Exit(IState previousState)
        {
            Debug.Log("[Climb] Exit");
        }
    }
}