using liwq;
using System;

namespace cocos2d
{
    /// <summary>CCOrbitCamera action Orbits the camera around the center of the screen using spherical coordinates</summary>
    public class CCOrbitCamera : CCActionCamera
    {
        protected float _radius;
        protected float _deltaRadius;
        protected float _angleZ;
        protected float _deltaAngleZ;
        protected float _angleX;
        protected float _deltaAngleX;
        protected float _radZ;
        protected float _radDeltaZ;
        protected float _radX;
        protected float _radDeltaX;

        /// <summary>initializes a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX </summary>
        public CCOrbitCamera(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            if (initWithDuration(t))
            {
                this._radius = radius;
                this._deltaRadius = deltaRadius;
                this._angleZ = angleZ;
                this._deltaAngleZ = deltaAngleZ;
                this._angleX = angleX;
                this._deltaAngleX = deltaAngleX;
                this._radDeltaZ = CCUtils.CC_DEGREES_TO_RADIANS(deltaAngleZ);
                this._radDeltaX = CCUtils.CC_DEGREES_TO_RADIANS(deltaAngleX);
            }
        }

        /// <summary>
        /// positions the camera according to spherical coordinates 
        /// </summary>
        public void sphericalRadius(out float newRadius, out float zenith, out float azimuth)
        {
            float ex, ey, ez, cx, cy, cz, x, y, z;
            float r; // radius
            float s;

            CCCamera camera = Target.Camera;
            camera.getEyeXYZ(out ex, out  ey, out ez);
            camera.getCenterXYZ(out cx, out  cy, out  cz);

            x = ex - cx;
            y = ey - cy;
            z = ez - cz;

            r = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2) + (float)Math.Pow(z, 2));
            s = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2));
            if (s == 0.0f)
                s = ccMacros.FLT_EPSILON;
            if (r == 0.0f)
                r = ccMacros.FLT_EPSILON;

            zenith = (float)Math.Acos(z / r);
            if (x < 0)
                azimuth = (float)Math.PI - (float)Math.Sin(y / s);
            else
                azimuth = (float)Math.Sin(y / s);

            newRadius = r / CCCamera.getZEye();
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.startWithTargetUsedByCCOrbitCamera(pTarget);

            float r, zenith, azimuth;
            this.sphericalRadius(out r, out  zenith, out azimuth);

            if (float.IsNaN(_radius))
                this._radius = r;
            if (float.IsNaN(_angleZ))
                this._angleZ = CCUtils.CC_RADIANS_TO_DEGREES(zenith);
            if (float.IsNaN(_angleX))
                this._angleX = CCUtils.CC_RADIANS_TO_DEGREES(azimuth);

            this._radZ = CCUtils.CC_DEGREES_TO_RADIANS(_angleZ);
            this._radX = CCUtils.CC_DEGREES_TO_RADIANS(_angleX);
        }

        public override void Update(float dt)
        {
            float r = (this._radius + this._deltaRadius * dt) * CCCamera.getZEye();
            float za = this._radZ + this._radDeltaZ * dt;
            float xa = this._radX + this._radDeltaX * dt;
            float i = (float)Math.Sin(za) * (float)Math.Cos(xa) * r + this._centerXOrig;
            float j = (float)Math.Sin(za) * (float)Math.Sin(xa) * r + this._centerYOrig;
            float k = (float)Math.Cos(za) * r + this._centerZOrig;
            Target.Camera.setEyeXYZ(i, j, k);
        }

    }
}