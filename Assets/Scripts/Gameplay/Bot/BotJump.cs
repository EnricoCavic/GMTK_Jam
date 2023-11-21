using UnityEngine;

namespace DPA.Gameplay
{
    using Generic;

    public class BotJump : IState
    {
        readonly BotNavigator bot;

        public BotJump(BotNavigator _bot) => bot = _bot;

        public void Enter()
        {
            bot.Jump();
            Debug.Log("JumpState Enter");
        }
        public void Tick()
        {

        }
        public IState CheckTransitions()
        {
            return this;
        }
        public void Exit()
        {

        }

    }

}