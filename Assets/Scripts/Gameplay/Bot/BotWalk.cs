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
            Debug.Log("BotWalk Enter");
        }

        public void Tick()
        {
            isNearWall = bot.IsNearWall();
            canJumpOver = bot.CanJumpOver();

            if(isNearWall)
            {
                if(!canJumpOver)
                    bot.ChangeDirection();
            }

        }

        public IState CheckTransitions()
        {
            if(isNearWall && canJumpOver)
                return bot.botJump;

            return this;
        }

        public void Exit()
        {
            
        }
    }

}