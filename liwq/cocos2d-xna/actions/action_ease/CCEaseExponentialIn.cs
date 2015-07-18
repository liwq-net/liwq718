using System;

namespace cocos2d
{
    public class CCEaseExponentialIn : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(time == 0 ? 0 : (float)Math.Pow(2, 10 * (time / 1 - 1)) - 1 * 0.001f);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseExponentialOut.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseExponentialIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseExponentialIn;
            }
            else
            {
                pCopy = new CCEaseExponentialIn();
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
        public new static CCEaseExponentialIn actionWithAction(CCActionInterval pAction)
        {
            CCEaseExponentialIn pRet = new CCEaseExponentialIn();

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
