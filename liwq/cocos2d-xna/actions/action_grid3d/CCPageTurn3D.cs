using System;

namespace cocos2d
{
    /// <summary>
    /// @brief This action simulates a page turn from the bottom right hand corner of the screen.
    /// It's not much use by itself but is used by the PageTurnTransition.
    /// Based on an original paper by L Hong et al.
    /// http://www.parc.com/publication/1638/turning-pages-of-3d-electronic-books.html
    /// @since v0.8.2
    /// </summary>
    public class CCPageTurn3D : CCGrid3DAction
    {
        /// <summary>
        /// Update each tick
        /// Time is the percentage of the way through the duration
        /// </summary>
        public override void Update(float time)
        {
            float tt = Math.Max(0, time - 0.25f);
            float deltaAy = (tt * tt * 500);
            float ay = -100 - deltaAy;

            float deltaTheta = -(float)Math.PI / 2 * (float)Math.Sqrt(time);
            float theta = /*0.01f */ +(float)Math.PI / 2 + deltaTheta;

            float sinTheta = (float)Math.Sin(theta);
            float cosTheta = (float)Math.Cos(theta);

            for (int i = 0; i <= m_sGridSize.x; ++i)
            {
                for (int j = 0; j <= m_sGridSize.y; ++j)
                {
                    // Get original vertex
                    ccVertex3F p = originalVertex(new ccGridSize(i, j));

                    float R = (float)Math.Sqrt((p.x * p.x) + ((p.y - ay) * (p.y - ay)));
                    float r = R * sinTheta;
                    float alpha = (float)Math.Asin(p.x / R);
                    float beta = alpha / sinTheta;
                    float cosBeta = (float)Math.Cos(beta);

                    // If beta > PI then we've wrapped around the cone
                    // Reduce the radius to stop these points interfering with others
                    if (beta <= Math.PI)
                    {
                        p.x = (r * (float)Math.Sin(beta));
                    }
                    else
                    {
                        // Force X = 0 to stop wrapped
                        // points
                        p.x = 0;
                    }

                    p.y = (R + ay - (r * (1 - cosBeta) * sinTheta));

                    // We scale z here to avoid the animation being
                    // too much bigger than the screen due to perspectve transform
                    p.z = (r * (1 - cosBeta) * cosTheta) / 7;// "100" didn't work for

                    //	Stop z coord from dropping beneath underlying page in a transition
                    // issue #751
                    if (p.z < 0.5f)
                    {
                        p.z = 0.5f;
                    }

                    // Set new coords
                    setVertex(new ccGridSize(i, j), p);
                }
            }
        }

        /// <summary>
        /// create the action
        /// </summary>
        public static CCPageTurn3D actionWithSize(ccGridSize gridSize, float time)
        {
            CCPageTurn3D pAction = new CCPageTurn3D();

            if (pAction.initWithSize(gridSize, time))
            {
                return pAction;
            }

            return null;
        }
    }
}
