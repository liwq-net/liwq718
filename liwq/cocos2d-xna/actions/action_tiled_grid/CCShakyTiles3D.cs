using System;

namespace cocos2d
{
    public class CCShakyTiles3D : CCTiledGrid3DAction
    {
        /// <summary>
        ///  initializes the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
        public bool initWithRange(int nRange, bool bShakeZ, ccGridSize gridSize,
            float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nRandrange = nRange;
                m_bShakeZ = bShakeZ;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShakyTiles3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCShakyTiles3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShakyTiles3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithRange(m_nRandrange, m_bShakeZ, m_sGridSize, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    ccQuad3 coords = originalTile(i, j);
                    if (coords == null)
                    {
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

                    if (m_bShakeZ)
                    {
                        coords.bl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.br.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tr.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    setTile(i, j, coords);
                }
            }
        }

        /// <summary>
        /// creates the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
        public static CCShakyTiles3D actionWithRange(int nRange, bool bShakeZ, ccGridSize gridSize, float duration)
        {
            CCShakyTiles3D pAction = new CCShakyTiles3D();

            if (pAction.initWithRange(nRange, bShakeZ, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRandrange;
        protected bool m_bShakeZ;
    }
}
