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
        }

        public void Tick()
        {
            isNearWall = bot.IsNearWall();
            canJumpOver = bot.CanJumpOver();
            bot.ApplyGravity(3f);

            if (isNearWall && !canJumpOver)
                bot.ChangeDirection();

        }

        public IState CheckTransitions()
        {
            if(!bot.IsGrounded())
                return bot.botFall;
         
            if((isNearWall || bot.IsNearHole()) && canJumpOver)
                return bot.botJump;  

            return this;
        }

        public void Exit()
        {
            
        }
    }

}