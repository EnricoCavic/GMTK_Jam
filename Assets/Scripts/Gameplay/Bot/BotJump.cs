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
        }
        public void Tick()
        {

        }
        public IState CheckTransitions()
        {
            if(bot.IsGrounded())
                return bot.botWalk;

            if(bot.IsFalling)
                return bot.botFall;

            return this;
        }
        public void Exit()
        {

        }

    }

}