
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionFadeDown : CCTransitionFadeTR
    {
        public override CCActionInterval actionWithSize(ccGridSize size)
        {
            return CCFadeOutDownTiles.actionWithSize(size, m_fDuration);
        }

        //public  DECLEAR_TRANSITIONWITHDURATION(CCTransitionFadeDown)
        public new static CCTransitionFadeDown transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionFadeDown pScene = new CCTransitionFadeDown();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }

    }
}
