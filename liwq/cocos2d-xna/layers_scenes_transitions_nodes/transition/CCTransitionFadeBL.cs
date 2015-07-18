using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionFadeBL:
    /// Fade the tiles of the outgoing scene from the top-right corner to the bottom-left corner.
    /// </summary>
    public class CCTransitionFadeBL : CCTransitionFadeTR
    {
        public override CCActionInterval actionWithSize(ccGridSize size)
        {
            return CCFadeOutBLTiles.actionWithSize(size, m_fDuration);
        }

        //public DECLEAR_TRANSITIONWITHDURATION(CCTransitionFadeBL)
        public new static CCTransitionFadeBL transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionFadeBL pScene = new CCTransitionFadeBL();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }
    }
}
