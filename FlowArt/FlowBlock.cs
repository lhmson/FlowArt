using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Northwoods.Go;

namespace FlowArt
{
    /// <summary>
    /// Specify different kinds of blocks.
    /// </summary>
    public enum BlockType
    {
        Start = 1,
        End = 2,
        Process = 3,
        Input = 4,
        Output = 5,
        Condition = 6,
    }

    /// <summary>
    /// A block representing a step or action in a flowchart.
    /// </summary>
    [Serializable]
    public class FlowBlock : GoTextNode
    {
        public FlowBlock()
        {
            InitCommon();
        }

        public FlowBlock(BlockType k)
        {
            InitCommon();
            this.Kind = k;
        }

        private void InitCommon()
        {
            // assume BlockType.Process
            this.Label.Wrapping = true;
            this.Label.Editable = false;
            this.Label.Alignment = Middle;
            this.Label.TextColor = Color.White;
            this.Label.Font = new Font("Calibri", 12, FontStyle.Bold);
            this.Editable = false;
            Color blockColor = ColorOfType(this.Kind);
            InitShape("Process", new GoDrawing(GoFigure.Rectangle), blockColor, blockColor, 22, 15, 22, 15);
        }

        /// <summary>
        /// The location for each block is the Center.
        /// </summary>
        public override PointF Location
        {
            get { return this.Center; }
            set { this.Center = value; }
        }

        /// <summary>
        /// Adjust port positions for certain background shapes.
        /// </summary>
        /// <param name="childchanged"></param>
        public override void LayoutChildren(GoObject childchanged)
        {
            base.LayoutChildren(childchanged);
            GoDrawing draw = this.Background as GoDrawing;
            if (draw != null)
            {
                PointF tempPoint;
                if (draw.Figure == GoFigure.Input || draw.Figure == GoFigure.Output)
                {
                    if (this.RightPort != null)
                    {
                        draw.GetNearestIntersectionPoint(new PointF(this.RightPort.Center.X + .01f, this.RightPort.Center.Y),
                            this.RightPort.Center, out tempPoint);
                        this.RightPort.Right = tempPoint.X;
                    }
                    if (this.LeftPort != null)
                    {
                        draw.GetNearestIntersectionPoint(new PointF(this.LeftPort.Center.X + .01f, this.LeftPort.Center.Y),
                            this.LeftPort.Center, out tempPoint);
                        this.LeftPort.Left = tempPoint.X;
                    }
                }
            }
        }

        /// <summary>
        /// When the mouse passes over a block, display all of its ports.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        /// <remarks>
        /// All ports on all blocks are hidden when the mouse hovers over the background.
        /// </remarks>
        public override bool OnMouseOver(GoInputEventArgs evt, GoView view)
        {
            FlowView v = view as FlowView;
            if (v != null)
            {
                foreach (GoPort p in this.Ports)
                {
                    p.SkipsUndoManager = true;
                    p.Style = GoPortStyle.Ellipse;
                    p.SkipsUndoManager = false;
                }
            }
            return false;
        }

        /// <summary>
        /// Bring up a FlowBlock specific context menu.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public override GoContextMenu GetContextMenu(GoView view)
        {
            if (view is GoOverview) return null;
            if (!(view.Document is FlowDocument)) return null;
            GoContextMenu cm = new GoContextMenu(view);
            if (!((FlowDocument)view.Document).IsReadOnly && this.IsPredecessor)
            {
                cm.MenuItems.Add(new MenuItem("Draw Link", new EventHandler(this.DrawRelationship_Command)));
                cm.MenuItems.Add(new MenuItem("-"));
            }
            if (CanDelete())
            {
                cm.MenuItems.Add(new MenuItem("Cut", new EventHandler(this.Cut_Command)));
            }
            if (CanCopy())
            {
                cm.MenuItems.Add(new MenuItem("Copy", new EventHandler(this.Copy_Command)));
            }
            if (CanDelete())
            {
                cm.MenuItems.Add(new MenuItem("Delete", new EventHandler(this.Delete_Command)));
            }

            return cm;
        }

        public void DrawRelationship_Command(Object sender, EventArgs e)
        {
            GoView v = GoContextMenu.FindView(sender as MenuItem);
            if (v != null)
            {
                linkTool t = new linkTool(v);
                t.Predecessor = this;
                v.Tool = t;
            }
        }

        public void Cut_Command(Object sender, EventArgs e)
        {
            GoView v = GoContextMenu.FindView(sender as MenuItem);
            if (v != null)
                v.EditCut();
        }

        public void Copy_Command(Object sender, EventArgs e)
        {
            GoView v = GoContextMenu.FindView(sender as MenuItem);
            if (v != null)
                v.EditCopy();
        }

        public void Delete_Command(Object sender, EventArgs e)
        {
            GoView v = GoContextMenu.FindView(sender as MenuItem);
            if (v != null)
                v.EditDelete();
        }

        public BlockType Kind
        {
            get { return myKind; }
            set
            {
                BlockType old = myKind;
                if (old != value)
                {
                    myKind = value;
                    Changed(ChangedKind, (int)old, null, NullRect, (int)value, null, NullRect);
                    OnKindChanged(old, value);
                }
            }
        }

        public virtual void InitShape(string content, GoShape shape, Color bColor, Color pColor, float topMarginX, float topMarginY, float bottomMarginX, float bottomMarginY)
        {
            bool solidColor = false;
            if (solidColor)
            {
                shape.BrushColor = bColor;
                shape.PenColor = pColor;
            }
            else
            {
                shape.FillSimpleGradient(bColor, Lighter(bColor), MiddleTop);
                shape.PenColor = Darker(bColor);
            }
            this.TopLeftMargin = new SizeF(topMarginX, topMarginY);
            this.BottomRightMargin = new SizeF(bottomMarginX, bottomMarginY);
            this.Background = shape;
            this.Text = content;
        }


        protected virtual void OnKindChanged(BlockType oldkind, BlockType newkind)
        {
            Color blockColor = ColorOfType(newkind);

            // update the parts, based on the Kind of block this now is
            switch (newkind)
            {
                case BlockType.Start:
                    {
                        //GoRoundedRectangle rect = new GoRoundedRectangle();
                        InitShape("Start Flow", new GoDrawing(GoFigure.Ellipse), blockColor, blockColor, 20, 5, 20, 3);
                        UpdatePorts("o", "o", "o", "o");
                        //rect.Corner = new SizeF(this.Height / 2, this.Height / 2); // perfect rounded ends
                        break;
                    }
                case BlockType.End:
                    {
                        //GoRoundedRectangle rect = new GoRoundedRectangle();
                        InitShape("End Flow", new GoDrawing(GoFigure.Ellipse), blockColor, blockColor, 20, 5, 20, 3);
                        UpdatePorts("i", "i", "i", "i");
                        //rect.Corner = new SizeF(this.Height / 2, this.Height / 2);
                        break;
                    }
                case BlockType.Process:
                    {
                        InitShape("Process", new GoDrawing(GoFigure.Rectangle), blockColor, blockColor, 22, 15, 22, 15);
                        UpdatePorts("io", "io", "io", "io");
                        break;
                    }
                case BlockType.Input:
                    {
                        InitShape("Scan Sth", new GoDrawing(GoFigure.Input), blockColor, blockColor, 28, 12, 28, 12);
                        UpdatePorts("io", "io", "io", "io");
                        break;
                    }
                case BlockType.Output:
                    {
                        InitShape("Print Sth", new GoDrawing(GoFigure.Card), blockColor, blockColor, 28, 12, 28, 12);
                        UpdatePorts("io", "io", "io", "io");
                        break;
                    }
                case BlockType.Condition:
                    {
                        InitShape("Cond.", new GoDrawing(GoFigure.Decision), blockColor, blockColor, 35, 20, 35, 20);
                        UpdatePorts("io", "io", "io", "io");
                        break;
                    }
                default: throw new InvalidEnumArgumentException("newkind", (int)newkind, typeof(BlockType));
            }
        }

        

        private void UpdatePorts(String top, String right, String bottom , String left)
        {  // TopPort, RightPort, BottomPort, LeftPort
            if (top == "")
            {
                this.TopPort = null;
            }
            else
            {
                if (this.TopPort == null) this.TopPort = CreatePort(MiddleTop);
                if (this.TopPort != null)
                {
                    this.TopPort.IsValidFrom = top.IndexOf('o') > -1;
                    this.TopPort.IsValidTo = top.IndexOf('i') > -1;
                }
            }
            if (right == "")
            {
                this.RightPort = null;
            }
            else
            {
                if (this.RightPort == null) this.RightPort = CreatePort(MiddleRight);
                if (this.RightPort != null)
                {
                    this.RightPort.IsValidFrom = right.IndexOf('o') > -1;
                    this.RightPort.IsValidTo = right.IndexOf('i') > -1;
                }
            }
            if (bottom == "")
            {
                this.BottomPort = null;
            }
            else
            {
                if (this.BottomPort == null) this.BottomPort = CreatePort(MiddleBottom);
                if (this.BottomPort != null)
                {
                    this.BottomPort.IsValidFrom = bottom.IndexOf('o') > -1;
                    this.BottomPort.IsValidTo = bottom.IndexOf('i') > -1;
                }
            }
            if (left == "")
            {
                this.LeftPort = null;
            }
            else
            {
                if (this.LeftPort == null) this.LeftPort = CreatePort(MiddleLeft);
                if (this.LeftPort != null)
                {
                    this.LeftPort.IsValidFrom = left.IndexOf('o') > -1;
                    this.LeftPort.IsValidTo = left.IndexOf('i') > -1;
                }
            }
        }

        public bool IsPredecessor
        {
            get { return this.BottomPort != null; }
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            if (e.SubHint == ChangedKind)
                myKind = (BlockType)e.GetInt(undo);
            else
                base.ChangeValue(e, undo);
        }

        public const int ChangedKind = GoObject.LastChangedHint + 7;

        private BlockType myKind = BlockType.Process;

        // Color handling
        private Color Darker(Color c)
        {
            return Color.FromArgb(Darken(c.R), Darken(c.G), Darken(c.B));
        }

        private int Darken(int a)
        {
            a = a - 30;
            if (a < 0) a = 0;
            return a;
        }
        private Color Lighter(Color c)
        {
            return Color.FromArgb(Lighten(c.R), Lighten(c.G), Lighten(c.B));
        }
        private int Lighten(int a)
        {
            a = a - 30;
            if (a < 0) a = 0;
            return a;
        }
        protected override GoPort CreatePort(int spot)
        {
            GoPort p = base.CreatePort(spot);
            p.Size = new SizeF(p.Size.Width * 2, p.Size.Height * 2);
            p.EndSegmentLength = 15 + 5;  // arrowheadlength + some offset 
            p.Brush = null;
            return p;
        }

        


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            if( this.Kind == BlockType.Start || this.Kind == BlockType.End )
                return true;
            
            // skip editing blocks in the pallete
            if (!(this.Document is FlowDocument))
                return true;

            frmBlockEdit frm = new frmBlockEdit(this);
            DialogResult dialogResult = frm.ShowDialog();
            if( dialogResult == DialogResult.OK )
                this.Text = frm.ExpressionResult;

            return true;
        }

        // darken block effect ////////////////////////////////////////////////////////////////////////////////////////////////// 

        // NonSerialize to fix pasting error
        [NonSerialized]
        private DarkenBlockEffectPlayer fxPlayer = null;
        public DarkenBlockEffectPlayer FxPlayer
        {
            get
            {
                if(fxPlayer == null)
                    InitializeDarkenBlockEffect();
                return fxPlayer;
            }
            set
            {
                fxPlayer = value;
            }
        }

        private void InitializeDarkenBlockEffect()
        {
            fxPlayer = new DarkenBlockEffectPlayer(this);
        }
        
        // pastel color /////////////////////////////////////////////////////////////////////////////////////////////////////////
        static Color ColorOfType(BlockType type)
        {
            switch (type)
            {
                case BlockType.Start:
                {
                    return Color.FromArgb(99, 216, 109);
                }
                case BlockType.End:
                {
                    return Color.FromArgb(255, 81, 72);
                }
                case BlockType.Input:
                {
                    return Color.FromArgb(162, 138, 210);               
                }
                case BlockType.Output:
                {
                    return Color.FromArgb(255, 164, 71);
                }
                case BlockType.Process:
                {
                    return Color.FromArgb(119, 158, 203);
                }
                case BlockType.Condition:
                {
                    return Color.FromArgb(242, 131, 180);
                }
            }
            return Color.FromArgb(0,0,0);
        }

    }
}
