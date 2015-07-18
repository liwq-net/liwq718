using System;

namespace cocos2d
{
    /// <summary>
    /// @brief CCShatteredTiles3D action
    /// </summary>
    public class CCShatteredTiles3D : CCTiledGrid3DAction
    {
        /// <summary>
        /// initializes the action with a range, whether or not to shatter Z vertices, a grid size and duration
        /// </summary>
        public bool initWithRange(int nRange, bool bShatterZ, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_bOnce = false;
                m_nRandrange = nRange;
                m_bShatterZ = bShatterZ;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShatteredTiles3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCShatteredTiles3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShatteredTiles3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            //copy super class's member
            base.copyWithZone(pZone);

            pCopy.initWithRange(m_nRandrange, m_bShatterZ, m_sGridSize, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            if (m_bOnce == false)
            {
                for (i = 0; i < m_sGridSize.x; ++i)
                {
                    for (j = 0; j < m_sGridSize.y; ++j)
                    {
                        ccQuad3 coords = originalTile(i, j);
                        if (coords == null)
                        {
                            // Always bail out here b/c the object is not setup correctly.
                            return;
                        }
                        Random rand = new Random();
                        // X
                        coords.bl.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.br.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tl.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tr.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        // Y
                        coords.bl.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.br.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tl.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tr.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        if (m_bShatterZ)
                        {
                            coords.bl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.br.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.tl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.tr.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        }

                        setTile(i, j, coords);
                    }
                }

                m_bOnce = true;
            }
        }

        /// <summary>
        /// creates the action with a range, whether of not to shatter Z vertices, a grid size and duration
        /// </summary>
        public static CCShatteredTiles3D actionWithRange(int nRange, bool bShatterZ, ccGridSize gridSize, float duration)
        {
            CCShatteredTiles3D pAction = new CCShatteredTiles3D();

            if (pAction.initWithRange(nRange, bShatterZ, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRandrange;
        protected bool m_bOnce;
        protected bool m_bShatterZ;
    }
}
