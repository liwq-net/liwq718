using System;

namespace cocos2d
{
    /// <summary>
    /// @brief Base class for CCGrid3D actions.
    /// Grid3D actions can modify a non-tiled grid.
    /// </summary>
    public class CCGrid3DAction : CCGridAction
    {
        /// <summary>
        /// returns the grid
        /// </summary>
        public override CCGridBase getGrid()
        {
            return CCGrid3D.gridWithSize(m_sGridSize);
        }

        /// <summary>
        ///  returns the vertex than belongs to certain position in the grid
        /// </summary>
        public ccVertex3F vertex(ccGridSize pos)
        {
            CCGrid3D g = (CCGrid3D)(Target.Grid);
            return g.vertex(pos);
        }

        /// <summary>
        ///  returns the non-transformed vertex than belongs to certain position in the grid
        /// </summary>
        public ccVertex3F originalVertex(ccGridSize pos)
        {
            CCGrid3D g = (CCGrid3D)Target.Grid;
            return g.originalVertex(pos);
        }

        public ccVertex3F originalVertex(int i, int j)
        {
            CCGrid3D g = (CCGrid3D)Target.Grid;
            return g.originalVertex(i, j);
        }

        /// <summary>
        /// sets a new vertex to a certain position of the grid
        /// </summary>
        public void setVertex(ccGridSize pos, ccVertex3F vertex)
        {
            CCGrid3D g = (CCGrid3D)Target.Grid;
            g.setVertex(pos, vertex);
        }

        public void setVertex(int i, int j, ccVertex3F vertex)
        {
            CCGrid3D g = (CCGrid3D)Target.Grid;
            g.setVertex(i, j, vertex);
        }

        /// <summary>
        /// creates the action with size and duration 
        /// </summary>
        public new static CCGrid3DAction actionWithSize(ccGridSize gridSize, float duration)
        {
            throw new NotImplementedException("win32 is not implemented");
        }
    }
}
