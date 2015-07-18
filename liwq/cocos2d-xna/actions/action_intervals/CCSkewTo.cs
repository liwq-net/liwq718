
namespace cocos2d
{
    public class CCSkewTo : CCActionInterval
    {
        public CCSkewTo() { }

        public virtual bool initWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.initWithDuration(t))
            {
                m_fEndSkewX = sx;
                m_fEndSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSkewTo pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCSkewTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSkewTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_fEndSkewX, m_fEndSkewY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override void StartWithTarget(Node pTarget)
        {

            base.StartWithTarget(pTarget);

            m_fStartSkewX = pTarget.SkewX;

            if (m_fStartSkewX > 0)
            {
                m_fStartSkewX = m_fStartSkewX % 180f;
            }
            else
            {
                m_fStartSkewX = m_fStartSkewX % -180f;
            }

            m_fDeltaX = m_fEndSkewX - m_fStartSkewX;

            if (m_fDeltaX > 180)
            {
                m_fDeltaX -= 360;
            }
            if (m_fDeltaX < -180)
            {
                m_fDeltaX += 360;
            }

            m_fStartSkewY = pTarget.SkewY;

            if (m_fStartSkewY > 0)
            {
                m_fStartSkewY = m_fStartSkewY % 360f;
            }
            else
            {
                m_fStartSkewY = m_fStartSkewY % -360f;
            }

            m_fDeltaY = m_fEndSkewY - m_fStartSkewY;

            if (m_fDeltaY > 180)
            {
                m_fDeltaY -= 360;
            }
            if (m_fDeltaY < -180)
            {
                m_fDeltaY += 360;
            }
        }

        public override void Update(float time)
        {
            Target.SkewX = m_fStartSkewX + m_fDeltaX * time;
            Target.SkewY = m_fStartSkewY + m_fDeltaY * time;
        }

        public static CCSkewTo actionWithDuration(float t, float sx, float sy)
        {

            CCSkewTo pSkewTo = new CCSkewTo();

            if (pSkewTo != null)
            {
                if (pSkewTo.initWithDuration(t, sx, sy))
                {
                    //pSkewTo->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pSkewTo);
                }
            }

            return pSkewTo;
        }

        protected float m_fSkewX;
        protected float m_fSkewY;
        protected float m_fStartSkewX;
        protected float m_fStartSkewY;
        protected float m_fEndSkewX;
        protected float m_fEndSkewY;
        protected float m_fDeltaX;
        protected float m_fDeltaY;
    }
}
