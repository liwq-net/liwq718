
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCTransitionFade : CCTransitionScene
    {
        const int kSceneFade = 2147483647;
        protected ccColor4B m_tColor;

        /// <summary>
        /// creates the transition with a duration and with an RGB color
        /// Example: FadeTransition::transitionWithDuration(2, scene, ccc3(255,0,0); // red color
        /// </summary>
        public static CCTransitionFade transitionWithDuration(float duration, CCScene scene, ccColor3B color)
        {
            CCTransitionFade pTransition = new CCTransitionFade();
            pTransition.initWithDuration(duration, scene, color);
            return pTransition;
        }

        /// <summary>
        /// initializes the transition with a duration and with an RGB color 
        /// </summary>
        public virtual bool initWithDuration(float duration, CCScene scene, ccColor3B color)
        {
            if (base.initWithDuration(duration, scene))
            {
                m_tColor = new ccColor4B();
                m_tColor.r = color.r;
                m_tColor.g = color.g;
                m_tColor.b = color.b;
                m_tColor.a = 0;
            }
            return true;
        }

        public new static CCTransitionScene transitionWithDuration(float t, CCScene scene)
        {
            return transitionWithDuration(t, scene, new ccColor3B());
        }

        public override bool initWithDuration(float t, CCScene scene)
        {
            this.initWithDuration(t, scene, new ccColor3B(Color.Black));
            return true;
        }

        public override void onEnter()
        {
            base.onEnter();

            CCLayerColor l = CCLayerColor.layerWithColor(m_tColor);
            m_pInScene.Visible = false;

            AddChild(l, 2, kSceneFade);
            Node f = GetChildByTag(kSceneFade);

            CCActionInterval a = (CCActionInterval)CCSequence.actions
                (
                    CCFadeIn.actionWithDuration(m_fDuration / 2),
                    CCCallFunc.actionWithTarget(this, (base.hideOutShowIn)),
                    CCFadeOut.actionWithDuration(m_fDuration / 2),
                    CCCallFunc.actionWithTarget(this, (base.finish))
                );
            f.RunAction(a);
        }

        public override void onExit()
        {
            base.onExit();
            this.removeChildByTag(kSceneFade, false);
        }
    }
}
