using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Northwoods.Go;

namespace FlowArt
{
    /// <summary>
    ///    Summary description for FlowMap.
    ///    Frm main children
    /// </summary>
    class FlowMap : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FlowMap()
        {
            InitializeComponent();
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

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlowMap));
            this.myView = new FlowArt.FlowView();
            this.logo1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logo1)).BeginInit();
            this.SuspendLayout();
            // 
            // myView
            // 
            this.myView.ArrowMoveLarge = 10F;
            this.myView.ArrowMoveSmall = 1F;
            this.myView.BackColor = System.Drawing.Color.White;
            this.myView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myView.DragsRealtime = true;
            this.myView.GridCellSizeHeight = 10F;
            this.myView.GridCellSizeWidth = 5F;
            this.myView.GridSnapDrag = Northwoods.Go.GoViewSnapStyle.Jump;
            this.myView.Location = new System.Drawing.Point(0, 0);
            this.myView.Name = "myView";
            this.myView.PortGravity = 30F;
            this.myView.Size = new System.Drawing.Size(1134, 523);
            this.myView.TabIndex = 0;
            this.myView.Text = "FlowArt";
            // 
            // logo1
            // 
            this.logo1.Image = global::FlowArt.Properties.Resources.logoFA;
            this.logo1.Location = new System.Drawing.Point(2, 2);
            this.logo1.Name = "logo1";
            this.logo1.Size = new System.Drawing.Size(265, 135);
            this.logo1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo1.TabIndex = 1;
            this.logo1.TabStop = false;
            // 
            // FlowMap
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1134, 523);
            this.Controls.Add(this.logo1);
            this.Controls.Add(this.myView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlowMap";
            ((System.ComponentModel.ISupportInitialize)(this.logo1)).EndInit();
            this.ResumeLayout(false);

        }


        
        

        public FlowView View
        {
            get { return myView; }
        }

        public FlowDocument Doc // being edited from public to public static
        {
            get { return myView.Doc; }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.View.DoEndEdit();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this.Doc.IsModified)
            {
                IList windows = FindWindows(this.MdiParent, this.Doc);
                if (windows.Count <= 1)
                {  // only one left, better ask if we need to save
                    String msg = "Save modified design?\r\n" + this.Doc.Name;
                    if (this.Doc.Location != "")
                        msg += "\r\n(" + this.Doc.Location + ") ";
                    DialogResult res = MessageBox.Show(this.MdiParent,
                                                       msg,
                                                       "Closing Modified Design",
                                                       MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (res == DialogResult.Yes)
                    {
                        if (!Save())
                            e.Cancel = true;
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IList windows = FindWindows(this.MdiParent, this.Doc);
            if (windows.Count <= 1)
                FlowDocument.RemoveDocument(this.Doc.Location);
        }

        public virtual bool Save()
        {
            String loc = this.Doc.Location;
            int lastslash = loc.LastIndexOf("\\");
            if (loc != "" && !this.Doc.IsReadOnly && lastslash >= 0 && loc.Substring(lastslash + 1) == Doc.Name)
            {
                FileStream file = null;
                try
                {
                    file = File.Open(loc, FileMode.Create);
                    this.Doc.Store(file, loc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error saving design as a file");
                    return false;
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
                return true;
            }
            else
            {
                return SaveAs();
            }
        }
        

        public virtual bool SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = " (*.fArt)|*.fArt";
            dlg.DefaultExt = "fArt";
            dlg.AddExtension = true;
            if (Doc.Name != "")
                dlg.FileName = Doc.Name;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                String loc = dlg.FileName;
                FileStream file = null;
                try
                {
                    file = File.Open(loc, FileMode.Create);
                    this.Doc.Store(file, loc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error saving design as a file");
                    return false;
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return true;
        }

        public virtual bool Reload()
        {
            String loc = this.Doc.Location;
            if (loc != "")
            {
                FileStream file = File.Open(loc, FileMode.Open);
                FlowDocument olddoc = this.View.Doc;
                FlowDocument newdoc = null;
                try
                {
                    newdoc = FlowDocument.Load(file, loc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error reading design from a file");
                    return false;
                }
                finally
                {
                    file.Close();
                }
                if (newdoc != null)
                {
                    IList windows = FlowMap.FindWindows(this.MdiParent, olddoc);
                    foreach (Object obj in windows)
                    {
                        FlowMap w = obj as FlowMap;
                        if (w != null)
                        {
                            w.View.Document = newdoc;
                        }
                    }
                }
            }
            return true;
        }


        public static FlowMap Open(Form mdiparent)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                String loc = dlg.FileName;
                FlowDocument olddoc = FlowDocument.FindDocument(loc);
                if (olddoc != null)
                {
                    IList windows = FlowMap.FindWindows(mdiparent, olddoc);
                    if (windows.Count > 0)
                    {
                        FlowMap w = windows[0] as FlowMap;

                        

                        if (w.Reload())
                        {
                            w.Show();
                            w.Activate();
                        }
                        return w;
                    }
                }
                else
                {
                    Stream file = dlg.OpenFile();
                    if (file != null)
                    {
                        FlowDocument doc = null;
                        try
                        {
                            
                            doc = FlowDocument.Load(file, loc);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(mdiparent, ex.Message, "Error reading design from a file");
                        }
                        finally
                        {
                            file.Close();
                        }
                        if (doc != null)
                        {
                            FlowMap w = new FlowMap();


                            w.View.Document = doc;
                            w.MdiParent = mdiparent;
                            w.Show();
                            w.Activate();
                            return w;
                        }
                    }
                }
            }
            return null;
        }

        public static IList FindWindows(Form mdiparent, FlowDocument doc)
        {
            ArrayList windows = new ArrayList();
            Form[] children = mdiparent.MdiChildren;
            foreach (Form f in children)
            {
                FlowMap w = f as FlowMap;
                if (w != null && w.Doc == doc)
                {
                    windows.Add(w);
                }
            }
            return windows;
        }

        private PictureBox logo1;

        public FlowView myView;
        

    }
}
