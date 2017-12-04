namespace Flusk.Utility
{
    public interface IState
    {
        /// <summary>
        /// First moment of state
        /// </summary>
        void Enter(IState previousState);

        /// <summary>
        /// State update
        /// </summary>
        void Tick();

        /// <summary>
        /// Last moment of state
        /// </summary>
        void Exit(IState nextState);
    }
}
