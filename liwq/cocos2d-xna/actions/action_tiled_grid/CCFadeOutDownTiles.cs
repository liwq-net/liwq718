using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFadeOutDownTiles action.
    /// Fades out the tiles in downwards direction
    /// </summary>
    public class CCFadeOutDownTiles : CCFadeOutUpTiles
    {
        public override float testFunc(ccGridSize pos, float time)
        {
            CCPoint n = new CCPoint((float)(m_sGridSize.x * (1.0f - time)), (float)(m_sGridSize.y * (1.0f - time)));
            if (pos.y == 0)
            {
                return 1.0f;
            }

            return (float)Math.Pow(n.Y / pos.y, 6);
        }

        /// <summary>
        ///  creates the action with the grid size and the duration 
        /// </summary>
        public new static CCFadeOutDownTiles actionWithSize(ccGridSize gridSize, float time)
        {
            CCFadeOutDownTiles pAction = new CCFadeOutDownTiles();
            if (pAction.initWithSize(gridSize, time))
            {
                return pAction;
            }

            return null;
        }
    }
}
