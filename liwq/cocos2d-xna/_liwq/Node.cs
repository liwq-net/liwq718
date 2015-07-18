using liwq;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    enum NodeTag
    {
        kCCNodeTagInvalid = -1,
    };

    /** @brief CCNode is the main element. Anything thats gets drawn or contains things that get drawn is a CCNode.
	The most popular CCNodes are: CCScene, CCLayer, CCSprite, CCMenu.

	The main features of a CCNode are:
	- They can contain other CCNode nodes (addChild, getChildByTag, removeChild, etc)
	- They can schedule periodic callback (schedule, unschedule, etc)
	- They can execute actions (runAction, stopAction, etc)

	Some CCNode nodes provide extra functionality for them or their children.

	Subclassing a CCNode usually means (one/all) of:
	- overriding init to initialize resources and schedule callbacks
	- create callbacks to handle the advancement of time
	- overriding draw to render the node

	Features of CCNode:
	- position
	- scale (x, y)
	- rotation (in degrees, clockwise)
	- CCCamera (an interface to gluLookAt )
	- CCGridBase (to do mesh transformations)
	- anchor point
	- size
	- visible
	- z-order
	- openGL z position

	Default values:
	- rotation: 0
	- position: (x=0,y=0)
	- scale: (x=1,y=1)
	- contentSize: (x=0,y=0)
	- anchorPoint: (x=0,y=0)

	Limitations:
	- A CCNode is a "void" object. It doesn't have a texture

	Order in transformations with grid disabled
	-# The node will be translated (position)
	-# The node will be rotated (rotation)
	-# The node will be scaled (scale)
	-# The node will be moved according to the camera values (camera)

	Order in transformations with grid enabled
	-# The node will be translated (position)
	-# The node will be rotated (rotation)
	-# The node will be scaled (scale)
	-# The grid will capture the screen
	-# The node will be moved according to the camera values (camera)
	-# The grid will render the captured screen

	Camera:
	- Each node has a camera. By default it points to the center of the CCNode.
	*/
    public class Node : CCObject, SelectorProtocol
    {
        static int kCCNodeTagInvalid = -1;

        public Node()
        {
            // Only initialize the members that are not default value.

            this._scaleX = 1.0f;
            this._scaleY = 1.0f;
            this._position = CCPoint.Zero;
            this._positionInPixels = CCPoint.Zero;
            this.Visible = true;
            this._anchorPoint = CCPoint.Zero;
            this.AnchorPointInPixels = CCPoint.Zero;
            this._contentSize = CCSize.Zero;
            this._contentSizeInPixels = CCSize.Zero;
            this.IsRelativeAnchorPoint = true;
            this.Tag = kCCNodeTagInvalid;
            this._isTransformDirty = true;
            this._isInverseDirty = true;
            this.Children = new List<Node>();
            this._isTransformGLDirty = true;
        }

        ~Node()
        {
            //@todo CCLOGINFO( "cocos2d: deallocing" );
            foreach (Node node in Children)
            {
                node.Parent = null;
            }
        }
        
        #region scene managment

        /// <summary>
        /// callback that is called every time the CCNode enters the 'stage'.
        /// If the CCNode enters the 'stage' with a transition, this callback is called when the transition starts.
        /// During onEnter you can't a "sister/brother" node.
        /// </summary>
        public virtual void onEnter()
        {
            foreach (Node node in Children)
            {
                if (node != null)
                {
                    node.onEnter();
                }
            }

            resumeSchedulerAndActions();

            IsRunning = true;
        }

        /// <summary>
        /// callback that is called when the CCNode enters in the 'stage'.
        /// If the CCNode enters the 'stage' with a transition, this callback is called when the transition finishes.
        /// @since v0.8
        /// </summary>
        public virtual void onEnterTransitionDidFinish()
        {
            foreach (Node node in Children)
            {
                if (node != null)
                {
                    node.onEnterTransitionDidFinish();
                }
            }
        }

        /// <summary>
        ///  callback that is called every time the CCNode leaves the 'stage'.
        ///  If the CCNode leaves the 'stage' with a transition, this callback is called when the transition finishes.
        ///  During onExit you can't access a sibling node.
        /// </summary>
        public virtual void onExit()
        {
            pauseSchedulerAndActions();

            IsRunning = false;

            foreach (Node node in Children)
            {
                if (node != null)
                {
                    node.onExit();
                }
            }
        }

        #endregion

        #region composition: children

        /// <summary>Array of childrens </summary>
        public List<Node> Children { get; protected set; }

        /// <summary>
        /// Adds a child to the container with z-order as 0.
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// </summary>
        public virtual void addChild(Node child)
        {
            this.AddChild(child, child.ZOrder, child.Tag);
        }

        /// <summary>
        /// Adds a child to the container with a z-order
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// </summary>
        public virtual void addChild(Node child, int zOrder)
        {
            this.AddChild(child, zOrder, child.Tag);
        }

        /// <summary>
        /// Adds a child to the container with z order and tag
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// </summary>
        public virtual void AddChild(Node child, int zOrder, int tag)
        {
            if (child == null)
            {
                throw (new ArgumentNullException("child", "Child can not be null."));
            }
            if (child.Parent != null)
            {
                Debug.WriteLine("child in addChild is already added. Child tag=" + tag + ", parent=" + child.Parent.Tag);
                return;
            }

            this._insertChild(child, zOrder);
            child.Tag = tag;
            child.Parent = this;
            if (this.IsRunning)
            {
                child.onEnter();
                child.onEnterTransitionDidFinish();
            }
        }

        /// <summary>helper that reorder a child</summary>
        private void _insertChild(Node child, int z)
        {
            // Get last member
            Node last = this.Children.Count > 0 ? this.Children[this.Children.Count - 1] : null;
            if (last == null || last.ZOrder <= z)
            {
                this.Children.Add(child);
            }
            else
            {
                int index = 0;
                foreach (Node node in this.Children)
                {
                    if (node != null && node.ZOrder > z)
                    {
                        this.Children.Insert(index, child);
                        break;
                    }
                    ++index;
                }
            }
            child.ZOrder = z;
        }

        private void _detachChild(Node child, bool doCleanup)
        {
            // IMPORTANT:
            //  -1st do onExit
            //  -2nd cleanup
            if (this.IsRunning)
            {
                child.onExit();
            }
            // If you don't do cleanup, the child's actions will not get removed and the
            // its scheduledSelectors_ dict will not get released!
            if (doCleanup)
            {
                child.Cleanup();
            }
            // set parent nil at the end
            child.Parent = null;
            this.Children.Remove(child);
        }

        #endregion

        #region composition: REMOVE

        /// <summary>
        /// Remove itself from its parent node. If cleanup is true, then also remove all actions and callbacks.
        /// If the node orphan, then nothing happens.
        /// </summary>
        public void RemoveFromParentAndCleanup(bool cleanup)
        {
            this.Parent.RemoveChild(this, cleanup);
        }

        /// <summary>
        /// Removes a child from the container. It will also cleanup all running actions depending on the cleanup parameter.
        /// "remove" logic MUST only be on this method
        /// If a class want's to extend the 'removeChild' behavior it only needs
        /// to override this method
        /// </summary>
        public virtual void RemoveChild(Node child, bool cleanup)
        {
            // explicit nil handling
            if (this.Children == null)
            {
                return;
            }
            if (this.Children.Contains(child))
            {
                this._detachChild(child, cleanup);
            }
        }

        /// <summary>Removes a child from the container by tag value. It will also cleanup all running actions depending on the cleanup parameter</summary>
        public void removeChildByTag(int tag, bool cleanup)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            Node child = this.GetChildByTag(tag);
            if (child == null)
            {
                System.Diagnostics.Debug.WriteLine("removeChildByTag: child not found!");
            }
            else
            {
                this.RemoveChild(child, cleanup);
            }
        }

        /// <summary>Removes all children from the container and do a cleanup all running actions depending on the cleanup parameter.</summary>
        public virtual void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            // not using detachChild improves speed here
            foreach (Node node in this.Children)
            {
                if (node != null)
                {
                    // IMPORTANT:
                    //  -1st do onExit
                    //  -2nd cleanup
                    if (this.IsRunning)
                    {
                        node.onExit();
                    }

                    if (cleanup)
                    {
                        node.Cleanup();
                    }
                    // set parent nil at the end
                    node.Parent = null;
                }
            }
            this.Children.Clear();
        }

        #endregion

        #region composition: GET

        /// <summary>Gets a child from the container given its tag</summary>
        /// <returns>returns a CCNode object</returns>
        public Node GetChildByTag(int tag)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            foreach (Node node in this.Children)
            {
                if (node != null && node.Tag == tag)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Reorders a child according to a new z value.
        /// The child MUST be already added.
        /// </summary>
        public virtual void ReorderChild(Node child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");
            this.Children.Remove(child);
            this._insertChild(child, zOrder);
        }

        /// <summary>Stops all running actions and schedulers</summary>
        public virtual void Cleanup()
        {
            // actions
            this.StopAllActions();
            this.unscheduleAllSelectors();
            // timers
            foreach (Node node in this.Children)
            {
                if (node != null)
                {
                    node.Cleanup();
                }
            }
        }

        #endregion

        /// <summary>
        /// Override this method to draw your own node.
        ///	The following GL states will be enabled by default:
        ///- glEnableClientState(GL_VERTEX_ARRAY);
        ///- glEnableClientState(GL_COLOR_ARRAY);
        ///	- glEnableClientState(GL_TEXTURE_COORD_ARRAY);
        ///	- glEnable(GL_TEXTURE_2D);
        ///AND YOU SHOULD NOT DISABLE THEM AFTER DRAWING YOUR NODE
        ///But if you enable any other GL state, you should disable it after drawing your node.
        /// </summary>
        public virtual void draw()
        {
            // override me
            // Only use- this function to draw your stuff.
            // DON'T draw your stuff outside this method
        }

        /// <summary>
        /// recursive method that visit its children and draw them
        /// </summary>
        public virtual void visit()
        {
            Matrix world = Application.SharedApplication.BasicEffect.World;
            // quick return if not visible
            if (!Visible)
            {
                return;
            }
            
            _nodeTransform = Matrix.Identity;
            Matrix ori = Matrix.Identity * Application.SharedApplication.BasicEffect.View;

            if (this.Grid != null && this.Grid.Active)
            {
                this.Grid.beforeDraw();
                this.TransformAncestors();
            }

            Transform();

            Node node;
            int i = 0;

            if ((Children != null) && (Children.Count > 0))
            {
                // draw children zOrder < 0
                for (; i < Children.Count; ++i)
                {
                    node = Children[i];
                    if (node != null && node.ZOrder < 0)
                    {
                        Matrix world2 = Application.SharedApplication.BasicEffect.World;
                        node.visit();
                        Application.SharedApplication.BasicEffect.World = world2;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // self draw
            draw();

            // draw children zOrder >= 0
            if ((Children != null) && (Children.Count > 0))
            {
                for (; i < Children.Count; ++i)
                {
                    node = Children[i];

                    if (node != null)
                    {
                        Matrix world2 = Application.SharedApplication.BasicEffect.World;
                        node.visit();
                        Application.SharedApplication.BasicEffect.World = world2;
                    }
                }
            }


            if (this.Grid != null && this.Grid.Active)
            {
                this.Grid.afterDraw(this);
            }

            Application.SharedApplication.BasicEffect.World = world; // Matrix.Invert(m_tCCNodeTransform) * CCApplication.SharedApplication.basicEffect.World;
            Application.SharedApplication.BasicEffect.View = ori;
            Application.SharedApplication.ViewMatrix = ori;
        }

        /// <summary>
        /// only use by CCTransitionScene,this is a bad method,it should be replace by some good way
        /// </summary>
        public virtual void visitDraw()
        {
            // quick return if not visible
            if (!Visible)
            {
                return;
            }

            if (this.Grid != null && this.Grid.Active)
            {
                this.Grid.blit();
            }
        }


        /// <summary>
        /// The update function
        /// </summary>
        public virtual void update(float dt)
        {

        }

        #region transformations

        /// <summary>performs OpenGL view-matrix transformation based on position, scale, rotation and other attributes.</summary>
        public void Transform()
        {
            // transformations

            Application app = Application.SharedApplication;

            // BEGIN alternative -- using cached transform

            if (this._isTransformGLDirty)
            {
                CCAffineTransform t = this.nodeToParentTransform();
                TransformUtils.CGAffineToGL(t, ref _transformGL);
                this._isTransformGLDirty = false;
            }

            this._nodeTransform = TransformUtils.CGAffineToMatrix(this._transformGL);

            if (this._vertexZ > 0)
            {
                this._nodeTransform *= Matrix.CreateRotationZ(_vertexZ);
            }

            // XXX: Expensive calls. Camera should be integrated into the cached affine matrix
            if (this._camera != null && !(this.Grid != null && this.Grid.Active))
            {
                bool translate = (this.AnchorPointInPixels.X != 0.0f || this.AnchorPointInPixels.Y != 0.0f);

                Matrix? matrix = this._camera.locate();
                if (matrix != null)
                {
                    this._nodeTransform = Matrix.CreateTranslation(-AnchorPointInPixels.X, -AnchorPointInPixels.Y, 0) *
                        matrix.Value *
                        Matrix.CreateTranslation(AnchorPointInPixels.X, AnchorPointInPixels.Y, 0) *
                        this._nodeTransform;
                }
            }

            // This is ok here. Visit() will restore the world transform for the next node.
            app.BasicEffect.World = this._nodeTransform * app.BasicEffect.World;
        }

        /// <summary>
        /// performs OpenGL view-matrix transformation of it's ancestors.
        /// Generally the ancestors are already transformed, but in certain cases (eg: attaching a FBO)
        /// it's necessary to transform the ancestors again.
        /// </summary>
        public void TransformAncestors()
        {
            if (Parent != null)
            {
                Parent.TransformAncestors();
                Parent.Transform();
            }
        }

        #endregion

        #region boundingBox

        /// <summary>
        /// returns a "local" axis aligned bounding box of the node.
        /// The returned box is relative only to its parent.
        /// </summary>
        public CCRect BoundingBox()
        {
            CCRect ret = boundingBoxInPixels();
            return ccMacros.CC_RECT_PIXELS_TO_POINTS(ret);
        }

        /// <summary>
        /// returns a "local" axis aligned bounding box of the node in pixels.
        /// The returned box is relative only to its parent.
        /// The returned box is in Points.
        /// @since v0.99.5
        /// </summary>
        public CCRect boundingBoxInPixels()
        {
            CCRect rect = new CCRect(0, 0, this._contentSizeInPixels.Width, this._contentSizeInPixels.Height);
            return CCAffineTransform.CCRectApplyAffineTransform(rect, nodeToParentTransform());
        }

        #endregion

        #region actions

        /// <summary>
        /// Executes an action, and returns the action that is executed.
        /// The node becomes the action's target.
        /// @warning Starting from v0.8 actions don't retain their target anymore.
        /// @return 
        /// </summary>
        /// <returns>An Action pointer</returns>
        public CCAction RunAction(CCAction action)
        {
            Debug.Assert(action != null, "Argument must be non-nil");
            CCActionManager.sharedManager().addAction(action, this, !IsRunning);
            return action;
        }

        /// <summary>
        /// Removes all actions from the running action list
        /// </summary>
        public void StopAllActions()
        {
            CCActionManager.sharedManager().removeAllActionsFromTarget(this);
        }

        /// <summary>
        /// Removes an action from the running action list
        /// </summary>
        public void StopAction(CCAction action)
        {
            CCActionManager.sharedManager().removeAction(action);
        }

        /// <summary>
        /// Removes an action from the running action list given its tag
        /// @since v0.7.1
        /// </summary>
        public void StopActionByTag(int tag)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            CCActionManager.sharedManager().removeActionByTag(tag, this);
        }

        /// <summary>
        /// Gets an action from the running action list given its tag
        /// </summary>
        /// <returns>the Action the with the given tag</returns>
        public CCAction GetActionByTag(int tag)
        {
            Debug.Assert((int)tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            return CCActionManager.sharedManager().getActionByTag((uint)tag, this);
        }

        /// <summary>
        /// Returns the numbers of actions that are running plus the ones that are schedule to run (actions in actionsToAdd and actions arrays). 
        /// Composable actions are counted as 1 action. Example:
        /// If you are running 1 Sequence of 7 actions, it will return 1.
        /// If you are running 7 Sequences of 2 actions, it will return 7.
        /// </summary>
        public uint NumberOfRunningActions()
        {
            return CCActionManager.sharedManager().numberOfRunningActionsInTarget(this);
        }

        #endregion

        #region timers/Schedule

        ///@todo
        //check whether a selector is scheduled
        // bool isScheduled(SEL_SCHEDULE selector);

        /// <summary>
        /// schedules the "update" method. It will use the order number 0. This method will be called every frame.
        /// Scheduled methods with a lower order value will be called before the ones that have a higher order value.
        /// Only one "update" method could be scheduled per node.
        /// @since v0.99.3
        /// </summary>
        public void scheduleUpdate()
        {
            scheduleUpdateWithPriority(0);
        }

        [Obsolete("Use scheduleUpdate() instead")]
        public void sheduleUpdate()
        {
            scheduleUpdateWithPriority(0);
        }

        /// <summary>
        /// schedules the "update" selector with a custom priority. This selector will be called every frame.
        /// Scheduled selectors with a lower priority will be called before the ones that have a higher value.
        /// Only one "update" selector could be scheduled per node (You can't have 2 'update' selectors).
        /// @since v0.99.3
        /// </summary>
        /// <param name="priority"></param>
        public void scheduleUpdateWithPriority(int priority)
        {
            CCScheduler.sharedScheduler().scheduleUpdateForTarget(this, priority, !IsRunning);
        }

        /// <summary>
        ///  unschedules the "update" method.
        /// @since v0.99.3
        /// </summary>
        public void unscheduleUpdate()
        {
            CCScheduler.sharedScheduler().unscheduleUpdateForTarget(this);
        }

        /// <summary>
        /// schedules a selector.
        /// The scheduled selector will be ticked every frame
        /// </summary>
        /// <param name="selector"></param>
        public void schedule(SEL_SCHEDULE selector)
        {
            this.schedule(selector, 0);
        }

        /// <summary>
        /// schedules a custom selector with an interval time in seconds.
        ///If time is 0 it will be ticked every frame.
        ///If time is 0, it is recommended to use 'scheduleUpdate' instead.
        ///If the selector is already scheduled, then the interval parameter 
        ///will be updated without scheduling it again.
        /// </summary>
        public void schedule(SEL_SCHEDULE selector, float interval)
        {
            CCScheduler.sharedScheduler().scheduleSelector(selector, this, interval, !IsRunning);
        }

        /// <summary>
        /// unschedules a custom selector.
        /// </summary>
        public void unschedule(SEL_SCHEDULE selector)
        {
            // explicit nil handling
            if (selector != null)
            {
                CCScheduler.sharedScheduler().unscheduleSelector(selector, this);
            }
        }

        /// <summary>
        /// unschedule all scheduled selectors: custom selectors, and the 'update' selector.
        /// Actions are not affected by this method.
        /// @since v0.99.3
        /// </summary>
        public void unscheduleAllSelectors()
        {
            CCScheduler.sharedScheduler().unscheduleAllSelectorsForTarget(this);
        }

        [Obsolete("use unscheduleAllSelectors()")]
        public void unsheduleAllSelectors()
        {
            unscheduleAllSelectors();
        }

        /// <summary>
        /// resumes all scheduled selectors and actions.
        /// Called internally by onEnter
        /// </summary>
        public void resumeSchedulerAndActions()
        {
            CCScheduler.sharedScheduler().resumeTarget(this);
            CCActionManager.sharedManager().resumeTarget(this);
        }

        /// <summary>
        /// pauses all scheduled selectors and actions.
        /// Called internally by onExit
        /// </summary>
        public void pauseSchedulerAndActions()
        {
            CCScheduler.sharedScheduler().pauseTarget(this);
            CCActionManager.sharedManager().pauseTarget(this);
        }

        #endregion

        #region transformation methods

        /// <summary>
        /// Returns the matrix that transform the node's (local) space coordinates into the parent's space coordinates.
        /// The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform nodeToParentTransform()
        {
            if (_isTransformDirty)
            {
                _transform = CCAffineTransform.CCAffineTransformMakeIdentity();

                if (!IsRelativeAnchorPoint && !AnchorPointInPixels.IsZero)
                {
                    _transform = CCAffineTransform.CCAffineTransformTranslate(_transform, AnchorPointInPixels.X, AnchorPointInPixels.Y);
                }

                if (!_positionInPixels.IsZero)
                {
                    _transform = CCAffineTransform.CCAffineTransformTranslate(_transform, _positionInPixels.X, _positionInPixels.Y);
                }

                if (_rotation != 0f)
                {
                    _transform = CCAffineTransform.CCAffineTransformRotate(_transform, -CCUtils.CC_DEGREES_TO_RADIANS(_rotation));
                }

                if (_skewX != 0f || _skewY != 0f)
                {
                    // create a skewed coordinate system
                    CCAffineTransform skew = CCAffineTransform.CCAffineTransformMake(1.0f,
                        (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(_skewY)),
                          (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(_skewX)), 1.0f, 0.0f, 0.0f);
                    // apply the skew to the transform
                    _transform = CCAffineTransform.CCAffineTransformConcat(skew, _transform);
                }

                if (!(_scaleX == 1f && _scaleY == 1f))
                {
                    _transform = CCAffineTransform.CCAffineTransformScale(_transform, _scaleX, _scaleY);
                }

                if (!AnchorPointInPixels.IsZero)
                {
                    _transform = CCAffineTransform.CCAffineTransformTranslate(_transform, -AnchorPointInPixels.X, -AnchorPointInPixels.Y);
                }

                _isTransformDirty = false;
            }

            return _transform;
        }

        /// <summary>
        /// Returns the matrix that transform parent's space coordinates to the node's (local) space coordinates.
        /// The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform parentToNodeTransform()
        {
            if (_isInverseDirty)
            {
                _inverse = CCAffineTransform.CCAffineTransformInvert(this.nodeToParentTransform());
                _isInverseDirty = false;
            }

            return _inverse;
        }

        /// <summary>
        /// Retrusn the world affine transform matrix. The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform nodeToWorldTransform()
        {
            CCAffineTransform t = this.nodeToParentTransform();

            Node p = Parent;
            while (p != null)
            {
                var temp = p.nodeToParentTransform();
                t = CCAffineTransform.CCAffineTransformConcat(t, temp);
                p = p.Parent;
            }

            return t;
        }

        /// <summary>
        /// Returns the inverse world affine transform matrix. The matrix is in Pixels.
        ///@since v0.7.1
        /// </summary>
        public CCAffineTransform worldToNodeTransform()
        {
            return CCAffineTransform.CCAffineTransformInvert(this.nodeToWorldTransform());
        }

        #endregion

        #region convertToSpace

        /// <summary>Converts a Point to node (local) space coordinates. The result is in Points.</summary>
        public CCPoint ConvertToNodeSpace(CCPoint worldPoint)
        {
            CCPoint ret;
            if (Director.SharedDirector.ContentScaleFactor == 1)
            {
                ret = CCAffineTransform.CCPointApplyAffineTransform(worldPoint, worldToNodeTransform());
            }
            else
            {
                ret = CCPointExtension.ccpMult(worldPoint, Director.SharedDirector.ContentScaleFactor);
                ret = CCAffineTransform.CCPointApplyAffineTransform(ret, worldToNodeTransform());
                ret = CCPointExtension.ccpMult(ret, 1 / Director.SharedDirector.ContentScaleFactor);
            }

            return ret;
        }

        /// <summary>Converts a Point to world space coordinates. The result is in Points.</summary>
        public CCPoint ConvertToWorldSpace(CCPoint nodePoint)
        {
            CCPoint ret;
            if (Director.SharedDirector.ContentScaleFactor == 1)
            {
                ret = CCAffineTransform.CCPointApplyAffineTransform(nodePoint, nodeToWorldTransform());
            }
            else
            {
                ret = CCPointExtension.ccpMult(nodePoint, Director.SharedDirector.ContentScaleFactor);
                ret = CCAffineTransform.CCPointApplyAffineTransform(ret, nodeToWorldTransform());
                ret = CCPointExtension.ccpMult(ret, 1 / Director.SharedDirector.ContentScaleFactor);
            }

            return ret;
        }

        /// <summary>
        /// Converts a Point to node (local) space coordinates. The result is in Points.
        /// treating the returned/received node point as anchor relative.
        /// </summary>
        public CCPoint ConvertToNodeSpaceAR(CCPoint worldPoint)
        {
            CCPoint nodePoint = ConvertToNodeSpace(worldPoint);
            CCPoint anchorInPoints;
            if (Director.SharedDirector.ContentScaleFactor == 1)
            {
                anchorInPoints = AnchorPointInPixels;
            }
            else
            {
                anchorInPoints = CCPointExtension.ccpMult(AnchorPointInPixels, 1 / Director.SharedDirector.ContentScaleFactor);
            }

            return CCPointExtension.ccpSub(nodePoint, anchorInPoints);
        }

        /// <summary>
        /// Converts a local Point to world space coordinates.The result is in Points.
        /// treating the returned/received node point as anchor relative.
        /// </summary>
        public CCPoint ConvertToWorldSpaceAR(CCPoint nodePoint)
        {
            CCPoint anchorInPoints;
            if (Director.SharedDirector.ContentScaleFactor == 1)
            {
                anchorInPoints = AnchorPointInPixels;
            }
            else
            {
                anchorInPoints = CCPointExtension.ccpMult(AnchorPointInPixels, 1 / Director.SharedDirector.ContentScaleFactor);
            }

            CCPoint pt = CCPointExtension.ccpAdd(nodePoint, anchorInPoints);
            return ConvertToWorldSpace(pt);
        }

        /// <summary>convenience methods which take a CCTouch instead of CCPoint </summary>
        public CCPoint ConvertTouchToNodeSpace(CCTouch touch)
        {
            CCPoint point = touch.locationInView(touch.view());
            point = Director.SharedDirector.ConvertToGL(point);
            return this.ConvertToNodeSpace(point);
        }

        /// <summary>converts a CCTouch (world coordinates) into a local coordiante. This method is AR (Anchor Relative).</summary>
        public CCPoint ConvertTouchToNodeSpaceAR(CCTouch touch)
        {
            CCPoint point = touch.locationInView(touch.view());
            point = Director.SharedDirector.ConvertToGL(point);
            return this.ConvertToNodeSpaceAR(point);
        }

        private CCPoint ConvertToWindowSpace(CCPoint nodePoint)
        {
            CCPoint worldPoint = this.ConvertToWorldSpace(nodePoint);
            return Director.SharedDirector.ConvertToUI(worldPoint);
        }

        #endregion

        #region Properties: The main features of a CCNode

        protected void _makeTransformDirty()
        {
            this._isTransformDirty = true;
            this._isInverseDirty = true;
            this._isTransformGLDirty = true;
        }

        protected CCPoint _position;
        /// <summary>Position (x,y) of the node in points. (0,0) is the left-bottom corner.</summary>
        public virtual CCPoint Position
        {
            get { return this._position; }
            set
            {
                this._position = value;
                this._positionInPixels = this._position;
                this._makeTransformDirty();
            }
        }

        protected CCPoint _positionInPixels;
        /// <summary>Position (x,y) of the node in pixels. (0,0) is the left-bottom corner.</summary>
        public virtual CCPoint PositionInPixels
        {
            get { return this._positionInPixels; }
            set
            {
                this._positionInPixels = value;
                this._position = this._positionInPixels;
                this._makeTransformDirty();
            }
        }

        /// <summary>The scale factor of the node. 1.0 is the default scale factor. It modifies the X and Y scale at the same time.</summary>
        public virtual float Scale
        {
            get { return this._scaleX; }
            set
            {
                this._scaleX = value;
                this._scaleY = value;
                this._makeTransformDirty();
            }
        }

        protected float _scaleX;
        /// <summary>The scale factor of the node. 1.0 is the default scale factor. It only modifies the X scale factor.</summary>
        public virtual float ScaleX
        {
            get { return this._scaleX; }
            set
            {
                this._scaleX = value;
                this._makeTransformDirty();
            }
        }

        protected float _scaleY;
        /// <summary>The scale factor of the node. 1.0 is the default scale factor. It only modifies the Y scale factor.</summary>
        public virtual float ScaleY
        {
            get { return this._scaleY; }
            set
            {
                this._scaleY = value;
                this._makeTransformDirty();
            }
        }

        protected float _rotation;
        /// <summary>
        /// The rotation (angle) of the node in degrees. 0 is the default rotation angle. Positive values rotate node CW
        /// </summary>
        public virtual float Rotation
        {
            get { return this._rotation; }
            set
            {
                this._rotation = value;
                this._makeTransformDirty();
            }
        }

        protected CCPoint _anchorPoint;
        /// <summary>
        /// anchorPoint is the point around which all transformations and positioning manipulations take place.
        /// It's like a pin in the node where it is "attached" to its parent.
        /// The anchorPoint is normalized, like a percentage. (0,0) means the bottom-left corner and (1,1) means the top-right corner.
        /// But you can use values higher than (1,1) and lower than (0,0) too.
        /// The default anchorPoint is (0,0). It starts in the bottom-left corner. CCSprite and other subclasses have a different default anchorPoint.
        /// </summary>
        public virtual CCPoint AnchorPoint
        {
            get
            {
                return this._anchorPoint;
            }
            set
            {
                if (value.Equals(this._anchorPoint) == false)
                {
                    this._anchorPoint = value;
                    this.AnchorPointInPixels = CCPointExtension.ccp(
                        this._contentSizeInPixels.Width * this._anchorPoint.X,
                        this._contentSizeInPixels.Height * this._anchorPoint.Y
                        );
                    this._makeTransformDirty();
                }
            }
        }

        /// <summary>
        /// The anchorPoint in absolute pixels.
        /// Since v0.8 you can only read it. If you wish to modify it, use anchorPoint instead
        /// </summary>
        public CCPoint AnchorPointInPixels { get; protected set; }

        protected CCSize _contentSize;
        /// <summary>
        /// The untransformed size of the node in Points
        /// The contentSize remains the same no matter the node is scaled or rotated.
        /// All nodes has a size. Layer and Scene has the same size of the screen.
        /// </summary>
        public virtual CCSize ContentSize
        {
            get
            {
                return this._contentSize;
            }
            set
            {
                if (value.Equals(this._contentSize) == false)
                {
                    this._contentSize = value;
                    this._contentSizeInPixels = this._contentSize;

                    this.AnchorPointInPixels = CCPointExtension.ccp(
                        this._contentSizeInPixels.Width * this._anchorPoint.X,
                        this._contentSizeInPixels.Height * this._anchorPoint.Y
                        );
                    this._makeTransformDirty();
                }
            }
        }

        protected CCSize _contentSizeInPixels;
        /// <summary>
        /// The untransformed size of the node in Pixels
        /// The contentSize remains the same no matter the node is scaled or rotated.
        /// All nodes has a size. Layer and Scene has the same size of the screen.
        /// </summary>
        public CCSize ContentSizeInPixels
        {
            get { return this._contentSizeInPixels; }
            set
            {
                if (value.Equals(_contentSizeInPixels) == false)
                {
                    this._contentSizeInPixels = value;
                    this._contentSize = value;
                    AnchorPointInPixels = CCPointExtension.ccp(
                        this._contentSizeInPixels.Width * this._anchorPoint.X,
                        this._contentSizeInPixels.Height * this._anchorPoint.Y
                        );
                    this._makeTransformDirty();
                }
            }
        }

        /// <summary>Whether of not the node is visible. Default is YES</summary>
        public virtual bool Visible { get; set; }

        /// <summary>The z order of the node relative to it's "brothers": children of the same parent</summary>
        public int ZOrder { get; set; }

        protected float _vertexZ;
        /// <summary>
        /// The real openGL Z vertex.
        /// Differences between openGL Z vertex and cocos2d Z order:
        /// OpenGL Z modifies the Z vertex, and not the Z order in the relation between parent-children
        /// OpenGL Z might require to set 2D projection
        /// cocos2d Z order works OK if all the nodes uses the same openGL Z vertex. eg: vertexZ = 0
        /// @warning: Use it at your own risk since it might break the cocos2d parent-children z order
        /// </summary>
        public virtual float VertexZ
        {
            get { return this._vertexZ / Director.SharedDirector.ContentScaleFactor; }
            set { this._vertexZ = value * Director.SharedDirector.ContentScaleFactor; }
        }

        protected float _skewX;
        /// <summary>
        /// The X skew angle of the node in degrees.
        /// This angle describes the shear distortion in the X direction.
        /// Thus, it is the angle between the Y axis and the left edge of the shape
        /// The default skewX angle is 0. Positive values distort the node in a CW direction.
        /// </summary>
        public virtual float SkewX
        {
            get { return this._skewX; }
            set
            {
                this._skewX = value;
                this._makeTransformDirty();
            }
        }

        protected float _skewY;
        /// <summary>
        /// The Y skew angle of the node in degrees.
        /// This angle describes the shear distortion in the Y direction.
        /// Thus, it is the angle between the X axis and the bottom edge of the shape
        /// The default skewY angle is 0. Positive values distort the node in a CCW direction.
        /// </summary>
        public virtual float SkewY
        {
            get { return this._skewY; }
            set
            {
                this._skewY = value;
                this._makeTransformDirty();
            }
        }

        /// <summary>whether or not the node is running </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>A weak reference to the parent</summary>
        public Node Parent { get; set; }

        protected bool _isRelativeAnchorPoint;
        /// <summary>
        /// If YES the transformtions will be relative to it's anchor point.
        /// Sprites, Labels and any other sizeble object use it have it enabled by default.
        /// Scenes, Layers and other "whole screen" object don't use it, have it disabled by default.
        /// </summary>
        public virtual bool IsRelativeAnchorPoint
        {
            get { return this._isRelativeAnchorPoint; }
            set
            {
                this._isRelativeAnchorPoint = value;
                this._makeTransformDirty();
            }
        }

        /// <summary>A tag used to identify the node easily</summary>
        public int Tag { get; set; }

        /// <summary>A custom user data pointer</summary>
        public object UserData { get; set; }

        #endregion


        private CCCamera _camera;
        /// <summary>A CCCamera object that lets you move the node using a gluLookAt</summary>
        public CCCamera Camera
        {
            get
            {
                if (this._camera == null) this._camera = new CCCamera();
                return this._camera;
            }
        }

        /// <summary>A CCGrid object that is used when applying effects</summary>
        public CCGridBase Grid { get; set; }

        // internal member variables

        // transform
        protected CCAffineTransform _transform;
        protected CCAffineTransform _inverse;
        protected Matrix _nodeTransform;
        protected Matrix _view;

        protected float[] _transformGL = new float[16];

        // To reduce memory, place bools that are not properties here:
        protected bool _isTransformDirty;
        protected bool _isInverseDirty;
        protected bool _isTransformGLDirty;
    }
}
