using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionFadeUp:
    /// Fade the tiles of the outgoing scene from the bottom to the top.
    /// </summary>
    public class CCTransitionFadeUp : CCTransitionFadeTR
    {
        public override CCActionInterval actionWithSize(ccGridSize size)
        {
            return CCFadeOutUpTiles.actionWithSize(size, m_fDuration);
        }

        //public  DECLEAR_TRANSITIONWITHDURATION(CCTransitionFadeUp)
        public new static CCTransitionFadeUp transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionFadeUp pScene = new CCTransitionFadeUp();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }
    }
}
