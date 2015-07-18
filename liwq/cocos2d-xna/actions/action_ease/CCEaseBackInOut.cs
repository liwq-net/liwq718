namespace cocos2d
{
    public class CCEaseBackInOut : CCActionEase
    {
        public override void Update(float time)
        {
            float overshoot = 1.70158f * 1.525f;

            time = time * 2;
            if (time < 1)
            {
                m_pOther.Update((time * time * ((overshoot + 1) * time - overshoot)) / 2);
            }
            else
            {
                time = time - 2;
                m_pOther.Update((time * time * ((overshoot + 1) + overshoot)) / 2 + 1);
            }
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBackInOut pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBackInOut;
            }
            else
            {
                pCopy = new CCEaseBackInOut();
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
        public new static CCEaseBackInOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseBackInOut pRet = new CCEaseBackInOut();

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
