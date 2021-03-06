namespace cocos2d
{
    /*
     * Use delegate to implement the correspongding selector
     */

    public delegate void SEL_SCHEDULE(float dt);
    public delegate void SEL_CallFunc();
    public delegate void SEL_CallFuncN(Node sender);
    public delegate void SEL_CallFuncND(Node sender, object data);
    public delegate void SEL_CallFuncO(CCObject sender);
    public delegate void SEL_MenuHandler(CCObject sender);
    public delegate void SEL_EventHandler(CCEvent event_);

    public interface SelectorProtocol
    {
        void update(float dt);
    }
}
