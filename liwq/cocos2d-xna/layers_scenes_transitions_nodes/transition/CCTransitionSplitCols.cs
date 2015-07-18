using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTransitionSplitCols:
    /// The odd columns goes upwards while the even columns goes downwards.
    /// </summary>
    public class CCTransitionSplitCols : CCTransitionScene, ICCTransitionEaseScene
    {
        public virtual CCActionInterval action()
        {
            return CCSplitCols.actionWithCols(3, m_fDuration / 2.0f);
        }

        public override void onEnter()
        {
            base.onEnter();
            m_pInScene.Visible = false;

            CCActionInterval split = action();
            CCActionInterval seq = (CCActionInterval)CCSequence.actions
            (
                split,
                CCCallFunc.actionWithTarget(this, (base.hideOutShowIn)),
                split.Reverse()
            );

            this.RunAction(CCSequence.actions(
                easeActionWithAction(seq),
                CCCallFunc.actionWithTarget(this, base.finish),
                CCStopGrid.action()));
        }

        public virtual CCFiniteTimeAction easeActionWithAction(CCActionInterval action)
        {
            return CCEaseInOut.actionWithAction(action, 3.0f);
        }

        //public   DECLEAR_TRANSITIONWITHDURATION(CCTransitionSplitCols);
        public new static CCTransitionSplitCols transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSplitCols pScene = new CCTransitionSplitCols();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }
    }
}
