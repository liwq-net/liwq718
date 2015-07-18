using System;
namespace cocos2d
{
    /** 
	@brief Instant actions are immediate actions. They don't have a duration like
	the CCIntervalAction actions.
	*/
    public class CCActionInstant : CCFiniteTimeAction
    {
        public CCActionInstant() { }

        ~CCActionInstant() { }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCActionInstant ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCActionInstant)tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCActionInstant();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);
            return ret;
        }

        public override bool IsDone()
        {
            return true;
        }

        public override void Step(float dt)
        {
            Update(1);
        }

        public override void Update(float dt)
        {
            // ignore
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCFiniteTimeAction)copy();
        }
    }
}
