using UnityEngine;

namespace DPA.Gameplay
{
    using Generic;

    public class BotWalk : IState
    {
        readonly BotNavigator bot;
        bool isNearWall;
        bool canJumpOver;
        bool isGrounded;

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
            isGrounded = bot.IsGrounded();

            if (isNearWall && !canJumpOver)
                bot.ChangeDirection();

        }

        public IState CheckTransitions()
        {
            if((isNearWall || bot.IsNearHole()) && canJumpOver && isGrounded)
                return bot.botJump;  
        
            if(!isGrounded)
                return bot.botFall;
            
            return this;
        }

        public void Exit()
        {
            
        }
    }

}