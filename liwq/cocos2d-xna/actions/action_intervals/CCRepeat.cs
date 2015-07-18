namespace cocos2d
{
    /** @brief Repeats an action a number of times.
     * To repeat an action forever use the CCRepeatForever action.
     */
    public class CCRepeat : CCActionInterval
    {
        /** initializes a CCRepeat action. Times is an unsigned integer between 1 and pow(2,30) */
        public bool initWithAction(CCFiniteTimeAction action, uint times)
        {
            float d = action.Duration * times;

            if (base.initWithDuration(d))
            {
                m_uTimes = times;
                m_pInnerAction = action;

                m_uTotal = 0;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRepeat ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRepeat;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRepeat();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param = m_pInnerAction.copy() as CCFiniteTimeAction;
            if (param == null)
            {
                return null;
            }
            ret.initWithAction(param, m_uTimes);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            m_uTotal = 0;
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pInnerAction.Stop();
            base.Stop();
        }

        // issue #80. Instead of hooking step:, hook update: since it can be called by any 
        // container action like Repeat, Sequence, AccelDeccel, etc..
        public override void Update(float dt)
        {
            float t = dt * m_uTimes;
            if (t > m_uTotal + 1)
            {
                m_pInnerAction.Update(1.0f);
                m_uTotal++;
                m_pInnerAction.Stop();
                m_pInnerAction.StartWithTarget(Target);

                // repeat is over?
                if (m_uTotal == m_uTimes)
                {
                    // so, set it in the original position
                    m_pInnerAction.Update(0);
                }
                else
                {
                    // no ? start next repeat with the right update
                    // to prevent jerk (issue #390)
                    m_pInnerAction.Update(t - m_uTotal);
                }
            }
            else
            {
                float r = t % 1.0f;

                // fix last repeat position
                // else it could be 0.
                if (dt == 1.0f)
                {
                    r = 1.0f;
                    m_uTotal++; // this is the added line
                }

                m_pInnerAction.Update(r > 1 ? 1 : r);
            }
        }

        public override bool IsDone()
        {
            return m_uTotal == m_uTimes;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCRepeat.actionWithAction(m_pInnerAction.Reverse(), m_uTimes);
        }

        /** creates a CCRepeat action. Times is an unsigned integer between 1 and pow(2,30) */
        public static CCRepeat actionWithAction(CCFiniteTimeAction action, uint times)
        {
            CCRepeat ret = new CCRepeat();
            ret.initWithAction(action, times);

            return ret;
        }

        protected CCFiniteTimeAction m_pInnerAction;
        public CCFiniteTimeAction InnerAction
        {
            get
            {
                return m_pInnerAction;
            }
            set
            {
                m_pInnerAction = value;
            }
        }

        protected uint m_uTimes;
        protected uint m_uTotal;
    }
}
