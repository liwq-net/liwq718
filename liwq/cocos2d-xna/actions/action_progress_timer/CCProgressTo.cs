using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// Progress to percentage
    /// @since v0.99.1
    /// </summary>
    public class CCProgressTo : CCActionInterval
    {
        /// <summary>
        /// Initializes with a duration and a percent
        /// </summary>
        public bool initWithDuration(float duration, float fPercent)
        {
            if (base.initWithDuration(duration))
            {
                m_fTo = fPercent;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCProgressTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCProgressTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCProgressTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_fTo);

            return pCopy;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_fFrom = ((CCProgressTimer)(pTarget)).Percentage;
            // XXX: Is this correct ?
            // Adding it to support CCRepeat
            if (m_fFrom == 100)
            {
                m_fFrom = 0;
            }
        }
        public override void update(float time)
        {
            ((CCProgressTimer)m_pTarget).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Creates and initializes with a duration and a percent
        /// </summary>
        public static CCProgressTo actionWithDuration(float duration, float fPercent)
        {
            CCProgressTo pProgressTo = new CCProgressTo();
            pProgressTo.initWithDuration(duration, fPercent);

            return pProgressTo;
        }

        protected float m_fTo;
        protected float m_fFrom;
    }
}
