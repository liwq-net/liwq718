using System;

namespace cocos2d
{
    public class CCEaseInOut : CCEaseRateAction
    {
        public override void Update(float time)
        {
            int sign = 1;
            int r = (int)m_fRate;

            if (r % 2 == 0)
            {
                sign = -1;
            }

            time *= 2;

            if (time < 1)
            {
                m_pOther.Update(0.5f * (float)Math.Pow(time, m_fRate));
            }
            else
            {
                m_pOther.Update(sign * 0.5f * ((float)Math.Pow(time - 2, m_fRate) + sign * 2));
            }
        }
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseInOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseInOut;
            }
            else
            {
                pCopy = new CCEaseInOut();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fRate);

            return pCopy;
        }
        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseInOut.actionWithAction((CCActionInterval)m_pOther.Reverse(), m_fRate);
        }

        /// <summary>
        /// Creates the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public new static CCEaseInOut actionWithAction(CCActionInterval pAction, float fRate)
        {
            CCEaseInOut pRet = new CCEaseInOut();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fRate))
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
