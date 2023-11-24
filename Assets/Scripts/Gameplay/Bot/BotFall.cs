using UnityEngine;

namespace DPA.Gameplay
{
    using Generic;

    public class BotFall : IState
    {
        readonly BotNavigator bot;
        public BotFall(BotNavigator _bot) => bot = _bot;

        public void Enter()
        {

        }

        public void Tick()
        {
            bot.ApplyGravityMultiplier();
            if(bot.IsNearWall() && !bot.CanJumpOverWall())
                bot.ChangeDirection();
        }

        public IState CheckTransitions()
        {
            if(bot.IsGrounded())
                return bot.botWalk;

            return this;
        }

        public void Exit()
        {

        }
    }

}