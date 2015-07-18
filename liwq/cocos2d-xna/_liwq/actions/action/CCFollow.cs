using liwq;
using System.Diagnostics;

namespace cocos2d
{
    public class CCFollow : CCAction
    {
        //node to follow
        protected Node _followedNode;

        //if screen size is bigger than the boundary - update not needed
        protected bool _isBoundaryFullyCovered;

        protected CCPoint _halfScreenSize;
        protected CCPoint _fullScreenSize;

        //world boundaries
        protected float _leftBoundary;
        protected float _rightBoundary;
        protected float _topBoundary;
        protected float _bottomBoundary;

        /// <summary>whether camera should be limited to certain area</summary>
        public bool IsBoundarySet { get; set; }

        public CCFollow(Node followedNode, CCRect rect)
        {
            Debug.Assert(followedNode != null);

            this._followedNode = followedNode;
            this._isBoundaryFullyCovered = false;
            this.IsBoundarySet = true;

            CCSize winSize = Director.SharedDirector.DesignSize;
            this._fullScreenSize = new CCPoint(winSize.Width, winSize.Height);
            this._halfScreenSize = this._fullScreenSize * 0.5f;

            this._leftBoundary = -((rect.Origin.X + rect.Size.Width) - this._fullScreenSize.X);
            this._rightBoundary = -rect.Origin.X;
            this._topBoundary = -rect.Origin.Y;
            this._bottomBoundary = -((rect.Origin.Y + rect.Size.Height) - this._fullScreenSize.Y);

            if (this._rightBoundary < this._leftBoundary)
            {
                //screen width is larger than world's boundary width set both in the middle of the world
                this._rightBoundary = this._leftBoundary = (this._leftBoundary + this._rightBoundary) / 2;
            }
            if (this._topBoundary < this._bottomBoundary)
            {
                //screen width is larger than world's boundary width set both in the middle of the world
                this._topBoundary = this._bottomBoundary = (this._topBoundary + this._bottomBoundary) / 2;
            }

            if ((this._topBoundary == this._bottomBoundary) && (this._leftBoundary == this._rightBoundary))
            {
                this._isBoundaryFullyCovered = true;
            }
        }

        public CCFollow(Node followedNode)
        {
            Debug.Assert(followedNode != null);

            this._followedNode = followedNode;
            this._isBoundaryFullyCovered = false;
            this.IsBoundarySet = false;

            CCSize winSize = Director.SharedDirector.DesignSize;
            this._fullScreenSize = new CCPoint(winSize.Width, winSize.Height);
            this._halfScreenSize = new CCPoint(winSize.Width * 0.5f, winSize.Height * 0.5f);
        }

        public override void Step(float dt)
        {
            if (this.IsBoundarySet)
            {
                // whole map fits inside a single screen, no need to modify the position - unless map boundaries are increased
                if (this._isBoundaryFullyCovered)
                    return;
                CCPoint tempPos = this._halfScreenSize - this._followedNode.Position;
                Target.Position = new CCPoint(
                    CCPoint.Clamp(tempPos.X, this._leftBoundary, this._rightBoundary),
                    CCPoint.Clamp(tempPos.Y, this._bottomBoundary, this._topBoundary)
                    );
            }
            else
            {
                Target.Position = this._halfScreenSize - this._followedNode.Position;
            }
        }

        public override bool IsDone()
        {
            return !this._followedNode.IsRunning;
        }

        public override void Stop()
        {
            this.Target = null;
            base.Stop();
        }

    }
}