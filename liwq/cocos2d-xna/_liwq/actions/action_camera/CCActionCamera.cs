namespace cocos2d
{
    /// <summary>Base class for CCCamera actions</summary>
    public class CCActionCamera : CCActionInterval
    {
        protected float _centerXOrig;
        protected float _centerYOrig;
        protected float _centerZOrig;

        protected float _eyeXOrig;
        protected float _eyeYOrig;
        protected float _eyeZOrig;

        protected float _upXOrig;
        protected float _upYOrig;
        protected float _upZOrig;

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            CCCamera camera = target.Camera;
            camera.getCenterXYZ(out this._centerXOrig, out this._centerYOrig, out this._centerZOrig);
            camera.getEyeXYZ(out this._eyeXOrig, out this._eyeYOrig, out this._eyeZOrig);
            camera.getUpXYZ(out this._upXOrig, out this._upYOrig, out this._upZOrig);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }
    }
}