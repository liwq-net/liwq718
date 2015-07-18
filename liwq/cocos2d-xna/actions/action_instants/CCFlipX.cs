using System;
namespace cocos2d
{
    /** 
    @brief Flips the sprite horizontally
    @since v0.99.0
    */
    public class CCFlipX : CCActionInstant
    {
        public CCFlipX()
        {
            m_bFlipX = false;
        }

        ~CCFlipX()
        {

        }

        public static CCFlipX actionWithFlipX(bool x)
        {
            CCFlipX pRet = new CCFlipX();

            if (pRet != null && pRet.initWithFlipX(x))
            {
                return pRet;
            }

            return null;
        }

        public bool initWithFlipX(bool x)
        {
            m_bFlipX = x;
            return true;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            ((CCSprite)(pTarget)).IsFlipX = m_bFlipX;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return actionWithFlipX(!m_bFlipX);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFlipX pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCFlipX)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCFlipX();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            pRet.initWithFlipX(m_bFlipX);
            return pRet;
        }

        private bool m_bFlipX;
    }
}