using liwq;
using System;
namespace cocos2d
{
    /** @brief Places the node in a certain position
    */
    public class CCPlace : CCActionInstant
    {
        public CCPlace()
        {
            m_tPosition = new CCPoint();
        }

        ~CCPlace()
        {

        }

        public static CCPlace actionWithPosition(CCPoint pos)
        {
            CCPlace pRet = new CCPlace();

            if (pRet != null && pRet.initWithPosition(pos))
            {
                return pRet;
            }

            return null;
        }

        public bool initWithPosition(CCPoint pos)
        {
            m_tPosition = pos;
            return true;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCPlace pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCPlace)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCPlace();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            pRet.initWithPosition(m_tPosition);
            return pRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            Target.Position = m_tPosition;
        }

        private CCPoint m_tPosition;
    }
}