namespace cocos2d
{
    public class CCEaseBounceIn : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT = 1 - bounceTime(1 - time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBounceOut.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBounceIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounceIn;
            }
            else
            {
                pCopy = new CCEaseBounceIn();
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
        public new static CCEaseBounceIn actionWithAction(CCActionInterval pAction)
        {
            CCEaseBounceIn pRet = new CCEaseBounceIn();

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
