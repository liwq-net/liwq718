using System.Diagnostics;
namespace cocos2d
{
    /** @brief Executes an action in reverse order, from time=duration to time=0
 
     @warning Use this action carefully. This action is not
     sequenceable. Use it as the default "reversed" method
     of your own actions, but using it outside the "reversed"
     scope is not recommended.
    */
    public class CCReverseTime : CCActionInterval
    {
        public bool initWithAction(CCFiniteTimeAction action)
        {
            Debug.Assert(action != null);
            Debug.Assert(action != m_pOther);

            if (base.initWithDuration(action.Duration))
            {
                m_pOther = action;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCReverseTime ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCReverseTime();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithAction(m_pOther.copy() as CCFiniteTimeAction);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_pOther.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pOther.Stop();
            base.Stop();
        }

        public override void Update(float dt)
        {
            if (m_pOther != null)
            {
                m_pOther.Update(1 - dt);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return m_pOther.copy() as CCFiniteTimeAction;
        }

        public static CCReverseTime actionWithAction(CCFiniteTimeAction action)
        {
            CCReverseTime ret = new CCReverseTime();
            ret.initWithAction(action);

            return ret;
        }

        protected CCFiniteTimeAction m_pOther;
    }
}
