using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class Idle : State
    {
        public Idle()
        {
            Debug.Log("This is the Idle State");
        }

        public override void Enter(IState previousState)
        {
            Debug.Log("[Idle] Enter");
        }
        
        public override void Exit(IState previousState)
        {
            Debug.Log("[Idle] Exit");
        }
    }
}