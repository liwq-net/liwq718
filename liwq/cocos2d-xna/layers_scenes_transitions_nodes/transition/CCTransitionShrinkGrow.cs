using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionShrinkGrow : CCTransitionScene, ICCTransitionEaseScene
    {
        public override void onEnter()
        {
            base.onEnter();

            m_pInScene.Scale = 0.001f;
            m_pOutScene.Scale = (1.0f);

            m_pInScene.AnchorPoint = new CCPoint(2 / 3.0f, 0.5f);
            m_pOutScene.AnchorPoint = new CCPoint(1 / 3.0f, 0.5f);

            CCActionInterval scaleOut = CCScaleTo.actionWithDuration(m_fDuration, 0.01f);
            CCActionInterval scaleIn = CCScaleTo.actionWithDuration(m_fDuration, 1.0f);

            m_pInScene.RunAction(this.easeActionWithAction(scaleIn));
            m_pOutScene.RunAction
            (
                CCSequence.actions
                (
                    this.easeActionWithAction(scaleOut),
                    CCCallFunc.actionWithTarget(this, (base.finish))
                )
            );
        }

        public virtual CCActionInterval easeActionWithAction(CCActionInterval action)
        {
            return CCEaseOut.actionWithAction(action, 2.0f);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionShrinkGrow);
        public static new CCTransitionShrinkGrow transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionShrinkGrow pScene = new CCTransitionShrinkGrow();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        CCFiniteTimeAction ICCTransitionEaseScene.easeActionWithAction(CCActionInterval action)
        {
            throw new NotImplementedException();
        }
    }
}
