namespace cocos2d
{
    /// <summary>
    /// @brief Base class for Grid actions
    /// </summary>
    public class CCGridAction : CCActionInterval
    {
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCGridAction pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCGridAction)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCGridAction();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithSize(m_sGridSize, Duration);

            if (pNewZone != null)
            {
                pNewZone = null;
            }
            return pCopy;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);

            CCGridBase newgrid = this.getGrid();

            Node t = Target;
            CCGridBase targetGrid = t.Grid;

            if (targetGrid != null && targetGrid.ReuseGrid > 0)
            {
                if (targetGrid.Active && targetGrid.GridSize.x == m_sGridSize.x
                    && targetGrid.GridSize.y == m_sGridSize.y)
                {
                    targetGrid.reuse();
                }
            }
            else
            {
                if (targetGrid != null && targetGrid.Active)
                {
                    targetGrid.Active = false;
                }

                t.Grid = newgrid;
                t.Grid.Active = true;
            }
        }
        public override CCFiniteTimeAction Reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }

        /// <summary>
        /// Always return null. Must be overridden in the specializer.
        /// </summary>
        public virtual CCGridBase getGrid()
        {
            return null;
        }

        /// <summary>
        /// creates the action with size and duration
        /// </summary>
        public static CCGridAction actionWithSize(ccGridSize gridSize, float duration)
        {
            CCGridAction pAction = new CCGridAction();
            if (pAction.initWithSize(gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /// <summary>
        /// initializes the action with size and duration
        /// </summary>
        public virtual bool initWithSize(ccGridSize gridSize, float duration)
        {
            if (base.initWithDuration(duration))
            {
                m_sGridSize = gridSize;

                return true;
            }

            return false;
        }

        protected ccGridSize m_sGridSize;
    }
}
