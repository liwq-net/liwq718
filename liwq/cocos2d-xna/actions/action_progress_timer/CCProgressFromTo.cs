using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief Progress from a percentage to another percentage
    /// @since v0.99.1
    /// </summary>
    public class CCProgressFromTo : CCActionInterval
    {
        /// <summary>
        /// Initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public bool initWithDuration(float duration, float fFromPercentage, float fToPercentage)
        {
            // if (CCActionInterval::initWithDuration(duration))
            if (initWithDuration(duration))
            {
                m_fTo = fToPercentage;
                m_fFrom = fFromPercentage;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCProgressFromTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCProgressFromTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCProgressFromTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            // CCActionInterval::copyWithZone(pZone);
            copyWithZone(pZone);
            pCopy.initWithDuration(m_fDuration, m_fFrom, m_fTo);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCProgressFromTo.actionWithDuration(m_fDuration, m_fTo, m_fFrom);
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
        }

        public override void update(float time)
        {
            ((CCProgressTimer)(m_pTarget)).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
        }

        /// <summary>
        /// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public static CCProgressFromTo actionWithDuration(float duration, float fFromPercentage, float fToPercentage)
        {
            CCProgressFromTo pProgressFromTo = new CCProgressFromTo();
            pProgressFromTo.initWithDuration(duration, fFromPercentage, fToPercentage);

            return pProgressFromTo;
        }

        protected float m_fTo;
        protected float m_fFrom;
    }
}
