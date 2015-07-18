namespace cocos2d
{
    /** 
    @brief Flips the sprite vertically
    @since v0.99.0
    */
    public class CCFlipY : CCActionInstant
    {
        public CCFlipY()
        {
            m_bFlipY = false;
        }

        ~CCFlipY()
        {

        }

        public static CCFlipY actionWithFlipY(bool y)
        {
            CCFlipY pRet = new CCFlipY();

            if (pRet != null && pRet.initWithFlipY(y))
            {
                return pRet;
            }

            return null;
        }

        public bool initWithFlipY(bool y)
        {
            m_bFlipY = y;
            return true;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            ((CCSprite)(pTarget)).IsFlipY = m_bFlipY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return actionWithFlipY(!m_bFlipY);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFlipY pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCFlipY)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCFlipY();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            pRet.initWithFlipY(m_bFlipY);
            return pRet;
        }

        private bool m_bFlipY;
    }
}