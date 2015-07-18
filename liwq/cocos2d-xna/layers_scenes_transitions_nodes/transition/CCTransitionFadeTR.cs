using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionFadeTR:
    /// Fade the tiles of the outgoing scene from the left-bottom corner the to top-right corner.
    /// </summary>
    public class CCTransitionFadeTR : CCTransitionScene, ICCTransitionEaseScene
    {
        public virtual CCActionInterval actionWithSize(ccGridSize size)
        {
            return CCFadeOutTRTiles.actionWithSize(size, m_fDuration);
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = Director.SharedDirector.DesignSize;
            float aspect = s.Width / s.Height;
            int x = (int)(12 * aspect);
            int y = 12;

            CCActionInterval action = actionWithSize(new ccGridSize(x, y));

            m_pOutScene.RunAction
            (
                CCSequence.actions
                (
                    easeActionWithAction(action),
                    CCCallFunc.actionWithTarget(this, base.finish),
                    CCStopGrid.action()
                )
            );
        }

        public virtual CCFiniteTimeAction easeActionWithAction(CCActionInterval action)
        {
            return action;
        }

        //public  DECLEAR_TRANSITIONWITHDURATION(CCTransitionFadeTR)
        public new static CCTransitionFadeTR transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionFadeTR pScene = new CCTransitionFadeTR();
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
