using System.Diagnostics;
namespace cocos2d
{
    public class CCRepeatForever : CCActionInterval
    {
        public static CCRepeatForever actionWithAction(CCActionInterval action)
        {
            CCRepeatForever ret = new CCRepeatForever();
            ret.initWithAction(action);

            return ret;
        }

        public bool initWithAction(CCActionInterval action)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRepeatForever ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRepeatForever;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRepeatForever();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCActionInterval param = m_pInnerAction.copy() as CCActionInterval;
            if (param == null)
            {
                return null;
            }
            ret.initWithAction(param);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Step(float dt)
        {
            m_pInnerAction.Step(dt);
            if (m_pInnerAction.IsDone())
            {
                float diff = dt + m_pInnerAction.Duration - m_pInnerAction.elapsed;
                m_pInnerAction.StartWithTarget(Target);
                m_pInnerAction.Step(diff);
            }
        }

        public override bool IsDone()
        {
            return false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCRepeatForever.actionWithAction(m_pInnerAction.Reverse() as CCActionInterval);
        }

        protected CCActionInterval m_pInnerAction;
        public CCActionInterval InnerAction
        {
            get { return m_pInnerAction; }
            set { m_pInnerAction = value; }
        }
    }
}
