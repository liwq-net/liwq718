using Microsoft.Xna.Framework;
using System;

namespace cocos2d
{
    public class CCEaseElasticInOut : CCEaseElastic
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
                time = time * 2;
                if (m_fPeriod==0)
                {
                    m_fPeriod = 0.3f * 1.5f;
                }

                float s = m_fPeriod / 4;

                time = time - 1;
                if (time < 0)
                {
                    newT = (float)(-0.5f * Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.TwoPi/ m_fPeriod));
                }
                else
                {
                    newT = (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * MathHelper.TwoPi / m_fPeriod) * 0.5f + 1);
                }
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse() 
        {
            return CCEaseInOut.actionWithAction((CCActionInterval)m_pOther.Reverse(), m_fPeriod);
        }

        public override CCObject copyWithZone(CCZone pZone) 
        {
            CCZone pNewZone = null;
            CCEaseElasticInOut pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElasticInOut;
            }
            else
            {
                pCopy = new CCEaseElasticInOut();
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
        public new static CCEaseElasticInOut actionWithAction(CCActionInterval pAction) 
        {
            CCEaseElasticInOut pRet = new CCEaseElasticInOut();

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
        public new static CCEaseElasticInOut actionWithAction(CCActionInterval pAction, float fPeriod) 
        {
            CCEaseElasticInOut pRet = new CCEaseElasticInOut();

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
