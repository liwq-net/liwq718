using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    /// <summary>
    /// FBO class that grabs the the contents of the screen
    /// </summary>
    public class CCGrabber : CCObject
    {
        protected uint m_fbo;
        protected int m_oldFBO;
        protected RenderTarget2D m_RenderTarget2D;
  
        public void grab(ref Texture pTexture)
        {
            m_RenderTarget2D = new RenderTarget2D(Application.SharedApplication.GraphicsDevice,
                (int)pTexture.Width,
                (int)pTexture.Height);

            pTexture.Texture2D = m_RenderTarget2D;
        }
        public void beforeRender(ref Texture pTexture)
        {
            Application.SharedApplication.GraphicsDevice.SetRenderTarget(m_RenderTarget2D);
        }
        public void afterRender(ref Texture pTexture)
        {
            Application.SharedApplication.GraphicsDevice.SetRenderTarget(null);
            pTexture.Texture2D = m_RenderTarget2D;
        }
    }
}
