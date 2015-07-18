namespace cocos2d
{
    public class CCTintBy : CCActionInterval
    {
        public bool initWithDuration(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            if (base.initWithDuration(duration))
            {
                m_deltaR = deltaRed;
                m_deltaG = deltaGreen;
                m_deltaB = deltaBlue;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCTintBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCTintBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCTintBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_deltaR, m_deltaG, m_deltaB);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);

            ICCRGBAProtocol protocol = target as ICCRGBAProtocol;
            if (protocol != null)
            {
                ccColor3B color = protocol.Color;
                m_fromR = color.r;
                m_fromG = color.g;
                m_fromB = color.b;
            }
        }

        public override void Update(float dt)
        {
            ICCRGBAProtocol protocol = Target as ICCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color = new ccColor3B((byte)(m_fromR + m_deltaR * dt),
                                               (byte)(m_fromG + m_deltaG * dt),
                                               (byte)(m_fromB + m_deltaB * dt));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCTintBy.actionWithDuration(Duration, (short)-m_deltaR, (short)-m_deltaG, (short)-m_deltaB) as CCFiniteTimeAction;
        }

        public static CCTintBy actionWithDuration(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            CCTintBy ret = new CCTintBy();
            ret.initWithDuration(duration, deltaRed, deltaGreen, deltaBlue);

            return ret;
        }

        protected short m_deltaR;
        protected short m_deltaG;
        protected short m_deltaB;

        protected short m_fromR;
        protected short m_fromG;
        protected short m_fromB;
    }
}
