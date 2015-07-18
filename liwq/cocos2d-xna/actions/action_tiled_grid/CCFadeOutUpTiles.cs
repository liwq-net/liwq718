using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFadeOutUpTiles action.
    /// Fades out the tiles in upwards direction
    /// </summary>
    public class CCFadeOutUpTiles : CCFadeOutTRTiles
    {
        public override float testFunc(ccGridSize pos, float time)
        {
            CCPoint n = new CCPoint((float)(m_sGridSize.x * time), (float)(m_sGridSize.y * time));
            if (n.Y == 0.0f)
            {
                return 1.0f;
            }

            return (float)Math.Pow(pos.y / n.Y, 6);
        }

        public override void transformTile(ccGridSize pos, float distance)
        {
            ccQuad3 coords = originalTile(pos);
            if (coords == null)
            {
                return;
            }
            CCPoint step = Target.Grid.Step;

            coords.bl.y += (step.Y / 2) * (1.0f - distance);
            coords.br.y += (step.Y / 2) * (1.0f - distance);
            coords.tl.y -= (step.Y / 2) * (1.0f - distance);
            coords.tr.y -= (step.Y / 2) * (1.0f - distance);

            setTile(pos, coords);
        }

        /// <summary>
        /// creates the action with the grid size and the duration 
        /// </summary>
        public static CCFadeOutUpTiles actionWithSize(ccGridSize gridSize, float time)
        {
            CCFadeOutUpTiles pAction = new CCFadeOutUpTiles();
            if (pAction.initWithSize(gridSize, time))
            {
                return pAction;
            }
            return null;
        }
    }
}
