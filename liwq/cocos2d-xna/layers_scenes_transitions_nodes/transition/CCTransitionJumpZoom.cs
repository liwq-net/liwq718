using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        public override void onEnter()
        {
            base.onEnter();
            CCSize s = Director.SharedDirector.DesignSize;

            m_pInScene.Scale = 0.5f;
            m_pInScene.Position = new CCPoint(s.Width, 0);
            m_pInScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval jump = CCJumpBy.actionWithDuration(m_fDuration / 4, new CCPoint(-s.Width, 0), s.Width / 4, 2);
            CCActionInterval scaleIn = CCScaleTo.actionWithDuration(m_fDuration / 4, 1.0f);
            CCActionInterval scaleOut = CCScaleTo.actionWithDuration(m_fDuration / 4, 0.5f);

            CCActionInterval jumpZoomOut = (CCActionInterval)(CCSequence.actions(scaleOut, jump));
            CCActionInterval jumpZoomIn = (CCActionInterval)(CCSequence.actions(jump, scaleIn));

            CCActionInterval delay = CCDelayTime.actionWithDuration(m_fDuration / 2);

            m_pOutScene.RunAction(jumpZoomOut);
            m_pInScene.RunAction
            (
                CCSequence.actions
                (
                    delay,
                    jumpZoomIn,
                    CCCallFunc.actionWithTarget(this, base.finish)
                )
            );
        }

        //public DECLEAR_TRANSITIONWITHDURATION(CCTransitionJumpZoom);
        public static new CCTransitionJumpZoom transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionJumpZoom pScene = new CCTransitionJumpZoom();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}
