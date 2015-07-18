namespace cocos2d
{
    /** @brief Rotates a CCNode object clockwise a number of degrees by modifying it's rotation attribute. */
    public class CCRotateBy : CCActionInterval
    {
        public bool initWithDuration(float duration, float fDeltaAngle)
        {
            if (base.initWithDuration(duration))
            {
                m_fAngle = fDeltaAngle;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRotateBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRotateBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRotateBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_fAngle);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_fStartAngle = target.Rotation;
        }

        public override void Update(float dt)
        {
            // XXX: shall I add % 360
            if (Target != null)
            {
                Target.Rotation = m_fStartAngle + m_fAngle * dt;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCRotateBy.actionWithDuration(Duration, -m_fAngle);
        }

        public static CCRotateBy actionWithDuration(float duration, float fDeltaAngle)
        {
            CCRotateBy ret = new CCRotateBy();
            ret.initWithDuration(duration, fDeltaAngle);

            return ret;
        }

        protected float m_fAngle;
        protected float m_fStartAngle;
    }
}
