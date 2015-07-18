using System;

namespace cocos2d
{
    public class CCReuseGrid : CCActionInstant
    {

        /// <summary>
        /// initializes an action with the number of times that the current grid will be reused
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool initWithTimes(int times)
        {
            m_nTimes = times;
            return true;
        }

        public virtual void startWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);

            if (Target.Grid != null && Target.Grid.Active != null)
            {
                Target.Grid.ReuseGrid = Target.Grid.ReuseGrid + m_nTimes;
            }
        }

        /// <summary>
        /// creates an action with the number of times that the current grid will be reused
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static CCReuseGrid actionWithTimes(int times)
        {
            CCReuseGrid pAction = new CCReuseGrid();
            if (pAction != null)
            {
                if (pAction.initWithTimes(times))
                {
                    //pAction->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pAction);
                }
            }

            return pAction;
        }

        protected int m_nTimes;
    }
}
