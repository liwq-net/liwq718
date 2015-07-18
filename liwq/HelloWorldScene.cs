using cocos2d;
using liwq;
using System.Collections.Generic;

namespace HelloCocos2d
{
    public class HelloCocos2dScene : Node
    {

        public HelloCocos2dScene()
        {
            //var sp = new Sprite();
            ////sp.Texture = new Texture(colors, 100, 100);
            //sp.Texture = new Texture("Content/android9.png");
            //sp.Scale = 0.2f;

            //var size = Director.SharedDirector.DisplaySize;
            //sp.Position = new CCPoint(size.Width / 2, size.Height / 2);
            //this.addChild(sp);


            //var act = new CCRotateBy();
            //act.initWithDuration(3.0f, 360);

            //var jump1 = CCJumpTo.actionWithDuration(5, new CCPoint(500, 0), 80, 10);
            //var jump2 = CCReverseTime.actionWithAction(jump1);
            //var quence = CCSequence.actionOneTwo(jump1, jump2);

            //var spawn = CCSpawn.actionOneTwo(act, quence);


            //var repeat = new CCRepeatForever();
            //repeat.initWithAction(spawn);
            //sp.RunAction(repeat);


            CCParticleSnow particle = new CCParticleSnow();
            //particle.initWithFile("particle_texture.plist");
            particle.Position = Director.SharedDirector.DisplaySize.Center;
            this.addChild(particle);

        }

        //public override bool init()
        //{
        //    if (!base.init())
        //    {
        //        return false;
        //    }

        //    this.m_bIsTouchEnabled = true;


        //    var sp = CCSprite.spriteWithFile("pic_guide_06");

        //    var size = Director.SharedDirector.DisplaySize;
        //    sp.position = new CCPoint(size.Width / 2, size.Height / 2);
        //    pSprite = sp;
        //    this.addChild(sp);


        //    var act = new CCRotateBy();
        //    act.initWithDuration(3.0f, 360);
        //    var repeat = new CCRepeatForever();
        //    repeat.initWithAction(act);
        //    sp.runAction(repeat);

        //    return true;
        //}
        //CCSprite pSprite;

        //public override void onEnter()
        //{
        //    base.onEnter();
        //    this.isTouchEnabled = true;
        //}
        //public override void registerWithTouchDispatcher()
        //{
        //    CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, false);
        //    CCTouchDispatcher.sharedDispatcher().addStandardDelegate(this, 0);
        //}



        public static CCScene scene()
        {
            //CCScene scene = CCScene.node();
            //CCLayer layer = HelloCocos2dScene.node();
            //scene.addChild(layer);
            //return scene;

            CCScene scene = new CCScene();
            scene.addChild(new HelloCocos2dScene());
            return scene;
        }

        //public static new CCLayer node()
        //{
        //    HelloCocos2dScene ret = new HelloCocos2dScene();
        //    if (ret.init()) { return ret; }
        //    else { ret = null; }
        //    return ret;
        //}

        //public override bool ccTouchBegan(CCTouch touch, CCEvent event_)
        //{
        //    CCSize s = Director.SharedDirector.DesignSize;
        //    CCActionInterval actionTo = CCMoveTo.actionWithDuration(3, new CCPoint(-s.Width, -s.Height));
        //    pSprite.runAction(actionTo);
        //    return true;
        //}

    }
}
