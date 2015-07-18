using System;
namespace cocos2d
{
    /** @brief Fades Out an object that implements the CCRGBAProtocol protocol. It modifies the opacity from 255 to 0.
     The "reverse" of this action is FadeIn
    */
    public class CCFadeOut : CCActionInterval
    {
        public static CCFadeOut actionWithDuration(float d)
        {
            CCFadeOut pAction = new CCFadeOut();

            pAction.initWithDuration(d);

            return pAction;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFadeOut pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeOut)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeOut();
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
                pRGBAProtocol.Opacity = (byte)(255 * (1 - time));
            }
            /*m_pTarget->setOpacity(GLubyte(255 * (1 - time)));*/
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCFadeIn.actionWithDuration(Duration);
        }


    }
}