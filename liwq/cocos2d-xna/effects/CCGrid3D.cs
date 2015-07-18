using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using liwq;

namespace cocos2d
{
    /// <summary> 
    /// CCGrid3D is a 3D grid implementation. Each vertex has 3 dimensions: x,y,z
    /// </summary>
    public class CCGrid3D : CCGridBase
    {
        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public ccVertex3F vertex(ccGridSize pos)
        {
            int index = (pos.x * (m_sGridSize.y + 1) + pos.y);
            ccVertex3F[] vertArray = m_pVertices;

            ccVertex3F vert = new ccVertex3F()
            {
                x = vertArray[index].x,
                y = vertArray[index].y,
                z = vertArray[index].z
            };

            return vert;
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public ccVertex3F originalVertex(ccGridSize pos)
        {
            int index = (pos.x * (m_sGridSize.y + 1) + pos.y);
            ccVertex3F[] vertArray = m_pOriginalVertices;

            ccVertex3F vert = new ccVertex3F()
            {
                x = vertArray[index].x,
                y = vertArray[index].y,
                z = vertArray[index].z
            };

            return vert;
        }

        public ccVertex3F originalVertex(int px, int py)
        {
            int index = (px * (m_sGridSize.y + 1) + py);
            ccVertex3F[] vertArray = m_pOriginalVertices;

            ccVertex3F vert = new ccVertex3F()
            {
                x = vertArray[index].x,
                y = vertArray[index].y,
                z = vertArray[index].z
            };

            return vert;
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void setVertex(ccGridSize pos, ccVertex3F vertex)
        {
            int index = pos.x * (m_sGridSize.y + 1) + pos.y;
            ccVertex3F[] vertArray = m_pVertices;
            vertArray[index].x = vertex.x;
            vertArray[index].y = vertex.y;
            vertArray[index].z = vertex.z;
        }

        public void setVertex(int px, int py, ccVertex3F vertex)
        {
            int index = px * (m_sGridSize.y + 1) + py;
            ccVertex3F[] vertArray = m_pVertices;
            vertArray[index].x = vertex.x;
            vertArray[index].y = vertex.y;
            vertArray[index].z = vertex.z;

            //System.Diagnostics.Debug.WriteLine("setVertex: {0},{1} = {2},{3},{4}", px, py, vertex.x, vertex.y, vertex.z);
        }

        public override void blit()
        {
            int n = m_sGridSize.x * m_sGridSize.y;

            //////// Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            //////// Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_TEXTURE_COORD_ARRAY
            //////// Unneeded states: GL_COLOR_ARRAY
            //////glDisableClientState(GL_COLOR_ARRAY);

            //////glVertexPointer(3, GL_FLOAT, 0, m_pVertices);
            //////glTexCoordPointer(2, GL_FLOAT, 0, m_pTexCoordinates);
            //////glDrawElements(GL_TRIANGLES, (GLsizei)n * 6, GL_UNSIGNED_SHORT, m_pIndices);

            //////// restore GL default state
            //////glEnableClientState(GL_COLOR_ARRAY);


            Application app = Application.SharedApplication;
            CCSize size = Director.SharedDirector.DesignSize;

            //app.basicEffect.World = app.worldMatrix * TransformUtils.CGAffineToMatrix(this.nodeToWorldTransform());
            app.BasicEffect.Texture = this.m_pTexture.Texture2D;
            app.BasicEffect.TextureEnabled = true;
            app.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            app.BasicEffect.VertexColorEnabled = true;
            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            //app.GraphicsDevice.RasterizerState = rs;

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
            for (int i = 0; i < (m_sGridSize.x + 1) * (m_sGridSize.y + 1); i++)
            {
                VertexPositionColorTexture vct = new VertexPositionColorTexture();
                vct.Position = new Vector3(m_pVertices[i].x, m_pVertices[i].y, m_pVertices[i].z);
                vct.TextureCoordinate = new Vector2(m_pTexCoordinates[i].X, m_pTexCoordinates[i].Y);
                vct.Color = Color.White;
                vertices.Add(vct);
            }

            short[] indexes = m_pIndices;

            foreach (var pass in app.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices.ToArray(), 0, vertices.Count,
                    indexes, 0, indexes.Length / 3);
            }
        }

        public override void reuse()
        {
            if (m_nReuseGrid > 0)
            {
                Array.Copy(m_pVertices, m_pOriginalVertices, (m_sGridSize.x + 1) * (m_sGridSize.y + 1));
                --m_nReuseGrid;
            }
        }

        public override void calculateVertexPoints()
        {
            float width = (float)m_pTexture.Width;
            float height = (float)m_pTexture.Height;
            float imageH = m_pTexture.Height;

            m_pVertices = new ccVertex3F[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pOriginalVertices = new ccVertex3F[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pTexCoordinates = new CCPoint[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pIndices = new short[m_sGridSize.x * m_sGridSize.y * 6];

            ccVertex3F[] vertArray = m_pVertices;
            CCPoint[] texArray = m_pTexCoordinates;
            short[] idxArray = m_pIndices;

            //int idx = -1;
            for (int x = 0; x < m_sGridSize.x; ++x)
            {
                for (int y = 0; y < m_sGridSize.y; ++y)
                {
                    int idx = (y * m_sGridSize.x) + x;

                    float x1 = x * m_obStep.X;
                    float x2 = x1 + m_obStep.X;
                    float y1 = y * m_obStep.Y;
                    float y2 = y1 + m_obStep.Y;

                    int a = x * (m_sGridSize.y + 1) + y;
                    int b = (x + 1) * (m_sGridSize.y + 1) + y;
                    int c = (x + 1) * (m_sGridSize.y + 1) + (y + 1);
                    int d = x * (m_sGridSize.y + 1) + (y + 1);

                    short[] tempidx = new short[6] { (short)a, (short)d, (short)b, (short)b, (short)d, (short)c };
                    Array.Copy(tempidx, 0, idxArray, 6 * idx, tempidx.Length);

                    int[] l1 = new int[4] { a, b, c, d };
                    ccVertex3F e = new ccVertex3F(x1, y1, 0);
                    ccVertex3F f = new ccVertex3F(x2, y1, 0);
                    ccVertex3F g = new ccVertex3F(x2, y2, 0);
                    ccVertex3F h = new ccVertex3F(x1, y2, 0);

                    ccVertex3F[] l2 = new ccVertex3F[4] { e, f, g, h };

                    int[] tex1 = new int[4] { a, b, c, d };
                    CCPoint[] tex2 = new CCPoint[4] 
                    {
                    new CCPoint(x1, y1), 
                    new CCPoint(x2, y1), 
                    new CCPoint(x2, y2),
                    new CCPoint(x1, y2)
                    };

                    for (int i = 0; i < 4; ++i)
                    {
                        vertArray[l1[i]] = new ccVertex3F();
                        vertArray[l1[i]].x = l2[i].x;
                        vertArray[l1[i]].y = l2[i].y;
                        vertArray[l1[i]].z = l2[i].z;

                        texArray[tex1[i]] = new CCPoint();
                        texArray[tex1[i]].X = tex2[i].X / width;
                        if (m_bIsTextureFlipped)
                        {
                            texArray[tex1[i]].Y = tex2[i].Y / height;
                        }
                        else
                        {
                            texArray[tex1[i]].Y = (imageH - tex2[i].Y) / height;
                        }
                    }
                }
            }

            Array.Copy(m_pVertices, m_pOriginalVertices, (m_sGridSize.x + 1) * (m_sGridSize.y + 1));
        }

        public new static CCGrid3D gridWithSize(ccGridSize gridSize, Texture pTexture, bool bFlipped)
        {
            CCGrid3D pRet = new CCGrid3D();

            if (pRet.initWithSize(gridSize, pTexture, bFlipped))
            {
                return pRet;
            }

            return null;
        }

        public new static CCGrid3D gridWithSize(ccGridSize gridSize)
        {
            CCGrid3D pRet = new CCGrid3D();

            if (pRet.initWithSize(gridSize))
            {
                return pRet;
            }

            return null;
        }

        protected CCPoint[] m_pTexCoordinates;
        protected ccVertex3F[] m_pVertices;
        protected ccVertex3F[] m_pOriginalVertices;
        protected short[] m_pIndices;
    }
}
