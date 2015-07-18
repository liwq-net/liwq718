namespace cocos2d
{
    public class CCRotateTo : CCActionInterval
    {
        public bool initWithDuration(float duration, float fDeltaAngle)
        {
            if (base.initWithDuration(duration))
            {
                m_fDstAngle = fDeltaAngle;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRotateTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRotateTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRotateTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_fDstAngle);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);

            m_fStartAngle = target.Rotation;

            if (m_fStartAngle > 0)
            {
                m_fStartAngle = m_fStartAngle % 350.0f;
            }
            else
            {
                m_fStartAngle = m_fStartAngle % -360.0f;
            }

            m_fDiffAngle = m_fDstAngle - m_fStartAngle;
            if (m_fDiffAngle > 180)
            {
                m_fDiffAngle -= 360;
            }

            if (m_fDiffAngle < -180)
            {
                m_fDiffAngle += 360;
            }
        }

        public override void Update(float dt)
        {
            if (Target != null)
            {
                Target.Rotation = m_fStartAngle + m_fDiffAngle * dt;
            }
        }

        public static CCRotateTo actionWithDuration(float duration, float fDeltaAngle)
        {
            CCRotateTo ret = new CCRotateTo();
            ret.initWithDuration(duration, fDeltaAngle);

            return ret;
        }

        protected float m_fDstAngle;
        protected float m_fStartAngle;
        protected float m_fDiffAngle;
    }
}
