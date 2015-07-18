using System;
namespace cocos2d
{
    /** @brief Fades an object that implements the CCRGBAProtocol protocol. It modifies the opacity from the current value to a custom one.
    @warning This action doesn't support "reverse"
    */
    public class CCFadeTo : CCActionInterval
    {
        public bool initWithDuration(float duration, byte opacity)
        {
            if (base.initWithDuration(duration))
            {
                m_toOpacity = opacity;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFadeTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_toOpacity);

            return pCopy;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);

            ICCRGBAProtocol pRGBAProtocol = pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                m_fromOpacity = pRGBAProtocol.Opacity;
            }
        }

        public override void Update(float time)
        {
            ICCRGBAProtocol pRGBAProtocol = Target as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte)(m_fromOpacity + (m_toOpacity - m_fromOpacity) * time);
            }
        }

        /** creates an action with duration and opacity */
        public static CCFadeTo actionWithDuration(float duration, byte opacity)
        {
            CCFadeTo pFadeTo = new CCFadeTo();
            pFadeTo.initWithDuration(duration, opacity);

            return pFadeTo;
        }


        protected byte m_toOpacity;
        protected byte m_fromOpacity;

    }
}