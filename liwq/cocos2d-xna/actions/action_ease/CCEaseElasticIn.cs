using Microsoft.Xna.Framework;
using System;

namespace cocos2d
{
    public class CCEaseElasticIn : CCEaseElastic
    {
        public override void Update(float time)
        {
            float newT = 0;

            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                float s = m_fPeriod / 4;
                time = time - 1;
                newT = -(float)(Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2.0f / m_fPeriod));
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseElasticOut.actionWithAction((CCActionInterval)m_pOther.Reverse(), m_fPeriod);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseElasticIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElasticIn;
            }
            else
            {
                pCopy = new CCEaseElasticIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fPeriod);

            return pCopy;
        }

        /// <summary>
        /// creates the action
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public new static CCEaseElasticIn actionWithAction(CCActionInterval pAction)
        {
            CCEaseElasticIn pRet = new CCEaseElasticIn();

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

        /// <summary>
        /// Creates the action with the inner action and the period in radians (default is 0.3) 
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fPeriod"></param>
        /// <returns></returns>
        public new static CCEaseElasticIn actionWithAction(CCActionInterval pAction, float fPeriod)
        {
            CCEaseElasticIn pRet = new CCEaseElasticIn();

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
