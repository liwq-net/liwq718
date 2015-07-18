using liwq;
using System;
namespace cocos2d
{
    /** @brief An action that moves the target with a cubic Bezier curve by a certain distance. */
    public class CCBezierBy : CCActionInterval
    {
        public bool initWithDuration(float t, ccBezierConfig c)
        {
            if (base.initWithDuration(t))
            {
                m_sConfig = c;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCBezierBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCBezierBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_sConfig);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
        }

        public override void Update(float dt)
        {
            if (Target != null)
            {
                float xa = 0;
                float xb = m_sConfig.controlPoint_1.X;
                float xc = m_sConfig.controlPoint_2.X;
                float xd = m_sConfig.endPosition.X;

                float ya = 0;
                float yb = m_sConfig.controlPoint_1.Y;
                float yc = m_sConfig.controlPoint_2.Y;
                float yd = m_sConfig.endPosition.Y;

                float x = bezierat(xa, xb, xc, xd, dt);
                float y = bezierat(ya, yb, yc, yd, dt);
                Target.Position = CCPointExtension.ccpAdd(m_startPosition, CCPointExtension.ccp(x, y));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            ccBezierConfig r;

            r.endPosition = CCPointExtension.ccpNeg(m_sConfig.endPosition);
            r.controlPoint_1 = CCPointExtension.ccpAdd(m_sConfig.controlPoint_2, CCPointExtension.ccpNeg(m_sConfig.endPosition));
            r.controlPoint_2 = CCPointExtension.ccpAdd(m_sConfig.controlPoint_1, CCPointExtension.ccpNeg(m_sConfig.endPosition));

            CCBezierBy action = CCBezierBy.actionWithDuration(Duration, r);
            return action;
        }

        public static CCBezierBy actionWithDuration(float t, ccBezierConfig c)
        {
            CCBezierBy ret = new CCBezierBy();
            ret.initWithDuration(t, c);

            return ret;
        }

        // Bezier cubic formula:
        //	((1 - t) + t)3 = 1 
        // Expands to¡­ 
        //   (1 - t)3 + 3t(1-t)2 + 3t2(1 - t) + t3 = 1 
        protected float bezierat(float a, float b, float c, float d, float t)
        {

            return (float)((Math.Pow(1 - t, 3) * a +
                            3 * t * (Math.Pow(1 - t, 2)) * b +
                            3 * Math.Pow(t, 2) * (1 - t) * c +
                            Math.Pow(t, 3) * d));
        }

        protected ccBezierConfig m_sConfig;
        protected CCPoint m_startPosition;
    }

    public struct ccBezierConfig
    {
        //! end position of the bezier
        public CCPoint endPosition;
        //! Bezier control point 1
        public CCPoint controlPoint_1;
        //! Bezier control point 2
        public CCPoint controlPoint_2;
    }
}
