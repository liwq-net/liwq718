using System;

namespace cocos2d
{
    public class CCEaseIn : CCEaseRateAction
    {

        public override void Update(float time)
        {
            m_pOther.Update((float)Math.Pow(time, m_fRate));
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseIn;
            }
            else
            {
                pCopy = new CCEaseIn();
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
        public static new CCEaseIn actionWithAction(CCActionInterval pAction, float fRate)
        {
            CCEaseIn pRet = new CCEaseIn();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fRate))
                {
                    //pRet.autorelease();
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
