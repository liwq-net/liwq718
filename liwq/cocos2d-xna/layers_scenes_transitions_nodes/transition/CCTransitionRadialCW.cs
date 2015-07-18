using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief A counter colock-wise radial transition to the next scene
    /// </summary>
    public class CCTransitionRadialCW : CCTransitionRadialCCW
    {
        const int kSceneRadial = int.MaxValue;

        public CCTransitionRadialCW()
        { }

        public override void onExit()
        {
            // remove our layer and release all containing objects 
            this.removeChildByTag(kSceneRadial, false);
            base.onExit();
        }

        public new static CCTransitionRadialCW transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionRadialCW pScene = new CCTransitionRadialCW();
            pScene.initWithDuration(t, scene);

            return pScene;
        }

        protected override CCProgressTimerType radialType()
        {
            return CCProgressTimerType.kCCProgressTimerTypeRadialCW;
        }
    }
}
