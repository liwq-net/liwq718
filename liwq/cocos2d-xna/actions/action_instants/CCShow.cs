using System;

namespace cocos2d
{
    public class CCShow : CCActionInstant
    {
        public static new CCShow action()
        {
            CCShow pRet = new CCShow();
            return pRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            pTarget.Visible = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCFiniteTimeAction)(CCHide.action());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShow pRet = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCShow)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCShow();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            return pRet;
        }
    }
}