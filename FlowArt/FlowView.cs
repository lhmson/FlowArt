using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;  // clipboard methods
using System.Drawing.Imaging;          // MetaFile
using System.IO;                       // MemoryStream (for metafile)
using Northwoods.Go;

namespace FlowArt
{
    class FlowView : GoView
    {
        public FlowView()
        {
            this.NewLinkClass = typeof(GraphLink);
            this.PortGravity = 30;
            this.GridCellSize = new SizeF(5, 10);
            this.GridSnapDrag = GoViewSnapStyle.Jump;
            this.DragsRealtime = true;
        }

        /// <summary>
        /// A new FlowView will have a FlowDocument as its document.
        /// </summary>
        /// <returns>A <see cref="FlowDocument"/></returns>
        public override GoDocument CreateDocument()
        {
            GoDocument doc = new FlowDocument();
            doc.UndoManager = new GoUndoManager();
            return doc;
        }

        /// <summary>
        /// A convenience property for getting the view's GoDocument as a FlowDocument.
        /// </summary>
        public FlowDocument Doc
        {
            get { return this.Document as FlowDocument; }
        }

        public override IGoLink CreateLink(IGoPort from, IGoPort to)
        {
            IGoLink il = base.CreateLink(from, to);
            if (il != null)
            {
                GoLabeledLink l = il.GoObject as GoLabeledLink;
                if (l != null)
                {
                    FlowBlock fromNode = from.Node.GoObject as FlowBlock;
                    if (fromNode != null && fromNode.Kind == BlockType.Condition)
                    {
                        GoText t = new GoText();
                        t.Text = "yes";
                        t.Selectable = false;
                        t.Editable = true;
                        l.FromLabel = t;
                    }
                    //l.Orthogonal = true;
                    //l.Style = GoStrokeStyle.RoundedLine;
                    //l.ToArrow = true;
                }
            }
            return il;
        }

        /// <summary>
        /// This method is responsible for updating all of the view's visible
        /// state outside of the GoView itself--the title bar, status bar, and properties grid.
        /// </summary>
        public virtual void UpdateFormInfo()
        {
            UpdateTitle();
            MainBase.App.SetStatusMessage(this.Doc.Location);
            MainBase.App.SetStatusZoom(this.DocScale);


        }

        /// <summary>
        /// Update the title bar with the view's document's Name, and an indication
        /// of whether the document is read-only and whether it has been modified.
        /// </summary>
        public virtual void UpdateTitle()
        {
            Form win = this.Parent as Form;
            if (win != null)
            {
                String title = this.Document.Name;
                if (this.Doc.IsReadOnly)
                    title += " [Read Only]";
                if (this.Doc.IsModified)
                    title += "*";
                win.Text = title;
            }
        }

        /// <summary>
        /// If the document's name changes, update the title;
        /// if the document's location changes, update the status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evt"></param>
        protected override void OnDocumentChanged(Object sender, GoChangedEventArgs e)
        {
            base.OnDocumentChanged(sender, e);
            if (e.Hint == GoDocument.ChangedName || e.Hint == GoDocument.FinishedTransaction || e.Hint == GoDocument.AbortedTransaction ||
                e.Hint == GoDocument.RepaintAll || e.Hint == GoDocument.FinishedUndo || e.Hint == GoDocument.FinishedRedo)
            {
                UpdateFormInfo();
            }
            else if (e.Hint == FlowDocument.ChangedLocation)
            {
                MainBase.App.SetStatusMessage(this.Doc.Location);
            }
            
        }

        /// <summary>
        /// If the view's document is replaced, update the titlegolink;
        /// if the view's scale changes, update the status bar
        /// </summary>
        /// <param name="evt"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == "Document")
                UpdateFormInfo();
            else if (e.PropertyName == "DocScale")
                MainBase.App.SetStatusZoom(this.DocScale);
        }


        private GoObject myPrimarySelection = null;

        protected override void OnObjectGotSelection(GoSelectionEventArgs e)
        {
            base.OnObjectGotSelection(e);
            if (myPrimarySelection != this.Selection.Primary)
            {
                myPrimarySelection = this.Selection.Primary;
            }
        }

        protected override void OnObjectLostSelection(GoSelectionEventArgs e)
        {
            base.OnObjectLostSelection(e);
            if (myPrimarySelection != this.Selection.Primary)
            {
                myPrimarySelection = this.Selection.Primary;
            }
        }

        protected override void OnBackgroundHover(GoInputEventArgs e)
        {
            foreach (GoObject obj in this.Document)
            {
                IGoNode n = obj as IGoNode;
                if (n != null)
                {
                    foreach (GoPort p in n.Ports)
                    {
                        p.SkipsUndoManager = true;
                        p.Style = GoPortStyle.None;
                        p.SkipsUndoManager = false;
                    }
                }
            }
            base.OnBackgroundHover(e);
        }


        protected override void OnClipboardCopied(EventArgs e)
        {
            base.OnClipboardCopied(e);
        }

        /// <summary>
        /// Bring up a context menu when the user context clicks in the background.
        /// </summary>
        /// <param name="evt"></param>
        protected override void OnBackgroundContextClicked(GoInputEventArgs e)
        {
            base.OnBackgroundContextClicked(e);
            // set up the background context menu
            GoContextMenu cm = new GoContextMenu(this);
            if (CanInsertObjects())
                cm.MenuItems.Add(new MenuItem("Paste", new EventHandler(this.Paste_Command)));
            if (cm.MenuItems.Count > 0)
                cm.MenuItems.Add(new MenuItem("-"));
            cm.MenuItems.Add(new MenuItem("Properties", new EventHandler(this.Properties_Command)));
            cm.Show(this, e.ViewPoint);
        }

        /// <summary>
        /// Called when the user clicks on the background context menu Paste menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This calls <see cref="GoView.EditPaste"/> and selects all of the newly pasted objects.
        /// </remarks>
        public void Paste_Command(Object sender, EventArgs e)
        {
            PointF docpt = this.LastInput.DocPoint;
            StartTransaction();
            this.Selection.Clear();
            EditPaste();  // selects all newly pasted objects
            RectangleF copybounds = GoDocument.ComputeBounds(this.Selection, this);
            SizeF offset = new SizeF(docpt.X - copybounds.X, docpt.Y - copybounds.Y);
            MoveSelection(this.Selection, offset, true);
            FinishTransaction("Just Paste");
        }

        /// <summary>
        /// Bring up the properties dialog for the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Properties_Command(Object sender, EventArgs e)
        {
            frmInfo dlg = new frmInfo();
            dlg.Doc = this.Doc;
            dlg.ShowDialog();
        }


        //align all object's horizontal centers to the horizontal center of the primary selection
        public virtual void AlignHorizontalCenters()
        {
            GoObject obj = this.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                float X = obj.SelectionObject.Center.X;
                foreach (GoObject temp in this.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink))
                    {
                        t.Center = new PointF(X, t.Center.Y);
                    }
                }
                FinishTransaction("Align Horizontally");
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a block.");
            }
        }

        //align all object's vertical centers to the vertical center of the primary selection
        public virtual void AlignVerticalCenters()
        {
            GoObject obj = this.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                float Y = obj.SelectionObject.Center.Y;
                foreach (GoObject temp in this.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink))
                        t.Center = new PointF(t.Center.X, Y);
                }
                FinishTransaction("Align Vertically");
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a block.");
            }
        }

        public virtual void ExportToImage()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            saveDialog.DefaultExt = "png";
            saveDialog.AddExtension = true;
            saveDialog.FileName = this.Doc.Name;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = this.GetBitmap(); // get the bitmap rendered from FlowArt
                bmp.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }



        // this makes the widths of all objects equal to the width of the main selection.
        public virtual void MakeWidthsSame()
        {
            GoObject obj = this.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                float W = obj.SelectionObject.Width;
                foreach (GoObject temp in this.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink))
                        t.Width = W;
                }
                FinishTransaction("Same Widths");
            }
            else
            {
                MessageBox.Show("Sizing failure: Primary Selection is empty or a link instead of a block.");
            }
        }

        // this makes the heights of all objects equal to the height of the main selection.
        public virtual void MakeHeightsSame()
        {
            GoObject obj = this.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                float H = obj.SelectionObject.Height;
                foreach (GoObject temp in this.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink))
                        t.Height = H;
                }
                FinishTransaction("Same Heights");
            }
            else
            {
                MessageBox.Show("Sizing failure: Primary Selection is empty or a link instead of a block.");
            }
        }

        // this makes the heights and widths of all objects equal to the height and
        //width of the main selection.
        public virtual void MakeSizesSame()
        {
            GoObject obj = this.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                SizeF S = obj.SelectionObject.Size;
                foreach (GoObject temp in this.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink))
                        t.Size = S;
                }
                FinishTransaction("Same Sizes");
            }
            else
            {
                MessageBox.Show("Sizing failure: Primary Selection is empty or a link instead of a block.");
            }
        }


        public virtual void ZoomIn()
        {
            myOriginalScale = true;
            float newscale = (float)(Math.Round(this.DocScale / 0.9f * 100) / 100);
            this.DocScale = newscale;
        }

        public virtual void ZoomOut()
        {
            myOriginalScale = true;
            float newscale = (float)(Math.Round(this.DocScale * 0.9f * 100) / 100);
            this.DocScale = newscale;
        }

        public virtual void ZoomNormal()
        {
            myOriginalScale = true;
            this.DocScale = 1;
        }

        public virtual void ZoomToFit()
        {
            if (myOriginalScale)
            {
                myOriginalDocPosition = this.DocPosition;
                myOriginalDocScale = this.DocScale;
                RescaleToFit();
            }
            else
            {
                this.DocPosition = myOriginalDocPosition;
                this.DocScale = myOriginalDocScale;
            }
            myOriginalScale = !myOriginalScale;
        }

        public static bool IsLinked(FlowBlock a, FlowBlock b)
        {
            if (a.BottomPort == null)
                return false;
            return a.BottomPort.IsLinked(b.TopPort) ||
                   a.BottomPort.IsLinked(b.LeftPort) ||
                   a.BottomPort.IsLinked(b.RightPort) ||
                   a.BottomPort.IsLinked(b.BottomPort);
        }

        public void CreateRelationshipsAmongSelection()
        {
            FlowBlock boss = this.Selection.Primary as FlowBlock;
            if (boss != null)
            {
                if (boss.BottomPort == null)
                {
                    MessageBox.Show(this, "Cannot create link originating from this block.", "Error", MessageBoxButtons.OK);
                }
                else
                {
                    StartTransaction();
                    foreach (GoObject obj in this.Selection)
                    {
                        FlowBlock n = obj as FlowBlock;
                        if (n != null && n != boss && !IsLinked(boss, n))
                        {
                            if (n.TopPort == null)
                            {
                                MessageBox.Show(this, "Cannot create link finishing with this block.", "Error", MessageBoxButtons.OK);
                            }
                            else
                            {
                                IGoLink l = CreateLink(boss.BottomPort, n.TopPort);
                                if (l != null)
                                {
                                    this.Document.Add(l.GoObject);
                                    RaiseLinkCreated(l.GoObject);
                                }
                            }
                        }
                    }
                    FinishTransaction("Create links among selection blocks");
                }
            }
        }

        public void StartDrawingRelationship()
        {
            this.Tool = new linkTool(this);
        }

        private bool myOriginalScale = true;
        private PointF myOriginalDocPosition = new PointF();
        private float myOriginalDocScale = 1.0f;



        // Logic for running flow //////////////////////////////////////////////////////////////////////////////////////////////////// 
        public FlowBlock GetStartBlock(out string ErrorReport)
        {
            FlowDocument doc = this.Document as FlowDocument;
            int countStart = 0;
            ErrorReport = "";
            FlowBlock result = null;

            foreach (GoObject obj in doc)
            {
                if (obj is GoTextNode)
                {
                    if ((obj as FlowBlock).Kind == BlockType.Start)
                    {
                        countStart++;
                        result = (obj as FlowBlock);
                    }
                }
            }

            if(countStart < 1)
            {
                ErrorReport = "Error: Your flow is lack of the Start block";
                return null;
            }
            if(countStart > 1)
            {
                ErrorReport = "Error: Your flow should only have one Start block";
                return null;
            }

            if(countStart == 1)
            {
                return result;
            }

            return null;
        }

        public FlowBlock GetNextBlock(FlowBlock CurrentBlock, VarInfo blockResult, out string ErrorReport)
        {
            if( CurrentBlock == null )
                return GetStartBlock(out ErrorReport);
            ErrorReport = "";

            if(CurrentBlock.Kind == BlockType.End)
            {
                return null;
            }
            
            List<IGoLink> OutLinks = new List<IGoLink>();
            OutLinks.Clear();
            foreach(IGoLink link in CurrentBlock.Links)
            {
                if( (link.FromNode as FlowBlock).GetHashCode() == CurrentBlock.GetHashCode() )
                    OutLinks.Add(link);
            }

            if( OutLinks.Count < 1 )
            {
                ErrorReport = "Error: No out-link is found";
                return null;
            }

            if(CurrentBlock.Kind == BlockType.Condition)
            {
                OutLinks[0] = PickLinkFromConditionBlock(OutLinks, blockResult, out ErrorReport);
                if( ErrorReport != "" )
                    return null;
            }
            else
            if( OutLinks.Count > 1 )
            {
                ErrorReport = "Error: Regular blocks should only have ONE out-link";
                return null;
            }

            if(OutLinks[0] != null)
            {
                return OutLinks[0].ToNode as FlowBlock;
            }
            else
            {
                // already handled before ;)
                return null;
            }
        }

        private IGoLink PickLinkFromConditionBlock(List<IGoLink> OutLinks, VarInfo blockResult, out string ErrorReport)
        {
            IGoLink pickedLink = null;
            ErrorReport = "";
            int countElement = OutLinks.Count;

            // check for duplicated link value:
            for(int i=0; i<countElement-1; i++)
                for(int j=i+1; j<countElement; j++)
                {
                    string linkValue1 = ( (OutLinks[i] as GoLabeledLink).FromLabel as GoText).Text;
                    linkValue1 = linkValue1.ToUpper();
                    linkValue1 = FlowDataManager.RemoveSpacing(linkValue1);
                    if (linkValue1 == "")
                    {
                        ErrorReport = "Error: Blank out-link label";
                        return null;
                    }
                    if (linkValue1[0] >= 'A' && linkValue1[0] <= 'Z')
                        linkValue1 = "$" + linkValue1;

                    string linkValue2 = ( (OutLinks[j] as GoLabeledLink).FromLabel as GoText).Text;
                    linkValue2 = linkValue2.ToUpper();
                    linkValue2 = FlowDataManager.RemoveSpacing(linkValue2);
                    if (linkValue2 == "")
                    {
                        ErrorReport = "Error: Blank out-link label";
                        return null;
                    }
                    if (linkValue2[0]>='A' && linkValue2[0]<='Z')
                        linkValue2 = "$" + linkValue2;

                    if(linkValue1 == "$ELSE")
                    {
                        pickedLink = OutLinks[i];
                    }        
                    if(linkValue2 == "$ELSE")
                    {
                        pickedLink = OutLinks[j];
                    }
                    if(linkValue1 == "$ELSE" && linkValue2 == "$ELSE")
                    {
                        ErrorReport = "Error: Duplicated out link label/value: ELSE";
                        return null;
                    }

                    VarInfo linkVar1 = FlowDataManager.HandleConst(linkValue1);
                    VarInfo linkvar2 = FlowDataManager.HandleConst(linkValue2);

                    if( VarInfo.IsEqual(linkVar1,linkvar2) )
                    {
                        ErrorReport = "Error: Duplicated out link label/value: " + linkValue1;
                        return null;
                    }
                    
                }

            // check all valid links
            foreach(IGoLink link in OutLinks)
            {
                string linkValue = ( (link as GoLabeledLink).FromLabel as GoText).Text;
                linkValue = linkValue.ToUpper();
                linkValue = FlowDataManager.RemoveSpacing(linkValue);
                if (linkValue == "")
                {
                    ErrorReport = "Error: Blank out-link label";
                    return null;
                }
                if (linkValue[0]>='A' && linkValue[0]<='Z')
                        linkValue = "$" + linkValue;

                if(linkValue == "$ELSE")
                {
                    continue;
                }

                VarInfo tempVar = FlowDataManager.HandleConst(linkValue);

                if(tempVar.isNull)
                {
                    ErrorReport = "Error: Illegal out-link label: " + linkValue;
                    return null;
                }      

                if( VarInfo.IsEqual(tempVar, blockResult) )
                {
                    pickedLink = link;
                }
            }

            if(pickedLink == null)
            {
                ErrorReport = "Error: Couldn't find a label that matches value: " + blockResult.value;
                return null;
            }

            return pickedLink;
        }

        
        // Search //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private ViewBlockSearcher blockSearcher = null;
        public ViewBlockSearcher BlockSearcher
        {
            get
            {
                if( blockSearcher == null )
                    blockSearcher = new ViewBlockSearcher(this);
                return blockSearcher;
            }
        }

        public void ScrollRectangleToVisible(FlowBlock Block)
        {
            RectangleF rect = new RectangleF();
            rect.Location = new PointF(Block.Location.X - Block.Size.Width/2, Block.Location.Y - Block.Size.Height/2);
            rect.Size = Block.Size;
            ScrollRectangleToVisible(rect);
        }


    }
}