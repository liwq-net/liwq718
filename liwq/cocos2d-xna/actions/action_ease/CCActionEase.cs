namespace cocos2d
{
    public class CCActionEase : CCActionInterval
    {
        /// <summary>
        /// initializes the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public bool initWithAction(CCActionInterval pAction)
        {
            if (base.initWithDuration(pAction.Duration))
            {
                m_pOther = pAction;
                return true;
            }
            return false;

        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCActionEase pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCActionEase;
            }
            else
            {
                pCopy = new CCActionEase();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()));

            return pCopy;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_pOther.StartWithTarget(Target);
        }

        public override void Stop()
        {
            m_pOther.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            m_pOther.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCActionEase.actionWithAction((CCActionInterval)m_pOther.Reverse());
        }

        /// <summary>
        /// creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public static CCActionEase actionWithAction(CCActionInterval pAction)
        {
            CCActionEase pRet = new CCActionEase();

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

        protected CCActionInterval m_pOther;

    }
}
