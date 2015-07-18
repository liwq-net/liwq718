using liwq;
namespace cocos2d
{
    /** @brief Moves a CCNode object to a parabolic position simulating a jump movement by modifying it's position attribute.*/
    public class CCJumpTo : CCJumpBy
    {
        public static CCJumpTo actionWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            CCJumpTo ret = new CCJumpTo();
            ret.initWithDuration(duration, position, height, jumps);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_delta = CCPointExtension.ccp(m_delta.X - m_startPosition.X, m_delta.Y - m_startPosition.Y);
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCJumpTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCJumpTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCJumpTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_delta, m_height, m_nJumps);

            return ret;
        }
    }
}
