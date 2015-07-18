namespace cocos2d
{
    public class CCEaseBounceInOut : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT = 0;

            if (time < 0.5f)
            {
                time = time * 2;
                newT = (1 - bounceTime(1 - time)) * 0.5f;
            }
            else
            {
                newT = bounceTime(time * 2 - 1) * 0.5f + 0.5f;
            }

            m_pOther.Update(newT);
        }
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBounceInOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounceInOut;
            }
            else
            {
                pCopy = new CCEaseBounceInOut();
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
        public new static CCEaseBounceInOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseBounceInOut pRet = new CCEaseBounceInOut();

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
