namespace cocos2d
{
    public class CCEaseBounceOut : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT = bounceTime(time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBounceIn.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBounceOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounceOut;
            }
            else
            {
                pCopy = new CCEaseBounceOut();
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
        public new static CCEaseBounceOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseBounceOut pRet = new CCEaseBounceOut();

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
