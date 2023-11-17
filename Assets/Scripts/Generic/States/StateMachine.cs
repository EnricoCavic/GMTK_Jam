using UnityEngine;

namespace DPA.Generic
{
    public abstract class StateMachine : MonoBehaviour
    {
        // Debug vars
        //public string currentStateName;
        //private string fullStateName;

        public IState CurrentState { get; protected set; }
        public IState DefaultState { get; protected set; }

        private IState queuedState;
        private IState stateDecision;

        protected void InitializeDefaultState(IState defaultState)
        {
            DefaultState = defaultState;
            CurrentState = defaultState;
            CurrentState.Enter();
        }

        public virtual void Update()
        {
            stateDecision = CurrentState.CheckTransitions();
            // fullStateName = CurrentState.ToString();
            // currentStateName = fullStateName[(fullStateName.LastIndexOf('.') + 1)..];

            CurrentState.Tick();

            if (queuedState == null)
            { 
                if (CurrentState == stateDecision) return;
            }
            else
            {
                stateDecision = queuedState;
                queuedState = null;
            }

            CurrentState.Exit();
            CurrentState = stateDecision;
            CurrentState.Enter();

        }

        public void SetNextState(IState _nextState) => queuedState = _nextState;

        public void SetDefaultState() => queuedState = DefaultState;
    }
}