using Microsoft.Xna.Framework;
using System;

namespace cocos2d
{
    public class CCEaseElasticOut : CCEaseElastic
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
                newT = (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2f / m_fPeriod) + 1);
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseElasticIn.actionWithAction((CCActionInterval)m_pOther.Reverse(), m_fPeriod);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseElasticOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElasticOut;
            }
            else
            {
                pCopy = new CCEaseElasticOut();
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
        public new static CCEaseElasticOut actionWithAction(CCActionInterval pAction)
        {
            CCEaseElasticOut pRet = new CCEaseElasticOut();

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
        public new static CCEaseElasticOut actionWithAction(CCActionInterval pAction, float fPeriod)
        {
            CCEaseElasticOut pRet = new CCEaseElasticOut();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fPeriod))
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
