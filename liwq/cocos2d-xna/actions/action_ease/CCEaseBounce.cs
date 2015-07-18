namespace cocos2d
{
    public class CCEaseBounce : CCActionEase
    {
        public float bounceTime(float time)
        {
            if (time < 1 / 2.75)
            {
                return 7.5625f * time * time;
            }
            else
                if (time < 2 / 2.75)
                {
                    time -= 1.5f / 2.75f;
                    return 7.5625f * time * time + 0.75f;
                }
                else
                    if (time < 2.5 / 2.75)
                    {
                        time -= 2.25f / 2.75f;
                        return 7.5625f * time * time + 0.9375f;
                    }

            time -= 2.625f / 2.75f;
            return 7.5625f * time * time + 0.984375f;
        }
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBounce pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounce;
            }
            else
            {
                pCopy = new CCEaseBounce();
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
        public new static CCEaseBounce actionWithAction(CCActionInterval pAction)
        {
            CCEaseBounce pRet = new CCEaseBounce();

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
