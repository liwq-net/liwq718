using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionRotoZoom : CCTransitionScene
    {
        public override void onEnter()
        {
            base.onEnter();

            m_pInScene.Scale = 0.001f;
            m_pOutScene.Scale = 1.0f;

            m_pInScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval rotozoom = (CCActionInterval)(CCSequence.actions
            (
                CCSpawn.actions
                (
                    CCScaleBy.actionWithDuration(m_fDuration / 2, 0.001f),
                    CCRotateBy.actionWithDuration(m_fDuration / 2, 360 * 2)
                ),
                CCDelayTime.actionWithDuration(m_fDuration / 2)
            ));

            m_pOutScene.RunAction(rotozoom);
            m_pInScene.RunAction
            (
                CCSequence.actions
                (
                    rotozoom.Reverse(),
                    CCCallFunc.actionWithTarget(this, (base.finish))
                )
            );
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionRotoZoom);
        public static CCTransitionRotoZoom transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionRotoZoom pScene = new CCTransitionRotoZoom();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}
