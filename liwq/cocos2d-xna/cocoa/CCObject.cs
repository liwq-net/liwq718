using System.Diagnostics;
namespace cocos2d
{
    public interface CCCopying
    {
        CCObject copyWithZone(CCZone zone);
    }

    /// <summary>
    /// Add functions if needed.
    /// </summary>
    public class CCObject : CCCopying
    {
        public virtual CCObject copy()
        {
            return copyWithZone(null);
        }

        public virtual CCObject copyWithZone(CCZone zone)
        {
            Debug.Assert(false);
            return null;
        }
    }
}
