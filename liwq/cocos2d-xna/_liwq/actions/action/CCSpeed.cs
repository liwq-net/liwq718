using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// Changes the speed of an action, making it take longer (speed大于1) or less (speed小于1) time.
    /// Useful to simulate 'slow motion' or 'fast forward' effect.
    /// warning This action can't be Sequenceable because it is not an CCIntervalAction
    /// </summary>
    public class CCSpeed : CCActionInterval //todo check 继承 CCActionInterval，而不是CCAction
    {
        public float Speed { get; set; }
        protected CCActionInterval _innerAction;

        public CCSpeed(CCActionInterval action, float speed)
        {
            Debug.Assert(action != null);
            this._innerAction = action;
            this.Speed = speed;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            this._innerAction.StartWithTarget(target);
        }

        public override void Stop()
        {
            this._innerAction.Stop();
            base.Stop();
        }

        public override void Step(float dt)
        {
            this._innerAction.Step(dt * Speed);
        }

        public override bool IsDone()
        {
            return this._innerAction.IsDone();
        }

        public virtual CCActionInterval reverse()
        {
            return (CCActionInterval)new CCSpeed((CCActionInterval)this._innerAction.Reverse(), Speed);
        }

    }
}
