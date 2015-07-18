using System;

namespace cocos2d
{
    public class CCEaseExponentialInOut : CCActionEase
    {
        public override void Update(float time)
        {
            time /= 0.5f;
            if (time < 1)
            {
                time = 0.5f * (float)Math.Pow(2, 10 * (time - 1));
            }
            else
            {
                time = 0.5f * (-(float)Math.Pow(2, 10 * (time - 1)) + 2);
            }

            m_pOther.Update(time);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseExponentialInOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseExponentialInOut;
            }
            else
            {
                pCopy = new CCEaseExponentialInOut();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()));

            return pCopy;
        }
        /** creates the action */
        public new static CCEaseExponentialInOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseExponentialInOut pRet = new CCEaseExponentialInOut();

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
