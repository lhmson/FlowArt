using System;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace FlowArt
{
    [Serializable]
    public class linkTool : GoTool
    {
        public linkTool(GoView view) : base(view) { }

        public override void Start()
        {
            myLink = null;
            if (this.Predecessor == null)
            {
                MainBase.App.SetStatusMessage("Create link -- click a block to start link and then a block that should follow it");
            }
            else
            {
                // already have a FlowBlock to start with
                MakeTemporaryLink();
                MainBase.App.SetStatusMessage("Create link by choosing a block");
            }
            StartTransaction();
        }

        public override void Stop()
        {
            if (myLink != null)
            {
                this.View.Layers.Default.Remove(myLink);
                myLink = null;
            }
            this.Predecessor = null;
            StopTransaction();
            MainBase.App.SetStatusMessage("Stop creating links");
        }

        private void MakeTemporaryLink()
        {
            if (myLink == null)
            {
                // create a new link starting at the bottom port of the first block
                GoLink l = new GoLink();
                l.Orthogonal = true;

                GoPort fp = new GoPort();
                fp.Style = GoPortStyle.Rectangle;
                fp.FromSpot = this.Predecessor.BottomPort.FromSpot;
                fp.Bounds = this.Predecessor.BottomPort.Bounds;
                l.FromPort = fp;

                GoPort tp = new GoPort();
                tp.Size = new SizeF(1, 1);
                tp.Position = this.LastInput.DocPoint;
                tp.ToSpot = GoObject.MiddleTop;
                l.ToPort = tp;

                // the link is temporarily a view object
                this.View.Layers.Default.Add(l);
                myLink = l;
            }
        }

        public override void DoMouseDown()
        {
            if (this.Predecessor == null)
            {
                FlowBlock gn = this.View.PickObject(true, false, this.LastInput.DocPoint, true) as FlowBlock;
                if (gn == null)
                    return;
                if (!gn.IsPredecessor)
                    return;
                this.Predecessor = gn;
                MainBase.App.SetStatusMessage("From block " + this.Predecessor.Text);
                MakeTemporaryLink();
            }
            else
            {
                FlowBlock gn = this.View.PickObject(true, false, this.LastInput.DocPoint, true) as FlowBlock;
                if (gn != null && gn != this.Predecessor && !FlowView.IsLinked(this.Predecessor, gn))
                {
                    // delete the temporary link
                    this.View.Layers.Default.Remove(myLink);
                    myLink = null;
                    // create the link in the design
                    GoPort nearest = FindNearestPort(this.LastInput.DocPoint, gn);
                    IGoLink link = this.View.CreateLink(this.Predecessor.BottomPort, nearest);
                    if (link != null)
                    {
                        this.TransactionResult = "New link";
                        this.View.RaiseLinkCreated(link.GoObject);
                        this.View.Selection.Select(link.GoObject);
                    }
                    StopTool();
                }
            }
        }

        public GoPort FindNearestPort(PointF pt, FlowBlock fb)
        {
            float maxdist = 10e20f;
            GoPort closest = null;
            GoPort p;
            p = fb.TopPort;
            if (p != null)
            {
                float dist = (p.Left - pt.X) * (p.Left - pt.X) + (p.Top - pt.Y) * (p.Top - pt.Y);
                if (dist < maxdist)
                {
                    maxdist = dist;
                    closest = p;
                }
            }
            p = fb.RightPort;
            if (p != null)
            {
                float dist = (p.Left - pt.X) * (p.Left - pt.X) + (p.Top - pt.Y) * (p.Top - pt.Y);
                if (dist < maxdist)
                {
                    maxdist = dist;
                    closest = p;
                }
            }
            p = fb.BottomPort;
            if (p != null)
            {
                float dist = (p.Left - pt.X) * (p.Left - pt.X) + (p.Top - pt.Y) * (p.Top - pt.Y);
                if (dist < maxdist)
                {
                    maxdist = dist;
                    closest = p;
                }
            }
            p = fb.LeftPort;
            if (p != null)
            {
                float dist = (p.Left - pt.X) * (p.Left - pt.X) + (p.Top - pt.Y) * (p.Top - pt.Y);
                if (dist < maxdist)
                {
                    maxdist = dist;
                    closest = p;
                }
            }
            return closest;
        }

        public override void DoMouseMove()
        {
            if (myLink != null && myLink.ToPort != null)
            {
                GoPort p = myLink.ToPort.GoObject as GoPort;
                if (p != null)
                {
                    p.Position = this.LastInput.DocPoint;
                    FlowBlock gn = this.View.PickObject(true, false, this.LastInput.DocPoint, true) as FlowBlock;
                    if (gn != null && gn != this.Predecessor && !FlowView.IsLinked(this.Predecessor, gn))
                    {
                        GoPort nearest = FindNearestPort(this.LastInput.DocPoint, gn);
                        if (nearest != null)
                        {
                            p.Position = nearest.Position;
                            p.ToSpot = nearest.ToSpot;
                        }
                    }
                }
            }
        }

        public override void DoMouseUp()
        {

        }

        public FlowBlock Predecessor
        {
            get { return myPredecessor; }
            set { myPredecessor = value; }
        }

        private GoLink myLink = null;
        private FlowBlock myPredecessor = null;
    }
}
