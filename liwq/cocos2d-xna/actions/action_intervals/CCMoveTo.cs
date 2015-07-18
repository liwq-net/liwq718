using liwq;
namespace cocos2d
{
    /** @brief Moves a CCNode object to the position x,y. x and y are absolute coordinates by modifying it's position attribute.*/
    public class CCMoveTo : CCActionInterval
    {
        public bool initWithDuration(float duration, CCPoint position)
        {
            if (base.initWithDuration(duration))
            {
                m_endPosition = position;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCMoveTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCMoveTo)tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCMoveTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);
            ret.initWithDuration(Duration, m_endPosition);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
            m_delta = CCPointExtension.ccpSub(m_endPosition, m_startPosition);
        }

        public override void Update(float dt)
        {
            if (Target != null)
            {
                Target.Position = CCPointExtension.ccp(m_startPosition.X + m_delta.X * dt,
                    m_startPosition.Y + m_delta.Y * dt);
            }
        }

        /** creates the action */
        public static CCMoveTo actionWithDuration(float duration, CCPoint position)
        {
            CCMoveTo moveTo = new CCMoveTo();
            moveTo.initWithDuration(duration, position);

            return moveTo;
        }

        protected CCPoint m_endPosition = new CCPoint(0f, 0f);
        protected CCPoint m_startPosition = new CCPoint(0f, 0f);
        protected CCPoint m_delta = new CCPoint(0f, 0f);
    }
}
