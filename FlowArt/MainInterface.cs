using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using Northwoods.Go;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace FlowArt
{
    public enum LogType
    {
        Normal = 0,
        Success = 1,
        Error = 2,
    }

    partial class MainInterface : MainBase // not design
    //partial class MainInterface : DevExpress.XtraBars.Ribbon.RibbonForm // design
    {
        private System.Data.DataTable tableWatcher = new System.Data.DataTable();
        private FlowMap canvas = new FlowMap();
        private bool isCatalogueShown = true;
        private bool isMessageShown = true;
        private bool isWatcherShown = true;

        public MainInterface()
        {
            InitializeComponent();

            InitializeFlowRunManager();
        }

        private void MainInterface_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;

            InitializeCatalog();

            pnlLeft.Show();
            pageMessage.Hide();
            dtvWatcher.Hide();
            isCatalogueShown = true;
            isMessageShown = false;
            isWatcherShown = false;

        }

        private void InitializeCatalog()
        {
            paletteMain.Width = pnlLeft.Width;
            paletteMain.Height = 1500;
            // paletteMain.Height = pnlLeft.Height;
            // paletteMain.Size = pnlLeft.Size;
            paletteMain.Location = new System.Drawing.Point(pnlLeft.Location.X, paletteMain.Location.Y);

            FlowBlock n;
            n = new FlowBlock(BlockType.Start);
            paletteMain.Document.Add(n);
            n = new FlowBlock(BlockType.Input);
            paletteMain.Document.Add(n);
            n = new FlowBlock(BlockType.Process);
            paletteMain.Document.Add(n);
            n = new FlowBlock(BlockType.Condition);
            paletteMain.Document.Add(n);
            n = new FlowBlock(BlockType.Output);
            paletteMain.Document.Add(n);
            n = new FlowBlock(BlockType.End);
            paletteMain.Document.Add(n);

            //paletteMain.GridCellSize = new SizeF(40, (int)n.Height + 3);
            // myPalette.Document.Add(c);

            GoComment c = new GoComment();
            c.TopLeftMargin = new SizeF(8, 8);
            c.BottomRightMargin = new SizeF(8, 8);
            c.Shadowed = true;
            c.Text = "comment sth";
            paletteMain.Document.Add(c);
        }


        #region ShowHideButton
        private void btnShowHideWatcher_Click(object sender, EventArgs e)
        {
            if (btnShowHideWatcher.Text == "Hide")
            {
                btnShowHideWatcher.Text = "Show";
                btnShowHideWatcher.BringToFront();
                dtvWatcher.Hide();
                btnShowHideWatcher.Location = new System.Drawing.Point(this.ClientSize.Width - btnShowHideWatcher.Width, btnShowHideWatcher.Location.Y);
            }
            else
            {
                btnShowHideWatcher.Text = "Hide";
                dtvWatcher.Show();
                btnShowHideWatcher.BringToFront();
                btnShowHideWatcher.Location = new System.Drawing.Point(dtvWatcher.Location.X-btnShowHideWatcher.Width,dtvWatcher.Location.Y);
            }
        }

        private void btnShowHideMessage_Click(object sender, EventArgs e)
        {
            if (btnShowHideMessage.Text == "Hide")
            {
                btnShowHideMessage.Text = "Show";
                btnShowHideMessage.BringToFront();
                pageMessage.Hide();
                btnShowHideMessage.Location = new System.Drawing.Point(btnShowHideMessage.Location.X, this.ClientSize.Height - btnShowHideMessage.Height);
            }
            else
            {
                btnShowHideMessage.Text = "Hide";
                pageMessage.Show();
                btnShowHideMessage.BringToFront();
                btnShowHideMessage.Location = new System.Drawing.Point(btnShowHideMessage.Location.X, pageMessage.Location.Y - btnShowHideMessage.Height); ;
            }
        }

        private void btnShowHideCatalog_Click(object sender, EventArgs e)
        {
            if (btnShowHideCatalog.Text == "Hide")
            {
                btnShowHideCatalog.Text = "Show";
                btnShowHideCatalog.BringToFront();
                pnlLeft.Hide();
                btnShowHideCatalog.Location = new System.Drawing.Point(0, btnShowHideCatalog.Location.Y);
            }
            else
            {
                btnShowHideCatalog.Text = "Hide";
                pnlLeft.Show();
                btnShowHideCatalog.BringToFront();
                btnShowHideCatalog.Location = new System.Drawing.Point(pnlLeft.Width, btnShowHideCatalog.Location.Y);
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// frmMain legacy

        }
        #endregion

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }


        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowMap canvas = new FlowMap();
            canvas.MdiParent = this;
            canvas.Show();
            canvas.WindowState = FormWindowState.Maximized;
            canvas.View.UpdateFormInfo();
            FlowDocument doc = canvas.View.Doc;

            doc.UndoManager.Clear();
            doc.IsModified = false;

            frmInfo dlg = new frmInfo();
            dlg.Doc = doc;
            dlg.ShowDialog();
        }

        private void btnOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowMap.Open(this);
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowMap canvas = this.ActiveMdiChild as FlowMap;
            if (canvas != null)
                canvas.Save();
        }

        private void btnSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowMap canvas = this.ActiveMdiChild as FlowMap;
            if (canvas != null)
                canvas.SaveAs();
        }

        private void btnSaveAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                FlowMap w = f as FlowMap;
                if (w != null)
                    w.Save();
            }
        }

        private void btnExportToImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.ExportToImage();
            }
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null)
            {
                view.Print();
            }
        }

        private void btnPrintPrev_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null)
            {
                view.PrintPreview();
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowMap canvas = this.ActiveMdiChild as FlowMap;
            if (canvas != null)
                canvas.Close();
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.CanUndo())
            {
                view.Undo();
            }
        }

        private void btnRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.CanRedo())
            {
                view.Redo();
            }
        }

        private void btnCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.CanEditCut())
            {
                view.EditCut();
            }
        }

        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.CanEditCopy())
            {
                view.EditCopy();
            }
        }

        private void btnPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.CanEditPaste())
            {
                view.EditPaste();
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.Selection.Count>=1 && view.CanEditDelete())
            {
                view.EditDelete();
            }
        }

        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null)
            {
                view.SelectAll();
            }
        }

        private void btnStartFlow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.Start);
            }
        }

        private void btnProcessBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.Process);
            }
        }

        private void btnConditionBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.Condition);
            }
        }

        private void btnScanBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.Input);
            }
        }

        private void btnPrintBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.Output);
            }
        }

        private void btnEndFlow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertNode(BlockType.End);
            }
        }

        private void btnDrawLink_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok)
            {
                view.StartDrawingRelationship();
            }
        }

        private void btnLinkAmongSelection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly && view.Selection.Count==2); // create links from only two blocks
            if (ok)
            {
                view.CreateRelationshipsAmongSelection();
            }
        }

        private void btnComment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly);
            if (ok == true)
            {
                view.Doc.InsertComment();
            }
        }

        private void btnAlignHor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly && (view.Selection.Count >= 2));
            if (ok)
                view.AlignHorizontalCenters();
        }

        private void btnAlignVer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            bool ok = (view != null && !view.Doc.IsReadOnly && (view.Selection.Count >= 2));
            if (ok)
                view.AlignVerticalCenters();
        }

        private void btnProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoView view = GetCurrentGoView();
            if (view != null && view.Document is FlowDocument)
            {
                frmInfo dlg = new frmInfo();
                dlg.Doc = (FlowDocument)view.Document;
                dlg.ShowDialog();
            }
        }

        private void btnWindowMode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
        }

        private void btnDistractionFree_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnZoomIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            if (view != null)
                view.ZoomIn();
        }

        private void btnZoomOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            if (view != null)
                view.ZoomOut();
        }

        private void btnZoomToFit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            if (view != null)
                view.ZoomToFit();
        }

        private void btnZoomNormal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FlowView view = GetCurrentGraphView();
            if (view != null)
                view.ZoomNormal();
        }

        private void btnCatalogue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //btnShowHideCatalog.PerformClick();
            if (isCatalogueShown)
            {
                pnlLeft.Hide();
                isCatalogueShown = false;
            }
            else
            {
                pnlLeft.Show();
                isCatalogueShown = true;
            }
        }

        private void btnMessageArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //btnShowHideMessage.PerformClick();
            if (isMessageShown)
            {
                pageMessage.Hide();
                isMessageShown = false;
            }
            else
            {
                pageMessage.Show();
                isMessageShown = true;
            }
        }

        private void btnWatchbox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //btnShowHideWatcher.PerformClick();
            if (isWatcherShown)
            {
                dtvWatcher.Hide();
                isWatcherShown = false;
            }
            else
            {
                dtvWatcher.Show();
                isWatcherShown = true;
            }
        }

        private void btnGuide_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/document/d/1kYf3f0grNUmoi8wpAfJ3A1H4UuOlu_1--ppnwhMC9Uk/edit?usp=sharing");
            /*FileInfo fi = new FileInfo("C:\\Users\\lehoa\\Dropbox\\My Code\\schoolcode\\Nam2\\LTTQ\\UI fix\\after rtb\\FlowArt\\Resources\\FALan_Help.docx");
            try
            {
                System.Diagnostics.Process.Start(@"C:\\Users\\lehoa\\Dropbox\\My Code\\schoolcode\\Nam2\\LTTQ\\UI fix\\after rtb\\FlowArt\\Resources\\FALan_Help.docx");
            }
            catch
            {
                return;
            }
            */
        }

        private void btnCheatSheet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/1Ngf9BVKLtFHz8ZMSuKnRr6CUisTE9wdrUKk7N49XeJ8/edit?usp=sharing");
            /*FileInfo fi = new FileInfo("C:\\Users\\lehoa\\Dropbox\\My Code\\schoolcode\\Nam2\\LTTQ\\UI fix\\after rtb\\FlowArt\\Resources\\FlowArt-Functions-Document.xlsx");
            try
            {
                System.Diagnostics.Process.Start(@"C:\\Users\\lehoa\\Dropbox\\My Code\\schoolcode\\Nam2\\LTTQ\\UI fix\\after rtb\\FlowArt\\Resources\\FlowArt-Functions-Document.xlsx");
            }
            catch
            {
                return;
            }
            */
        }

        private void btnLibrary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }


        private void btnAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/document/d/1qE3qJ1tgOmUQT0bgJdF_S6LQ51FAoAJjBDg7bR55hVU/edit?usp=sharing");
        }


        /// initialize Flow Run Manager //////////////////////////////////////////////////////////////////////////////////////////////////
        /// Author: CuteTN
        FlowRunManager flowRunManager;

        private void InitializeFlowRunManager()
        {
            flowRunManager = new FlowRunManager(this, 201, true, true);
            flowRunManager.ReachEndBlock += FlowRunManager_ReachEndBlock_FocusResult;
            flowRunManager.RaiseError += FlowRunManager_RaiseError_FocusLog;
            flowRunManager.FlowRunStateChanged += flowRunManager_FlowRunStateChanged;

            UpdateEnabledButtons(flowRunManager.State, flowRunManager.Auto);
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            flowRunManager.HandleControlCommand();
        }

        private void btnPause_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            flowRunManager.HandlePauseCommand();
        }

        private void btnTerminate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            flowRunManager.HandleTerminateCommand();
        }

        private void ApplyRunSetting(frmRunSetting dlg)
        {
            flowRunManager.Auto = dlg.AutoRunResult;
            flowRunManager.ClearOldResult = dlg.ClearResultResult;
            flowRunManager.PlayFadingEffect = dlg.FadingEffectResult;
            flowRunManager.NotifyOnEnd = dlg.NotificationResult;
            flowRunManager.PlaySoundEffect = dlg.SoundEffectResult;

            flowRunManager.TimePerBlock = (10 - dlg.SpeedResult)*100 + 1;            
            UpdateEnabledButtons(flowRunManager.State , flowRunManager.Auto);
        }

        private void btnSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool Auto = flowRunManager.Auto;
            int Speed = 10 - (flowRunManager.TimePerBlock - 1)/100;   
            bool FadingEffect = flowRunManager.PlayFadingEffect;
            bool SoundEffect = flowRunManager.PlaySoundEffect;
            bool ClearResult = flowRunManager.ClearOldResult;
            bool NotifyOnEnd = flowRunManager.NotifyOnEnd;
                     
            frmRunSetting dlg = new frmRunSetting(Auto, Speed, FadingEffect, SoundEffect, ClearResult, NotifyOnEnd);
            DialogResult dialogResult = dlg.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                ApplyRunSetting(dlg);
            }
        }

        private void btnLogo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void flowRunManager_FlowRunStateChanged(object sender, EventArgs e)
        {
            UpdateEnabledButtons( flowRunManager.State, flowRunManager.Auto );
        }

        private void UpdateEnabledButtons(FlowRunState state, bool AutoRun)
        {
            switch (state)
            {
                case FlowRunState.Start:
                {
                    btnPlay.Enabled = true;
                    btnPlay.Caption = "Start";
                    btnPause.Enabled = false;
                    btnTerminate.Enabled = false;
                    btnSetting.Enabled = true;
                    break;
                }
                case FlowRunState.Run:
                {
                    btnPlay.Enabled = false;
                    btnPause.Enabled = true;
                    btnTerminate.Enabled = true;
                    btnSetting.Enabled = false;
                    break;
                }
                case FlowRunState.Pause:
                {
                    btnPlay.Enabled = true;
                    if( AutoRun )
                        btnPlay.Caption = "Resume";
                    else
                        btnPlay.Caption = "Next";

                    btnPause.Enabled = false;
                    btnTerminate.Enabled = true;
                    btnSetting.Enabled = true;
                    break;
                }
                default:
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Author: CuteTN /////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        // singleton MainInterface (LeeSown connect)
        private static MainInterface instance = null;
        public static MainInterface Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainInterface();
                }
                return instance;
            }
        }

        // WATCH TABLE
        public void UpdateWatch(FlowDataManager flowData)
        {
            ClearWatch();
            List<KeyValuePair<string, VarInfo>> varList = flowData.GetVarList();

            foreach (var element in varList)
            {
                string varName = element.Key;
                string varType = element.Value.type.ToString().Remove(0, 3);
                string varValue = element.Value.value;

                switch (varType)
                {
                    case "Number":
                        {
                            break;
                        }
                    case "String":
                        {
                            varValue = varValue.Replace("\"", "\"\"");
                            varValue = "\"" + varValue + "\"";
                            break;
                        }
                    case "Boolean":
                        {
                            varValue = "$" + varValue;
                            break;
                        }
                    default:
                        {
                            varValue = "Unhandled data type!";
                            break;
                        }
                }

                if (element.Value.isNull)
                {
                    varValue = "$NULL";
                }

                dtvWatcher.Rows.Add(varName, varType, varValue);
            }
        }

        public void ClearWatch()
        {
            dtvWatcher.Rows.Clear();
        }


        // USER LOG
        public void UserLog(string s, LogType logType)
        {
            RichTextBox rtbToUpdate;
            try
            {
                rtbToUpdate = pageLog.Controls["rtbLog"] as RichTextBox;
            }
            catch
            {
                // merry Xmas
                return;
            }

            int CurrentSelectionStart = rtbToUpdate.SelectionStart;
            rtbToUpdate.AppendText(s + Environment.NewLine);
            rtbToUpdate.SelectionStart = CurrentSelectionStart;
            rtbToUpdate.SelectionLength = s.Length;

            switch (logType)
            {
                case LogType.Normal:
                    {
                        rtbToUpdate.SelectionColor = Color.Black;
                        break;
                    }

                case LogType.Error:
                    {
                        rtbToUpdate.SelectionColor = Color.OrangeRed;
                        break;
                    }

                case LogType.Success:
                    {
                        rtbToUpdate.SelectionColor = Color.Green;
                        break;
                    }

                default:
                    break;
            }

            rtbToUpdate.SelectionStart = rtbToUpdate.Text.Length;
            rtbToUpdate.ScrollToCaret();
        }

        public void UserOutput(string s, LogType logType)
        {
            RichTextBox rtbToUpdate;
            try
            {
                rtbToUpdate = pageResult.Controls["rtbResult"] as RichTextBox;
            }
            catch
            {
                // merry Xmas
                return;
            }

            int CurrentSelectionStart = rtbToUpdate.SelectionStart;
            rtbToUpdate.AppendText(s + Environment.NewLine);
            rtbToUpdate.SelectionStart = CurrentSelectionStart;
            rtbToUpdate.SelectionLength = s.Length;

            switch (logType)
            {
                case LogType.Normal:
                    {
                        rtbToUpdate.SelectionColor = Color.Black;
                        break;
                    }

                case LogType.Error:
                    {
                        rtbToUpdate.SelectionColor = Color.OrangeRed;
                        break;
                    }

                case LogType.Success:
                    {
                        rtbToUpdate.SelectionColor = Color.Green;
                        break;
                    }

                default:
                    break;
            }

            rtbToUpdate.SelectionStart = rtbToUpdate.Text.Length;
            rtbToUpdate.ScrollToCaret();
        }

        public void ClearUserLog()
        {
            RichTextBox rtbToUpdate;
            try
            {
                rtbToUpdate = pageLog.Controls["rtbLog"] as RichTextBox;
            }
            catch
            {
                // merry Xmas
                return;
            }
            rtbToUpdate.Clear();
        }

        public void ClearUserOutput()
        {
            RichTextBox rtbToUpdate;
            try
            {
                rtbToUpdate = pageResult.Controls["rtbResult"] as RichTextBox;
            }
            catch
            {
                // merry Xmas
                return;
            }
            rtbToUpdate.Clear();
        }

        // LeeSown FOCUS ON RTB LOG WHEN WRONG AND RTB RESULT WHEN RIGHT
        //LeeSown
        public void FlowRunManager_ReachEndBlock_FocusResult(object sender, EventArgs e)
        {
            rtbResultFocus();
        }

        public void FlowRunManager_RaiseError_FocusLog(object sender, EventArgs e)
        {
            rtbLogFocus();
        }

        public void rtbResultFocus()
        {
            if( pageMessage.Visible )
                return;
                
            pageMessage.Show();
            isMessageShown = true;
            this.pageMessage.SelectedTabPage = this.pageResult;
        }

        public void rtbLogFocus()
        {
            pageMessage.Show();
            isMessageShown = true;
            this.pageMessage.SelectedTabPage = this.pageLog;
        }


        // search for blocks //////////////////////////////////////////////////////////////////////////////////////////////////// 
        string OldSearchKeyWord = "";
        FlowBlock BlockSearchResult = null;

        private void repositoryItemSearchControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.KeyCode == Keys.Return )
            {
                string text = (sender as TextEdit).Text;
                FlowView view = this.GetCurrentGraphView();
                if(view == null)
                    return;

                // if the search keyword has change, then reset the search enumerator
                // otherwise, keep searching for the next result
                if(text != OldSearchKeyWord)
                    view.BlockSearcher.SearchReset = true;
                OldSearchKeyWord = text;

                BlockSearchResult?.FxPlayer.FadeToNormalColor();

                BlockSearchResult = view.BlockSearcher.SearchNextBlock(text);
                if( BlockSearchResult == null )
                    return;

                // highlight search result
                BlockSearchResult?.FxPlayer?.DarkenColor();
                view.ScrollRectangleToVisible(BlockSearchResult);
            }
        }

    }
}
