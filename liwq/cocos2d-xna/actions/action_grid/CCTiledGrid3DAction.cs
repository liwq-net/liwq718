
namespace cocos2d
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        /// <summary>
        /// returns the tile that belongs to a certain position of the grid
        /// </summary>
        public virtual ccQuad3 tile(ccGridSize pos)
        {
            return (tile(pos.x, pos.y));
        }
        /// <summary>
        /// returns the tile that belongs to a certain position of the grid
        /// </summary>
        public virtual ccQuad3 tile(int x, int y)
        {
            CCTiledGrid3D g = (CCTiledGrid3D)Target.Grid;
            if (g != null)
            {
                return g.tile(x, y);
            }
            return (null);
        }

        /// <summary>
        /// returns the non-transformed tile that belongs to a certain position of the grid. This 
        /// can return null if the scene is not setup and the update pipeline calls this method
        /// during an action update.
        /// </summary>
        public virtual ccQuad3 originalTile(ccGridSize pos)
        {
            return (originalTile(pos.x, pos.y));
        }

        /// <summary>
        /// returns the non-transformed tile that belongs to a certain position of the grid. This 
        /// can return null if the scene is not setup and the update pipeline calls this method
        /// during an action update.
        /// </summary>
        public virtual ccQuad3 originalTile(int x, int y)
        {
            CCTiledGrid3D g = (CCTiledGrid3D)Target.Grid;
            if (g != null)
            {
                return g.originalTile(x, y);
            }
            return (null);
        }

        /// <summary>
        /// sets a new tile to a certain position of the grid
        /// </summary>
        public virtual void setTile(ccGridSize pos, ccQuad3 coords)
        {
            setTile(pos.x, pos.y, coords);
        }

        /// <summary>
        /// sets a new tile to a certain position of the grid
        /// </summary>
        public virtual void setTile(int x, int y, ccQuad3 coords)
        {
            if (coords == null)
            {
                return;
            }
            CCTiledGrid3D g = (CCTiledGrid3D)Target.Grid;
            if (g != null)
            {
                g.setTile(x, y, coords);
            }
        }

        /// <summary>
        /// returns the grid using CCTileGrid3D.gridWithSize()
        /// </summary>
        /// <see cref="CCTileGrid3D"/>
        public override CCGridBase getGrid()
        {
            return CCTiledGrid3D.gridWithSize(m_sGridSize);
        }

        /// <summary>
        /// creates the action with size and duration. See CCGridAction.initWithSize().
        /// </summary>
        /// <seealso cref="CCGridAction"/>
        public static CCTiledGrid3DAction actionWithSize(ccGridSize gridSize, float duration)
        {
            CCTiledGrid3DAction action = new CCTiledGrid3DAction();
            action.initWithSize(gridSize, duration);
            return (action);
        }
    }
}
