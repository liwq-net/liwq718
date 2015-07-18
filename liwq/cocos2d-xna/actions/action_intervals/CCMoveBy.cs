
using liwq;
namespace cocos2d
{
    /** @brief Moves a CCNode object x,y pixels by modifying it's position attribute.
     x and y are relative to the position of the object.
     Duration is is seconds.
    */
    public class CCMoveBy : CCMoveTo
    {
        public new bool initWithDuration(float duration, CCPoint position)
        {
            if (base.initWithDuration(duration))
            {
                m_delta = position;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCMoveBy ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCMoveBy;

                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCMoveBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_delta);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            CCPoint dTmp = m_delta;
            base.StartWithTarget(target);
            m_delta = dTmp;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCMoveBy.actionWithDuration(Duration, CCPointExtension.ccp(-m_delta.X, -m_delta.Y));
        }

        public static new CCMoveBy actionWithDuration(float duration, CCPoint position)
        {
            CCMoveBy ret = new CCMoveBy();
            ret.initWithDuration(duration, position);

            return ret;
        }
    }
}
