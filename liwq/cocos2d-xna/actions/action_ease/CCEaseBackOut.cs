namespace cocos2d
{
    public class CCEaseBackOut : CCActionEase
    {
        public override void Update(float time)
        {
            float overshoot = 1.70158f;

            time = time - 1;
            m_pOther.Update(time * time * ((overshoot + 1) * time + overshoot) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBackIn.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBackOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBackOut;
            }
            else
            {
                pCopy = new CCEaseBackOut();
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
        public new static CCEaseBackOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseBackOut pRet = new CCEaseBackOut();

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
