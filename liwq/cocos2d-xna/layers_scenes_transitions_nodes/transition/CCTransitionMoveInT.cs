using liwq;

namespace cocos2d
{
    public class CCTransitionMoveInT : CCTransitionMoveInL
    {
        public override void initScenes()
        {
            CCSize s = Director.SharedDirector.DesignSize;
            m_pInScene.Position = new CCPoint(0, s.Height);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionMoveInT);
        public static new CCTransitionMoveInR transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionMoveInR pScene = new CCTransitionMoveInR();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}
