using Flusk.Utility;
using UnityEngine;

namespace StateBasedController.Example.States
{
    public class Run : State
    {
        public Run()
        {
            Debug.Log("This is the Run State");
        }
        
        public override void Enter(IState previousState)
        {
            Debug.Log("[Run] Enter");
        }
        
        public override void Exit(IState previousState)
        {
            Debug.Log("[Run] Exit");
        }
    }
}