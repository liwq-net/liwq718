
namespace cocos2d
{
    public class CCEaseBackIn : CCActionEase
    {
        public override void Update(float time)
        {
            float overshoot = 1.70158f;
            m_pOther.Update(time * time * ((overshoot + 1) * time - overshoot));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBackOut.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseBackIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBackIn;
            }
            else
            {
                pCopy = new CCEaseBackIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()));

            return pCopy;
        }

        /// <summary>
        ///  creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new static CCEaseBackIn actionWithAction(CCActionInterval pAction)
        {
            CCEaseBackIn pRet = new CCEaseBackIn();

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
