namespace cocos2d
{
    /** @brief An action that moves the target with a cubic Bezier curve to a destination point.
     @since v0.8.2
     */
    public class CCBezierTo : CCBezierBy
    {
        public static CCBezierTo actionWithDuration(float t, ccBezierConfig c)
        {
            CCBezierTo ret = new CCBezierTo();
            ret.initWithDuration(t, c);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_sConfig.controlPoint_1 = CCPointExtension.ccpSub(m_sConfig.controlPoint_1, m_startPosition);
            m_sConfig.controlPoint_2 = CCPointExtension.ccpSub(m_sConfig.controlPoint_2, m_startPosition);
            m_sConfig.endPosition = CCPointExtension.ccpSub(m_sConfig.endPosition, m_startPosition);
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCBezierTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCBezierTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration, m_sConfig);

            return ret;
        }
    }
}
