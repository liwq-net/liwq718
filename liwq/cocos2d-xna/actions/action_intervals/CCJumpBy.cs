using liwq;
namespace cocos2d
{
    public class CCJumpBy : CCActionInterval
    {
        public bool initWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            if (base.initWithDuration(duration))
            {
                m_delta = position;
                m_height = height;
                m_nJumps = jumps;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCJumpBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCJumpBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCJumpBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_delta, m_height, m_nJumps);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
        }

        public override void Update(float dt)
        {
            if (Target != null)
            {
                // Is % equal to fmodf()???
                float frac = (dt * m_nJumps) % 1.0f;
                float y = m_height * 4 * frac * (1 - frac);
                y += m_delta.Y * dt;
                float x = m_delta.X * dt;
                Target.Position = CCPointExtension.ccp(m_startPosition.X + x, m_startPosition.Y + y);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCJumpBy.actionWithDuration(Duration, CCPointExtension.ccp(-m_delta.X, -m_delta.Y), m_height, m_nJumps);
        }

        public static CCJumpBy actionWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            CCJumpBy ret = new CCJumpBy();
            ret.initWithDuration(duration, position, height, jumps);

            return ret;
        }

        protected CCPoint m_startPosition;
        protected CCPoint m_delta;
        protected float m_height;
        protected uint m_nJumps;
    }
}
