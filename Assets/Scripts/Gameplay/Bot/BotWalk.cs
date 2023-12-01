using UnityEngine;

namespace DPA.Gameplay
{
    using Generic;

    public class BotWalk : IState
    {
        readonly BotNavigator bot;
        bool isNearWall;
        bool canJumpOver;

        public BotWalk(BotNavigator _bot) => bot = _bot;

        public void Enter()
        {
            bot.animator.Play("Walk");
            bot.rb.velocity = new Vector2(bot.rb.velocity.x, 0f);
        }

        public void Tick()
        {
            isNearWall = bot.IsNearWall();
            canJumpOver = bot.CanJumpOverWall();

            if (isNearWall && !canJumpOver)
                bot.ChangeDirection();

        }

        public IState CheckTransitions()
        {
            if(bot.IsGrounded())
            {
                if((isNearWall || bot.IsNearHole()) && canJumpOver)
                    return bot.botJump;  
            }
            else
                return bot.botFall;
            
            return this;
        }

        public void Exit()
        {
            
        }
    }

}