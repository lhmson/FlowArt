using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Northwoods.Go;
using System.Collections.Generic;

namespace FlowArt
{
    /// <summary>
    ///    Summary description for MainForm.
    /// </summary>
    class MainBase : DevExpress.XtraBars.Ribbon.RibbonForm // not design
    //class frmMain : MainInterface // design
    {
        #region Declaration of components

        // status bar
        private StatusBarPanel statusMessagePanel;
        private StatusBarPanel statusZoomPanel;
        private StatusBar statusBar;
        private ImageList imageList1;
        private MdiClient mdiClient1;
        private MenuItem menuItem1;
        private MenuItem menuItem3;
        private MenuItem drawLinkMenuItem;
        private System.ComponentModel.IContainer components;
        #endregion

        private static MainBase mainForm = null;

        private Form myOverviewForm = null;
        private GoOverview myOverview = null;
        private Panel myPanel = null;
        private BackgroundWorker backgroundWorker1;
        private MenuItem menuItem2;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private GoPalette myPalette = null;


        public MainBase()
        {
            mainForm = this;

           
            InitializeComponent();

            

            // start up with an empty map
            //fileNewMenuItem_Click(this, null);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            /// initialize backend
            FunctionHandler.InitBuiltInFunctions();
            SpecialConstHandler.InitBuiltInConst();
            
            Application.EnableVisualStyles();
            //Application.Run(new MainInterface());
            Application.Run(MainInterface.Instance);
            /**
             * Application.Run(new frmMain());
            **/
        }

        public static MainBase App
        {
            get { return mainForm; }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainBase));
            
            this.statusMessagePanel = new System.Windows.Forms.StatusBarPanel();
            
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusZoomPanel = new System.Windows.Forms.StatusBarPanel();
            
            this.mdiClient1 = new System.Windows.Forms.MdiClient();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.statusMessagePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusZoomPanel)).BeginInit();
            this.SuspendLayout();
            

            // 
            // statusMessagePanel
            // 
            this.statusMessagePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusMessagePanel.Name = "statusMessagePanel";
            this.statusMessagePanel.Width = 1040;
            
            
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 763);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusMessagePanel,
            this.statusZoomPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(1100, 20);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusBar";
            // 
            // statusZoomPanel
            // 
            this.statusZoomPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusZoomPanel.Name = "statusZoomPanel";
            this.statusZoomPanel.Text = "100%";
            this.statusZoomPanel.Width = 43;


            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(1100, 783); //1100,783
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mdiClient1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            //this.Menu = this.mainMenuBar;
            this.Name = "frmMain";
            this.Text = "FlowArt - Team ProVision";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statusMessagePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusZoomPanel)).EndInit();
            this.ResumeLayout(false);

        }

        private void InitializeCatalog()
        {
            
        }
        
        
        protected override void OnMdiChildActivate(EventArgs evt) // MDI child form is a form with the same model created inside a form
        {
            base.OnMdiChildActivate(evt);
            FlowMap w = this.ActiveMdiChild as FlowMap;
            w?.View.UpdateFormInfo();
        }


        // globally useful methods
        public void SetStatusMessage(String s)
        {
            statusMessagePanel.Text = "                    " + s;
        }

        public void SetStatusZoom(float scale)
        {
            String m = Math.Round((double)scale * 100, 3).ToString();
            statusZoomPanel.Text = m + @"%";
        }


        public GoView GetCurrentGoView()
        {
            if (this.ActiveMdiChild != null)
                return this.ActiveMdiChild.ActiveControl as GoView;
            else
            {
                if (this.MdiChildren.Length > 0)
                {
                    this.ActivateMdiChild( this.MdiChildren[0] );
                    return this.ActiveMdiChild.ActiveControl as GoView;
                }
            }

            return null;
        }

        public FlowView GetCurrentGraphView()
        {
            if (this.ActiveMdiChild != null)
                return this.ActiveMdiChild.ActiveControl as FlowView;
            else
            {
                if (this.MdiChildren.Length > 0)
                {
                    this.ActivateMdiChild( this.MdiChildren[0] );
                    return this.ActiveMdiChild.ActiveControl as FlowView;
                }
            }

            return null;
        }


        

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class PaletteSorter : IComparer<GoObject>
    {
        public PaletteSorter() { }
        public int Compare(GoObject x, GoObject y)
        {
            GoGroup m = (GoGroup)x;
            GoGroup n = (GoGroup)y;
            if (m == null || n == null) return 0;
            if (m.Width == n.Width) return 0;
            //if (m.Width > n.Width) return 1;
            //if (m.Height == n.Height) return 0;
            if (m.Height > n.Height) return 1;
            return -1;
        }
    }

    
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class MyPalette : GoPalette
    {
        public override void LayoutItems()
        {
            if (!this.AutomaticLayout) return;

            bool vert = (this.Orientation == Orientation.Vertical);

            ICollection<GoObject> coll = this.Document;
            if (this.Sorting != SortOrder.None && this.Comparer != null)
            {
                GoObject[] a = this.Document.CopyArray();
                // Array.Sort<GoObject>(a, 0, a.Length, this.Comparer);
                // if (this.Sorting == SortOrder.Descending)
                //     Array.Reverse(a, 0, a.Length);
                coll = a;
            }

            //Note align
            //this.AlignsSelectionObject = false;


            // position all objects so they don't overlap and no
            // opposite scrollbar is needed
            SizeF viewsize = this.DocExtentSize;
            SizeF cellsize = this.GridCellSize;
            PointF gridorigin = this.GridOrigin;
            bool useselobj = this.AlignsSelectionObject;
            bool first = true;
            PointF pnt = gridorigin;
            float maxcol = Math.Min(gridorigin.X, 0);
            float maxrow = Math.Min(gridorigin.Y, 0);

            foreach (GoObject obj in coll)
            {
                // maybe operate on SelectionObject instead of whole object
                GoObject selobj = obj;
                if (useselobj)
                {
                    selobj = obj.SelectionObject;
                    if (selobj == null)
                        selobj = obj;
                }
                selobj.Position = pnt;
                if (vert)
                {
                    pnt = ShiftRight(obj, selobj, maxcol, pnt, cellsize);
                    if (!first && obj.Right >= viewsize.Width)
                    {  // new row?
                        maxcol = Math.Min(gridorigin.X, 0);
                        pnt.X = gridorigin.X;
                        while (pnt.Y < maxrow)
                            pnt.Y = pnt.Y + cellsize.Height;
                        pnt.Y = Math.Max(pnt.Y + cellsize.Height, maxrow); // align the shape catalogue
                        selobj.Position = pnt;
                        pnt = ShiftRight(obj, selobj, maxcol, pnt, cellsize);
                    }
                    pnt.X += cellsize.Width;
                }
                else
                {  // horizontal orientation
                    pnt = ShiftDown(obj, selobj, maxrow, pnt, cellsize);
                    if (!first && obj.Bottom >= viewsize.Height)
                    {  // new column?
                        maxrow = Math.Min(gridorigin.Y, 0);
                        pnt.Y = gridorigin.Y;
                        pnt.X = Math.Max(pnt.X + cellsize.Width, maxcol);
                        selobj.Position = pnt;
                        pnt = ShiftDown(obj, selobj, maxrow, pnt, cellsize);
                    }
                    pnt.Y += cellsize.Height;
                }
                maxcol = Math.Max(maxcol, obj.Right);
                maxrow = Math.Max(maxrow, obj.Bottom);
                first = false;
            }

            // minimize the size of the document
            this.Document.Bounds = ComputeDocumentBounds();
            
        }

        private PointF ShiftDown(GoObject obj, GoObject selobj, float maxrow, PointF pnt, SizeF cellsize)
        {
            while (obj.Top < maxrow)
            {
                pnt.Y += cellsize.Height;
                float old = obj.Top;
                selobj.Top = pnt.Y;
                if (obj.Top <= old) break;
            }
            return pnt;
        }

        private PointF ShiftRight(GoObject obj, GoObject selobj, float maxcol, PointF pnt, SizeF cellsize)
        {
            while (obj.Left < maxcol)
            {
                pnt.X += cellsize.Width;
                float old = obj.Left;
                selobj.Left = pnt.X;
                if (obj.Left <= old) break;
            }
            return pnt;
        }


        


    }
}
