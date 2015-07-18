using Microsoft.Xna.Framework;
using System;

namespace cocos2d
{
    public class CCEaseSineIn : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(-1 * (float)Math.Cos(time * (float)MathHelper.TwoPi) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseSineOut.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseSineIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseSineIn)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseSineIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()));

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        /// <summary>
        /// creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new static CCEaseSineIn actionWithAction(CCActionInterval pAction)
        {
            CCEaseSineIn pRet = new CCEaseSineIn();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction))
                {
                    //pRet->autorelease();
                }
                else
                {
                    //CC_SAFE_RELEASE_NULL(pRet);
                }
            }

            return pRet;
        }
    }
}
