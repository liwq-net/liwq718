using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionSplitRows:
    /// The odd rows goes to the left while the even rows goes to the right.
    /// </summary>
    public class CCTransitionSplitRows : CCTransitionSplitCols
    {
        public override CCActionInterval action()
        {
            return CCSplitRows.actionWithRows(3, m_fDuration / 2.0f);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionSplitRows)
        public new static CCTransitionSplitRows transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSplitRows pScene = new CCTransitionSplitRows();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }
    }
}
