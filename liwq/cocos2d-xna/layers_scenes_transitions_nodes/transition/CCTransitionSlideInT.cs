using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionSlideInT : CCTransitionSlideInL
    {

        /// <summary>
        /// initializes the scenes
        /// </summary>
        public override void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(0, s.Height - 0.5f);
        }

        /// <summary>
        /// returns the action that will be performed by the incomming and outgoing scene
        /// </summary>
        /// <returns></returns>
        public override CCActionInterval action()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            return CCMoveBy.actionWithDuration(m_fDuration, new CCPoint(0, -(s.Height - 0.5f)));
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionSlideInT);
        public static new CCTransitionSlideInT transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSlideInT pScene = new CCTransitionSlideInT();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        protected override void sceneOrder()
        {
            m_bIsInSceneOnTop = false;
        }
    }
}
