namespace cocos2d
{
    public class CCZone
    {
        public CCZone(CCObject obj)
        {
            this.m_pCopyObject = obj;
        }

        public CCZone() { }

        public CCObject m_pCopyObject;
    }
}
