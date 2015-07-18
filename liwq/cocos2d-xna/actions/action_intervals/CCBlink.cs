using System;
namespace cocos2d
{
    /** @brief Blinks a CCNode object by modifying it's visible attribute
    */
    public class CCBlink : CCActionInterval
    {
        public static CCBlink actionWithDuration(float duration, uint uBlinks)
        {
            CCBlink pBlink = new CCBlink();
            pBlink.initWithDuration(duration, uBlinks);

            return pBlink;
        }

        public bool initWithDuration(float duration, uint uBlinks)
        {
            if (base.initWithDuration(duration))
            {
                m_nTimes = uBlinks;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCBlink pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCBlink)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCBlink();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_nTimes);

            return pCopy;
        }

        public override void Update(float time)
        {
            if (Target != null && !IsDone())
            {
                float slice = 1.0f / m_nTimes;
                // float m = fmodf(time, slice);
                float m = time % slice;
                Target.Visible = m > slice / 2 ? true : false;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCBlink.actionWithDuration(Duration, m_nTimes);
        }

        protected uint m_nTimes;
    }
}