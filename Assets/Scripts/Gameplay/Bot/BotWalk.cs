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

            if (isNearWall && !canJumpOver)
                bot.ChangeDirection();

        }

        public IState CheckTransitions()
        {
            if(isNearWall && canJumpOver || bot.IsNearHole())
                return bot.botJump;

            return this;
        }

        public void Exit()
        {
            
        }
    }

}