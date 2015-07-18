# liwq

linux,ios,windows,quicker!
liwq是一套基于monogame的跨平台框架,同时适用于开发软件或者游戏。
框架参考cocos2d的设计，对引擎核心部分进行简化，把多余的去掉，把必须的补回来，简化代码编写，优化性能，添加WPF风格UI。

##当前版本主要工作内容如下：
 * 1、重构所有代码，统一使用UI左上角坐标
 * 2、Director 与 AppDelegate合并一个"大总管"
 * 3、Texture，Sprite，SpriteFrame 合并
 * 4、优化贴图性能，添加贴图动态合并功能（合成一张大贴图共用，发挥batch效能）。基于场景（node)的texture缓存，node释放，贴图释放
 * 5、废掉原来的menu等UI系统，废掉Layer，只基于Node以及上层的WPF风格UI（矢量控件）
 * 6、添加矢量字体以及高速的点阵字体（矢量字体选用stb ttf或者xmlreader方式的svg字体）
 * 7、保留 actions，particle system，scenes_transitions
 * 8、废弃xnb模式资源管理模式，支持ogg，png，gif,jpg，压缩贴图资源
 * 9、添加事件驱动模式，而不是恒定帧率（省电）
 * 10、actions 改为begin end连续函数控制
 * 11、支持xaml设计，支持metro风格控件
 * 12、.net风格的事件绑定模式
 * 12、3D模型支持（2.0）
 * 13、XNAVG支持（2.0）
 * 14、适配手机特有功能（震动，gps，webview，水平仪，相机调用）(2.0)

 ##命名范例
	private int _field;
	static private int _Field;
	public int Field { get; set; }
	static public int Field { get; set; }
	
	public int Add(int count)
	{
		return this._field++;
	}
	private int add(int count)
	{
		return this._field++;
	}
	static public int Add(int count)
	{
		return Class.Field++;
	}