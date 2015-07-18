namespace cocos2d
{
    public class CCEaseRateAction:CCActionEase
    {
        protected float m_fRate;

        /// <summary>
        ///  get rate value for the actions
        ///  set rate value for the actions
        /// </summary>
        public float Rate 
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        /// <summary>
        /// Initializes the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public bool initWithAction(CCActionInterval pAction, float fRate) 
        {
            if (base.initWithAction(pAction))
		    {
			    m_fRate = fRate;
			    return true;
		    }

		return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseRateAction pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseRateAction)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseRateAction();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fRate);

            return pCopy;
        }
        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseRateAction.actionWithAction((CCActionInterval)m_pOther.Reverse(), 1 / m_fRate);
        }

        /// <summary>
        /// Creates the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public static CCEaseRateAction actionWithAction(CCActionInterval pAction, float fRate)
        {
            CCEaseRateAction pRet = new CCEaseRateAction();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fRate))
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
