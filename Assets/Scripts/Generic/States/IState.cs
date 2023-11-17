namespace DPA.Generic
{
    public interface IState
    {
        void Enter();

        void Tick();

        IState CheckTransitions();

        void Exit();

    }
}