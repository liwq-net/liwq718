using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief A transition which peels back the bottom right hand corner of a scene
    /// to transition to the scene beneath it simulating a page turn.
    ///    This uses a 3DAction so it's strongly recommended that depth buffering
    /// is turned on in CCDirector using:
    /// CCDirector::sharedDirector()->setDepthBufferFormat(kDepthBuffer16);
    /// @since v0.8.2
    /// </summary>
    public class CCTransitionPageTurn : CCTransitionScene
    {
        protected bool m_bBack;

        public CCTransitionPageTurn()
        { }

        /// <summary>
        /// Creates a base transition with duration and incoming scene.
        /// If back is true then the effect is reversed to appear as if the incoming 
        /// scene is being turned from left over the outgoing scene.
        /// </summary>
        public static CCTransitionPageTurn transitionWithDuration(float t, CCScene scene, bool backwards)
        {
            CCTransitionPageTurn pTransition = new CCTransitionPageTurn();
            pTransition.initWithDuration(t, scene, backwards);

            return pTransition;
        }

        /// <summary>
        /// Creates a base transition with duration and incoming scene.
        /// If back is true then the effect is reversed to appear as if the incoming 
        /// scene is being turned from left over the outgoing scene.
        /// </summary>
        public virtual bool initWithDuration(float t, CCScene scene, bool backwards)
        {
            // XXX: needed before [super init]
            m_bBack = backwards;

            if (base.initWithDuration(t, scene))
            {
                // do something
            }

            return true;
        }

        public CCActionInterval actionWithSize(ccGridSize vector)
        {
            if (m_bBack)
            {
                // Get hold of the PageTurn3DAction
                return CCReverseTime.actionWithAction
                (
                    CCPageTurn3D.actionWithSize(vector, m_fDuration)
                );
            }
            else
            {
                // Get hold of the PageTurn3DAction
                return CCPageTurn3D.actionWithSize(vector, m_fDuration);
            }
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = Director.SharedDirector.DesignSize;
            int x, y;
            if (s.Width > s.Height)
            {
                x = 16;
                y = 12;
            }
            else
            {
                x = 12;
                y = 16;
            }

            CCActionInterval action = this.actionWithSize(ccTypes.ccg(x, y));

            if (!m_bBack)
            {
                m_pOutScene.RunAction(CCSequence.actions
                    (
                        action,
                        CCCallFunc.actionWithTarget(this, base.finish),
                        CCStopGrid.action()));
            }
            else
            {
                // to prevent initial flicker
                m_pInScene.Visible = false;
                m_pInScene.RunAction(CCSequence.actions
                    (
                        CCShow.action(),
                        action,
                        CCCallFunc.actionWithTarget(this, base.finish),
                        CCStopGrid.action()));
            }
        }

        protected override void sceneOrder()
        {
            m_bIsInSceneOnTop = m_bBack;
        }
    }
}
