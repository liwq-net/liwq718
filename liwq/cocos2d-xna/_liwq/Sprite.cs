using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace cocos2d
{
    public class Sprite : Node
    {
        public Texture Texture { get; set; }

        protected ccV3F_C4B_T2F_Quad _quad = new ccV3F_C4B_T2F_Quad();
        //protected void _updateQuad()
        //{
        //    //贴图颜色
        //    this._quad.BottomLeft.Colors = ccColor4B.White;
        //    this._quad.BottomRight.Colors = CCColor4B.White;
        //    this._quad.TopLeft.Colors = CCColor4B.White;
        //    this._quad.TopRight.Colors = CCColor4B.White;

        //    //贴图来源坐标
        //    this._quad.TopLeft.TexCoords = new CCTex2F(0, 0);
        //    this._quad.TopRight.TexCoords = new CCTex2F(1, 0);
        //    this._quad.BottomLeft.TexCoords = new CCTex2F(0, 1);
        //    this._quad.BottomRight.TexCoords = new CCTex2F(1, 1);

        //    //贴图目标坐标，如果不等于整个CCNode的大小，变形时会异常
        //    float x = 0;
        //    float y = 0;
        //    float w = this.Width;
        //    float h = this.Height;

        //    this._quad.BottomLeft.Vertices = new CCVertex3F(x, y, 0);
        //    this._quad.BottomRight.Vertices = new CCVertex3F(x + w, y, 0);
        //    this._quad.TopLeft.Vertices = new CCVertex3F(x, y + h, 0);
        //    this._quad.TopRight.Vertices = new CCVertex3F(x + w, y + h, 0);
        //}
        //private void _init(CCTexture2D texture, CCRect rect)
        //{
        //    if (texture == null) throw new ArgumentNullException();
        //    this.Texture = texture;
        //    this.Width = texture.PixelsWide;
        //    this.Height = texture.PixelsHigh;
        //    this.BlendFunc = (this.Texture.HasPremultipliedAlpha == true) ? CCBlendFunc.AlphaBlend : CCBlendFunc.NonPremultiplied;
        //    this._quad = new CCV3F_C4B_T2F_Quad();
        //}

        //public Sprite(CCTexture2D texture, CCRect rect) { this._init(texture, rect); }
        //public Sprite(CCTexture2D texture) { this._init(texture, new CCRect(0, 0, texture.PixelsWide, texture.PixelsHigh)); }
        //public Sprite(string filename) { CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(filename); this._init(texture, new CCRect(0, 0, texture.PixelsWide, texture.PixelsHigh)); }
        //public Sprite(string filename, CCRect rect) { CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(filename); this._init(texture, rect); }
        //public Sprite(CCSpriteFrame spriteFrame) { this._init(spriteFrame.Texture, spriteFrame.TextureRectInPixels); }

        //protected bool _isQuadUpdated;
        //protected override void Draw()
        //{
        //    if (this._isQuadUpdated == false)
        //    {
        //        this._isQuadUpdated = true;
        //        this._updateQuad();
        //    }
        //    CCDrawManager drawManager = Window.DrawManager;
        //    drawManager.BlendFunc(this.BlendFunc);
        //    drawManager.BindTexture(this.Texture);
        //    drawManager.DrawQuad(ref this._quad);
        //}

        public override void draw()
        {
            if (this.Texture != null)
            {
                Application.SharedApplication.BasicEffect.Texture = this.Texture.Texture2D;
                Application.SharedApplication.BasicEffect.TextureEnabled = true;
                //app.BasicEffect.Alpha = (float)this.Opacity / 255.0f;
                Application.SharedApplication.BasicEffect.VertexColorEnabled = true;

                //贴图颜色
                this._quad.bl.colors = new ccColor4B(0xFF, 0xFF, 0xFF, 0xFF);
                this._quad.br.colors = new ccColor4B(0xFF, 0xFF, 0xFF, 0xFF);
                this._quad.tl.colors = new ccColor4B(0xFF, 0xFF, 0xFF, 0xFF);
                this._quad.tr.colors = new ccColor4B(0xFF, 0xFF, 0xFF, 0xFF);

                //贴图来源坐标
                this._quad.tl.texCoords = new ccTex2F(0, 0);
                this._quad.tr.texCoords = new ccTex2F(1, 0);
                this._quad.bl.texCoords = new ccTex2F(0, 1);
                this._quad.br.texCoords = new ccTex2F(1, 1);

                //贴图目标坐标，如果不等于整个CCNode的大小，变形时会异常
                float x = 0;
                float y = 0;
                float w = this.Texture.Width;
                float h = this.Texture.Height;
                this._quad.bl.vertices = new ccVertex3F(x, y, 0);
                this._quad.br.vertices = new ccVertex3F(x + w, y, 0);
                this._quad.tl.vertices = new ccVertex3F(x, y + h, 0);
                this._quad.tr.vertices = new ccVertex3F(x + w, y + h, 0);

                VertexPositionColorTexture[] vertices = this._quad.getVertices(DirectorProjection.Projection3D);
                short[] indexes = this._quad.getIndexes(DirectorProjection.Projection3D);

                BlendState blendBackup = Application.SharedApplication.GraphicsDevice.BlendState;
                {
                    //Application.SharedApplication.BasicEffect.GraphicsDevice.BlendState.ColorSourceBlend = Blend.SourceColor;
                    //Application.SharedApplication.BasicEffect.GraphicsDevice.BlendState.AlphaSourceBlend = Blend.SourceAlpha;
                    //Application.SharedApplication.BasicEffect.GraphicsDevice.BlendState.ColorDestinationBlend = Blend.DestinationColor;
                    //Application.SharedApplication.BasicEffect.GraphicsDevice.BlendState.AlphaDestinationBlend = Blend.DestinationAlpha;

                    //ccBlendFunc m_sBlendFunc = new ccBlendFunc(1, 771);
                    //BlendState bs = new BlendState();
                    //bs.ColorSourceBlend = OGLES.GetXNABlend(m_sBlendFunc.src);
                    //bs.AlphaSourceBlend = OGLES.GetXNABlend(m_sBlendFunc.src);
                    //bs.ColorDestinationBlend = OGLES.GetXNABlend(m_sBlendFunc.dst);
                    //bs.AlphaDestinationBlend = OGLES.GetXNABlend(m_sBlendFunc.dst);
                    //Application.SharedApplication.GraphicsDevice.BlendState = bs;
                    Application.SharedApplication.GraphicsDevice.BlendState.ColorSourceBlend = Blend.One;
                    Application.SharedApplication.GraphicsDevice.BlendState.AlphaSourceBlend = Blend.One;
                    Application.SharedApplication.GraphicsDevice.BlendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
                    Application.SharedApplication.GraphicsDevice.BlendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
                }

                foreach (var pass in Application.SharedApplication.BasicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Application.SharedApplication.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                        PrimitiveType.TriangleList,
                        vertices, 0, 4,
                        indexes, 0, 2
                        );
                }
                Application.SharedApplication.BasicEffect.VertexColorEnabled = false;
                Application.SharedApplication.GraphicsDevice.BlendState = blendBackup;
            }
        }
    }
}