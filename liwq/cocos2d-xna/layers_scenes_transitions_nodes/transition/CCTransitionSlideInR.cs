using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionSlideInR : CCTransitionSlideInL
    {
        /// <summary>
        /// initializes the scenes 
        /// </summary>
        public override void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(s.Width - 0.5f, 0);
        }

        /// <summary>
        /// returns the action that will be performed by the incomming and outgoing scene
        /// </summary>
        /// <returns></returns>
        public override CCActionInterval action()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            return CCMoveBy.actionWithDuration(m_fDuration, new CCPoint(-(s.Width - 0.5f), 0));
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionSlideInR);
        public static new CCTransitionSlideInR transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSlideInR pScene = new CCTransitionSlideInR();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        protected override void sceneOrder()
        {
            m_bIsInSceneOnTop = true;
        }
    }
}
