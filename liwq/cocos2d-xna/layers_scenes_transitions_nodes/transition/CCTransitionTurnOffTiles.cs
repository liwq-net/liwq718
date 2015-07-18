using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionTurnOffTiles:
    /// Turn off the tiles of the outgoing scene in random order
    /// </summary>
    public class CCTransitionTurnOffTiles : CCTransitionScene, ICCTransitionEaseScene
    {
        public override void onEnter()
        {
            base.onEnter();
            CCSize s = Director.SharedDirector.DesignSize;
            float aspect = s.Width / s.Height;
            int x = (int)(12 * aspect);
            int y = 12;

            CCTurnOffTiles toff = CCTurnOffTiles.actionWithSize(new ccGridSize(x, y), m_fDuration);
            CCFiniteTimeAction action = easeActionWithAction(toff);
            m_pOutScene.RunAction
            (
                CCSequence.actions
                (
                    action,
                    CCCallFunc.actionWithTarget(this, (base.finish)),
                    CCStopGrid.action()
                )
            );
        }

        public virtual CCFiniteTimeAction easeActionWithAction(CCActionInterval action)
        {
            return action;
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionTurnOffTiles);
        public new static CCTransitionTurnOffTiles transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionTurnOffTiles pScene = new CCTransitionTurnOffTiles();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }

        protected override void sceneOrder()
        {
            m_bIsInSceneOnTop = false;
        }
    }
}
