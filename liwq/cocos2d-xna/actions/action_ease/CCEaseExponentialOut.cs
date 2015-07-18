using System;

namespace cocos2d
{
    public class CCEaseExponentialOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(time == 1 ? 1 : (-(float)Math.Pow(2, -10 * time / 1) + 1));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseExponentialIn.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseExponentialOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseExponentialOut;
            }
            else
            {
                pCopy = new CCEaseExponentialOut();
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
        public new static CCEaseExponentialOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseExponentialOut pRet = new CCEaseExponentialOut();

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
