using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief CCTurnOffTiles action.
    /// Turn off the files in random order
    /// </summary>
    public class CCTurnOffTiles : CCTiledGrid3DAction
    {
        /// <summary>
        /// initializes the action with a random seed, the grid size and the duration 
        /// </summary>
        public bool initWithSeed(int s, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nSeed = s;
                m_pTilesOrder = null;

                return true;
            }

            return false;
        }

        public void shuffle(int[] pArray, int nLen)
        {
            int i;
            for (i = nLen - 1; i >= 0; i--)
            {
                int j = random.Next() % (i + 1);
                int v = pArray[i];
                pArray[i] = pArray[j];
                pArray[j] = v;
            }
        }

        public void turnOnTile(ccGridSize pos)
        {
            setTile(pos, originalTile(pos));
        }

        public void turnOffTile(ccGridSize pos)
        {
            ccQuad3 coords = new ccQuad3();

            //memset(coords, 0, sizeof(ccQuad3));
            setTile(pos, coords);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCTurnOffTiles pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCTurnOffTiles)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCTurnOffTiles();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithSeed(m_nSeed, m_sGridSize, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void StartWithTarget(Node pTarget)
        {
            int i;

            base.StartWithTarget(pTarget);

            if (m_nSeed != -1)
            {
                random.Next(m_nSeed);
            }

            m_nTilesCount = m_sGridSize.x * m_sGridSize.y;
            m_pTilesOrder = new int[m_nTilesCount];

            for (i = 0; i < m_nTilesCount; ++i)
            {
                m_pTilesOrder[i] = i;
            }

            shuffle(m_pTilesOrder, m_nTilesCount);
        }

        public override void Update(float time)
        {
            int i, l, t;

            l = (int)(time * (float)m_nTilesCount);

            for (i = 0; i < m_nTilesCount; i++)
            {
                t = m_pTilesOrder[i];
                ccGridSize tilePos = new ccGridSize(t / m_sGridSize.y, t % m_sGridSize.y);

                if (i < l)
                {
                    turnOffTile(tilePos);
                }
                else
                {
                    turnOnTile(tilePos);
                }
            }
        }

        /// <summary>
        /// creates the action with the grid size and the duration
        /// </summary>
        public new static CCTurnOffTiles actionWithSize(ccGridSize size, float d)
        {
            CCTurnOffTiles pAction = new CCTurnOffTiles();
            if (pAction.initWithSize(size, d))
            {
                return pAction;
            }

            return null;
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public static CCTurnOffTiles actionWithSeed(int s, ccGridSize gridSize, float duration)
        {
            CCTurnOffTiles pAction = new CCTurnOffTiles();
            if (pAction.initWithSeed(s, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        Random random = new Random();
        protected int m_nSeed;
        protected int m_nTilesCount;
        protected int[] m_pTilesOrder;
    }
}
