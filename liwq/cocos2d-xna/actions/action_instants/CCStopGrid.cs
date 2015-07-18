using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCStopGrid action.
    /// @warning Don't call this action if another grid action is active.
    /// Call if you want to remove the the grid effect. Example:
    /// CCSequence::actions(Lens::action(...), CCStopGrid::action(...), NULL);
    /// </summary>
    public class CCStopGrid : CCActionInstant
    {
        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);

            CCGridBase pGrid = Target.Grid;
            if (pGrid != null && pGrid.Active)
            {
                pGrid.Active = false;
            }
        }

        public static new CCStopGrid action()
        {
            CCStopGrid pAction = new CCStopGrid();

            return pAction;
        }
    }
}