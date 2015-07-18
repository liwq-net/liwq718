using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTransitionMoveInL : CCTransitionScene, ICCTransitionEaseScene
    {
        /// <summary>
        /// initializes the scenes
        /// </summary>
        public virtual void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(-s.Width, 0);
        }

        /// <summary>
        /// returns the action that will be performed
        /// </summary>
        /// <returns></returns>
        public virtual CCActionInterval action()
        {
            return CCMoveTo.actionWithDuration(m_fDuration, new CCPoint(0, 0));
        }

        public override void onEnter()
        {
            base.onEnter();
            this.initScenes();

            CCActionInterval a = this.action();

            m_pInScene.RunAction
            (
                CCSequence.actions
                (
                    this.easeActionWithAction(a),
                    CCCallFunc.actionWithTarget(this, base.finish)
                )
            );
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionMoveInL);
        public static new CCTransitionMoveInL transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionMoveInL pScene = new CCTransitionMoveInL();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        public CCFiniteTimeAction easeActionWithAction(CCActionInterval action)
        {
            return CCEaseOut.actionWithAction(action, 2.0f);
        }
    }
}
