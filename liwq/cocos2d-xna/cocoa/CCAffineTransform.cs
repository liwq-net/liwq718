using System;
using Microsoft.Xna.Framework;
using liwq;
namespace cocos2d
{
    public class CCAffineTransform
    {
        public float a, b, c, d;
        public float tx, ty;

        private CCAffineTransform() { }

        public static CCAffineTransform CCAffineTransformMakeIdentity()
        {
            return CCAffineTransformMake(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);
        }

        public static CCAffineTransform CCAffineTransformMake(float a, float b, float c, float d, float tx, float ty)
        {
            return new CCAffineTransform() { a = a, b = b, c = c, d = d, tx = tx, ty = ty };
        }

        public static CCPoint CCPointApplyAffineTransform(CCPoint point, CCAffineTransform t)
        {
            CCPoint p = new CCPoint();
            p.X = (float)((double)t.a * point.X + (double)t.c * point.Y + t.tx);
            p.Y = (float)((double)t.b * point.X + (double)t.d * point.Y + t.ty);
            return p;
        }

        public static CCSize CCSizeApplyAffineTransform(CCSize size, CCAffineTransform t)
        {
            CCSize s = new CCSize();
            s.Width = (float)((double)t.a * size.Width + (double)t.c * size.Height);
            s.Height = (float)((double)t.b * size.Width + (double)t.d * size.Height);
            return s;
        }

        public static CCRect CCRectApplyAffineTransform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            float top = rect.MinY;
            float left = rect.MinX;
            float right = rect.MaxX;
            float bottom = rect.MaxY;

            CCPoint topLeft = CCPointApplyAffineTransform(new CCPoint(left, top), anAffineTransform);
            CCPoint topRight = CCPointApplyAffineTransform(new CCPoint(right, top), anAffineTransform);
            CCPoint bottomLeft = CCPointApplyAffineTransform(new CCPoint(left, bottom), anAffineTransform);
            CCPoint bottomRight = CCPointApplyAffineTransform(new CCPoint(right, bottom), anAffineTransform);

            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new CCRect(minX, minY, (maxX - minX), (maxY - minY));
        }

        public static CCAffineTransform CCAffineTransformTranslate(CCAffineTransform t, float tx, float ty)
        {
            return CCAffineTransformMake(t.a, t.b, t.c, t.d, t.tx + t.a * tx + t.c * ty, t.ty + t.b * tx + t.d * ty);
        }

        public static CCAffineTransform CCAffineTransformRotate(CCAffineTransform t, float anAngle)
        {
            float fSin = (float)Math.Sin(anAngle);
            float fCos = (float)Math.Cos(anAngle);

            return CCAffineTransformMake(
                t.a * fCos + 
                t.c * fSin, 
                t.b * fCos +
                t.d * fSin, 
                t.c * fCos - t.a * fSin,
                t.d * fCos - t.b * fSin,
                t.tx, t.ty
                );
        }

        public static CCAffineTransform CCAffineTransformScale(CCAffineTransform t, float sx, float sy)
        {
            return CCAffineTransformMake(t.a * sx, t.b * sx, t.c * sy, t.d * sy, t.tx, t.ty);
        }

        /// <summary>
        /// Concatenate `t2' to `t1' and return the result:
        /// t' = t1 * t2 */s
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static CCAffineTransform CCAffineTransformConcat(CCAffineTransform t1, CCAffineTransform t2)
        {
            return CCAffineTransformMake(t1.a * t2.a + t1.b * t2.c, t1.a * t2.b + t1.b * t2.d, //a,b
                                    t1.c * t2.a + t1.d * t2.c, t1.c * t2.b + t1.d * t2.d, //c,d
                                    t1.tx * t2.a + t1.ty * t2.c + t2.tx,				  //tx
                                    t1.tx * t2.b + t1.ty * t2.d + t2.ty);				  //ty
        }

        /// <summary>
        ///  Return true if `t1' and `t2' are equal, false otherwise. 
        /// </summary>
        public static bool CCAffineTransformEqualToTransform(CCAffineTransform t1, CCAffineTransform t2)
        {
            return (t1.a == t2.a && t1.b == t2.b && t1.c == t2.c && t1.d == t2.d && t1.tx == t2.tx && t1.ty == t2.ty);
        }

        public static CCAffineTransform CCAffineTransformInvert(CCAffineTransform t)
        {
            float determinant = 1 / (t.a * t.d - t.b * t.c);

            return CCAffineTransformMake(determinant * t.d, -determinant * t.b, -determinant * t.c, determinant * t.a,
                                    determinant * (t.c * t.ty - t.d * t.tx), determinant * (t.b * t.tx - t.a * t.ty));
        }
    }
}
