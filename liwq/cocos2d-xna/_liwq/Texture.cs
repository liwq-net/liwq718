using liwq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace cocos2d
{
    //贴图缓存可以基于局部，node。或者基于全局，application
    public class Texture
    {
        static int _Name = 0;
        public int Name { get; protected set; }

        public Texture2D Texture2D { get; set; }

        public Texture(byte[] data, int width, int height)
        {
            this.Texture2D = new Texture2D(Application.SharedApplication.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            this.Texture2D.SetData<byte>(data);
            this.Name = _Name++;
        }

        public Texture(Texture2D texture2d)
        {
            this.Texture2D = texture2d;
            this.Name = _Name++;
        }

        public Texture(string filename)
        {
            liwq.PngDecoder png = new liwq.PngDecoder();
            byte[] pixels = png.Decode(System.IO.File.OpenRead(filename));
            this.Texture2D = new Texture2D(Application.SharedApplication.GraphicsDevice, png.Width, png.Height, false, SurfaceFormat.Color);
            this.Texture2D.SetData<byte>(pixels);
            this.Name = _Name++;
        }

        public Texture(int width, int hegiht)
        {
            this.Texture2D = new Texture2D(Application.SharedApplication.GraphicsDevice, width, hegiht);
            this.Name = _Name++;
        }

        public int Width
        {
            get
            {
                if (this.Texture2D != null) return this.Texture2D.Width;
                return 0;
            }
        }

        public int Height
        {
            get
            {
                if (this.Texture2D != null) return this.Texture2D.Height;
                return 0;
            }
        }

        public CCSize ContentSize
        {
            get
            {
                if (this.Texture2D != null)
                    return new CCSize(this.Texture2D.Width, this.Texture2D.Height);
                return CCSize.Zero;
            }
        }

    }
}
