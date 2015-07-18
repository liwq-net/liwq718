using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFadeOutBLTiles action.
    /// Fades out the tiles in a Bottom-Left direction
    /// </summary>
    public class CCFadeOutBLTiles : CCFadeOutTRTiles
    {
        public override float testFunc(ccGridSize pos, float time)
        {
            CCPoint n = new CCPoint((float)(m_sGridSize.x * (1.0f - time)), (float)(m_sGridSize.y * (1.0f - time)));
            if ((pos.x + pos.y) == 0)
            {
                return 1.0f;
            }

            return (float)Math.Pow((n.X + n.Y) / (pos.x + pos.y), 6);
        }

        /// <summary>
        /// creates the action with the grid size and the duration
        /// </summary>
        public static CCFadeOutBLTiles actionWithSize(ccGridSize gridSize, float time)
        {
            CCFadeOutBLTiles pAction = new CCFadeOutBLTiles();
            if (pAction.initWithSize(gridSize, time))
            {
                return pAction;
            }

            return null;
        }
    }
}
