using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFadeOutTRTiles action
    /// Fades out the tiles in a Top-Right direction
    /// </summary>
    public class CCFadeOutTRTiles : CCTiledGrid3DAction
    {
        public virtual float testFunc(ccGridSize pos, float time)
        {
            CCPoint n = new CCPoint((float)(m_sGridSize.x * time), (float)(m_sGridSize.y * time));
            if ((n.X + n.Y) == 0.0f)
            {
                return 1.0f;
            }

            return (float)Math.Pow((pos.x + pos.y) / (n.X + n.Y), 6);
        }

        public void turnOnTile(ccGridSize pos)
        {
            setTile(pos, originalTile(pos));
        }

        public void turnOffTile(ccGridSize pos)
        {
            ccQuad3 coords = new ccQuad3();
            //memset(&coords, 0, sizeof(ccQuad3));
            setTile(pos, coords);
        }

        public virtual void transformTile(ccGridSize pos, float distance)
        {
            ccQuad3 coords = originalTile(pos);
            if (coords == null)
            {
                return;
            }
            CCPoint step = Target.Grid.Step;

            coords.bl.x += (step.X / 2) * (1.0f - distance);
            coords.bl.y += (step.Y / 2) * (1.0f - distance);

            coords.br.x -= (step.X / 2) * (1.0f - distance);
            coords.br.y += (step.Y / 2) * (1.0f - distance);

            coords.tl.x += (step.X / 2) * (1.0f - distance);
            coords.tl.y -= (step.Y / 2) * (1.0f - distance);

            coords.tr.x -= (step.X / 2) * (1.0f - distance);
            coords.tr.y -= (step.Y / 2) * (1.0f - distance);

            setTile(pos, coords);
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    float distance = testFunc(new ccGridSize(i, j), time);
                    if (distance == 0)
                    {
                        turnOffTile(new ccGridSize(i, j));
                    }
                    else
                        if (distance < 1)
                        {
                            transformTile(new ccGridSize(i, j), distance);
                        }
                        else
                        {
                            turnOnTile(new ccGridSize(i, j));
                        }
                }
            }
        }

        /// <summary>
        /// creates the action with the grid size and the duration
        /// </summary>
        public new static CCFadeOutTRTiles actionWithSize(ccGridSize gridSize, float time)
        {
            CCFadeOutTRTiles pAction = new CCFadeOutTRTiles();

            if (pAction.initWithSize(gridSize, time))
            {
                return pAction;
            }

            return null;
        }
    }
}
