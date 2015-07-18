using System;

namespace cocos2d
{
    public class CCEaseSineInOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(-0.5f * ((float)Math.Cos((float)Math.PI * time) - 1));
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseSineInOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseSineInOut)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseSineInOut();
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
        public new static CCEaseSineInOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseSineInOut pRet = new CCEaseSineInOut();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction))
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
