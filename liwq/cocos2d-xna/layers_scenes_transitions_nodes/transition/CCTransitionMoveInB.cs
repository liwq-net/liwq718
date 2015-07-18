using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionMoveInB : CCTransitionMoveInL
    {

        public override void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(0, -s.Height);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionMoveInB);
        public static new CCTransitionMoveInB transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionMoveInB pScene = new CCTransitionMoveInB();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}