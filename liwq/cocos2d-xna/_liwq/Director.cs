using liwq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

//todo 与application合并？

namespace cocos2d
{
    #region DeviceOrientation
    /// <summary>Possible device orientations</summary>
    public enum DeviceOrientation
    {
        Default = 0,
        LandscapeLeft = 1,
        LandscapeRight = 2,
        Portrait = 4,
        PortraitDown = 8,
        Unknown = 16,
    }
    #endregion //DeviceOrientation


    #region DirectorProjection
    public enum DirectorProjection
    {
        /// sets a 2D projection (orthogonal projection)
        Projection2D,
        /// sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        Projection3D,
        /// it calls "updateProjection" on the projection delegate.
        ProjectionCustom,
        /// Detault projection is 3D projection
        ProjectionDefault = Projection3D,
    }
    #endregion //DirectorProjection

    public class Director
    {
        static Director _SharedDirector;
        public static Director SharedDirector
        {
            get
            {
                if (_SharedDirector == null)
                {
                    _SharedDirector = new Director();
                    _SharedDirector.Init();
                }
                return _SharedDirector;
            }
        }

        // this flag will be set to true in end()
        protected bool _isPurgeDirecotorInNextLoop;


        public void MainLoop(GameTime gameTime)
        {
            if (this._isPurgeDirecotorInNextLoop)
            {
                this._purgeDirector();
                this._isPurgeDirecotorInNextLoop = false;
            }
            else if (this._animationStoped == false)
            {
                this._drawScene(gameTime);
            }
        }

        public void EndLoop() { this._isPurgeDirecotorInNextLoop = true; }

        protected void _purgeDirector()
        {
            if (this.RunningScene != null)
            {
                this.RunningScene.onExit();
                this.RunningScene.Cleanup();
            }
            this.RunningScene = null;
            this._nextScene = null;
            this._scenesStack.Clear();
            this.StopAnimation();
        }

        public virtual bool Init()
        {
            const double DefaultFPS = 60.0;
            this._animationInterval = 1.0 / DefaultFPS;

            this.IsPaused = false;
            this._isPurgeDirecotorInNextLoop = false;
            this._contentScaleFactor = 1.0f;

            // Set default projection (3D)
            this._projection = DirectorProjection.ProjectionDefault;
            this.Projection = DirectorProjection.ProjectionDefault;

            Application.SharedApplication.ScreenScaleFactor = this.ContentScaleFactor;
            Application.SharedApplication.TouchDelegate = CCTouchDispatcher.sharedDispatcher();
            CCTouchDispatcher.sharedDispatcher().IsDispatchEvents = true;

            return true;
        }

        #region Scenes Management

        //will be the next 'runningScene' in the next frame
        //nextScene is a weak reference.
        protected CCScene _nextScene;

        /// <summary>scheduled scenes</summary>
        protected List<CCScene> _scenesStack = new List<CCScene>();

        // This object will be visited after the scene. Useful to hook a notification node
        protected Node _notificationNode;

        /// <summary>Whether or not the Director is paused</summary>
        public bool IsPaused { get; protected set; }

        /// <summary>Draw the scene. This method is called every frame. Don't call it manually.</summary>
        protected void _drawScene(GameTime gameTime)
        {
            //tick before glClear: issue #533
            if (this.IsPaused == false)
            {
                this.FPSOneFrameElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //to avoid flickr, nextScene MUST be here: after tick and before draw.
            //XXX: Which bug is this one. It seems that it can't be reproduced with v0.9
            if (this._nextScene != null)
            {
                this._setNextScene();
            }

            // draw the scene
            if (this.RunningScene != null)
            {
                this.RunningScene.visit();
            }

            // draw the notifications node
            if (this._notificationNode != null)
            {
                this._notificationNode.visit();
            }

            if (this.IsDisplayFPS == true)
            {
                this._showFPS();
            }
        }

        protected void _setNextScene()
        {
            // If it is not a transition, call onExit/cleanup
            if ((this._nextScene is CCTransitionScene) == false)
            {
                if (this.RunningScene != null)
                {
                    this.RunningScene.onExit();
                    Application.SharedApplication.ClearTouches();
                }

                // issue #709. the root node (scene) should receive the cleanup message too otherwise it might be leaked.
                if (this.IsSendCleanupToScene && this.RunningScene != null)
                {
                    this.RunningScene.Cleanup();
                }
            }

            this.RunningScene = this._nextScene;
            this._nextScene = null;
            if (this.RunningScene != null)
            {
                this.RunningScene.onEnter();
                if (this.RunningScene is CCTransitionScene)
                {
                    this.RunningScene.onEnterTransitionDidFinish();
                }
            }
        }

        /// <summary>
        /// Suspends the execution of the running scene, pushing it on the stack of suspended scenes.
        /// The new scene will be executed.
        /// Try to avoid big stacks of pushed scenes to reduce memory allocation. 
        /// ONLY call it if there is a running scene.
        /// </summary>
        public void PushScene(CCScene scene)
        {
            Debug.Assert(scene != null, "scene cannot be null");
            this.IsSendCleanupToScene = false;
            this._scenesStack.Add(scene);
            this._nextScene = scene;
        }

        /// <summary>
        /// Pops out a scene from the queue. This scene will replace the running one.
        /// The running scene will be deleted. If there are no more scenes in the stack the execution is terminated.
        /// ONLY call it if there is a running scene.
        /// </summary>
        public void PopScene()
        {
            Debug.Assert(this.RunningScene != null, "RunningScene cannot be null");
            if (this._scenesStack.Count > 0)
            {
                this._scenesStack.RemoveAt(this._scenesStack.Count - 1);
            }
            int c = this._scenesStack.Count;
            if (c == 0)
            {
                Application.SharedApplication.Game.Exit();
                this.EndLoop();
            }
            else
            {
                this.IsSendCleanupToScene = true;
                this._nextScene = this._scenesStack[c - 1];
            }
        }

        /// <summary>
        /// Enters the Director's main loop with the given Scene. Call it to run only your FIRST scene.
        /// Don't call it if there is already a running scene.
        /// </summary>
        public void RunWithScene(CCScene scene)
        {
            Debug.Assert(scene != null, "scene cannot be null");
            Debug.Assert(this.RunningScene == null, "RunningScene cannot be null");
            this.PushScene(scene);
            this.StartAnimation();
        }

        /// <summary>
        /// Replaces the running scene with a new one. The running scene is terminated.
        /// ONLY call it if there is a running scene.
        /// </summary>
        public void ReplaceScene(CCScene scene)
        {
            Debug.Assert(scene != null, "scene cannot be null");
            int index = this._scenesStack.Count;
            this.IsSendCleanupToScene = true;
            this._scenesStack[index - 1] = scene;
            this._nextScene = scene;
        }

        public CCScene LastScene
        {
            get
            {
                if (this._scenesStack.Count > 1)
                    return this._scenesStack[this._scenesStack.Count - 2];
                else
                    return null;
            }
        }

        ///<summary>
        /// If YES, then "old" scene will receive the cleanup message
        /// Whether or not the replaced scene will receive the cleanup message.
        /// If the new scene is pushed, then the old scene won't receive the "cleanup" message.
        /// If the new scene replaces the old one, the it will receive the "cleanup" message.
        /// </summary>
        public bool IsSendCleanupToScene { get; protected set; }

        #endregion //Scenes Management

        #region FPS

        /// <summary>Whether or not to display the FPS on the bottom-left corner</summary>
        public bool IsDisplayFPS { get; set; }
        /// <summary>How many frames were called since the director started</summary>
        public uint FPSFrames { get; protected set; }
        public float FPSOneFrameElapsed { get; protected set; }
        public float FPSElapsed { get; protected set; }
        public float FPS { get; protected set; }

        protected void _showFPS()
        {
            this.FPSFrames++;
            this.FPSElapsed += this.FPSOneFrameElapsed;

            const float CC_DIRECTOR_FPS_INTERVAL = 0.5f;
            if (this.FPSElapsed > CC_DIRECTOR_FPS_INTERVAL)
            {
                this.FPS = this.FPSFrames / this.FPSElapsed;
                this.FPSFrames = 0;
                this.FPSElapsed = 0;
                System.Diagnostics.Debug.WriteLine(this.FPS);

                //SpriteFont font = Application.SharedApplication.Game.Content.Load<SpriteFont>(@"fonts/Arial");
                //Application.SharedApplication.SpriteBatch.Begin();
                //Application.SharedApplication.SpriteBatch.DrawString(
                //    font,
                //    fpsText,
                //    new Vector2(0, Application.SharedApplication.Size.Height - 50),
                //    new Color(0, 255, 255));
                //Application.SharedApplication.SpriteBatch.End();
            }
        }

        #endregion //FPS

        /// <summary>Get current running Scene. Director can only run one Scene at the time </summary>
        public CCScene RunningScene { get; protected set; }

        protected double _animationInterval;
        public double animationInterval
        {
            get { return this._animationInterval; }
            set
            {
                this._animationInterval = value;
                if (this._animationStoped == false)
                {
                    this.StopAnimation();
                    this.StartAnimation();
                }
            }
        }

        protected DirectorProjection _projection;
        public DirectorProjection Projection
        {
            set
            {
                CCSize size = Application.SharedApplication.Size;
                switch (value)
                {
                    case DirectorProjection.Projection2D:
                        {
                            Application.SharedApplication.ViewMatrix = Matrix.CreateLookAt(
                                new Vector3(size.Width / 2.0f, size.Height / 2.0f, 5.0f),
                                new Vector3(size.Width / 2.0f, size.Height / 2.0f, 0),
                                Vector3.Up
                                );
                            Application.SharedApplication.ProjectionMatrix = Matrix.CreateOrthographicOffCenter(
                                -size.Width / 2.0f,
                                size.Width / 2.0f, -size.Height / 2.0f,
                                size.Height / 2.0f, -1024.0f, 1024.0f
                                );
                            Application.SharedApplication.WorldMatrix = Matrix.Identity;
                        }
                        break;

                    case DirectorProjection.Projection3D:
                        {
                            Application.SharedApplication.ViewMatrix = Matrix.CreateLookAt(
                                new Vector3(size.Width / 2.0f, size.Height / 2.0f, size.Height / 1.1566f),
                                new Vector3(size.Width / 2.0f, size.Height / 2.0f, 0), Vector3.Up
                                );
                            Application.SharedApplication.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.0f, size.Width / size.Height, 0.5f, 1500.0f);
                            Application.SharedApplication.WorldMatrix = Matrix.Identity;
                        }
                        break;

                    default:
                        throw new System.NotImplementedException("Not Implemented Projection");
                }
                this._projection = value;
            }
        }

        /// <summary>
        /// returns the size of the OpenGL view in points.
        /// It takes into account any possible rotation (device orientation) of the window
        /// </summary>
        /// <returns></returns>
        public CCSize DesignSize
        {
            get { return Application.SharedApplication.Size; }
        }

        /// <summary>
        /// It takes into account any possible rotation (device orientation) of the window.
        /// On Mac winSize and winSizeInPixels return the same value.
        /// </summary>
        public CCSize DisplaySize
        {
            get { return new CCSize(this.DesignSize.Width * this.ContentScaleFactor, this.DesignSize.Height * this.ContentScaleFactor); }
        }

        //todo 移到ccpoint
        /// <summary>
        /// converts a UIKit coordinate to an OpenGL coordinate
        /// Useful to convert (multi) touches coordinates to the current layout (portrait or landscape)
        /// </summary>
        public CCPoint ConvertToGL(CCPoint p)
        {
            return new CCPoint(p.X, this.DesignSize.Height - p.Y);
        }

        //todo 移到ccpoint
        /// <summary>
        /// converts an OpenGL coordinate to a UIKit coordinate
        /// Useful to convert node points to window points for calls such as glScissor
        /// </summary>
        public CCPoint ConvertToUI(CCPoint p)
        {
            return new CCPoint(p.X, this.DesignSize.Height - p.Y);
        }

        protected double _animationIntervalPauseBackup;

        /// <summary>
        /// Pauses the running scene.
        /// The running scene will be _drawed_ but all scheduled timers will be paused
        /// While paused, the draw rate will be 4 FPS to reduce CPU consumption
        /// </summary>
        public void Pause()
        {
            if (this.IsPaused == true) return;
            this._animationIntervalPauseBackup = this._animationInterval;
            // when paused, don't consume CPU
            this.animationInterval = 1 / 4.0;   //todo 还没实现帧率改变，要添加app事件驱动模式
            this.IsPaused = true;
        }

        /// <summary>
        /// Resumes the paused scene
        /// The scheduled timers will be activated again.
        /// The "delta time" will be 0 (as if the game wasn't paused)
        /// </summary>
        public void Resume()
        {
            if (this.IsPaused == false) return;
            this.animationInterval = this._animationIntervalPauseBackup;
            this.IsPaused = false;
        }

        protected bool _animationStoped;
        /// <summary>
        ///  Stops the animation. Nothing will be drawn. The main loop won't be triggered anymore.
        ///  If you don't want to pause your animation call [pause] instead.
        /// </summary>
        public void StopAnimation() { this._animationStoped = true; }

        /// <summary>
        /// The main loop is triggered again. Call this function only if [stopAnimation] was called earlier
        /// warning Don't call this function to start the main loop. To run the main loop call runWithScene
        /// </summary>
        public void StartAnimation() { this._animationStoped = false; }


        //todo 该属性没意义，关键有用还是 Application.SharedApplication.ScreenScaleFactor
        protected float _contentScaleFactor = 1.0f;
        /// <summary>
        /// The size in pixels of the surface. It could be different than the screen size.
        /// High-res devices might have a higher surface size than the screen size.
        /// Only available when compiled using SDK >= 4.0.
        /// </summary>
        public float ContentScaleFactor
        {
            get { return this._contentScaleFactor; }
            set
            {
                if (value != this._contentScaleFactor)
                {
                    this._contentScaleFactor = value;
                    Application.SharedApplication.ScreenScaleFactor = value;
                    this.Projection = this._projection;
                }
            }
        }

    }

}
