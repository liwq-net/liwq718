namespace cocos2d
{
    public class CCEaseElastic : CCActionEase
    {
        protected float m_fPeriod;

        /// <summary>
        /// get period of the wave in radians. default is 0.3
        /// set period of the wave in radians.
        /// </summary>
        public float Period
        {
            get { return m_fPeriod; }
            set { m_fPeriod = value; }
        }

        /// <summary>
        /// Initializes the action with the inner action and the period in radians (default is 0.3) 
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fPeriod"></param>
        /// <returns></returns>
        public bool initWithAction(CCActionInterval pAction, float fPeriod)
        {
            if (base.initWithAction(pAction))
            {
                m_fPeriod = fPeriod;
                return true;
            }

            return false;
        }

        /// <summary>
        /// initializes the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new bool initWithAction(CCActionInterval pAction)
        {
            return initWithAction(pAction, 0.3f);
        }

        public override CCFiniteTimeAction Reverse()
        {
            //assert(0);
            return null;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseElastic pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElastic;
            }
            else
            {
                pCopy = new CCEaseElastic();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fPeriod);

            return pCopy;
        }

        /// <summary>
        ///  creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new static CCEaseElastic actionWithAction(CCActionInterval pAction)
        {
            CCEaseElastic pRet = new CCEaseElastic();

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

        /// <summary>
        /// Creates the action with the inner action and the period in radians (default is 0.3)
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fPeriod"></param>
        /// <returns></returns>
        public static CCEaseElastic actionWithAction(CCActionInterval pAction, float fPeriod)
        {
            CCEaseElastic pRet = new CCEaseElastic();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fPeriod))
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
