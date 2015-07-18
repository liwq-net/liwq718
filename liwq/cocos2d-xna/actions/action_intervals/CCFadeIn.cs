using System;
namespace cocos2d
{
    /** @brief Fades In an object that implements the CCRGBAProtocol protocol. It modifies the opacity from 0 to 255.
     The "reverse" of this action is FadeOut
     */
    public class CCFadeIn : CCActionInterval
    {
        public static CCFadeIn actionWithDuration(float d)
        {
            CCFadeIn pAction = new CCFadeIn();

            pAction.initWithDuration(d);

            return pAction;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFadeIn pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeIn)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            return pCopy;
        }

        public override void Update(float time)
        {
            ICCRGBAProtocol pRGBAProtocol = Target as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte)(255 * time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCFadeOut.actionWithDuration(Duration);
        }


    }
}