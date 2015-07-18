using System.Diagnostics;
namespace cocos2d
{
    public class CCFiniteTimeAction : CCAction
    {
        /// <summary>duration in seconds</summary>
        public float Duration { get; set; }

        public virtual CCFiniteTimeAction Reverse()
        {
            return null;
        }
    }
}
