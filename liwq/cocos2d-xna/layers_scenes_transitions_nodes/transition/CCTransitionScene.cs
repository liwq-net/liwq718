using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// Base class for CCTransition scenes
    /// </summary>
    public class CCTransitionScene : CCScene
    {
        protected CCScene m_pInScene;
        protected CCScene m_pOutScene;
        protected float m_fDuration;
        protected bool m_bIsInSceneOnTop;
        protected bool m_bIsSendCleanupToScene;

        public CCTransitionScene()
        {
        }

        public override void draw()
        {
            base.draw();

            if (m_bIsInSceneOnTop)
            {
                m_pInScene.visit();
                m_pOutScene.visit();
                m_pInScene.visitDraw();
            }
            else
            {
                m_pOutScene.visit();
                m_pInScene.visit();
                m_pOutScene.visitDraw();
            }
        }

        public override void onEnter()
        {
            base.onEnter();
            m_pInScene.onEnter();
        }

        public override void onExit()
        {
            base.onExit();
            m_pOutScene.onExit();

            // inScene should not receive the onExit callback
            // only the onEnterTransitionDidFinish
            m_pInScene.onEnterTransitionDidFinish();
        }

        public override void Cleanup()
        {
            base.Cleanup();

            if (m_bIsSendCleanupToScene)
                m_pOutScene.Cleanup();
        }


        /// <summary>
        /// creates a base transition with duration and incoming scene 
        /// </summary>
        public static CCTransitionScene transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionScene pScene = new CCTransitionScene();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }

        /// <summary>
        ///  initializes a transition with duration and incoming scene
        /// </summary>
        public virtual bool initWithDuration(float t, CCScene scene)
        {
            if (scene == null)
            {
                throw (new ArgumentNullException("scene", "Target scene must not be null"));
            }

            if (base.init())
            {
                m_fDuration = t;

                // retain
                m_pInScene = scene;
                m_pOutScene = Director.SharedDirector.RunningScene;
                m_eSceneType = ccSceneFlag.ccTransitionScene;

                if (m_pInScene == m_pOutScene)
                {
                    throw (new ArgumentException("scene", "Target and source scenes must be different"));
                }

                // disable events while transitions
                CCTouchDispatcher.sharedDispatcher().IsDispatchEvents = false;
                this.sceneOrder();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// called after the transition finishes
        /// </summary>
        public void finish()
        {
            // clean up 	
            m_pInScene.Visible = true;
            m_pInScene.Position = new CCPoint(0, 0);
            m_pInScene.Scale = 1.0f;
            m_pInScene.Rotation = 0.0f;
            m_pInScene.Camera.restore();

            m_pOutScene.Visible = false;
            m_pOutScene.Position = new CCPoint(0, 0);
            m_pOutScene.Scale = 1.0f;
            m_pOutScene.Rotation = 0.0f;
            m_pOutScene.Camera.restore();

            //[self schedule:@selector(setNewScene:) interval:0];
            this.schedule(this.setNewScene, 0);
        }

        /// <summary>
        ///  used by some transitions to hide the outter scene
        /// </summary>
        public void hideOutShowIn()
        {
            m_pInScene.Visible = true;
            m_pOutScene.Visible = false;
        }

        protected virtual void sceneOrder()
        {
            m_bIsInSceneOnTop = true;
        }

        private void setNewScene(float dt)
        {
            // [self unschedule:_cmd]; 
            // "_cmd" is a local variable automatically defined in a method 
            // that contains the selector for the method
            this.unschedule(this.setNewScene);
            Director director = Director.SharedDirector;
            // Before replacing, save the "send cleanup to scene"
            m_bIsSendCleanupToScene = director.IsSendCleanupToScene;
            director.ReplaceScene(m_pInScene);
            // enable events while transitions
            CCTouchDispatcher.sharedDispatcher().IsDispatchEvents = true;
            // issue #267
            m_pOutScene.Visible = true;
        }
    }
}
