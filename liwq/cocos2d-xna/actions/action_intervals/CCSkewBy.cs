
namespace cocos2d
{
    public class CCSkewBy : CCSkewTo
    {
        public override bool initWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.initWithDuration(t, sx, sy))
            {
                m_fSkewX = sx;
                m_fSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_fDeltaX = m_fSkewX;
            m_fDeltaY = m_fSkewY;
            m_fEndSkewX = m_fStartSkewX + m_fDeltaX;
            m_fEndSkewY = m_fStartSkewY + m_fDeltaY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return actionWithDuration(Duration, -m_fSkewX, -m_fSkewY);
        }

        public new static CCSkewBy actionWithDuration(float t, float deltaSkewX, float deltaSkewY)
        {
            CCSkewBy pSkewBy = new CCSkewBy();
            if (pSkewBy != null)
            {
                if (pSkewBy.initWithDuration(t, deltaSkewX, deltaSkewY))
                {
                    //pSkewBy->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pSkewBy);
                }
            }

            return pSkewBy;
        }
    }
}
