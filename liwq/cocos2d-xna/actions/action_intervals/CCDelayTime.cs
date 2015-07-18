using System;
namespace cocos2d
{
    /// <summary>
    /// @brief Delays the action a certain amount of seconds
    /// </summary>
    public class CCDelayTime : CCActionInterval
    {
        public static CCDelayTime actionWithDuration(float d)
        {
            CCDelayTime pAction = new CCDelayTime();

            pAction.initWithDuration(d);

            return pAction;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCDelayTime pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCDelayTime)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCDelayTime();
                pZone = pNewZone = new CCZone(pCopy);
            }


            base.copyWithZone(pZone);

            return pCopy;
        }

        public override void Update(float time)
        {
            return;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return actionWithDuration(Duration);
        }
    }
}