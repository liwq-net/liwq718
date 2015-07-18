using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionSlideInL : CCTransitionScene, ICCTransitionEaseScene
    {

        /// <summary>
        /// initializes the scenes
        /// </summary>
        public virtual void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(-(s.Width - 0.5f), 0);
        }

        /// <summary>
        /// returns the action that will be performed by the incomming and outgoing scene
        /// </summary>
        /// <returns></returns>
        public virtual CCActionInterval action()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            return CCMoveBy.actionWithDuration(m_fDuration, new CCPoint(s.Width - 0.5f, 0));
        }

        public override void onEnter()
        {
            base.onEnter();
            this.initScenes();

            CCActionInterval incAction = this.action();
            CCActionInterval outcAction = this.action();

            CCActionInterval inAction = easeActionWithAction(incAction);
            CCActionInterval outAction = (CCActionInterval)CCSequence.actions
            (
                easeActionWithAction(outcAction),
                CCCallFunc.actionWithTarget(this, (base.finish))
            );
            m_pInScene.RunAction(inAction);
            m_pOutScene.RunAction(outAction);
        }

        public virtual CCActionInterval easeActionWithAction(CCActionInterval action)
        {
            return CCEaseOut.actionWithAction(action, 2.0f);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionSlideInL);
        public static new CCTransitionSlideInL transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSlideInL pScene = new CCTransitionSlideInL();
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

        CCFiniteTimeAction ICCTransitionEaseScene.easeActionWithAction(CCActionInterval action)
        {
            throw new NotImplementedException();
        }
    }
}
