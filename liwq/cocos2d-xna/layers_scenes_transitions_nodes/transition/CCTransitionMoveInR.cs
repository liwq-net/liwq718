using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionMoveInR : CCTransitionMoveInL
    {
        public override void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(s.Width, 0);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionMoveInR);
        public static new CCTransitionMoveInR transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionMoveInR pScene = new CCTransitionMoveInR();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}
