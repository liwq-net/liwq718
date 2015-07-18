using System;

namespace cocos2d
{
    public class CCToggleVisibility : CCActionInstant
    {
        public static new CCToggleVisibility action()
        {
            CCToggleVisibility pRet = new CCToggleVisibility();
            return pRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            pTarget.Visible = !pTarget.Visible;
        }
    }
}