using System;
using System.Diagnostics;

namespace cocos2d
{
    public class CCAction : CCObject
    {
        public const int INVALID_ACTION_TAGB = -1;

        /// <summary>
        /// The action tag. An identifier of the action
        /// </summary>
        public int Tag { get; set; }

        public CCAction() { this.Tag = INVALID_ACTION_TAGB; }

        /// <summary>
        /// The "target".
        /// The target will be set with the 'startWithTarget' method.
        /// When the 'stop' method is called, target will be set to nil.
        /// The target is 'assigned', it is not 'retained'.
        /// </summary>
        public Node OriginalTarget { get; set; }

        /// <summary>
        /// Set the original target, since target can be nil.
        /// Is the target that were used to run the action. 
        /// Unless you are doing something complex, like CCActionManager, you should NOT call this method.
        /// The target is 'assigned', it is not 'retained'.
        /// </summary>
        public Node Target { get; set; }

        /// <summary>
        /// called before the action start. 
        /// It will also set the target.
        /// </summary>
        public virtual void StartWithTarget(Node target)
        {
            this.OriginalTarget = target;
            this.Target = target;
        }

        /// <summary>
        /// called after the action has finished. It will set the 'target' to nil.
        /// IMPORTANT: You should never call "[action stop]" manually. Instead, use: "target->stopAction(action);"
        /// </summary>
        public virtual void Stop()
        {
            this.Target = null;
        }

        /// <summary>
        /// called every frame with it's delta time.
        /// DON'T override unless you know what you are doing.
        /// </summary>
        public virtual void Step(float dt)
        {
            Debug.WriteLine("[Action step]. override me");
        }

        /// <summary>
        /// called once per frame. time a value between 0 and 1
        /// For example: 
        /// - 0 means that the action just started
        /// - 0.5 means that the action is in the middle
        /// - 1 means that the action is over
        /// </summary>
        public virtual void Update(float dt)
        {
            Debug.WriteLine("[Action update]. override me");
        }

        /// <summary>return true if the action has finished</summary>
        public virtual bool IsDone() { return true; }

    }
}
