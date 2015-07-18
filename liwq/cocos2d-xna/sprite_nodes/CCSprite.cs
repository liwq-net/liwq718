using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using liwq;
namespace cocos2d
{
    /// <summary>
    /// Whether or not an CCSprite will rotate, scale or translate with it's parent.
    /// Useful in health bars, when you want that the health bar translates with it's parent but you don't
    /// want it to rotate with its parent.
    /// @since v0.99.0
    /// </summary>
    public enum ccHonorParentTransform
    {
        //! Translate with it's parent
        CC_HONOR_PARENT_TRANSFORM_TRANSLATE = 1 << 0,
        //! Rotate with it's parent
        CC_HONOR_PARENT_TRANSFORM_ROTATE = 1 << 1,
        //! Scale with it's parent
        CC_HONOR_PARENT_TRANSFORM_SCALE = 1 << 2,
        //! Skew with it's parent
        CC_HONOR_PARENT_TRANSFORM_SKEW = 1 << 3,

        //! All possible transformation enabled. Default value.
        CC_HONOR_PARENT_TRANSFORM_ALL = CC_HONOR_PARENT_TRANSFORM_TRANSLATE | CC_HONOR_PARENT_TRANSFORM_ROTATE | CC_HONOR_PARENT_TRANSFORM_SCALE | CC_HONOR_PARENT_TRANSFORM_SKEW,
    }

    /** CCSprite is a 2d image ( http://en.wikipedia.org/wiki/Sprite_(computer_graphics) )
    *
    * CCSprite can be created with an image, or with a sub-rectangle of an image.
    *
    * If the parent or any of its ancestors is a CCSpriteBatchNode then the following features/limitations are valid
    *	- Features when the parent is a CCBatchNode:
    *		- MUCH faster rendering, specially if the CCSpriteBatchNode has many children. All the children will be drawn in a single batch.
    *
    *	- Limitations
    *		- Camera is not supported yet (eg: CCOrbitCamera action doesn't work)
    *		- GridBase actions are not supported (eg: CCLens, CCRipple, CCTwirl)
    *		- The Alias/Antialias property belongs to CCSpriteBatchNode, so you can't individually set the aliased property.
    *		- The Blending function property belongs to CCSpriteBatchNode, so you can't individually set the blending function property.
    *		- Parallax scroller is not supported, but can be simulated with a "proxy" sprite.
    *
    *  If the parent is an standard CCNode, then CCSprite behaves like any other CCNode:
    *    - It supports blending functions
    *    - It supports aliasing / antialiasing
    *    - But the rendering will be slower: 1 draw per children.
    *
    * The default anchorPoint in CCSprite is (0.5, 0.5).
    */
    public class CCSprite : Node, ICCTextureProtocol, ICCRGBAProtocol
    {
        #region Properties

        private byte m_nOpacity;
        /// <summary>
        /// Opacity: conforms to CCRGBAProtocol protocol
        /// </summary>
        public byte Opacity
        {
            get { return m_nOpacity; }
            set
            {
                m_nOpacity = value;

                // special opacity for premultiplied textures
                if (m_bOpacityModifyRGB)
                {
                    Color = m_sColorUnmodified;
                }

                updateColor();
            }
        }

        private ccColor3B m_sColor = new ccColor3B();
        /// <summary>
        /// Color: conforms with CCRGBAProtocol protocol
        /// </summary>
        public ccColor3B Color
        {
            get
            {
                if (m_bOpacityModifyRGB)
                {
                    return m_sColorUnmodified;
                }

                return m_sColor;
            }
            set
            {
                m_sColor = new ccColor3B(value.r, value.g, value.b);
                m_sColorUnmodified = new ccColor3B(value.r, value.g, value.b);

                if (m_bOpacityModifyRGB)
                {
                    m_sColor.r = (byte)(value.r * m_nOpacity / 255);
                    m_sColor.g = (byte)(value.g * m_nOpacity / 255);
                    m_sColor.b = (byte)(value.b * m_nOpacity / 255);
                }

                updateColor();
            }
        }

        /// <summary>
        /// opacity: conforms to CCRGBAProtocol protocol
        /// </summary>
        public virtual bool IsOpacityModifyRGB
        {
            get
            {
                return m_bOpacityModifyRGB;
            }
            set
            {
                ccColor3B oldColor = m_sColor;
                m_bOpacityModifyRGB = value;
                m_sColor = oldColor;
            }
        }

        private bool m_bDirty;
        /// <summary>
        /// whether or not the Sprite needs to be updated in the Atlas
        /// </summary>
        public bool dirty
        {
            get
            {
                return m_bDirty;
            }
            set
            {
                m_bDirty = value;
            }
        }

        private ccV3F_C4B_T2F_Quad m_sQuad = new ccV3F_C4B_T2F_Quad();
        /// <summary>
        /// get the quad (tex coords, vertex coords and color) information
        /// </summary>
        public ccV3F_C4B_T2F_Quad quad
        {
            // read only
            get
            {
                return m_sQuad;
            }
        }

        private bool m_bRectRotated;
        /// <summary>
        /// returns whether or not the texture rectangle is rotated
        /// </summary>
        public bool rectRotated
        {
            get { return m_bRectRotated; }
        }

        private int m_uAtlasIndex;
        /// <summary>
        /// The index used on the TextureAtlas. Don't modify this value unless you know what you are doing
        /// </summary>
        public int atlasIndex
        {
            get
            {
                return m_uAtlasIndex;
            }
            set
            {
                m_uAtlasIndex = value;
            }
        }

        private CCRect m_obTextureRect;
        /// <summary>
        /// returns the rect of the CCSprite in points
        /// </summary>
        public CCRect textureRect
        {
            // read only
            get { return m_obTextureRect; }
        }

        /// <summary>
        /// whether or not the Sprite is rendered using a CCSpriteBatchNode
        /// </summary>
        private bool m_bUseBatchNode;
        public bool IsUseBatchNode
        {
            get
            {
                return m_bUseBatchNode;
            }
            set
            {
                m_bUseBatchNode = value;
            }
        }

        /** conforms to CCTextureProtocol protocol */
        private ccBlendFunc m_sBlendFunc = new ccBlendFunc();
        public ccBlendFunc BlendFunc
        {
            get { return m_sBlendFunc; }
            set { m_sBlendFunc = value; }
        }

        protected CCTextureAtlas m_pobTextureAtlas;
        protected CCSpriteBatchNode m_pobBatchNode;

        /** weak reference of the CCTextureAtlas used when the sprite is rendered using a CCSpriteBatchNode */
        ///@todo add m_pobTextureAtlas
        ///

        /** weak reference to the CCSpriteBatchNode that renders the CCSprite */
        ///@todo add m_pobBatchNode
        ///

        private ccHonorParentTransform m_eHonorParentTransform;
        /// <summary>
        /// whether or not to transform according to its parent transfomrations.
        /// Useful for health bars. eg: Don't rotate the health bar, even if the parent rotates.
        /// IMPORTANT: Only valid if it is rendered using an CCSpriteBatchNode.
        /// @since v0.99.0
        /// </summary>
        public ccHonorParentTransform honorParentTransform
        {
            get { return m_eHonorParentTransform; }
            set { m_eHonorParentTransform = value; }
        }

        /// <summary>
        /// Gets offset position in pixels of the sprite in points. Calculated automatically by editors like Zwoptex.
        /// @since v0.99.0
        /// </summary>
        private CCPoint m_obOffsetPositionInPixels;
        public CCPoint offsetPositionInPixels
        {
            // read only
            get { return m_obOffsetPositionInPixels; }
        }

        #endregion

        public CCSprite()
        {
            m_obOffsetPositionInPixels = new CCPoint();
            m_obRectInPixels = new CCRect();
            m_obUnflippedOffsetPositionFromCenter = new CCPoint();
        }

        #region spriteWith:CCTexture2D,CCSpriteFrame,File,BatchNode

        /// <summary>
        /// Creates an sprite with a texture.
        /// The rect used will be the size of the texture.
        /// The offset will be (0,0).
        /// </summary>
        public static CCSprite spriteWithTexture(Texture texture)
        {
            CCSprite sprite = new CCSprite();
            if (sprite != null && sprite.initWithTexture(texture))
            {
                return sprite;
            }

            sprite = null;
            return null;
        }

        /// <summary>
        /// Creates an sprite with a texture and a rect.
        /// The offset will be (0,0).
        /// </summary>
        public static CCSprite spriteWithTexture(Texture texture, CCRect rect)
        {
            CCSprite sprite = new CCSprite();
            if (sprite != null && sprite.initWithTexture(texture, rect))
            {
                return sprite;
            }

            sprite = null;
            return null;
        }

        /// <summary>
        /// Creates an sprite with a texture, a rect and offset. 
        /// </summary>
        public static CCSprite spriteWithTexture(Texture texture, CCRect rect, CCPoint offset)
        {
            // not implement
            return null;
        }

        /// <summary>
        /// Creates an sprite with an sprite frame.
        /// </summary>
        public static CCSprite spriteWithSpriteFrame(CCSpriteFrame pSpriteFrame)
        {
            CCSprite pobSprite = new CCSprite();
            if (pobSprite != null && pobSprite.initWithSpriteFrame(pSpriteFrame))
            {
                return pobSprite;
            }
            return null;
        }

        /// <summary>
        /// Creates an sprite with an sprite frame name.
        /// An CCSpriteFrame will be fetched from the CCSpriteFrameCache by name.
        /// If the CCSpriteFrame doesn't exist it will raise an exception.
        /// @since v0.9
        /// </summary>
        public static CCSprite spriteWithSpriteFrameName(string pszSpriteFrameName)
        {
            //CCSpriteFrame pFrame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(pszSpriteFrameName);

            //string msg = string.Format("Invalid spriteFrameName: {0}", pszSpriteFrameName);
            //Debug.Assert(pFrame != null, msg);
            //return spriteWithSpriteFrame(pFrame);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an sprite with an image filename.
        /// The rect used will be the size of the image.
        /// The offset will be (0,0).
        /// </summary>
        public static CCSprite spriteWithFile(string fileName)
        {
            CCSprite sprite = new CCSprite();
            if (sprite.initWithFile(fileName))
            {
                return sprite;
            }

            sprite = null;
            return sprite;
        }

        /// <summary>
        /// Creates an sprite with an image filename and a rect.
        /// The offset will be (0,0).
        /// </summary>
        public static CCSprite spriteWithFile(string fileName, CCRect rect)
        {
            CCSprite sprite = new CCSprite();

            if (sprite.initWithFile(fileName, rect))
            {
                return sprite;
            }

            sprite = null;
            return sprite;
        }

        /// <summary>
        /// Creates an sprite with an CCBatchNode and a rect 
        /// </summary>
        public static CCSprite spriteWithBatchNode(CCSpriteBatchNode batchNode, CCRect rect)
        {
            CCSprite pobSprite = new CCSprite();
            if (pobSprite.initWithBatchNode(batchNode, rect))
            {
                return pobSprite;
            }

            return null;
        }

        #endregion

        public virtual bool init()
        {
            m_bDirty = m_bRecursiveDirty = false;

            // by default use "Self Render".
            // if the sprite is added to an batchnode, then it will automatically switch to "SpriteSheet Render"
            useSelfRender();

            m_bOpacityModifyRGB = true;
            m_nOpacity = 255;
            m_sColor = new ccColor3B(255, 255, 255);
            m_sColorUnmodified = new ccColor3B(255, 255, 255);

            m_sBlendFunc = new ccBlendFunc();
            m_sBlendFunc.src = ccMacros.CC_BLEND_SRC;
            m_sBlendFunc.dst = ccMacros.CC_BLEND_DST;


            // update texture (calls updateBlendFunc)
            Texture = null;

            // clean the Quad
            m_sQuad = new ccV3F_C4B_T2F_Quad();

            m_bFlipX = m_bFlipY = false;

            // default transform anchor: center
            AnchorPoint = (CCPointExtension.ccp(0.5f, 0.5f));

            // zwoptex default values
            m_obOffsetPositionInPixels = new CCPoint();

            m_eHonorParentTransform = ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ALL;
            m_bHasChildren = false;

            // Atlas: Color
            ccColor4B tmpColor = new ccColor4B(255, 255, 255, 255);
            m_sQuad.bl.colors = tmpColor;
            m_sQuad.br.colors = tmpColor;
            m_sQuad.tl.colors = tmpColor;
            m_sQuad.tr.colors = tmpColor;

            // Atlas: Vertex

            // updated in "useSelfRender"

            // Atlas: TexCoords

            setTextureRectInPixels(new CCRect(), false, new CCSize());

            return true;
        }

        public override void draw()
        {
            base.draw();

            Debug.Assert(!m_bUsesBatchNode);

            Application app = Application.SharedApplication;
            CCSize size = Director.SharedDirector.DesignSize;

            bool newBlend = m_sBlendFunc.src != ccMacros.CC_BLEND_SRC || m_sBlendFunc.dst != ccMacros.CC_BLEND_DST;
            BlendState origin = app.GraphicsDevice.BlendState;
            if (newBlend)
            {
                BlendState bs = new BlendState();

                bs.ColorSourceBlend = OGLES.GetXNABlend(m_sBlendFunc.src);
                bs.AlphaSourceBlend = OGLES.GetXNABlend(m_sBlendFunc.src);
                bs.ColorDestinationBlend = OGLES.GetXNABlend(m_sBlendFunc.dst);
                bs.AlphaDestinationBlend = OGLES.GetXNABlend(m_sBlendFunc.dst);

                app.GraphicsDevice.BlendState = bs;
                //glBlendFunc(m_sBlendFunc.src, m_sBlendFunc.dst);
            }

            if (this.Texture != null)
            {
                app.BasicEffect.Texture = this.Texture.Texture2D;
                app.BasicEffect.TextureEnabled = true;
                if (!newBlend) // @@ TotallyEvil
                {
                    app.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                }
                app.BasicEffect.Alpha = (float)this.Opacity / 255.0f;
                app.BasicEffect.VertexColorEnabled = true;
            }

            VertexPositionColorTexture[] vertices = this.m_sQuad.getVertices(DirectorProjection.Projection3D);
            short[] indexes = this.m_sQuad.getIndexes(DirectorProjection.Projection3D);

            //VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
            //    {
            //        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            //        new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Color, 0),
            //        new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            //    });

            foreach (var pass in app.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices, 0, 4,
                    indexes, 0, 2);
            }

            app.BasicEffect.VertexColorEnabled = false;

            if (newBlend)
            {
                BlendState bs = new BlendState();

                bs.ColorSourceBlend = OGLES.GetXNABlend(ccMacros.CC_BLEND_SRC);
                bs.AlphaSourceBlend = OGLES.GetXNABlend(ccMacros.CC_BLEND_SRC);
                bs.ColorDestinationBlend = OGLES.GetXNABlend(ccMacros.CC_BLEND_DST);
                bs.AlphaDestinationBlend = OGLES.GetXNABlend(ccMacros.CC_BLEND_DST);

                app.GraphicsDevice.BlendState = bs;

                //glBlendFunc(m_sBlendFunc.src, m_sBlendFunc.dst);
            }
        }

        #region add,remove child

        public override void RemoveChild(Node child, bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                m_pobBatchNode.removeSpriteFromAtlas((CCSprite)(child));
            }

            base.RemoveChild(child, cleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                foreach (Node node in Children)
                {
                    m_pobBatchNode.removeSpriteFromAtlas((CCSprite)(node));
                }
            }

            base.RemoveAllChildrenWithCleanup(cleanup);

            m_bHasChildren = false;
        }

        public override void ReorderChild(Node child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(Children.Contains(child));

            if (zOrder == child.ZOrder)
            {
                return;
            }

            if (m_bUseBatchNode)
            {
                // XXX: Instead of removing/adding, it is more efficient to reorder manually
                RemoveChild(child, false);
                addChild(child, zOrder);
            }
            else
            {
                base.ReorderChild(child, zOrder);
            }
        }

        public override void addChild(Node child)
        {
            base.addChild(child);
        }

        public override void addChild(Node child, int zOrder)
        {
            base.addChild(child, zOrder);
        }

        public override void AddChild(Node child, int zOrder, int tag)
        {
            Debug.Assert(child != null);

            base.AddChild(child, zOrder, tag);

            if (m_bUseBatchNode)
            {
                //Debug.Assert(((CCSprite)child).Texture.Name == m_pobTextureAtlas.Texture.Name);
                int index = m_pobBatchNode.atlasIndexForChild((CCSprite)child, zOrder);
                m_pobBatchNode.insertChild((CCSprite)child, index);
            }

            m_bHasChildren = true;
        }

        #endregion

        #region override prpperty

        public virtual void setDirtyRecursively(bool bValue)
        {
            m_bDirty = m_bRecursiveDirty = bValue;
            // recursively set dirty
            if (m_bHasChildren)
            {
                foreach (Node child in Children)
                {
                    ((CCSprite)child).setDirtyRecursively(true);
                }
            }
        }

        private void SET_DIRTY_RECURSIVELY()
        {
            if (m_bUseBatchNode)
            {
                m_bDirty = m_bRecursiveDirty = true;
                if (m_bHasChildren)
                {
                    setDirtyRecursively(true);
                }
            }
        }

        public override CCPoint Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override CCPoint PositionInPixels
        {
            get
            {
                return base.PositionInPixels;
            }
            set
            {
                base.PositionInPixels = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                base.Rotation = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float SkewX
        {
            get
            {
                return base.SkewX;
            }
            set
            {
                base.SkewX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float SkewY
        {
            get
            {
                return base.SkewY;
            }
            set
            {
                base.SkewY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float ScaleX
        {
            get
            {
                return base.ScaleX;
            }
            set
            {
                base.ScaleX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float ScaleY
        {
            get
            {
                return base.ScaleY;
            }
            set
            {
                base.ScaleY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float VertexZ
        {
            get
            {
                return base.VertexZ;
            }
            set
            {
                base.VertexZ = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override CCPoint AnchorPoint
        {
            get
            {
                return base.AnchorPoint;
            }
            set
            {
                base.AnchorPoint = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool IsRelativeAnchorPoint
        {
            get
            {
                return base.IsRelativeAnchorPoint;
            }
            set
            {
                base.IsRelativeAnchorPoint = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        #endregion

        /// <summary>
        /// whether or not the sprite is flipped horizontally. 
        /// It only flips the texture of the sprite, and not the texture of the sprite's children.
        /// Also, flipping the texture doesn't alter the anchorPoint.
        /// If you want to flip the anchorPoint too, and/or to flip the children too use:
        /// sprite->setScaleX(sprite->getScaleX() * -1);
        /// </summary>
        public bool IsFlipX
        {
            set
            {
                if (m_bFlipX != value)
                {
                    m_bFlipX = value;
                    setTextureRectInPixels(m_obRectInPixels, m_bRectRotated, _contentSizeInPixels);
                }
            }
            get { return m_bFlipX; }
        }

        /// <summary>
        /// whether or not the sprite is flipped vertically.
        /// It only flips the texture of the sprite, and not the texture of the sprite's children.
        /// Also, flipping the texture doesn't alter the anchorPoint.
        /// If you want to flip the anchorPoint too, and/or to flip the children too use:
        /// sprite->setScaleY(sprite->getScaleY() * -1);
        /// </summary>
        public bool IsFlipY
        {
            set
            {
                if (m_bFlipY != value)
                {
                    m_bFlipY = value;
                    setTextureRectInPixels(m_obRectInPixels, m_bRectRotated, _contentSizeInPixels);
                }
            }
            get { return m_bFlipY; }
        }

        void updateColor()
        {
            m_sQuad.bl.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.br.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.tl.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.tr.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);

            // renders using Sprite Manager
            if (m_bUseBatchNode)
            {
                if (m_uAtlasIndex != ccMacros.CCSpriteIndexNotInitialized)
                {
                    m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
                }
                else
                {
                    // no need to set it recursively
                    // update dirty_, don't update recursiveDirty_
                    m_bDirty = true;
                }
            }

            // self render
            // do nothing
        }

        // CCTextureProtocol
        public virtual Texture Texture
        {
            set
            {
                Debug.Assert(!m_bUseBatchNode, "CCSprite: setTexture doesn't work when the sprite is rendered using a CCSpriteBatchNode");

                m_pobTexture = value;

                updateBlendFunc();
            }
            get { return m_pobTexture; }
        }

        #region initWith: Texture,File,SpriteFrame,BatchNode

        /// <summary>
        /// Initializes an sprite with a texture.
        /// The rect used will be the size of the texture.
        /// The offset will be (0,0).
        /// </summary>
        public bool initWithTexture(Texture texture)
        {
            if (texture == null)
            {
                throw (new ArgumentNullException("texture", "Texture cannot be null"));
            }

            CCRect rect = new CCRect();
            rect.Size = texture.ContentSize;

            return initWithTexture(texture, rect);
        }

        /// <summary>
        /// Initializes an sprite with a texture and a rect.
        /// The offset will be (0,0).
        /// </summary>
        public bool initWithTexture(Texture texture, CCRect rect)
        {
            if (texture == null)
            {
                throw (new ArgumentNullException("texture", "Texture cannot be null"));
            }
            init();
            Texture = texture;
            setTextureRect(rect);

            return true;
        }

        /// <summary>
        /// Initializes an sprite with an sprite frame.
        /// </summary>
        public bool initWithSpriteFrame(CCSpriteFrame pSpriteFrame)
        {
            if (pSpriteFrame == null)
            {
                throw (new ArgumentNullException("pSpriteFrame", "SpriteFrame cannot be null"));
            }

            bool bRet = initWithTexture(pSpriteFrame.Texture, pSpriteFrame.Rect);
            DisplayFrame = pSpriteFrame;

            return bRet;
        }

        /// <summary>
        /// Initializes an sprite with an sprite frame name.
        /// An CCSpriteFrame will be fetched from the CCSpriteFrameCache by name.
        /// If the CCSpriteFrame doesn't exist it will raise an exception.
        /// @since v0.9
        /// </summary>
        public bool initWithSpriteFrameName(string spriteFrameName)
        {
            //if (spriteFrameName == null)
            //{
            //    throw (new ArgumentNullException("spriteFrameName", "spriteFrameName cannot be null"));
            //}

            //CCSpriteFrame pFrame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(spriteFrameName);
            //return initWithSpriteFrame(pFrame);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes an sprite with an image filename.
        /// The rect used will be the size of the image.
        /// The offset will be (0,0).
        /// </summary>
        public bool initWithFile(string fileName)
        {
            Debug.Assert(null != fileName, "fileName is null");

            Texture textureFromFile = CCTextureCache.sharedTextureCache().addImage(fileName);

            if (null != textureFromFile)
            {
                CCRect rect = new CCRect();
                rect.Size = textureFromFile.ContentSize;
                return initWithTexture(textureFromFile, rect);
            }

            return false;
        }

        /// <summary>
        /// Initializes an sprite with an image filename, and a rect.
        /// The offset will be (0,0).
        /// </summary>
        public bool initWithFile(string fileName, CCRect rect)
        {
            Debug.Assert(fileName != null);

            Texture pTexture = CCTextureCache.sharedTextureCache().addImage(fileName);
            if (pTexture != null)
            {
                return initWithTexture(pTexture, rect);
            }

            // don't release here.
            // when load texture failed, it's better to get a "transparent" sprite then a crashed program
            // this->release(); 
            return false;
        }

        /// <summary>
        /// Initializes an sprite with an CCSpriteBatchNode and a rect in points
        /// </summary>
        public bool initWithBatchNode(CCSpriteBatchNode batchNode, CCRect rect)
        {
            if (initWithTexture(batchNode.Texture, rect))
            {
                useBatchNode(batchNode);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes an sprite with an CCSpriteBatchNode and a rect in pixels
        /// @since v0.99.5
        /// </summary>
        public bool initWithBatchNodeRectInPixels(CCSpriteBatchNode batchNode, CCRect rect)
        {
            if (initWithTexture(batchNode.Texture))
            {
                setTextureRectInPixels(rect, false, rect.Size);
                useBatchNode(batchNode);
                return true;
            }
            return false;
        }

        #endregion

        // BatchNode methods

        /// <summary>
        /// updates the quad according the the rotation, position, scale values. 
        /// </summary>
        public void updateTransform()
        {
            Debug.Assert(m_bUsesBatchNode != null);

            // optimization. Quick return if not dirty
            if (!m_bDirty)
            {
                return;
            }

            CCAffineTransform matrix;

            // Optimization: if it is not visible, then do nothing
            if (!Visible)
            {
                m_sQuad.br.vertices = m_sQuad.tl.vertices = m_sQuad.tr.vertices = m_sQuad.bl.vertices = new ccVertex3F(0, 0, 0);
                m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
                //m_bDirty = m_bRecursiveDirty = false;
                return;
            }

            // Optimization: If parent is batchnode, or parent is nil
            // build Affine transform manually
            if (Parent == null || Parent == m_pobBatchNode)
            {
                float radians = -CCUtils.CC_DEGREES_TO_RADIANS(_rotation);
                float c = (float)Math.Cos(radians);
                float s = (float)Math.Sin(radians);

                matrix = CCAffineTransform.CCAffineTransformMake(c * _scaleX, s * _scaleX,
                    -s * _scaleY, c * _scaleY,
                    _positionInPixels.X, _positionInPixels.Y);
                if (_skewX > 0 || _skewY > 0)
                {
                    CCAffineTransform skewMatrix = CCAffineTransform.CCAffineTransformMake(1.0f, (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(_skewY)),
                        (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(_skewX)), 1.0f,
                        0.0f, 0.0f);
                    matrix = CCAffineTransform.CCAffineTransformConcat(skewMatrix, matrix);
                }
                matrix = CCAffineTransform.CCAffineTransformTranslate(matrix, -AnchorPointInPixels.X, -AnchorPointInPixels.Y);
            }
            else // parent_ != batchNode_ 
            {
                // else do affine transformation according to the HonorParentTransform
                matrix = CCAffineTransform.CCAffineTransformMakeIdentity();
                ccHonorParentTransform prevHonor = ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ALL;

                Node p = this;
                while (p != null && p is CCSprite && p != m_pobBatchNode)
                {
                    // Might happen. Issue #1053
                    // how to implement, we can not use dynamic
                    // CCAssert( [p isKindOfClass:[CCSprite class]], @"CCSprite should be a CCSprite subclass. Probably you initialized an sprite with a batchnode, but you didn't add it to the batch node." );
                    transformValues_ tv = new transformValues_();
                    ((CCSprite)p).getTransformValues(tv);

                    // If any of the parents are not visible, then don't draw this node
                    if (!tv.visible)
                    {
                        m_sQuad.br.vertices = m_sQuad.tl.vertices = m_sQuad.tr.vertices = m_sQuad.bl.vertices = new ccVertex3F(0, 0, 0);
                        m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
                        m_bDirty = m_bRecursiveDirty = false;

                        return;
                    }

                    CCAffineTransform newMatrix = CCAffineTransform.CCAffineTransformMakeIdentity();

                    // 2nd: Translate, Skew, Rotate, Scale
                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_TRANSLATE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformTranslate(newMatrix, tv.pos.X, tv.pos.Y);
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ROTATE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformRotate(newMatrix, -CCUtils.CC_DEGREES_TO_RADIANS(tv.rotation));
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SKEW != 0)
                    {
                        CCAffineTransform skew = CCAffineTransform.CCAffineTransformMake(1.0f,
                            (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(tv.skew.Y)),
                            (float)Math.Tan(CCUtils.CC_DEGREES_TO_RADIANS(tv.skew.X)), 1.0f, 0.0f, 0.0f);
                        // apply the skew to the transform
                        newMatrix = CCAffineTransform.CCAffineTransformConcat(skew, newMatrix);
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SCALE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformScale(newMatrix, tv.scale.X, tv.scale.Y);
                    }

                    // 3rd: Translate anchor point
                    newMatrix = CCAffineTransform.CCAffineTransformTranslate(newMatrix, -tv.ap.X, -tv.ap.Y);

                    // 4th: Matrix multiplication
                    matrix = CCAffineTransform.CCAffineTransformConcat(matrix, newMatrix);

                    prevHonor = ((CCSprite)p).honorParentTransform;

                    p = p.Parent;
                }
            }

            //
            // calculate the Quad based on the Affine Matrix
            //
            CCSize size = m_obRectInPixels.Size;

            float x1 = m_obOffsetPositionInPixels.X;
            float y1 = m_obOffsetPositionInPixels.Y;

            float x2 = x1 + size.Width;
            float y2 = y1 + size.Height;
            float x = matrix.tx;
            float y = matrix.ty;

            float cr = matrix.a;
            float sr = matrix.b;
            float cr2 = matrix.d;
            float sr2 = -matrix.c;
            float ax = x1 * cr - y1 * sr2 + x;
            float ay = x1 * sr + y1 * cr2 + y;

            float bx = x2 * cr - y1 * sr2 + x;
            float by = x2 * sr + y1 * cr2 + y;

            float cx = x2 * cr - y2 * sr2 + x;
            float cy = x2 * sr + y2 * cr2 + y;

            float dx = x1 * cr - y2 * sr2 + x;
            float dy = x1 * sr + y2 * cr2 + y;

            m_sQuad.bl.vertices = new ccVertex3F((float)ax, (float)ay, _vertexZ);
            m_sQuad.br.vertices = new ccVertex3F((float)bx, (float)by, _vertexZ);
            m_sQuad.tl.vertices = new ccVertex3F((float)dx, (float)dy, _vertexZ);
            m_sQuad.tr.vertices = new ccVertex3F((float)cx, (float)cy, _vertexZ);

            m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);

            m_bDirty = m_bRecursiveDirty = false;
        }

        /// <summary>
        /// tell the sprite to use self-render.
        /// @since v0.99.0
        /// </summary>
        public void useSelfRender()
        {
            m_uAtlasIndex = ccMacros.CCSpriteIndexNotInitialized;
            m_bUseBatchNode = false;
            m_pobTextureAtlas = null;
            m_pobBatchNode = null;
            m_bDirty = m_bRecursiveDirty = false;

            float x1 = 0 + m_obOffsetPositionInPixels.X;
            float y1 = 0 + m_obOffsetPositionInPixels.Y;
            float x2 = x1 + m_obRectInPixels.Size.Width;
            float y2 = y1 + m_obRectInPixels.Size.Height;
            m_sQuad.bl.vertices = ccTypes.vertex3(x1, y1, 0);
            m_sQuad.br.vertices = ccTypes.vertex3(x2, y1, 0);
            m_sQuad.tl.vertices = ccTypes.vertex3(x1, y2, 0);
            m_sQuad.tr.vertices = ccTypes.vertex3(x2, y2, 0);
        }

        /// <summary>
        /// updates the texture rect of the CCSprite in points.
        /// </summary>
        public void setTextureRect(CCRect rect)
        {
            CCRect rectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(rect);
            m_obTextureRect = rect;
            setTextureRectInPixels(rectInPixels, false, rectInPixels.Size);
        }

        /// <summary>
        /// updates the texture rect, rectRotated and untrimmed size of the CCSprite in pixels
        /// </summary>
        public void setTextureRectInPixels(CCRect rect, bool rotated, CCSize size)
        {
            m_obRectInPixels = rect;
            m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);
            m_bRectRotated = rotated;

            ContentSizeInPixels = size;
            updateTextureCoords(m_obRectInPixels);

            CCPoint relativeOffsetInPixels = m_obUnflippedOffsetPositionFromCenter;

            if (m_bFlipX)
            {
                relativeOffsetInPixels.X = -relativeOffsetInPixels.X;
            }
            if (m_bFlipY)
            {
                relativeOffsetInPixels.Y = -relativeOffsetInPixels.Y;
            }

            m_obOffsetPositionInPixels.X = relativeOffsetInPixels.X + (_contentSizeInPixels.Width - m_obRectInPixels.Size.Width) / 2;
            m_obOffsetPositionInPixels.Y = relativeOffsetInPixels.Y + (_contentSizeInPixels.Height - m_obRectInPixels.Size.Height) / 2;

            // rendering using batch node
            if (m_bUseBatchNode)
            {
                // update dirty_, don't update recursiveDirty_
                m_bDirty = true;
            }
            else
            {
                // self rendering

                // Atlas: Vertex
                float x1 = 0 + m_obOffsetPositionInPixels.X;
                float y1 = 0 + m_obOffsetPositionInPixels.Y;
                float x2 = x1 + m_obRectInPixels.Size.Width;
                float y2 = y1 + m_obRectInPixels.Size.Height;

                // Don't update Z.
                m_sQuad.bl.vertices = ccTypes.vertex3(x1, y1, 0);
                m_sQuad.br.vertices = ccTypes.vertex3(x2, y1, 0);
                m_sQuad.tl.vertices = ccTypes.vertex3(x1, y2, 0);
                m_sQuad.tr.vertices = ccTypes.vertex3(x2, y2, 0);
            }
        }

        /// <summary>
        /// tell the sprite to use batch node render.
        /// @since v0.99.0
        /// </summary>
        public void useBatchNode(CCSpriteBatchNode batchNode)
        {
            m_bUseBatchNode = true;
            m_pobTextureAtlas = batchNode.TextureAtlas; // weak ref
            m_pobBatchNode = batchNode;
        }

        #region Frames

        /// <summary>
        /// sets a new display frame to the CCSprite.
        /// </summary>
        public CCSpriteFrame DisplayFrame
        {
            set
            {
                m_obUnflippedOffsetPositionFromCenter = value.OffsetInPixels;

                Texture pNewTexture = value.Texture;
                // update texture before updating texture rect
                if (pNewTexture != m_pobTexture)
                {
                    this.Texture = pNewTexture;
                }

                // update rect
                m_bRectRotated = value.IsRotated;
                setTextureRectInPixels(value.RectInPixels, value.IsRotated, value.OriginalSizeInPixels);
            }
            get
            {
                return CCSpriteFrame.frameWithTexture(m_pobTexture,
                                                 m_obRectInPixels,
                                                 m_bRectRotated,
                                                 m_obUnflippedOffsetPositionFromCenter,
                                                 _contentSizeInPixels);
            }
        }

        /// <summary>
        /// returns whether or not a CCSpriteFrame is being displayed
        /// </summary>
        public bool isFrameDisplayed(CCSpriteFrame pFrame)
        {
            CCRect r = pFrame.Rect;

            return (r.Equals(m_obRect) && pFrame.Texture.Name == m_pobTexture.Name);
        }

        #endregion

        // Animation

        /// <summary>
        /// changes the display frame with animation name and index.
        /// The animation name will be get from the CCAnimationCache
        /// @since v0.99.5
        /// </summary>
        public void setDisplayFrameWithAnimationName(string animationName, int frameIndex)
        {
            Debug.Assert(animationName != null);

            CCAnimation a = CCAnimationCache.sharedAnimationCache().animationByName(animationName);

            Debug.Assert(a != null);

            CCSpriteFrame frame = a.getFrames()[frameIndex];

            Debug.Assert(frame != null);

            DisplayFrame = frame;
        }

        protected void updateTextureCoords(CCRect rect)
        {
            Texture tex = m_bUsesBatchNode ? m_pobTextureAtlas.Texture : m_pobTexture;
            if (tex == null)
            {
                return;
            }

            float atlasWidth = (float)tex.Width;
            float atlasHeight = (float)tex.Height;

            float left, right, top, bottom;

            if (m_bRectRotated)
            {
#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
		left	= (2*rect.origin.x+1)/(2*atlasWidth);
		right	= left+(rect.size.height*2-2)/(2*atlasWidth);
		top		= (2*rect.origin.y+1)/(2*atlasHeight);
		bottom	= top+(rect.size.width*2-2)/(2*atlasHeight);
#else
                left = rect.Origin.X / atlasWidth;
                right = left + (rect.Size.Height / atlasWidth);
                top = rect.Origin.Y / atlasHeight;
                bottom = top + (rect.Size.Width / atlasHeight);
#endif // CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                if (m_bFlipX)
                {
                    ccMacros.CC_SWAP<float>(ref top, ref bottom);
                }

                if (m_bFlipY)
                {
                    ccMacros.CC_SWAP<float>(ref left, ref right);
                }

                m_sQuad.bl.texCoords.u = left;
                m_sQuad.bl.texCoords.v = top;
                m_sQuad.br.texCoords.u = left;
                m_sQuad.br.texCoords.v = bottom;
                m_sQuad.tl.texCoords.u = right;
                m_sQuad.tl.texCoords.v = top;
                m_sQuad.tr.texCoords.u = right;
                m_sQuad.tr.texCoords.v = bottom;
            }
            else
            {
#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
		left	= (2*rect.origin.x+1)/(2*atlasWidth);
		right	= left + (rect.size.width*2-2)/(2*atlasWidth);
		top		= (2*rect.origin.y+1)/(2*atlasHeight);
		bottom	= top + (rect.size.height*2-2)/(2*atlasHeight);
#else
                left = rect.Origin.X / atlasWidth;
                right = left + rect.Size.Width / atlasWidth;
                top = rect.Origin.Y / atlasHeight;
                bottom = top + rect.Size.Height / atlasHeight;
#endif // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                if (m_bFlipX)
                {
                    ccMacros.CC_SWAP<float>(ref left, ref right);
                }

                if (m_bFlipY)
                {
                    ccMacros.CC_SWAP<float>(ref top, ref bottom);
                }

                m_sQuad.bl.texCoords.u = left;
                m_sQuad.bl.texCoords.v = bottom;
                m_sQuad.br.texCoords.u = right;
                m_sQuad.br.texCoords.v = bottom;
                m_sQuad.tl.texCoords.u = left;
                m_sQuad.tl.texCoords.v = top;
                m_sQuad.tr.texCoords.u = right;
                m_sQuad.tr.texCoords.v = top;
            }
        }

        protected void updateBlendFunc()
        {
            // CCSprite: updateBlendFunc doesn't work when the sprite is rendered using a CCSpriteSheet
            Debug.Assert(m_bUsesBatchNode != null);

            //// it's possible to have an untextured sprite
            //if (m_pobTexture == null || !m_pobTexture.HasPremultipliedAlpha)
            //{
            //    m_sBlendFunc.src = OGLES.GL_SRC_ALPHA;
            //    m_sBlendFunc.dst = OGLES.GL_ONE_MINUS_SRC_ALPHA;
            //    IsOpacityModifyRGB = false;
            //}
            //else
            {
                m_sBlendFunc.src = ccMacros.CC_BLEND_SRC;
                m_sBlendFunc.dst = ccMacros.CC_BLEND_DST;
                IsOpacityModifyRGB = true;
            }
        }

        protected void getTransformValues(transformValues_ tv)
        {
            tv.pos = _positionInPixels;
            tv.scale.X = _scaleX;
            tv.scale.Y = _scaleY;
            tv.rotation = _rotation;
            tv.skew.X = _skewX;
            tv.skew.Y = _skewY;
            tv.ap = AnchorPointInPixels;
            tv.visible = Visible;
        }

        // Subchildren needs to be updated
        protected bool m_bRecursiveDirty;

        // optimization to check if it contain children
        protected bool m_bHasChildren;

        // Data used when the sprite is self-rendered
        protected Texture m_pobTexture;

        // whether or not it's parent is a CCSpriteBatchNode
        bool m_bUsesBatchNode = false;

        // texture
        protected CCRect m_obRect;
        protected CCRect m_obRectInPixels;

        // Offset Position (used by Zwoptex)
        protected CCPoint m_obUnflippedOffsetPositionFromCenter;

        // opacity and RGB protocol
        protected ccColor3B m_sColorUnmodified;
        protected bool m_bOpacityModifyRGB;

        // image is flipped
        protected bool m_bFlipX;
        protected bool m_bFlipY;
    }

    public class transformValues_
    {
        public transformValues_()
        {
            this.pos = new CCPoint();
            this.scale = new CCPoint();
            this.skew = new CCPoint();
            this.ap = new CCPoint();
        }

        public CCPoint pos;  // position  x and y
        public CCPoint scale;  // scale x and y
        public float rotation;
        public CCPoint skew;   // skew x and y
        public CCPoint ap;     // anchor point in pixels
        public bool visible;
    }
}
