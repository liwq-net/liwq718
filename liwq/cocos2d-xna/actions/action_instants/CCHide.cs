using System;
namespace cocos2d
{
    /** 
    @brief Hide the node
    */
    public class CCHide : CCActionInstant
    {
        public CCHide()
        {

        }

        ~CCHide()
        {

        }

        public static new CCHide action()
        {
            CCHide pRet = new CCHide();

            return pRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            pTarget.Visible = false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCFiniteTimeAction)(CCShow.action());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCHide pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCHide)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCHide();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            return pRet;
        }

    }
}