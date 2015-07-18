using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public enum tOrientation
    {
        /// An horizontal orientation where the Left is nearer
        kOrientationLeftOver = 0,
        /// An horizontal orientation where the Right is nearer
        kOrientationRightOver = 1,
        /// A vertical orientation where the Up is nearer
        kOrientationUpOver = 0,
        /// A vertical orientation where the Bottom is nearer
        kOrientationDownOver = 1,
    }

    public class CCTransitionSceneOriented : CCTransitionScene
    {
        protected tOrientation m_eOrientation;

        /// <summary>
        /// creates a base transition with duration and incoming scene
        /// </summary>
        public static CCTransitionSceneOriented transitionWithDuration(float t, CCScene scene, tOrientation orientation)
        {
            CCTransitionSceneOriented pScene = new CCTransitionSceneOriented();
            pScene.initWithDuration(t, scene, orientation);
            return pScene;
        }

        /// <summary>
        /// initializes a transition with duration and incoming scene
        /// </summary>
        public virtual bool initWithDuration(float t, CCScene scene, tOrientation orientation)
        {
            if (base.initWithDuration(t, scene))
            {
                m_eOrientation = orientation;
            }

            return true;
        }
    }
}
