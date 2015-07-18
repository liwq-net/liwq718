
namespace cocos2d
{
    public class CCScaleBy : CCScaleTo
    {
        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCScaleBy.actionWithDuration(Duration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCScaleTo pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleBy)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCScaleBy();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_fEndScaleX, m_fEndScaleY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        /// <summary>
        /// creates the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public new static CCScaleBy actionWithDuration(float duration, float s)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, s);
            //pScaleBy->autorelease();

            return pScaleBy;
        }

        /// <summary>
        ///  creates the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public new static CCScaleBy actionWithDuration(float duration, float sx, float sy)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, sx, sy);
            //pScaleBy->autorelease();

            return pScaleBy;
        }
    }
}
