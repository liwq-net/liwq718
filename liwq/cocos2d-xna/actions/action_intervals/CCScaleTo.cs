
namespace cocos2d
{
    public class CCScaleTo : CCActionInterval
    {
        /// <summary>
        /// initializes the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool initWithDuration(float duration, float s)
        {
            if (base.initWithDuration(duration))
            {
                m_fEndScaleX = s;
                m_fEndScaleY = s;

                return true;
            }

            return false;
        }

        /// <summary>
        /// initializes the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public bool initWithDuration(float duration, float sx, float sy)
        {
            if (base.initWithDuration(duration))
            {
                m_fEndScaleX = sx;
                m_fEndScaleY = sy;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCScaleTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCScaleTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_fEndScaleX, m_fEndScaleY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }
        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_fStartScaleX = pTarget.ScaleX;
            m_fStartScaleY = pTarget.ScaleY;
            m_fDeltaX = m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fEndScaleY - m_fStartScaleY;
        }
        public override void Update(float time)
        {
            if (Target != null)
            {
                Target.ScaleX = m_fStartScaleX + m_fDeltaX * time;
                Target.ScaleY = m_fStartScaleY + m_fDeltaY * time;
            }
        }

        /// <summary>
        /// creates the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CCScaleTo actionWithDuration(float duration, float s)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, s);
            //pScaleTo->autorelease();

            return pScaleTo;
        }

        /// <summary>
        /// creates the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static CCScaleTo actionWithDuration(float duration, float sx, float sy)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, sx, sy);
            //pScaleTo->autorelease();

            return pScaleTo;
        }

        protected float m_fScaleX;
        protected float m_fScaleY;
        protected float m_fStartScaleX;
        protected float m_fStartScaleY;
        protected float m_fEndScaleX;
        protected float m_fEndScaleY;
        protected float m_fDeltaX;
        protected float m_fDeltaY;
    }
}
