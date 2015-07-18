using System;

namespace cocos2d
{
    public class CCEaseOut : CCEaseRateAction
    {
        public override void Update(float time)
        {
            m_pOther.Update((float)(Math.Pow(time, 1 / m_fRate)));
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseOut)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseOut();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fRate);

            return pCopy;
        }

        /// <summary>
        /// Creates the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public new static CCEaseOut actionWithAction(CCActionInterval pAction, float fRate)
        {
            CCEaseOut pRet = new CCEaseOut();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fRate))
                {

                }
                else
                {

                }
            }

            return pRet;
        }
    }
}
