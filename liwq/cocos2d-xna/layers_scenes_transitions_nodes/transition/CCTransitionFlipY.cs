
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionFlipY : CCTransitionSceneOriented
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval inA, outA;
            m_pInScene.Visible = false;

            float inDeltaZ, inAngleZ;
            float outDeltaZ, outAngleZ;

            if (m_eOrientation == tOrientation.kOrientationUpOver)
            {
                inDeltaZ = 90;
                inAngleZ = 270;
                outDeltaZ = 90;
                outAngleZ = 0;
            }
            else
            {
                inDeltaZ = -90;
                inAngleZ = 90;
                outDeltaZ = -90;
                outAngleZ = 0;
            }

            inA = (CCActionInterval)CCSequence.actions
                (
                    CCDelayTime.actionWithDuration(m_fDuration / 2),
                    CCShow.action(),
                    new CCOrbitCamera(m_fDuration / 2, 1, 0, inAngleZ, inDeltaZ, 90, 0),
                    CCCallFunc.actionWithTarget(this, base.finish)
                );
            outA = (CCActionInterval)CCSequence.actions
                (
                    new CCOrbitCamera(m_fDuration / 2, 1, 0, outAngleZ, outDeltaZ, 90, 0),
                    CCHide.action(),
                    CCDelayTime.actionWithDuration(m_fDuration / 2)
                );

            m_pInScene.RunAction(inA);
            m_pOutScene.RunAction(outA);
        }

        public static CCTransitionFlipY transitionWithDuration(float t, CCScene s, tOrientation o)
        {
            CCTransitionFlipY pScene = new CCTransitionFlipY();
            pScene.initWithDuration(t, s, o);

            return pScene;
        }
    }
}
