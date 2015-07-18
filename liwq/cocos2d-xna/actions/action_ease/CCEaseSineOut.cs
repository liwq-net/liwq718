using Microsoft.Xna.Framework;
using System;

namespace cocos2d
{
    public class CCEaseSineOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update((float)Math.Sin(time * (float)MathHelper.TwoPi));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseSineIn.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseSineOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseSineOut;
            }
            else
            {
                pCopy = new CCEaseSineOut();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()));

            return pCopy;
        }

        /// <summary>
        /// creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new static CCEaseSineOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseSineOut pRet = new CCEaseSineOut();

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
